using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Core;

namespace Core;
public class Address:BaseEntity
{
    [ForeignKey("UserId")]
    public Guid UserId { get; set; } // fk navigate to user 
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public bool IsDefault { get; set; } = true;
}



