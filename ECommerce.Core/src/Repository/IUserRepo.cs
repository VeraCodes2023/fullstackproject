using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceCore;

namespace Core;
public interface IUserRepo
{
    IEnumerable<User> GetAll(UserQueryParameters options);// admin auth
    User GetById(Guid id); // admin auth
    User getByEmail(string email);
    User GetUserProfile(Guid id);
    User UpdateUserProfile(Guid userId, User user); 
    User Update(Guid userId, User user); // admin auth
    bool Delete(Guid id); // admin auth
    bool CheckEmailExist(string email);
    User Register(User user); // public 
    User? Login(string email);
    string GenerateToken(User user);
    bool UnregisterUser(Guid userId);// customer auth
}
