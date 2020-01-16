using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace realworldapp.Infrastructure.Security.Jwt
{
    public class Jwt : IJwt
    {
        private readonly double _tokenExpirationTime; private string _securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;
        private readonly string _secretCode; 
        private readonly string _issuer;
        private readonly JwtSecurityTokenHandler _securityHandler;

        public Jwt(IOptions<JwtSettings> settings)
        {
            _securityHandler = new JwtSecurityTokenHandler();
            _issuer = settings.Value.Issuer;
            _secretCode = settings.Value.SecretCode;
            _tokenExpirationTime = settings.Value.TokenExpiration;

        }
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Invalid Jwt token. Token cannot be empty");

            if (!_securityHandler.CanReadToken(token))
                return false;
            
            try
            {
                _securityHandler.ValidateToken(token, GetTokenValidationParameters(), out _);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string GenerateToken(IEnumerable<Claim> claim)
        {
            var securityKey = GetSymmetricSecurityKey(_secretCode);
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
            if (!IsTokenValid(token))
            {
                throw new ArgumentException("Invalid JWT token");
            }

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

        public static SecurityKey GetSymmetricSecurityKey(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters();
        }
    }
}
