using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace realworldapp.Infrastructure.Security.JWT
{
    public interface IJwt
    {
        bool IsTokenValid(string token);
        string GenerateToken(IEnumerable<Claim> claim);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
