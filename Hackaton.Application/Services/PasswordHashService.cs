using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Hackaton.Application.Services
{
    public class PasswordHashService
    {
        // Gera um salt aleatório para uso no hash da senha
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8]; // 16 bytes = 128 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Gera um hash da senha usando PBKDF2 com HMAC-SHA256
        public string HashPassword(string password, byte[] salt)
        {
            // Deriva uma chave de 256 bits (32 bytes) da senha usando PBKDF2 com 10,000 iterações
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        // Verifica se a senha fornecida corresponde ao hash armazenado
        public bool VerifyPassword(string providedPassword, string storedHash, byte[]? storedSalt)
        {
            // Se não houver salt, faz comparação direta (para compatibilidade com senhas antigas)
            if (storedSalt == null || storedSalt.Length == 0)
            {
                return providedPassword == storedHash;
            }

            // Se houver salt, usa o método de hash
            string hashedProvidedPassword = HashPassword(providedPassword, storedSalt);
            return hashedProvidedPassword == storedHash;
        }
    }
}