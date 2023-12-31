using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core;
public class ProductQueryParameters:BaseQueryParameter
{
    public int CategoryId { get; set; }
    public SortDirection? SortByPrice { get; set; }
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Ascending,
    Descending
}
