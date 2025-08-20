using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Commands.DeleteProfileImage
{
    public class DeleteProfileImageHandler(
        ILogger<DeleteProfileImageHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository,
        IFileService _fileService
        ) : IRequestHandler<DeleteProfileImageCommand>
    {
        public async Task Handle(DeleteProfileImageCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("Deleting profile image for user {UserId}", user.Id);

            var dbUser = await _userRepository.GetByIdAsync(user.Id) ?? throw new NotFoundException(message: "User not found.");

            if (string.IsNullOrEmpty(dbUser.ImagePath))
                throw new NotFoundException("No Image Found!");

            _fileService.DeleteFile(dbUser.ImagePath);

            dbUser.ImagePath = null;

            await _userRepository.UpdateAsync(dbUser);
        }
    }
}
