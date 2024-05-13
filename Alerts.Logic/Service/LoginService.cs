using Alerts.Logic.DTO;
using Alerts.Logic.Security;
using Alerts.Persistence.Data;
using Alerts.Persistence.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Service
{
    /// <summary>
    /// Clase que proporciona servicios para el proceso de inicio de sesión de usuario.
    /// </summary>
    public class LoginService
    {
        /// <summary>
        /// Constructor de la clase LoginService.
        /// </summary>
        /// <param name="context">Contexto de la base de datos para acceder a los datos de usuario.</param>
        /// <param name="jwtService">Servicio para la generación de tokens JWT.</param>
        private readonly AlertsDbContext _context;
        private readonly JwtService _jwtService;
        public LoginService(AlertsDbContext context, JwtService jwtService)
        {
            this._context = context;
            this._jwtService = jwtService;
        }

        /// <summary>
        /// Método para iniciar sesión de usuario.
        /// </summary>
        /// <param name="userDTO">DTO que contiene la información de autenticación del usuario.</param>
        /// <returns>DTO que contiene el token JWT generado si el inicio de sesión es exitoso, de lo contrario, devuelve null.</returns>
        public async Task<tokenDTO> login(AuthDTO userDTO)
        {
            // Busca el usuario en la base de datos por su correo electrónico y verificar si está activo
            var user = await _context.Users.Where(u => u.Email == userDTO.email && u.Active).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            // Verificar si la contraseña proporcionada coincide con la contraseña almacenada para el usuario
            if (!user.Password.SequenceEqual(userDTO.password))
            {
                return null;
            }
            // Generar un token JWT para el usuario y devolverlo
            return _jwtService.Generate(user);
        }
    }
}
