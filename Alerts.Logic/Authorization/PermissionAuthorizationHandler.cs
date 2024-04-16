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
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        private readonly UserRolePermissionRepository _rolePermissionRepository;

        public PermissionAuthorizationHandler(UserRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }
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

        private Role? GetCurrentUserRole(AuthorizationHandlerContext context)
        {
            // Obtener las reclamaciones del usuario del contexto de autorización
            var userClaims = context.User.Claims;

            // Buscar la reclamación que contiene el rol del usuario
            var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar si se encontró la reclamación del rol y si se pudo convertir a un valor del enum Role
            if (!string.IsNullOrEmpty(roleClaim) && Enum.TryParse(roleClaim, out Role userRole))
            {
                return userRole;
            }

            // Si no se encontró la reclamación del rol o no se pudo convertir, devolver null
            return null;

        }

        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
        public class HasPermissionAttribute : AuthorizeAttribute
        {
            public HasPermissionAttribute(Permission permission) : base(permission.ToString()) { }
        }
    }
}

