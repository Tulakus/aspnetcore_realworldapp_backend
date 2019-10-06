using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace realworldapp.Infrastructure.Security
{
    public interface IPasswordHashProvider
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
