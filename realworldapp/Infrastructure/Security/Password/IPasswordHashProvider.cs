namespace realworldapp.Infrastructure.Security.Password
{
    public interface IPasswordHashProvider
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
