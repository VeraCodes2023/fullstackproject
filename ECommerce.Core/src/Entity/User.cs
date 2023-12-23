using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace Core;
public class User:BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
    public string Avatar { get; set; }
    public Role Role { get; set; } 
    public List <Purchase> Purchases { get; set; } 
    public List <Address> Addresses{ get; set; } 
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Admin,
    Customer
}





