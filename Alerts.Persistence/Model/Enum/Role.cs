using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Persistence.Model.Enum
{
    public enum Role
    {
        [EnumMember(Value = "User")]
        User,

        [EnumMember(Value = "Administrator")]
        Administrator

    }
}
