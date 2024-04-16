using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alerts.Persistence.Model.Enum;

namespace Alerts.Logic.Authorization
{
    public class UserRolePermissionRepository
    {
        public UserRolePermissionRepository()
        {
            permissionsPerRole = new Dictionary<Role, HashSet<Permission>>() {
                { Role.Administrator, AllPermissions },
                { Role.User,
                    new HashSet<Permission>{
                        Permission.Create,
                        Permission.Read
                    }
                }
            };
        }

        public static HashSet<Permission> AllPermissions =>
            new HashSet<Permission>(Enum.GetValues(typeof(Permission)).Cast<Permission>());

        private readonly Dictionary<Role, HashSet<Permission>> permissionsPerRole;
        public IReadOnlyCollection<Permission> GetUserRolePermission(Role? userRole)
        {
            return userRole != null && permissionsPerRole.ContainsKey(userRole.Value)
                    ? permissionsPerRole[userRole.Value]
                    : new HashSet<Permission>();
        }
    }
}
