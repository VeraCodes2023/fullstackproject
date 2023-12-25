using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerce.Business;

public class ImageReadDTO
{
    public string Url { get; set; }
}

public class ImageCreateDTO
{
    public string Url { get; set; }
}

public class ImageUpdateDTO
{
    public string Url { get; set; }
}
