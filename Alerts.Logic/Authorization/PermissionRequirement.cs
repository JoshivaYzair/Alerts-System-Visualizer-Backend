using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Authorization
{
    /// <summary>
    /// Clase que representa un requisito de autorización basado en un permiso específico.
    /// Implementa la interfaz IAuthorizationRequirement.
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Permiso requerido para cumplir con el requisito de autorización.
        /// </summary>
        public Permission Permission { get; }

        /// <summary>
        /// Constructor de la clase PermissionRequirement.
        /// </summary>
        /// /// <param name="permission">Permiso requerido para cumplir con el requisito de autorización.</param>
        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }
    }
}
