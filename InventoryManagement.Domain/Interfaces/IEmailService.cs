using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string[] recipients, string subject, string body);
        Task SendEmailConfirmationCodeAsync(string email, string firstName, string confirmationCode);
        Task SendPasswordResetCodeAsync(string email, string firstName, string resetCode);
        Task SendProductAddedConfirmationAsync(string email, string firstName, Product product);
        Task SendTransactionConfirmationAsync(string email, string firstName, Transaction transaction);
        Task SendLowStockAlertAsync(Product product, int threshold);
    }
}
