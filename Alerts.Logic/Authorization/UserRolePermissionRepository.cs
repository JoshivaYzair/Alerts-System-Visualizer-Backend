using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alerts.Persistence.Model.Enum;

namespace Alerts.Logic.Authorization
{
    /// <summary>
    /// Clase que gestiona los permisos por rol de usuario.
    /// </summary>
    public class UserRolePermissionRepository
    {
        public UserRolePermissionRepository()
        {
            // Inicializa los permisos por rol
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

        // Obtiene todos los permisos disponibles
        public static HashSet<Permission> AllPermissions =>
            new HashSet<Permission>(Enum.GetValues(typeof(Permission)).Cast<Permission>());

        // Almacena los permisos por rol en un diccionario
        private readonly Dictionary<Role, HashSet<Permission>> permissionsPerRole;

        /// <summary>
        /// Obtiene los permisos asociados a un rol de usuario.
        /// </summary>
        /// <param name="userRole">Rol de usuario del cual obtener los permisos.</param>
        /// <returns>Una colección de permisos asociados al rol de usuario.</returns>
        public IReadOnlyCollection<Permission> GetUserRolePermission(Role? userRole)
        {
            // Si el rol de usuario no es nulo y está presente en el diccionario, se devuelven los permisos correspondientes
            return userRole != null && permissionsPerRole.ContainsKey(userRole.Value)
                    ? permissionsPerRole[userRole.Value]
                    // Si no se encuentra el rol, se devuelve una colección vacía
                    : new HashSet<Permission>();
        }
    }
}
