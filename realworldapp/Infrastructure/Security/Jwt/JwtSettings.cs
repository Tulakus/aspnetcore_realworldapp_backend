namespace realworldapp.Infrastructure.Security.Jwt
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string SecretCode { get; set; }
        public double TokenExpiration { get; set; }
    }
}
