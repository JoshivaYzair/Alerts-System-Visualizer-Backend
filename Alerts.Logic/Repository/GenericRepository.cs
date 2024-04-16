using Alerts.Logic.DTO;
using Alerts.Logic.Interface;
using Alerts.Persistence.Data;
using Alerts.Persistence.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;

namespace Alerts.Logic.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AlertsDbContext _context;
        private readonly DbSet<T> EntitySet = null;

        public GenericRepository(AlertsDbContext context)
        {
            _context = context;
            EntitySet = _context.Set<T>();
        }

        public async Task Delete(T obj)
        {
            var property = obj.GetType().GetProperty("Active");
            property.SetValue(obj, false);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetById(long id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.Where(e => EF.Property<bool>(e, "Active")).ToListAsync();
        }

        public async Task Insert(T obj)
        {
            EntitySet.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T obj)
        {
            EntitySet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IPagedList<T>> GetPaginator(Expression<Func<T, bool>> filter, int pageNumber, int pageSize)
        {
            var result = await EntitySet.Where(filter).ToPagedListAsync(pageNumber, pageSize);

            return result;
        }
        
    }
}
