using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core;
public class BaseQueryParameter
{
    public int Limit { get; set; } =200;
    public int Offset { get; set; } =0;
    public string Search { get; set; } = string.Empty;
}
