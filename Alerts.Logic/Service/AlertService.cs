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
    public class AlertService
    {
        private readonly IGenericRepository<Alert> _alertsRepository;
        private readonly EventAggregator _eventAggregator;
        private readonly EventSubscriber _eventSubscriber;
        public AlertService(IGenericRepository<Alert> genericRepository,
            EventAggregator eventAggregator, EventSubscriber eventSubscriber)
        {
            this._alertsRepository = genericRepository;
            this._eventAggregator = eventAggregator;
            this._eventSubscriber = eventSubscriber;

        }

        public async Task DeleteAlert(Alert alert)
        {
            await _alertsRepository.Delete(alert);
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        public async Task UpdateAlert(Alert alert)
        {
            await _alertsRepository.Update(alert);
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        public async Task CreateAlert(Alert alert)
        {
            await _alertsRepository.Insert(alert);
            await _eventAggregator.PublishEvent(Event.UserCreated.ToString());
        }

        public async Task<IEnumerable<Alert>> GetAllAlerts()
        {
            return await _alertsRepository.GetAll();
        }

        public async Task<Alert> GetAlertById(long id)
        {
            return await _alertsRepository.GetById(id);
        }
        public async Task<ItemsResponse<Alert>> getWithPaginator(int page, int pageSize, string filter, String startDate, String endDate)
        {
            Expression<Func<Alert, bool>> defaultPredicate = e => EF.Property<bool>(e, "Active");

            Expression<Func<Alert, bool>> filterExpression = defaultPredicate;

            if(!filter.IsNullOrEmpty()) 
            {
                filterExpression = filterExpression.And(
                   e => EF.Property<string>(e, "StackTrace").Contains(filter) ||
                        EF.Property<string>(e, "Name").Contains(filter) ||
                        EF.Property<string>(e, "ApplicationCode").Contains(filter)
                );
            }

            DateTime? parsedStartDate = StringToDateTimeMapper.MapStringToDateTime(startDate);
            DateTime? parsedEndDate = StringToDateTimeMapper.MapStringToDateTime(endDate);

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                if (parsedStartDate != null && parsedEndDate != null)
                {
                    filterExpression = filterExpression.And(
                       e => e.CreationDate.Date >= parsedStartDate && e.CreationDate.Date <= parsedEndDate
                    );
                }
            }

            var alerts = await _alertsRepository.GetPaginator(filterExpression, page, pageSize);

            return new ItemsResponse<Alert> { items = alerts.ToList(), TotalItems = alerts.TotalItemCount };
        }
    }
}
