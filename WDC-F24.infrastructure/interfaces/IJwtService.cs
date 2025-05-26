using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDC_F24.Domain.Entities;

namespace WDC_F24.infrastructure.interfaces
{
    public  interface IJwtService
    {
        //Task<string> GenerateToken(User user, UserManager<User> userManager);
        string GenerateToken(User user, IList<string> roles);
    }
}
