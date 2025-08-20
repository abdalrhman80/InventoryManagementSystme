using AutoMapper;
using InventoryManagement.Application.Transactions.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Transactions.Query.GetTransaction
{
    public class GetTransactionQueryHandler(
        ILogger<GetTransactionQueryHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetTransactionQuery, TransactionDto>
    {
        public async Task<TransactionDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is retrieving transaction with ID {TransactionId}", currentUser.Id, request.Id);

            var transactionSpec = new TransactionSpecifications(request.Id);

            var transaction = await unitOfWork.TransactionRepository.GetEntityWithSpecificationAsync(transactionSpec)
                ?? throw new NotFoundException("Transaction not found.");

            var transactionDto = mapper.Map<TransactionDto>(transaction);

            return transactionDto;
        }
    }
}
