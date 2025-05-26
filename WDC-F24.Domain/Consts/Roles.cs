using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WDC_F24.Domain.Consts
{
    public enum Roles
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "User")]
        User
    }
}
