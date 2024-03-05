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
        public static string HashedPassword(string password)
        {
            // Convert the password string to byte array
            byte[] passwordBytes = new byte[password.Length * sizeof(char)];
            Buffer.BlockCopy(password.ToCharArray(), 0, passwordBytes, 0, passwordBytes.Length);


            // Create SHA256 hash algorithm instance
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute hash value from the password bytes
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert byte array to a string representation
                // (In a real-world scenario, consider encoding bytes in a way suitable for storage)
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashedPassword;
            }
        }
    }
}
