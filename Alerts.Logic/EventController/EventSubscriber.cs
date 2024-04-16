using Alerts.Logic.HubController;
using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.SignalR;

namespace Alerts.Logic.EventController
{
    public class EventSubscriber
    {
        private EventAggregator _eventAggregator;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventSubscriber(EventAggregator eventAggregator, IHubContext<NotificationHub> hubContext)
        {
            _eventAggregator = eventAggregator;
            _hubContext = hubContext;
            _eventAggregator.SubscribeEvent(Event.UserCreated.ToString(), sentNoti);
        }
        public async Task sentNoti()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate");
        }
    }
}
