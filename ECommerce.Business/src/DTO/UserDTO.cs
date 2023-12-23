using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerceBusiness;
public class UserReadDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List <AddressReadDTO> AddressReadDTOs{ get; set; }

}
public class UserCreateDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
    public string Avatar { get; set; }
    public Role Role { get; set; }
    public List <AddressCreateDTO> AddressCreateDTOs{ get; set; }

}

public class UserUpdateDTO
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public List <AddressUpdateDTO> AddressUpdateDTOs{ get; set; }
}



