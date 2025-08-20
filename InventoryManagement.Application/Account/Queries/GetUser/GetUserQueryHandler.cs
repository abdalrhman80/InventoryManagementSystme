using AutoMapper;
using InventoryManagement.Application.Account.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Queries.GetUser
{
    public class GetUserQueryHandler(
        ILogger<GetUserQueryHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository,
        IMapper _mapper
        ) : IRequestHandler<GetUserQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbUser = await _userRepository.GetByIdAsync(currentUser.Id)
                ?? throw new NotFoundException(message: $"User with ID {currentUser.Id} not found.");

            _logger.LogInformation("User with ID {UserId} requested his profile information.", dbUser.Id);

            var user = _mapper.Map<UserDto>(dbUser);
            user.Roles = [.. currentUser.Roles];

            return user;
        }
    }
}
