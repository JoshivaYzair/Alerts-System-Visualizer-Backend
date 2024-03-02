using Alerts.Logic.EventController;
using Alerts.Logic.Interface;
using Alerts.Persistence.Data;
using Alerts.Persistence.Model.Enum;
using Microsoft.EntityFrameworkCore;

namespace Alerts.Logic.Service
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AlertsDbContext _context;
        private readonly DbSet<T> EntitySet = null;
        private readonly EventAggregator _eventAggregator;
        private readonly EventSubscriber _eventSubscriber; 

        public GenericRepository(AlertsDbContext context, EventAggregator eventAggregator, EventSubscriber eventSubscriber)
        {
            this._context = context;
            EntitySet = _context.Set<T>();
            _eventAggregator = eventAggregator; 
            _eventSubscriber = eventSubscriber;
        }

        public async Task Delete(T alert)
        {
            //T existing = await EntitySet.FindAsync(id);
            var property = alert.GetType().GetProperty("Active");
            property.SetValue(alert, false);
            _context.Entry(alert).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.Where(e => EF.Property<bool>(e, "Active")).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllPaginarot(int page, int pageSize)
        {
            return await EntitySet
            .Where(e => EF.Property<bool>(e, "Active"))
            .OrderBy(e => EF.Property<bool>(e, "Id")) // Ordena los resultados si es necesario
            .Skip((page - 1) * pageSize) // Salta los registros de las páginas anteriores
            .Take(pageSize) // Toma solo la cantidad de registros de la página actual
            .ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task Insert(T obj)
        {
            EntitySet.Add(obj);
            await _context.SaveChangesAsync();
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        public async Task Update(T obj)
        {
            EntitySet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }
    }
}
