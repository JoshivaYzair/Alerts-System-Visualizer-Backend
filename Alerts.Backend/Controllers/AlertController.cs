using Alerts.Logic.Interface;
using Alerts.Logic.Mapper;
using Alerts.Logic.DTO;
using Microsoft.AspNetCore.Mvc;
using Alerts.Persistence.Model;
using Microsoft.AspNetCore.Authorization;
using Alerts.Logic.Service;
using Alerts.Persistence.Model.Enum;
using static Alerts.Logic.Authorization.PermissionAuthorizationHandler;
using Microsoft.IdentityModel.Tokens;

namespace Alerts.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlertController : ControllerBase
    {
        private readonly IGenericRepository<Application> _applicationRepository;
        private readonly AlertService _alertService;
        public AlertController(IGenericRepository<Application> applicationRepository,
                AlertService alertService)
        {
            _applicationRepository = applicationRepository;
            _alertService = alertService;
        }

        [HttpGet]
        [HasPermission(Permission.Read)]
        public async Task<IActionResult> GetAllAlerts()
        {
            var alerts = await _alertService.GetAllAlerts();
            if (alerts == null)
            {
                return NoContent();
            }
            return Ok(alerts);

        }

        [HttpGet("paginator/filter")]
        [HasPermission(Permission.Read)]
        public async Task<IActionResult> Filter2([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? filter = null, [FromQuery] string? startDate = null, [FromQuery] string? endDate = null)
        {
            var alerts = await _alertService.getWithPaginator(page, pageSize, filter, startDate, endDate);
            if (alerts.items.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(alerts);

        }

        [HttpGet("{id}")]
        [HasPermission(Permission.Read)]
        public async Task<IActionResult> GetAlertById(long id)
        {
            var alert = await _alertService.GetAlertById(id);
            
            if (alert==null)
            {
                return BadRequest($"The system could not locate a alert with the ID {id}. Please verify the ID and try again.");
            }

            return Ok(alert);

        }

        [HttpPost]
        [HasPermission(Permission.Create)]
        public async Task<IActionResult> CreateAlert([FromBody] AlertDTO alertDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var app = await _applicationRepository.GetById(alertDTO.ApplicationCode);
            if (app == null)
            {
                return NotFound($"The system could not locate a application with the ID {alertDTO.ApplicationCode}. Please verify the ID and try again.");
            }
            var alert = AlertDTOMapper.MapToAlert(alertDTO, app);

            await _alertService.CreateAlert(alert);
            return CreatedAtAction(nameof(GetAlertById), new { id = alert.Id }, alert);
        }

        [HttpPut("{id}")]
        [HasPermission(Permission.Update)]
        public async Task<IActionResult> UpdateAlert(long id, [FromBody] AlertUpdateDTO alertUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingAlert = await _alertService.GetAlertById(id);
            if (existingAlert == null)
            {
                return NotFound($"The system could not locate a Alert with the ID {id}. Please verify the ID and try again.");
            }
            var alert = AlertDTOMapper.MapUpdateToAlert(existingAlert, alertUpdate);
            await _alertService.UpdateAlert(alert);

            return Ok(alert);
        }

        [HttpDelete("{id}")]
        [HasPermission(Permission.Delete)]
        public async Task<IActionResult> DeleteAlert(long id)
        {
            var existingAlert = await _alertService.GetAlertById(id);
            if (existingAlert == null)
            {
                return NotFound($"The system could not locate a alert with the ID {id}. Please verify the ID and try again.");
            }
            await _alertService.DeleteAlert(existingAlert);

            return NoContent();
        }
    }
}
