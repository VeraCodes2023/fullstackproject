using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public interface IPasswordHashRepo
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string userInputPassword);

}
