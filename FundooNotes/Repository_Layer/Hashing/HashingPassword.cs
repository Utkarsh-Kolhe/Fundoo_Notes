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
        private const int Iterations = 10000; // Adjust based on your security requirements
        private const int SaltSize = 16; // Choose an appropriate size for your salt

        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create the hash with the password and salt
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(20); // 20 is the size of the hash

            // Combine the salt and hash and convert to Base64 for storage
            byte[] hashBytes = new byte[SaltSize + 20];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, 20);
            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            // Extract the salt from the stored hashed password
            byte[] hashBytes = Convert.FromBase64String(storedHashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Hash the entered password using the stored salt
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000); // Use the same parameters as during hashing
            byte[] enteredHash = pbkdf2.GetBytes(20);

            // Compare the hashes
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != enteredHash[i])
                    return false;
            }

            return true;
        }
    }
}
