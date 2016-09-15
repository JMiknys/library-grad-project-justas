using LibraryGradProject.Context;
using LibraryGradProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Data.Entity;

namespace LibraryGradProject.Repos
{
    public class UserRepository
    {
        public void Add(User entity)
        {
            using (var context = new LibraryContext())
            {
                // Generate salt
                var saltBytes = new byte[32];
                using (var random = new RNGCryptoServiceProvider())
                {
                    random.GetNonZeroBytes(saltBytes);
                }

                entity.Password = computeHash(entity.Password, saltBytes);
                context.Users.Add(entity);
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var context = new LibraryContext())
            {
                return context.Users.ToList();
            }
        }

        public User Get(int id)
        {
            using (var context = new LibraryContext())
            {
                User u = context.Users.Where(user => user.Id == id).SingleOrDefault();
                if (u == null)
                {
                    throw new ArgumentException("User not found.");
                }
                return u;
            }
        }

        public User GetByName(string name)
        {
            using (var context = new LibraryContext())
            {
                return context.Users.Where(user => user.Name == name).SingleOrDefault();
            }
        }

        public bool verifyUser(User user)
        {
            using (var context = new LibraryContext())
            {
                User userRecord = context.Users.Where(usr => usr.Name == user.Name).SingleOrDefault();
                if (userRecord == null)
                {
                    return false;
                }

                // Convert base64-encoded hash value into a byte array.
                byte[] hashWithSaltBytes = Convert.FromBase64String(userRecord.Password);

                // We must know size of hash (without salt)
                int hashSizeInBits = 256;
                int hashSizeInBytes = hashSizeInBits / 8;

                // Make sure that the specified hash value is long enough.
                if (hashWithSaltBytes.Length < hashSizeInBytes)
                    return false;

                // Allocate array to hold original salt bytes retrieved from hash.
                byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                            hashSizeInBytes];

                // Copy salt from the end of the hash to the new array.
                for (int i = 0; i < saltBytes.Length; i++)
                    saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

                // Compute a new hash string.
                string expectedHashString = computeHash(user.Password, saltBytes);

                return (userRecord.Password == expectedHashString);
            }            
        }

        public String computeHash(String input, byte[] saltBytes)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(input);

            byte[] textWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                textWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                textWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            HashAlgorithm hash = new SHA256Managed();
            byte[] hashBytes = hash.ComputeHash(textWithSaltBytes);

            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            return Convert.ToBase64String(hashWithSaltBytes);
        }
        
        public byte[] getRandomBytes (int byteSize)
        {
            // Generate salt
            var saltBytes = new byte[byteSize];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(saltBytes);
            }
            return saltBytes;
        }
    }
}