using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Musync.Application.Contracts.Identity;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Application.Models.Identity;
using Musync.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Musync.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }
        public Task<AuthResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> Register(RegistrationRequest request)
        {
            ApplicationUser user = GenerateApplicationUser(request);

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded) return GenerateAuthResponse(user);

            throw new BadRequestException("Error creating user", result.Errors);
        }

        private JwtSecurityToken GenerateAccessToken(ApplicationUser user)
        {
            
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id.ToString())
            };

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );
            return jwtSecurityToken;
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private int CalculateAge(DateOnly bornDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - bornDate.Year;

            if (bornDate > today.AddYears(-age)) age--;

            return age;
        }

        private AuthResponse GenerateAuthResponse(ApplicationUser user)
        {
            JwtSecurityToken accessToken = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken();
            UserDTO userDTO = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Age = CalculateAge(user.BornDate),
            };
            return new AuthResponse(
                userDTO, new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken
            );
        }

        private ApplicationUser GenerateApplicationUser(RegistrationRequest request)
        {
            return new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                BornDate = request.BornDate,
                Email = request.Email,
                ProfilePicture = "profile-pictures/default.jpg"
            };
        }
    }
}
