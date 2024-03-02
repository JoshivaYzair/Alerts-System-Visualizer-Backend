using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllPaginarot(int page, int pageSize);
        Task<T> GetById(long id);
        Task Insert(T obj);
        Task Update(T obj); 
        Task Delete(T id);
    }
}
