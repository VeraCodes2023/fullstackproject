using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Core;
using ECommerce.Business;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceWebAPI;
public class UserRepo : IUserRepo
{
    protected DbSet<User> _users;
    private DatabaseContext _database;
     private IConfiguration _config;
    public UserRepo(DatabaseContext database,IConfiguration config)
    {
        _users = database.Users;
        _database = database;
        _config= config;
    }
    public bool Delete(Guid id)
    {
        var user = _users.Find(id);
        if (user != null)
        {
            _users.Remove(user);
            _database.SaveChanges();
            return true;
        }
        return false;
    }

    public IEnumerable<User> GetAll(UserQueryParameters options)
    {
        return _users.AsNoTracking()
        .Include(u=>u.Addresses)
        .Where(u => u.Name.Contains(options.Search))
        .Skip(0)
        .Take(200);
    }

    public User Register(User user)
    {
        if (user.Addresses != null && user.Addresses.Any())
        {
            foreach (var address in user.Addresses)
            {
                address.UserId = user.Id; 
            }
        }
        _users.Add(user);
        _database.SaveChanges();
        return user;
    }
    public User GetByEmail(string email)
    {
        var foundUser = _users.Include(u=>u.Addresses).FirstOrDefault(p=>p.Email ==email);
        return foundUser!;
    }
    public User GetById(Guid id)
    {
        var foundUser = _users.Include(u=>u.Addresses).FirstOrDefault(p=>p.Id ==id);
        return foundUser!;
    }
    public User Update(Guid userId, User user)
    {
        var existingUser = _users.Find(userId);
        existingUser!.Name = user.Name;
        existingUser.Avatar = user.Avatar;
        existingUser.Email = user.Email;
        _users.Update(existingUser);
        _database.SaveChanges();
        return existingUser;
    }

    public User GetUserProfile(Guid id)
    {
        var foundUser = _users.Include(u=>u.Addresses).FirstOrDefault(p=>p.Id ==id);
        return foundUser!;
    }


    public bool UnregisterUser(Guid userId)
    {
        var foundUser = _users.Find(userId);
        _users.Remove(foundUser!);
        return true;
    }

    public User UpdateUserProfile(Guid userId, User user)
    {
        var targetUser = _users.Find(userId);
        targetUser!.Name = user.Name;
        targetUser.Avatar = user.Avatar;
        targetUser.Email = user.Email;
        targetUser.Addresses = user.Addresses;

        _users.Update(targetUser);
        _database.SaveChanges();
        return targetUser;
    }

    public bool CheckEmailExist(string email)
    {
        var result= _users.FirstOrDefault(u=>u.Email ==email);
        if (result != null)
        {
            return true;
        }
        return false;
    }

    public User? Login(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public string GenerateToken(User user)
    {
        var issuer = _config.GetSection("Jwt:Issuer").Value;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        var audience = _config.GetSection("Jwt:Audience").Value;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:key").Value!));
        var signingKey = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience =audience,
            Expires = DateTime.Now.AddDays(2),
            Subject =new ClaimsIdentity(claims),
            SigningCredentials = signingKey,

        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);

    }

  
}
