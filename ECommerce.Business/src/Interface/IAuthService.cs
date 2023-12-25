using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerce.Business;
public interface IAuthService
{
    string Login (Credential  credential);
}
