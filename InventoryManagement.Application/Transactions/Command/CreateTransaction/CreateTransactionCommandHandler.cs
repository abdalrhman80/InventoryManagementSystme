using AutoMapper;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Transactions.Command.CreateTransaction
{
    public class CreateTransactionCommandHandler(
        ILogger<CreateTransactionCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmailService emailService
        ) : IRequestHandler<CreateTransactionCommand, int>
    {
        public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var currentStaffUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is creating a transaction: {@Transaction}", currentStaffUser.Id, request);

            var existingProduct = await unitOfWork.ProductRepository.GetByIdAsync(request.ProductId)
                ?? throw new NotFoundException("No product found.");

            var transactionId = await ExecuteTransactionAsync(request, currentStaffUser, existingProduct);

            await CheckLowStockAlertsAsync(existingProduct);

            return transactionId;
        }


        private async Task<int> ExecuteTransactionAsync(CreateTransactionCommand request, CurrentUser currentStaffUser, Product existingProduct)
        {
            EnsureTransactionIsValid(request, existingProduct);

            await unitOfWork.BeginTransactionAsync();

            try
            {
                ChangeStockQuantity(request, existingProduct);

                var transaction = AddNewTransactionField(request, existingProduct, currentStaffUser.Id);

                unitOfWork.ProductRepository.Update(existingProduct);
                await unitOfWork.Repository<Transaction>().AddAsync(transaction);
                await unitOfWork.SaveChangesAsync();
                await unitOfWork.CommitTransactionAsync();

                // await emailService.SendTransactionConfirmationAsync(currentStaffUser.Email, currentStaffUser.UserName, transaction);

                return transaction.Id;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error ({ExceptionType}) occurred while creating transaction.", ex.GetType().Name);
                throw;
            }
        }

        private static void EnsureTransactionIsValid(CreateTransactionCommand request, Product existingProduct)
        {
            if (!Enum.IsDefined(typeof(TransactionType), request.TransactionType))
                throw new BadRequestException("Invalid transaction type.");

            if (request.Quantity <= 0)
                throw new BadRequestException("Quantity must be greater than zero for transactions.");

            if (request.TransactionType == TransactionType.Sale)
            {
                if (request.Quantity > existingProduct.StockQuantity)
                    throw new BadRequestException("Insufficient stock for the transaction.");
            }
        }

        private static void ChangeStockQuantity(CreateTransactionCommand request, Product product)
        {
            if (request.TransactionType == TransactionType.Sale)
            {
                product.StockQuantity -= request.Quantity;
            }
            else if (request.TransactionType == TransactionType.Purchase)
            {
                product.StockQuantity += request.Quantity;
            }
        }

        private Transaction AddNewTransactionField(CreateTransactionCommand request, Product product, string userId)
        {
            var transaction = mapper.Map<Transaction>(request);
            transaction.CreatedBy = userId;
            transaction.Status = Statuses.Success;
            transaction.TotalAmount = request.Quantity * product.Price;

            return transaction;
        }

        private async Task CheckLowStockAlertsAsync(Product product)
        {
            var alertSpec = new LowStockAlertSpecification(productId: product.Id);
            var alerts = await unitOfWork.Repository<LowStockAlert>().GetAllWithSpecificationAsync(alertSpec);

            foreach (var alert in alerts)
            {
                if (product.StockQuantity <= alert.Threshold)
                {
                    var shouldSendAlert = !alert.AlertSent ||
                                          alert.LastAlertSent == null ||
                                          alert.LastAlertSent < DateTime.UtcNow.AddHours(-6); // Wait 6 hours between alerts

                    if (shouldSendAlert)
                    {
                        await emailService.SendLowStockAlertAsync(product, alert.Threshold);
                        await MarkAlertAsSentAsync(alert);

                        logger.LogInformation("Low stock alert sent for product {ProductId} ({ProductName}). Current stock: {CurrentStock}, Threshold: {Threshold}",
                            product.Id, product.Name, product.StockQuantity, alert.Threshold);
                    }
                }
            }
        }

        private async Task MarkAlertAsSentAsync(LowStockAlert alert)
        {
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
            alert.AlertSent = true;
            alert.SentAt ??= currentTime;
            alert.LastAlertSent = currentTime;
            unitOfWork.Repository<LowStockAlert>().Update(alert);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
