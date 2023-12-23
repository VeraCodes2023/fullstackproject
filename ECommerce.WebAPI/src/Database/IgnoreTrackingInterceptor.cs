using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerceWebAPI;

public class IgnoreTrackingInterceptor:IMaterializationInterceptor
{
      public static IgnoreTrackingInterceptor Instance { get; } = new();
      private IgnoreTrackingInterceptor() { }
     public void Loaded()
    {
     
    }
}
