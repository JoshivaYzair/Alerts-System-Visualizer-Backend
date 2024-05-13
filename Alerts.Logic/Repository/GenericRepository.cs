using Alerts.Logic.DTO;
using Alerts.Logic.Interface;
using Alerts.Persistence.Data;
using Alerts.Persistence.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;

namespace Alerts.Logic.Repository
{
    /// <summary>
    /// Repositorio genérico para operaciones CRUD en la base de datos.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad con la que trabajará el repositorio.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AlertsDbContext _context;
        private readonly DbSet<T> EntitySet = null;

        /// <summary>
        /// Constructor para GenericRepository.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public GenericRepository(AlertsDbContext context)
        {
            _context = context;
            EntitySet = _context.Set<T>();
        }

        /// <summary>
        /// Elimina un objeto de la base de datos estableciendo su propiedad "Active" como false.
        /// </summary>
        /// <param name="obj">Objeto a eliminar.</param>
        public async Task Delete(T obj)
        {
            var property = obj.GetType().GetProperty("Active");
            property.SetValue(obj, false);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene un objeto por su ID.
        /// </summary>
        /// <param name="id">ID del objeto a obtener.</param>
        /// <returns>El objeto correspondiente al ID especificado.</returns>
        public async Task<T> GetById(long id)
        {
            return await EntitySet.FindAsync(id);
        }

        /// <summary>
        /// Obtiene todos los objetos con propiedad "Active" sea igual a true en la base de datos.
        /// </summary>
        /// <returns>Una lista de objetos.</returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.Where(e => EF.Property<bool>(e, "Active")).ToListAsync();
        }

        /// <summary>
        /// Inserta un nuevo objeto en la base de datos.
        /// </summary>
        /// <param name="obj">Objeto a insertar.</param>
        public async Task Insert(T obj)
        {
            EntitySet.Add(obj);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza un objeto existente en la base de datos.
        /// </summary>
        /// <param name="obj">Objeto a actualizar.</param>
        public async Task Update(T obj)
        {
            EntitySet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene una lista paginada de objetos que cumplen con un filtro específico.
        /// </summary>
        /// <param name="filter">Expresión de filtro para seleccionar los objetos deseados.</param>
        /// <param name="pageNumber">Número de página deseado.</param>
        /// <param name="pageSize">Tamaño de la página.</param>
        /// <returns>Una lista paginada de objetos.</returns>
        public async Task<IPagedList<T>> GetPaginator(Expression<Func<T, bool>> filter, int pageNumber, int pageSize)
        {
            var result = await EntitySet.Where(filter).ToPagedListAsync(pageNumber, pageSize);

            return result;
        }
        
    }
}
