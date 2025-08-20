using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Transactions.Command.CancelTransaction
{
    public class CancelTransactionCommandHandler(
        ILogger<CancelTransactionCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<CancelTransactionCommand>
    {
        public async Task Handle(CancelTransactionCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is is attempting to cancel transaction {TransactionId}", currentUser.Id, request.Id);

            var transactionSpec = new TransactionSpecifications(request.Id);
            var transaction = await unitOfWork.TransactionRepository.GetEntityWithSpecificationAsync(transactionSpec)
                            ?? throw new NotFoundException($"Transaction with ID {request.Id} not found.");

            await CancelTransactionAsync(transaction, request.Reason, currentUser.Id);
        }

        private async Task CancelTransactionAsync(Transaction transaction, string? reason, string userId)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                ValidateTransactionForCancellation(transaction);
                ApplyTransactionCancellation(transaction);
                UpdateTransactionMetadata(transaction, reason, userId);
                await SaveChangesAsync(transaction);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error cancelling transaction {TransactionId}: {ErrorMessage}", transaction.Id, ex.Message);
                throw;
            }
        }

        private static void ValidateTransactionForCancellation(Transaction transaction)
        {
            if (transaction.Status == Statuses.Voided || transaction.Status == Statuses.Rejected)
            {
                throw new BadRequestException("Transaction is already cancelled.");
            }

            var cancellableStatuses = new[] { Statuses.Pending, Statuses.Success, Statuses.Processed };

            if (!cancellableStatuses.Contains(transaction.Status))
            {
                throw new InvalidOperationException($"Transaction {transaction.Id} cannot be cancelled. Current status: {transaction.Status}");
            }

            if (transaction.Quantity <= 0)
            {
                throw new BadRequestException($"Transaction {transaction.Id} has invalid quantity: {transaction.Quantity}");
            }
        }

        private static void ApplyTransactionCancellation(Transaction transaction)
        {
            switch (transaction.Type)
            {
                case TransactionType.Sale:
                    transaction.Status = Statuses.Voided;
                    transaction.Product.StockQuantity += transaction.Quantity;
                    break;

                case TransactionType.Purchase:
                    var originalPurchaseStatus = transaction.Status;
                    transaction.Status = Statuses.Rejected;
                    if (originalPurchaseStatus == Statuses.Success || originalPurchaseStatus == Statuses.Processed)
                    {
                        if (transaction.Product.StockQuantity < transaction.Quantity)
                        {
                            throw new InvalidOperationException(
                                $"Cannot reject purchase transaction {transaction.Id}. " +
                                $"Insufficient stock: {transaction.Product.StockQuantity} available, " +
                                $"{transaction.Quantity} required for rejection.");
                        }

                        transaction.Product.StockQuantity -= transaction.Quantity;
                    }
                    break;

                default:
                    throw new BadRequestException($"Unknown transaction type: {transaction.Type}");
            }
        }

        private static void UpdateTransactionMetadata(Transaction transaction, string? reason, string userId)
        {
            transaction.CancellationReason = reason;
            transaction.CancelledBy = userId;
            transaction.CancelledDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
        }

        private async Task SaveChangesAsync(Transaction transaction)
        {
            unitOfWork.Repository<Product>().Update(transaction.Product);
            unitOfWork.Repository<Transaction>().Update(transaction);
            await unitOfWork.CommitTransactionAsync();
            await unitOfWork.SaveChangesAsync();
        }
    }
}
