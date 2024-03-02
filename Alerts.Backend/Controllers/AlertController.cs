using Alerts.Logic.Interface;
using Alerts.Logic.Mapper;
using Alerts.Logic.DTO;
using Microsoft.AspNetCore.Mvc;
using Alerts.Persistence.Model;
using Microsoft.AspNetCore.Authorization;
using Alerts.Persistence.Model.Enum;

namespace Alerts.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlertController : ControllerBase
    {
        private readonly IGenericRepository<Alert> _alertRepository;
        private readonly IGenericRepository<Application> _applicationRepository;
        public AlertController(IGenericRepository<Alert> alertRepository, IGenericRepository<Application> applicationRepository)
        {
            _alertRepository = alertRepository;
            _applicationRepository = applicationRepository;
        }

        [HttpGet]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetAllAlerts()
        {
            var alerts = await _alertRepository.GetAll();
            if (alerts == null)
            {
                return NoContent();
            }
            return Ok(alerts);

        }

        [HttpGet("{page},{pageSize}")]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetAllAlertPaginator(int page, int pageSize)
        {
            var alerts = await _alertRepository.GetAllPaginarot(page,pageSize);
            if (alerts == null)
            {
                return NoContent();
            }
            return Ok(alerts);

        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetAlertById(long id)
        {
            var alert = await _alertRepository.GetById(id);

            if (alert == null)
            {
                return BadRequest($"The system could not locate a alert with the ID {id}. Please verify the ID and try again.");
            }

            return Ok(alert);
        }

        [HttpPost]
        [Authorize(Policy = "Create")]
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

            await _alertRepository.Insert(alert);
            return CreatedAtAction(nameof(GetAlertById), new { id = alert.Id }, alert);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateAlert(long id, [FromBody] AlertUpdateDTO alertUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingAlert = await _alertRepository.GetById(id);
            if (existingAlert == null)
            {
                return NotFound($"The system could not locate a Alert with the ID {id}. Please verify the ID and try again.");
            }
            var alert = AlertDTOMapper.MapUpdateToAlert(existingAlert, alertUpdate);
            await _alertRepository.Update(alert);

            return Ok(alert);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteAlert(long id)
        {
            var existingAlert = await _alertRepository.GetById(id);
            if (existingAlert == null)
            {
                return NotFound($"The system could not locate a alert with the ID {id}. Please verify the ID and try again.");
            }
            await _alertRepository.Delete(existingAlert);

            return NoContent();
        }
    }
}
