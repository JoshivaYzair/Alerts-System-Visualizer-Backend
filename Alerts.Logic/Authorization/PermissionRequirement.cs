using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission Permission { get; }
        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }
    }
}
