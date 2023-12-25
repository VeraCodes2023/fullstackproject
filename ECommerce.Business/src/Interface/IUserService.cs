using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
namespace ECommerce.Business;
public interface IUserService
{
    IEnumerable <UserReadDTO> GetAllUsers(UserQueryParameters options); //Admin auth
    UserReadDTO GetUserById(Guid id); //Admin auth
    UserReadDTO GetUserByEmail(string email); //Admin auth
    UserReadDTO GetUserProfile(Guid id);
    UserReadDTO UpdateUserProfile(Guid userId,UserUpdateDTO updatedProfile); //loggedin customer/ 
    UserReadDTO UpdateUser(Guid userId,UserUpdateDTO userUpdateDto);//Admin auth
    bool DeleteUser(Guid id);//Admin auth
    UserReadDTO Register(UserCreateDTO userCreateDto);//public 
    string Login(Credential credential);
    bool Unregister(Guid userId); //loggedin customer
}






