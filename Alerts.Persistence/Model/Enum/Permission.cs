using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Persistence.Model.Enum
{
    public enum Permission
    {
        [EnumMember(Value = "Create")]
        Create,
        [EnumMember(Value = "Read")]
        Read,
        [EnumMember(Value = "Update")]
        Update,
        [EnumMember(Value = "Delete")]
        Delete
    }
}
