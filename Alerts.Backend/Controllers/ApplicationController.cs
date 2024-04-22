using Alerts.Logic.DTO;
using Alerts.Logic.Mapper;
using Alerts.Logic.Service;
using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using static Alerts.Logic.Authorization.PermissionAuthorizationHandler;

namespace Alerts.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationService _applicationService;
        public ApplicationController(ApplicationService applicationService)
        {
            this._applicationService = applicationService;
        }

        [HttpGet("paginator/filter")]
        [HasPermission(Permission.Read)]
        public async Task<IActionResult> GetAllAppPaginator([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? filter = null)
        {
            var apps = await _applicationService.getAppList(page,pageSize,filter);
            if (apps.items.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(apps);

        }

        [HttpGet("{id}")]
        [HasPermission(Permission.Read)]
        public async Task<IActionResult> GetApplicationtById(long id)
        {
            var app = await this._applicationService.getAppById(id);

            if (app == null)
            {
                return BadRequest($"The system could not locate a Application with the ID {id}. Please verify the ID and try again.");
            }

            return Ok(app);
        }

        [HttpPost]
        [HasPermission(Permission.Create)]
        public async Task<IActionResult> CreateApplication([FromBody] ApplicationDTO ApplicationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var app = ApplicationDTOMapper.MapToApplication(ApplicationDTO);
            
            await this._applicationService.createApp(app);
            return CreatedAtAction(nameof(GetApplicationtById), new { id = app.Id }, app);
        }

        [HttpPut("{id}")]
        [HasPermission(Permission.Update)]
        public async Task<IActionResult> UpdateApplication(long id, [FromBody] ApplicationUpdateDTO appUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingApplication = await this._applicationService.getAppById(id);
            if (existingApplication == null)
            {
                return NotFound($"The system could not locate a Application with the ID {id}. Please verify the ID and try again.");
            }
            var App = ApplicationDTOMapper.MapUpdateToApplication(existingApplication, appUpdate);
            await this._applicationService.updateApp(App);

            return Ok(App);
        }

        [HttpDelete("{id}")]
        [HasPermission(Permission.Delete)]
        public async Task<IActionResult> DeleteApplication(long id)
        {
            var existingApp = await this._applicationService.getAppById(id);
            if (existingApp == null)
            {
                return NotFound($"The system could not locate a Application with the ID {id}. Please verify the ID and try again.");
            }
            await this._applicationService.deleteApp(existingApp);
            return NoContent();
        }
    }
}
