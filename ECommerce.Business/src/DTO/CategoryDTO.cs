using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Business;
public class CategoryReadDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
}

public class CategoryCreateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
}

public class CategoryUpdateDTO
{
    public string Name { get; set; }
    public string Image { get; set; }
}

