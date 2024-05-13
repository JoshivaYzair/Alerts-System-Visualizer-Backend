using Alerts.Logic.DTO;
using Alerts.Logic.EventController;
using Alerts.Logic.ExpressionFilter;
using Alerts.Logic.Interface;
using Alerts.Logic.Mapper;
using Alerts.Persistence.Data;
using Alerts.Persistence.Model;
using Alerts.Persistence.Model.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;

namespace Alerts.Logic.Service
{
    /// <summary>
    /// Clase que proporciona servicios para la gestión de alertas.
    /// </summary>
    public class AlertService
    {
        private readonly IGenericRepository<Alert> _alertsRepository;
        private readonly EventAggregator _eventAggregator;
        private readonly EventSubscriber _eventSubscriber;

        /// <summary>
        /// Constructor de la clase AlertService.
        /// </summary>
        /// <param name="genericRepository">Repositorio genérico para operaciones de base de datos con alertas.</param>
        /// <param name="eventAggregator">Agente de eventos para la gestión de eventos relacionados con las alertas.</param>
        /// <param name="eventSubscriber">Suscriptor de eventos para la gestión de eventos relacionados con las alertas.</param>
        public AlertService(IGenericRepository<Alert> genericRepository,
            EventAggregator eventAggregator, EventSubscriber eventSubscriber)
        {
            this._alertsRepository = genericRepository;
            this._eventAggregator = eventAggregator;
            this._eventSubscriber = eventSubscriber;

        }

        // Métodos para realizar operaciones CRUD en las alertas

        /// <summary>
        /// Elimina una alerta de la base de datos.
        /// </summary>
        /// <param name="alert">Alerta a eliminar.</param>
        /// <returns>Tarea asincrónica.</returns>

        public async Task DeleteAlert(Alert alert)
        {
            await _alertsRepository.Delete(alert);
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        /// <summary>
        /// Actualiza una alerta en la base de datos.
        /// </summary>
        /// <param name="alert">Alerta a actualizar.</param>
        /// <returns>Tarea asincrónica.</returns>
        public async Task UpdateAlert(Alert alert)
        {
            await _alertsRepository.Update(alert);
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        /// <summary>
        /// Crea una nueva alerta en la base de datos.
        /// </summary>
        /// <param name="alert">Alerta a crear.</param>
        /// <returns>Tarea asincrónica.</returns>
        public async Task CreateAlert(Alert alert)
        {
            await _alertsRepository.Insert(alert);
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        /// <summary>
        /// Obtiene todas las alertas de la base de datos.
        /// </summary>
        /// <returns>Todas las alertas de la base de datos.</returns>
        public async Task<IEnumerable<Alert>> GetAllAlerts()
        {
            return await _alertsRepository.GetAll();
        }

        /// <summary>
        /// Obtiene una alerta por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único de la alerta.</param>
        /// <returns>La alerta correspondiente al identificador proporcionado.</returns>
        public async Task<Alert> GetAlertById(long id)
        {
            var alert = await _alertsRepository.GetById(id);
            return alert;
        }

        /// <summary>
        /// Obtiene una lista paginada de alertas con filtros opcionales.
        /// </summary>
        /// <param name="page">Número de página.</param>
        /// <param name="pageSize">Tamaño de la página.</param>
        /// <param name="filter">Cadena de texto para filtrar las alertas.</param>
        /// <param name="startDate">Fecha de inicio para filtrar las alertas.</param>
        /// <param name="endDate">Fecha de fin para filtrar las alertas.</param>
        /// <returns>Una respuesta que contiene la lista paginada de alertas y el número total de alertas.</returns>
        public async Task<ItemsResponse<Alert>> getWithPaginator(int page, int pageSize, string? filter, string? startDate, string? endDate)
        {
            // Crear una expresión de filtro por defecto para buscar por alertas activas
            Expression<Func<Alert, bool>> defaultPredicate = e => EF.Property<bool>(e, "Active");

            Expression<Func<Alert, bool>> filterExpression = defaultPredicate;

            // Agregar condiciones de filtro adicionales si se proporcionan
            if (!filter.IsNullOrEmpty()) 
            {
                filterExpression = filterExpression.And(
                   e => EF.Property<string>(e, "StackTrace").Contains(filter) ||
                        EF.Property<string>(e, "Name").Contains(filter) ||
                        EF.Property<string>(e, "ApplicationCode").Contains(filter)
                );
            }

            // Convertir las cadenas de fecha proporcionadas a DateTime
            DateTime? parsedStartDate = StringToDateTimeMapper.MapStringToDateTime(startDate);
            DateTime? parsedEndDate = StringToDateTimeMapper.MapStringToDateTime(endDate);

            // Si se proporcionan ambas fechas de inicio y fin, se aplica al filtro por rango de fechas
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                if (parsedStartDate != null && parsedEndDate != null)
                {
                    filterExpression = filterExpression.And(
                       e => e.CreationDate.Date >= parsedStartDate && e.CreationDate.Date <= parsedEndDate
                    );
                }
            }

            // Obtiene la lista paginada de alertas que cumplen con los criterios de filtro
            var alerts = await _alertsRepository.GetPaginator(filterExpression, page, pageSize);

            // Si no se encontraron alertas, devolver una respuesta vacía
            if (alerts == null)
            {
                return new ItemsResponse<Alert> { items = new List<Alert>(), TotalItems = 0 };
            }
            // Devuelve la lista paginada de alertas junto con el número total de alertas
            return new ItemsResponse<Alert> { items = alerts.ToList(), TotalItems = alerts.TotalItemCount };
        }
    }
}
