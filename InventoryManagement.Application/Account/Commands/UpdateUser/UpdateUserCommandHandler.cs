using AutoMapper;
using InventoryManagement.Application.Account.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        ILogger<UpdateUserCommandHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository,
        IMapper _mapper
        ) : IRequestHandler<UpdateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbUser = await _userRepository.GetByIdAsync(currentUser.Id)
                ?? throw new NotFoundException(message: $"User with ID {currentUser.Id} not found.");

            _logger.LogInformation("Updating user: {UserId}, with {@Request}", dbUser.Id, request);

            dbUser.FirstName = request.FirstName ?? dbUser.FirstName;
            dbUser.LastName = request.LastName ?? dbUser.LastName;
            dbUser.Email = request.Email ?? dbUser.Email;
            dbUser.PhoneNumber = request.PhoneNumber ?? dbUser.PhoneNumber;
            dbUser.Address = request.Address ?? dbUser.Address;

            await _userRepository.UpdateAsync(dbUser);

            var user = _mapper.Map<UserDto>(dbUser);
            user.Roles = [.. currentUser.Roles];

            return user;
        }
    }
}
