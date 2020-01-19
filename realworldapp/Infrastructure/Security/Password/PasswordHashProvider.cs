﻿namespace realworldapp.Infrastructure.Security.Password
{
    class PasswordHashProvider : IPasswordHashProvider
    {
        private const int WorkFactor = 15;
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
