using Alerts.Logic.DTO;
using Alerts.Logic.Service;
using Microsoft.AspNetCore.Mvc;

namespace Alerts.Backend.Controllers
{
    [Route("app/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        public LoginController(LoginService loginService)
        {
            this._loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> login([FromBody] AuthDTO userDTO)
        {
            var userToken = await _loginService.login(userDTO);
            if (userToken == null)
            {
                return NotFound("User not found");
            }
            return Ok(userToken);
        }
    }
}
