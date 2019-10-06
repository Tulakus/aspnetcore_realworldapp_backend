using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace realworldapp.Infrastructure.Security.JWT
{
    public class Jwt: IJwt
    {
        private double _tokenExpirationTime = 7 * 24 * 60;//todo read from config file
        private string _securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
        private string _secretKey = "ReallySecretCode"; //todo find better place
        private string _issuer = "muj.pokus.cz"; //todo read from config
        private readonly JwtSecurityTokenHandler _securityHandler;

        public Jwt()
        {
            _securityHandler = new JwtSecurityTokenHandler();
        }
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Invalid Jwt token. Token cannot be empty");

            if (!_securityHandler.CanReadToken(token))
                return false;
            try
            {
                _securityHandler.ValidateToken(token, GetTokenValidationParameters(), out var validatedToken);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string GenerateToken(IEnumerable<Claim> claim)
        {
            var securityKey = GetSymmetricSecurityKey();
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claim),
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(securityKey, _securityAlgorithm),
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationTime)
            };

            var securityToken = _securityHandler.CreateJwtSecurityToken(descriptor);
            return _securityHandler.WriteToken(securityToken);
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if(!IsTokenValid(token))
                throw new ArgumentException("Invalid JWT token");
            
            try
            {
                var jwtToken = _securityHandler.ReadJwtToken(token);

                return jwtToken.Claims;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid JWT token");
            }
        }

        SecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }

        TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters();
        }
    }
}
