using System.Collections.Generic;
using System.Security.Claims;

namespace realworldapp.Infrastructure.Security.Jwt
{
    public interface IJwt
    {
        bool IsTokenValid(string token);
        string GenerateToken(IEnumerable<Claim> claim);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
