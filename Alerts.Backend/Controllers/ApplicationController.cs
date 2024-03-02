using Alerts.Logic.DTO;
using Alerts.Logic.Interface;
using Alerts.Logic.Mapper;
using Alerts.Logic.Service;
using Alerts.Persistence.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Alerts.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IGenericRepository<Application> _applicationRepository;

        public ApplicationController(IGenericRepository<Application> applicationRepository)
        {
            this._applicationRepository = applicationRepository;
        }

        [HttpGet]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetAllApplications()
        {
            var app = await _applicationRepository.GetAll();
            if (app == null)
            {
                return NoContent();
            }
            return Ok(app);

        }

        [HttpGet("{page},{pageSize}")]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetAllAppPaginator(int page, int pageSize)
        {
            var alerts = await _applicationRepository.GetAllPaginarot(page, pageSize);
            if (alerts == null)
            {
                return NoContent();
            }
            return Ok(alerts);

        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> GetApplicationtById(long id)
        {
            var app = await _applicationRepository.GetById(id);

            if (app == null)
            {
                return BadRequest($"The system could not locate a Application with the ID {id}. Please verify the ID and try again.");
            }

            return Ok(app);
        }

        [HttpPost]
        [Authorize(Policy = "Create")]
        public async Task<IActionResult> CreateApplication([FromBody] ApplicationDTO ApplicationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var App = ApplicationDTOMapper.MapToApplication(ApplicationDTO);
            try {
                await _applicationRepository.Insert(App);
            }catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException postgresException && postgresException.SqlState == "23505")
                {
                    return BadRequest($"The code: {App.Code} you have entered already exists.");
                }
            }
            return CreatedAtAction(nameof(GetApplicationtById), new { id = App.Id }, App);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Update")]
        public async Task<IActionResult> UpdateApplication(long id, [FromBody] ApplicationUpdateDTO appUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingApplication = await _applicationRepository.GetById(id);
            if (existingApplication == null)
            {
                return NotFound($"The system could not locate a Application with the ID {id}. Please verify the ID and try again.");
            }
            var App = ApplicationDTOMapper.MapUpdateToApplication(existingApplication, appUpdate);
            await _applicationRepository.Update(App);

            return Ok(App);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteApplication(long id)
        {
            var existingApp = await _applicationRepository.GetById(id);
            if (existingApp == null)
            {
                return NotFound($"The system could not locate a Application with the ID {id}. Please verify the ID and try again.");
            }
            await _applicationRepository.Delete(existingApp);
            return NoContent();
        }
    }
}
