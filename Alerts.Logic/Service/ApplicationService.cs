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
    public class ApplicationService
    {
        private readonly IGenericRepository<Application> _appRepository;
        private readonly IGenericRepository<Alert> _alertRepository;

        public ApplicationService(IGenericRepository<Application> appRepository, IGenericRepository<Alert> alertRepository)
        {
            this._appRepository = appRepository;
            _alertRepository = alertRepository;

        }

        public async Task createApp(Application app) {
            await this._appRepository.Insert(app);
        }

        public async Task deleteApp(Application app) {
            var alerts = await _alertRepository.GetAll();
            if (alerts.Any(a => a.ApplicationCode == app.Code))
            {
                throw new Exception("No se puede eliminar la aplicación porque tiene alertas asociadas.");
            }
            await this._appRepository.Delete(app);
        }

        public async Task updateApp(Application app) {
            await this._appRepository.Update(app);
        }

        public async Task<Application> getAppById(long id) {
            return await this._appRepository.GetById(id);
        }

        public async Task<ItemsResponse<Application>> getAppList(int page, int pageSize, string filter) {
            Expression<Func<Application, bool>> defaultPredicate = e => EF.Property<bool>(e, "Active");
            Expression<Func<Application, bool>> filterExpression = defaultPredicate;

            if (!filter.IsNullOrEmpty())
            {
                filterExpression = filterExpression.And(
                   e => EF.Property<string>(e, "Code").Contains(filter) ||
                        EF.Property<string>(e, "Name").Contains(filter)
                );
            }
            
            var apps = await _appRepository.GetPaginator(filterExpression, page, pageSize);
            
            return new ItemsResponse<Application> { items = apps.ToList(), TotalItems = apps.TotalItemCount };
        }
    }
}
