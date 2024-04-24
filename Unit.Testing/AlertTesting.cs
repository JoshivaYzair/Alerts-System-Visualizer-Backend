using Alerts.Backend.Controllers;
using Alerts.Logic.Interface;
using Alerts.Logic.Repository;
using Alerts.Logic.Service;
using Alerts.Persistence.Data;
using Alerts.Persistence.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Unit.Testing
{
    public class AlertTesting
    {
        private readonly AlertsDbContext dbContext;
        private readonly IGenericRepository<Alert> genericAlertRepository;
        private readonly IGenericRepository<Application> genericAppRepository;
        private readonly AlertService alertService;
        private readonly AlertController _Alertcontroller;
        public AlertTesting()
        {
            // Cargar la configuración de la aplicación desde appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AlertsDB");

            var options = new DbContextOptionsBuilder<AlertsDbContext>()
            .UseNpgsql(connectionString) 
            .Options;

            dbContext = new AlertsDbContext(options);

            genericAlertRepository = new GenericRepository<Alert>(dbContext);
            genericAppRepository = new GenericRepository<Application>(dbContext);

            alertService = new AlertService(genericAlertRepository,null,null);
            _Alertcontroller = new AlertController(genericAppRepository,alertService);

        }

        [Fact]
        public async Task Filter2_ReturnsOkResult_WhenAlertsExist(){

            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "";
            var startDate = "";
            var endDate = "";

            // Act
            var result = await _Alertcontroller.GetAllAlertPaginator(page, pageSize, filter, startDate, endDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Filter2_ReturnsNoContentResult_WhenAlertsDontExist(){

            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "NoExistThisFilz";
            var startDate = "";
            var endDate = "";

            // Act
            var result = await _Alertcontroller.GetAllAlertPaginator(page, pageSize, filter, startDate, endDate);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAlertById_ReturnsOkResult_WhenAlertsExist()
        {

            // Arrange
            var id = 8;

            // Act
            var result = await _Alertcontroller.GetAlertById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAlertById_ReturnsBadRequestResult_WhenAlertsIdDontExist()
        {

            // Arrange
            var id = 0;

            // Act
            var result = await _Alertcontroller.GetAlertById(id);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
        }
    }

    public class ApplicationTesting 
    {
        private readonly AlertsDbContext dbContext;
        private readonly IGenericRepository<Alert> genericAlertRepository;
        private readonly IGenericRepository<Application> genericAppRepository;
        private readonly ApplicationService appService;
        private readonly ApplicationController _apptroller;
        public ApplicationTesting()
        {
            // Cargar la configuración de la aplicación desde appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AlertsDB");

            var options = new DbContextOptionsBuilder<AlertsDbContext>()
            .UseNpgsql(connectionString)
            .Options;

            dbContext = new AlertsDbContext(options);

            genericAlertRepository = new GenericRepository<Alert>(dbContext);
            genericAppRepository = new GenericRepository<Application>(dbContext);

            appService = new ApplicationService(genericAppRepository, genericAlertRepository);
            _apptroller = new ApplicationController(appService);

        }

        [Fact]
        public async Task GetAllAppPaginator_ReturnsOkResult_WhenAlertsExist()
        {

            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "";

            // Act
            var result = await _apptroller.GetAllAppPaginator(page, pageSize, filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllAppPaginator_ReturnsNoContentResult_WhenAlertsDontExist()
        {

            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "NoExist";

            // Act
            var result = await _apptroller.GetAllAppPaginator(page, pageSize, filter);

            // Assert
            var okResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAppById_ReturnsOkResult_WhenAlertsExist()
        {

            // Arrange
            var id = 2;

            // Act
            var result = await _apptroller.GetApplicationtById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAppById_ReturnsBadRequestResult_WhenAlertsIdDontExist()
        {

            // Arrange
            var id = 0;

            // Act
            var result = await _apptroller.GetApplicationtById(id);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}