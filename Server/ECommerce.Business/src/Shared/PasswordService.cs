using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business;
public class PasswordService
{
     private const int SaltSize = 128 / 8;
     private const int KeySize = 256 / 8;
     private const int Iteration = 10000;
     private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
     private static char Delimiter = ';';

     public string HashPassword(string password)
     {
          var salt = RandomNumberGenerator.GetBytes(SaltSize);
          var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iteration, _hashAlgorithmName, KeySize);
          return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
     }

   public bool VerifyPassword(string hashedPassword, string userInputPassword)
   {
     var elements = hashedPassword.Split(Delimiter);
     var salt = Convert.FromBase64String(elements[0]);
     var hash = Convert.FromBase64String(elements[1]);

     var hashInput = Rfc2898DeriveBytes.Pbkdf2(userInputPassword, salt, Iteration, _hashAlgorithmName, KeySize);
     return CryptographicOperations.FixedTimeEquals(hash, hashInput);
  }
}