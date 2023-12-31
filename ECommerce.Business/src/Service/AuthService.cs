using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Business;
public class AuthService:IAuthService
{
    private readonly IUserRepo _userRepo;
    private ITokenService _tokenService;
    public AuthService(IUserRepo userRepo, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService= tokenService;
    }
   
    public string Login(Credential credential)
    {
        var foundByEmal = _userRepo.GetByEmail(credential.Email);
        if(foundByEmal !=null)
        {
            var isPasswordMtach = new PasswordService().VerifyPassword(foundByEmal.Password,credential.Password);
            if(isPasswordMtach)
            {
                return _tokenService.GenerateToken(foundByEmal);
            }
        }
        
        throw new Exception("Unauthorized User");
    }
    
}
