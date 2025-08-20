using AutoMapper;
using InventoryManagement.Application.Transactions.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Transactions.Query.GetAllTransactions
{
    public class GetAllTransactionsQueryHandler(
        ILogger<GetAllTransactionsQueryHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllTransactionsQuery, PaginationResponse<TransactionDto>>
    {
        public async Task<PaginationResponse<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is retrieving transaction with parameters {@Params}", currentUser.Id, request);

            var transactionSpec = new TransactionSpecifications(
                request.PageSize, request.PageNumber, request.SortBy, request.SortDirection, request.TransactionType, request.StartDate, request.EndDate);

            var dbTransactions = await unitOfWork.TransactionRepository.GetAllWithSpecificationAsync(transactionSpec);

            if (dbTransactions == null || !dbTransactions.Any())
                return new PaginationResponse<TransactionDto>([], 0, request.PageNumber, request.PageSize);

            var transactions = mapper.Map<IReadOnlyList<TransactionDto>>(dbTransactions);

            var paginationResponse = new PaginationResponse<TransactionDto>(transactions, dbTransactions.Count, request.PageNumber, request.PageSize);

            return paginationResponse;
        }
    }
}
