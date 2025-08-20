using AutoMapper;
using InventoryManagement.Application.Account.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler(
        ILogger<GetAllUsersQueryHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository,
        IMapper _mapper
        ) : IRequestHandler<GetAllUsersQuery, PaginationResponse<UserDto>>
    {
        public async Task<PaginationResponse<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var adminUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("Admin with ID {AdminId} requested to get users with parameters {@Params}.", adminUser.Id, request);

            var userSpec = new UserSpecifications(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);

            var dbUsers = await _userRepository.GetAllWithSpecificationAsync(userSpec);

            if (dbUsers == null || !dbUsers.Any())
                return new PaginationResponse<UserDto>([], 0, request.PageSize, request.PageNumber);

            var users = _mapper.Map<IReadOnlyList<UserDto>>(dbUsers.Where(u => u.Id != adminUser.Id));

            var paginationResponse = new PaginationResponse<UserDto>(users, users.Count, request.PageSize, request.PageNumber);

            return paginationResponse;
        }
    }
}
