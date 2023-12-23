using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace ECommerceBusiness;

public interface ITokenService
{
    string GenerateToken(User user);
}
