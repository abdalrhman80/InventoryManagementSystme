using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventoryManagement.Infrastructure.Services
{
    internal class AuthService(
        UserManager<User> _userManager,
        IOptions<JwtOptions> jwtOptions,
        IEmailService _emailService
        ) : IAuthService
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public async Task<JwtSecurityToken> GenerateAccessTokenAsync(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            var rolesClaims = new List<Claim>();

            foreach (var role in roles)
                rolesClaims.Add(new Claim(JwtClaimsTypes.Role, role));

            var claims = new Claim[]
            {
                new(JwtClaimsTypes.UserId, user.Id),
                new(JwtClaimsTypes.Email, user.Email!),
                new(JwtClaimsTypes.UserName, user.UserName!),
                new(JwtClaimsTypes.FullName, $"{user.FirstName} {user.LastName}")
            }
            .Union(userClaims)
            .Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
                (
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.ExpirationInMinutes),
                signingCredentials: singingCredentials
                );

            return jwtSecurityToken;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber).Replace('+', '%'),
                ExpirationOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
            };
        }

        public async Task SendCodeConfirmationEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = new Random().Next(100000, 999999).ToString();

            user.EmailConfirmationToken = token;
            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiresAt = DateTime.UtcNow.AddHours(24);
            await _userManager.UpdateAsync(user);

            await _emailService.SendEmailConfirmationCodeAsync(user.Email!, user.FirstName, code);
        }

        public async Task SendPasswordResetCodeAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var code = new Random().Next(100000, 999999).ToString();

            user.PasswordResetCode = code;
            user.PasswordResetToken = token;
            user.PasswordResetCodeExpiresAt = DateTime.UtcNow.AddHours(1);
            await _userManager.UpdateAsync(user);

            await _emailService.SendPasswordResetCodeAsync(user.Email!, user.FirstName, code);
        }

    }
}
