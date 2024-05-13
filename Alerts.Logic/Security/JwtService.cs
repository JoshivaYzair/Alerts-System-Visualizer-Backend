using Alerts.Logic.DTO;
using Alerts.Persistence.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Security
{
    /// <summary>
    /// Servicio para la generación de tokens JWT.
    /// </summary>
    public class JwtService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor para JwtService.
        /// </summary>
        /// <param name="config">Configuración de la aplicación.</param>
        public JwtService(IConfiguration config)
        {
            this._config = config;
        }


        /// <summary>
        /// Genera un token JWT basado en el usuario proporcionado.
        /// </summary>
        /// <param name="user">Usuario para el cual se generará el token.</param>
        /// <returns>DTO que contiene el token JWT generado.</returns>
        internal tokenDTO Generate(User user)
        {
            // Obtener la clave de seguridad desde la configuración
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            // Crear credenciales para la firma del token
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims (información incluida en el token)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.role.ToString())
            };

            // Crear el token JWT con los claims, configuraciones y credenciales proporcionadas
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            // Escribir el token en formato JWT y lo retorna dentro de un tokenDTO
            return new tokenDTO { token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}
