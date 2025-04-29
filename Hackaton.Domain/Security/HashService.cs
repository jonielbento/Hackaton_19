using System;
using System.Security.Cryptography;
using System.Text;

namespace Hackaton.Domain.Security
{
    public class HashService
    {
        // Método para criptografar a senha
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // Método para verificar se a senha está correta
        public bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedPasswordToCheck = HashPassword(password);
            return hashedPasswordToCheck == hashedPassword;
        }
    }
} 