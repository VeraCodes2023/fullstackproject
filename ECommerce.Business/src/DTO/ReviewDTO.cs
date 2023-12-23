using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceBusiness;
public class ReviewReadDTO
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string Feedback { get; set; }
    
}

public class ReviewCreateDTO
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string Feedback { get; set; }
    
}


public class ReviewUpdateDTO
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string Feedback { get; set; }
    
}


 