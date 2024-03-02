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
    public class LoginService
    {
        private readonly AlertsDbContext _context;
        private readonly JwtService _jwtService;
        public LoginService(AlertsDbContext context, JwtService jwtService)
        {
            this._context = context;
            this._jwtService = jwtService;
        }
        public async Task<tokenDTO> login(AuthDTO userDTO)
        {
            var user = await _context.Users.Where(u => u.Email == userDTO.email && u.Active).FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            if (!user.Password.SequenceEqual(userDTO.password))
            {
                return null;
            }
            return _jwtService.Generate(user);
        }
    }
}
