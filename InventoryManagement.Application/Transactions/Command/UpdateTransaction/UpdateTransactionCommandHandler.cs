using AutoMapper;
using InventoryManagement.Application.Transactions.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Transactions.Command.UpdateTransaction
{
    public class UpdateTransactionCommandHandler(
        ILogger<UpdateTransactionCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<UpdateTransactionCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is updating transaction {TransactionId}", currentUser.Id, request.Id);

            var transactionSpec = new TransactionSpecifications(request.Id);
            var transaction = await unitOfWork.TransactionRepository.GetEntityWithSpecificationAsync(transactionSpec)
                            ?? throw new NotFoundException($"Transaction with ID {request.Id} not found.");

            var result = await ExecuteTransactionAsync(request, transaction);

            return mapper.Map<TransactionDto>(result);
        }


        private async Task<Transaction> ExecuteTransactionAsync(UpdateTransactionCommand request, Transaction transaction)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                ValidateTransactionForUpdate(transaction);

                var newQuantity = request.NewQuantity ?? transaction.Quantity;
                var newType = request.TransactionType ?? transaction.Type;

                ValidateStockAvailability(transaction, transaction.Quantity, transaction.Type, newQuantity, newType);

                RevertOriginalTransactionStock(transaction.Type, transaction.Quantity, transaction.Product);

                UpdateTransactionProperties(request, transaction);

                ApplyNewTransactionStock(transaction);

                await SaveChangesAsync(transaction);

                return transaction;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error updating transaction {TransactionId}: {ErrorMessage}", request.Id, ex.Message);
                throw;
            }
        }

        private static void ValidateTransactionForUpdate(Transaction transaction)
        {
            var nonUpdatableStatuses = new[] { Statuses.Voided, Statuses.Rejected };
            if (nonUpdatableStatuses.Contains(transaction.Status))
            {
                throw new InvalidOperationException(
                    $"Cannot update transaction {transaction.Id} with status: {transaction.Status}");
            }
        }

        private static void ValidateStockAvailability(Transaction transaction, int originalQuantity, TransactionType originalType, int newQuantity, TransactionType newType)
        {
            if (newType == TransactionType.Sale)
            {
                var stockAfterRevert = originalType switch
                {
                    TransactionType.Sale => transaction.Product.StockQuantity + originalQuantity,
                    TransactionType.Purchase => transaction.Product.StockQuantity - originalQuantity,
                    _ => transaction.Product.StockQuantity
                };

                if (stockAfterRevert < newQuantity)
                {
                    throw new BadRequestException($"Insufficient stock for sale. Available after update: {stockAfterRevert}, Required: {newQuantity}");
                }
            }
        }

        private static void RevertOriginalTransactionStock(TransactionType originalType, int originalQuantity, Product product)
        {
            switch (originalType)
            {
                case TransactionType.Sale:
                    product.StockQuantity += originalQuantity;
                    break;

                case TransactionType.Purchase:
                    product.StockQuantity -= originalQuantity;
                    break;

                default:
                    throw new BadRequestException($"Unknown transaction type: {originalType}");
            }
        }

        private static void UpdateTransactionProperties(UpdateTransactionCommand request, Transaction transaction)
        {
            if (request.TransactionType.HasValue)
            {
                transaction.Type = request.TransactionType.Value;
            }

            if (request.NewQuantity.HasValue)
            {
                transaction.Quantity = request.NewQuantity.Value;
            }

            // Recalculate total amount - preserve original unit price if possible
            var originalUnitPrice = transaction.TotalAmount / Math.Max(1, transaction.Quantity);
            transaction.TotalAmount = transaction.Quantity * originalUnitPrice;

            transaction.UpdateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
        }

        private static void ApplyNewTransactionStock(Transaction transaction)
        {
            switch (transaction.Type)
            {
                case TransactionType.Sale:
                    transaction.Product.StockQuantity -= transaction.Quantity;
                    break;

                case TransactionType.Purchase:
                    transaction.Product.StockQuantity += transaction.Quantity;
                    break;

                default:
                    break;
            }
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

