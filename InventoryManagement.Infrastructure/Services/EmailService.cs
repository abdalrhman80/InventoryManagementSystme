using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Infrastructure.Data;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace InventoryManagement.Infrastructure.Services
{
    internal class EmailService(ApplicationDbContext _dbContext, IConfiguration _configuration, ILogger<EmailService> _logger) : IEmailService
    {
        public async Task SendEmailAsync(string[] recipients, string subject, string body)
        {
            try
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_configuration["EmailSettings:Email"]),
                    Subject = subject
                };

                //email.To.Add(MailboxAddress.Parse(to));

                foreach (var recipient in recipients)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                email.Body = bodyBuilder.ToMessageBody();
                email.From.Add(new MailboxAddress(_configuration["EmailSettings:DisplayName"], _configuration["EmailSettings:Email"]));

                using var smtpClient = new SmtpClient();
                smtpClient.Connect(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]!), SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"]);
                await smtpClient.SendAsync(email);
                smtpClient.Disconnect(true);

                _logger.LogInformation("Email sent successfully to {To} with subject: {Subject}", recipients, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To} with subject: {Subject}", recipients, subject);
                throw;
            }
        }

        public async Task SendEmailConfirmationCodeAsync(string email, string firstName, string confirmationCode)
        {
            var subject = "Email Confirmation Code";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #2c3e50; margin-bottom: 10px;'>Email Confirmation Code</h1>
                        <div style='width: 50px; height: 3px; background-color: #3498db; margin: 0 auto;'></div>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 30px; border-radius: 10px; margin-bottom: 20px;'>
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>Hi {firstName},</h2>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>
                            Thank you for registering with our Inventory Management System! To complete your registration, please use the following confirmation code:
                        </p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <span style='font-size: 24px; font-weight: bold; color: #3498db;'>{confirmationCode}</span>
                        </div>
                        
                        <p style='color: #777; font-size: 14px; line-height: 1.5; margin-top: 25px;'>
                            Please enter this code in the confirmation field on our app to verify your email address.
                        </p>
                    </div>
                    
                    <div style='border-top: 1px solid #eee; padding-top: 20px; margin-top: 30px;'>
                        <p style='color: #555; font-size: 14px; margin-top: 20px;'>
                            Best regards,<br>
                            <strong>The Inventory Management Team</strong>
                        </p>
                    </div>
                </div>
            ";
            await SendEmailAsync([email], subject, body);
        }

        public async Task SendPasswordResetCodeAsync(string email, string firstName, string resetCode)
        {
            var subject = "Password Reset Code";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #e74c3c; margin-bottom: 10px;'>Password Reset Code</h1>
                        <div style='width: 50px; height: 3px; background-color: #e74c3c; margin: 0 auto;'></div>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 30px; border-radius: 10px; margin-bottom: 20px;'>
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>Hello {firstName},</h2>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>
                            You have requested to reset your password.
                        </p>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 25px;'>
                            Use the following code to reset your password:
                        </p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <span style='font-size: 24px; font-weight: bold; color: #e74c3c;'>{resetCode}</span>
                        </div>
                        
                        <p style='color: #777; font-size: 14px; line-height: 1.5; margin-top: 25px;'>
                            Please enter this code in the confirmation field on our app to verify your email address.
                        </p>
                    </div>
                    
                    <div style='border-top: 1px solid #eee; padding-top: 20px; margin-top: 30px;'>
                        <p style='color: #555; font-size: 14px; margin-top: 20px;'>
                            Best regards,<br>
                            <strong>The Inventory Management Team</strong>
                        </p>
                    </div>
                </div>
            ";

            await SendEmailAsync([email], subject, body);
        }

        public async Task SendProductAddedConfirmationAsync(string email, string firstName, Product product)
        {
            var subject = $"Product Added Successfully: {product.Name}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #3498db; margin-bottom: 10px;'>Product Added Successfully</h1>
                        <div style='width: 50px; height: 3px; background-color: #3498db; margin: 0 auto;'></div>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 30px; border-radius: 10px; margin-bottom: 20px;'>
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>Hi {firstName},</h2>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>
                            Great news! The following product has been successfully added to the inventory system:
                        </p>
                        
                        <div style='background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 5px solid #3498db;'>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Product Name:</strong> {product.Name}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Description:</strong> {product.Description}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Price:</strong> <span style='color: #27ae60; font-weight: bold;'>${product.Price:F2}</span></p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Initial Stock Quantity:</strong> {product.StockQuantity}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Supplier:</strong> {product.Supplier}</p>
                        </div>
                        
                        <p style='color: #777; font-size: 14px; line-height: 1.5; margin-top: 25px;'>
                            The product is now available in the inventory system and ready for transactions.
                        </p>
                    </div>
                    
                    <div style='border-top: 1px solid #eee; padding-top: 20px; margin-top: 30px;'>
                        <p style='color: #555; font-size: 14px; margin-top: 20px;'>
                            Best regards,<br>
                            <strong>The Inventory Management Team</strong>
                        </p>
                    </div>
                </div>
            ";

            await SendEmailAsync([email], subject, body);
        }

        public async Task SendTransactionConfirmationAsync(string email, string firstName, Transaction transaction)
        {
            var subject = $"Transaction Confirmation - {transaction.Type}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #27ae60; margin-bottom: 10px;'>Transaction Confirmation</h1>
                        <div style='width: 50px; height: 3px; background-color: #27ae60; margin: 0 auto;'></div>
                    </div>
                    
                    <div style='background-color: #d4edda; padding: 30px; border-radius: 10px; margin-bottom: 20px; border-left: 5px solid #27ae60;'>
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>Hi {firstName},</h2>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>
                            Your transaction has been processed successfully! Here are the details:
                        </p>
                        
                        <div style='background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Transaction ID:</strong> {transaction.Id}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Type:</strong> {transaction.Type}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Quantity:</strong> {transaction.Quantity}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Total Amount:</strong> <span style='color: #27ae60; font-weight: bold;'>${transaction.TotalAmount:F2}</span></p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Date:</strong> {transaction.CreateDate:yyyy-MM-dd HH:mm:ss}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Status:</strong> <span style='color: #27ae60; font-weight: bold;'>{transaction.Status}</span></p>
                        </div>
                        
                        <p style='color: #777; font-size: 14px; line-height: 1.5; margin-top: 25px;'>
                            Thank you for using our inventory management system!
                        </p>
                    </div>
                    
                    <div style='border-top: 1px solid #eee; padding-top: 20px; margin-top: 30px;'>
                        <p style='color: #555; font-size: 14px; margin-top: 20px;'>
                            Best regards,<br>
                            <strong>The Inventory Management Team</strong>
                        </p>
                    </div>
                </div>
            ";

            await SendEmailAsync([email], subject, body);
        }

        public async Task SendLowStockAlertAsync(Product product, int threshold)
        {
            var subject = $"Low Stock Alert: {product.Name}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #f39c12; margin-bottom: 10px;'>Low Stock Alert</h1>
                        <div style='width: 50px; height: 3px; background-color: #f39c12; margin: 0 auto;'></div>
                    </div>
                    
                    <div style='background-color: #fff3cd; padding: 30px; border-radius: 10px; margin-bottom: 20px; border-left: 5px solid #f39c12;'>
                        <h2 style='color: #2c3e50; margin-bottom: 20px;'>Attention Admin/Manager,</h2>
                        <p style='color: #555; font-size: 16px; line-height: 1.6; margin-bottom: 20px;'>
                            The following product is running low on stock and requires immediate attention:
                        </p>
                        
                        <div style='background-color: white; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Product:</strong> {product.Name}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Current Stock:</strong> <span style='color: #e74c3c; font-weight: bold;'>{product.StockQuantity}</span></p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Threshold:</strong> {threshold}</p>
                            <p style='color: #333; font-size: 16px; margin: 10px 0;'><strong>Supplier:</strong> {product.Supplier}</p>
                        </div>
                        
                        <p style='color: #777; font-size: 14px; line-height: 1.5; margin-top: 25px;'>
                            Please consider restocking this item to avoid any disruption in operations.
                        </p>
                    </div>
                    
                    <div style='border-top: 1px solid #eee; padding-top: 20px; margin-top: 30px;'>
                        <p style='color: #555; font-size: 14px; margin-top: 20px;'>
                            Best regards,<br>
                            <strong>The Inventory Management Team</strong>
                        </p>
                    </div>
                </div>
            ";

            var adminAndManagerEmails = await GetAdminAndManagerEmailsAsync();

            if (adminAndManagerEmails.Length != 0)
            {
                await SendEmailAsync(adminAndManagerEmails, subject, body);
            }
        }

        private async Task<string[]> GetAdminAndManagerEmailsAsync()
        {
            var adminAndManagerEmails = await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == RoleNames.Admin || ur.Role.Name == RoleNames.Manager))
                .Select(u => u.Email)
                .Where(e => e != "admin@gmail.com")
                .Distinct()
                .ToArrayAsync();

            return adminAndManagerEmails!;
        }
    }
}
