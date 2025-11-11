using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Musync.Application.Contracts.Identity;
using Musync.Application.Models.Identity;
using Musync.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Musync.Application.Providers
{
    public sealed class TokenProvider : ITokenProvider
    {
        private readonly JwtSettings _jwtSettings;

        public TokenProvider(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public JwtSecurityToken GenerateAccessToken(ApplicationUser user)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
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

        public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
