using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Authorization
{
    /// <summary>
    /// Manejador de autorización para verificar los permisos de un usuario.
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        private readonly UserRolePermissionRepository _rolePermissionRepository;

        /// <summary>
        /// Constructor del manejador de autorización.
        /// </summary>
        /// <param name="rolePermissionRepository">Repositorio de permisos por rol.</param>
        public PermissionAuthorizationHandler(UserRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }

        /// <summary>
        /// Método para manejar los requisitos de autorización.
        /// </summary>
        /// <param name="context">Contexto de autorización.</param>
        /// <param name="requirement">Requisito de permiso.</param>
        /// <returns>Tarea que representa la operación asincrónica.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userRole = GetCurrentUserRole(context); // Implementa la lógica para obtener el rol del usuario actual

            if (userRole == null)
            {
                context.Fail(); // El usuario no está autenticado, falla la autorización
                return Task.CompletedTask;
            }

            var userPermissions = _rolePermissionRepository.GetUserRolePermission(userRole);

            if (userPermissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement); // El usuario tiene el permiso necesario, la autorización tiene éxito
            }
            else
            {
                context.Fail(); // El usuario no tiene el permiso necesario, la autorización falla
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Método para obtener el rol del usuario actual a partir del contexto de autorización.
        /// </summary>
        /// <param name="context">Contexto de autorización.</param>
        /// <returns>El rol del usuario actual.</returns>
        private Role? GetCurrentUserRole(AuthorizationHandlerContext context)
        {
            // Obtiene las reclamaciones(Claims) del usuario del contexto de autorización
            var userClaims = context.User.Claims;

            // Buscar la reclamación(Claim) que contiene el rol del usuario
            var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar si se encontró la reclamación(Claim) del rol y si se pudo convertir a un valor del enum Role
            if (!string.IsNullOrEmpty(roleClaim) && Enum.TryParse(roleClaim, out Role userRole))
            {
                return userRole;
            }

            // Si no se encontró la reclamación(Claim) del rol o no se pudo convertir, devuelve null
            return null;

        }

        /// <summary>
        /// Atributo de autorización que especifica un permiso requerido.
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
        /// <summary>
        /// Constructor del atributo HasPermission.
        /// </summary>
        /// <param name="permission">Permiso requerido.</param>
        public class HasPermissionAttribute : AuthorizeAttribute
        {
            public HasPermissionAttribute(Permission permission) : base(permission.ToString()) { }
        }
    }
}

