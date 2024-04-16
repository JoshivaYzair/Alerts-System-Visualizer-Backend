using Alerts.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Alerts.Logic.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(long id);
        Task Insert(T obj);
        Task <IPagedList<T>> GetPaginator(Expression<Func<T, bool>> filter, int pageNumber, int pageSize);
        Task Update(T obj); 
        Task Delete(T id);
    }
}
