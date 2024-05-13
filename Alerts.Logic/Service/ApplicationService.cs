using Alerts.Logic.DTO;
using Alerts.Logic.ExpressionFilter;
using Alerts.Logic.Interface;
using Alerts.Persistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alerts.Logic.Service
{
    /// <summary>
    /// Clase que proporciona servicios para la gestión de aplicaciones.
    /// </summary>
    public class ApplicationService
    {
        private readonly IGenericRepository<Application> _appRepository;
        private readonly IGenericRepository<Alert> _alertRepository;

        /// <summary>
        /// Constructor de la clase ApplicationService.
        /// </summary>
        /// <param name="appRepository">Repositorio genérico para operaciones de base de datos con aplicaciones.</param>
        /// <param name="alertRepository">Repositorio genérico para operaciones de base de datos con alertas.</param>
        public ApplicationService(IGenericRepository<Application> appRepository, IGenericRepository<Alert> alertRepository)
        {
            this._appRepository = appRepository;
            _alertRepository = alertRepository;

        }

        // Métodos para realizar operaciones CRUD en las aplicaciones

        /// <summary>
        /// Crea una nueva aplicación en la base de datos.
        /// </summary>
        /// <param name="app">Aplicación a crear.</param>
        /// <returns>Tarea asincrónica.</returns>
        public async Task createApp(Application app) {
            await this._appRepository.Insert(app);
        }

        /// <summary>
        /// Elimina una aplicación de la base de datos.
        /// </summary>
        /// <param name="app">Aplicación a eliminar.</param>
        /// <returns>Tarea asincrónica.</returns>
        public async Task deleteApp(Application app) {
            // Verifica si hay alertas asociadas a la aplicación

            var alerts = await _alertRepository.GetAll();
            if (alerts.Any(a => a.ApplicationCode == app.Code))
            {
                throw new Exception("No se puede eliminar la aplicación porque tiene alertas asociadas.");
            }
            await this._appRepository.Delete(app);
        }

        /// <summary>
        /// Actualiza una aplicación en la base de datos.
        /// </summary>
        /// <param name="app">Aplicación a actualizar.</param>
        /// <returns>Tarea asincrónica.</returns>
        public async Task updateApp(Application app) {
            await this._appRepository.Update(app);
        }

        /// <summary>
        /// Obtiene una aplicación por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único de la aplicación.</param>
        /// <returns>La aplicación correspondiente al identificador proporcionado.</returns>
        public async Task<Application> getAppById(long id) {
            return await this._appRepository.GetById(id);
        }

        /// <summary>
        /// Obtiene una lista paginada de aplicaciones con filtros opcionales.
        /// </summary>
        /// <param name="page">Número de página.</param>
        /// <param name="pageSize">Tamaño de la página.</param>
        /// <param name="filter">Cadena de texto para filtrar las aplicaciones.</param>
        /// <returns>Una respuesta que contiene la lista paginada de aplicaciones y el número total de aplicaciones.</returns>
        public async Task<ItemsResponse<Application>> getAppList(int page, int pageSize, string? filter) {

            // Crear una expresión de filtro por defecto para buscar por Aplicaciones activas
            Expression<Func<Application, bool>> defaultPredicate = e => EF.Property<bool>(e, "Active");
            Expression<Func<Application, bool>> filterExpression = defaultPredicate;

            // Agregar condiciones de filtro adicionales si se proporcionan
            if (!filter.IsNullOrEmpty())
            {
                filterExpression = filterExpression.And(
                   e => EF.Property<string>(e, "Code").Contains(filter) ||
                        EF.Property<string>(e, "Name").Contains(filter)
                );
            }

            // Obtiene la lista paginada de aplicaciones que cumplen con los criterios de filtro
            var apps = await _appRepository.GetPaginator(filterExpression, page, pageSize);

            // Si no se encontraron aplicaciones, devolver una respuesta vacía
            if (apps == null)
            {
                return new ItemsResponse<Application> { items = new List<Application>(), TotalItems = 0 };
            }
            // Devuelve la lista paginada de aplicaciones junto con el número total de aplicaciones
            return new ItemsResponse<Application> { items = apps.ToList(), TotalItems = apps.TotalItemCount };
        }
    }
}
