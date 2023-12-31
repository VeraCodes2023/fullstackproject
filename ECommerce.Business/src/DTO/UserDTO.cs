using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Core;

namespace ECommerce.Business;
public class UserReadDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; } 
    public string Avatar {get;set;}
    public List <AddressReadDTO> Addresses{ get; set; }
}
public class UserCreateDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
    public string Avatar { get; set; }
    public Role Role { get; set; }
    public List <AddressCreateDTO> Addresses{ get; set; }

}

public class UserUpdateDTO
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public List <AddressUpdateDTO> Addresses{ get; set; }
  
}






