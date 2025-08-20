using AutoMapper;
using InventoryManagement.Application.Account.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandHandler(
        ILogger<UploadProfileImageCommandHandler> _logger,
        IUserRepository _userRepository,
        IUserContext _userContext,
        IFileService _fileService,
        IMapper _mapper
        ) : IRequestHandler<UploadProfileImageCommand, UserDto>
    {
        public async Task<UserDto> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} is uploading a profile image", currentUser.Id);

            if (request.Image is null)
                throw new BadRequestException("Image is required.");

            if (!FileSettings.AllowedExtensions.Contains(Path.GetExtension(request.Image.FileName).ToLower()))
                throw new BadRequestException($"Allowed extensions are: {string.Join(", ", FileSettings.AllowedExtensions)}");

            if (request.Image.Length > FileSettings.MaxFileSizeInBytes)
                throw new BadRequestException($"File size must not exceed {FileSettings.MaxFileSizeInBytes / 1024 / 1024} MB.");

            var dbUser = await _userRepository.GetByIdAsync(currentUser.Id)
                ?? throw new NotFoundException(message: $"User with ID {currentUser.Id} not found.");

            // Delete old image for current user if exist
            if (!string.IsNullOrEmpty(dbUser.ImagePath))
            {
                _fileService.DeleteFile(dbUser.ImagePath);
            }

            var imagePath = await _fileService.UploadFileAsync(request.Image, FileSettings.ProfilesFolderPath);

            dbUser.ImagePath = imagePath;

            await _userRepository.UpdateAsync(dbUser);

            var user = _mapper.Map<UserDto>(dbUser);
            user.Roles = [.. currentUser.Roles];

            return user;
        }
    }
}
