using AutoMapper;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Auth.Commands.Register
{
    public class RegisterCommandHandler(
        ILogger<RegisterCommandHandler> _logger,
        UserManager<User> _userManager,
        IRoleRepository _roleRepository,
        IMapper _mapper,
        IAuthService _authService
        ) : IRequestHandler<RegisterCommand, string>
    {
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                throw new BadRequestException("Email Is Already Registered");

            if (await _userManager.FindByNameAsync(request.UserName) is not null)
                throw new BadRequestException("UserName Is Already Registered");

            var user = _mapper.Map<User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Aggregate(string.Empty, (current, error) => current + $"{error.Description}, ", errors => errors.TrimEnd(',', ' ')));

            await _roleRepository.AddUserToRoleAsync(user, RoleNames.Staff);

            await _authService.SendCodeConfirmationEmailAsync(user);

            _logger.LogInformation("User {UserId} registered successfully with email {Email}", user.Id, user.Email);

            return "Registration successful. Please check your email to confirm your account.";
        }
    }
}
