using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Repository_Layer.Hashing
{
    public class HashingPassword
    {
        private const int Iterations = 10000; 
        private const int SaltSize = 16;
        private const int HashSize = 20;

        public string HashPassword(string password)
        {
            
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHashedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);


            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations); 
            byte[] enteredHash = pbkdf2.GetBytes(HashSize);


            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != enteredHash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
