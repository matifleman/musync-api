using Musync.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Musync.Application.Contracts.Identity
{
    public interface ITokenProvider
    {
        JwtSecurityToken GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
