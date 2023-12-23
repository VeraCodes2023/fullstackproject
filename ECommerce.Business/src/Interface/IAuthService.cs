using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceCore;

namespace ECommerceBusiness;
public interface IAuthService
{
    string Login (Credential  credential);
}
