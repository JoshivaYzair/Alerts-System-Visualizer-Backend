using Alerts.Logic.HubController;
using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _eventAggregator.SubscribeEvent(Event.UserCreated.ToString(), mostrar);
        }

        public async Task mostrar()
        {
            Console.WriteLine("ELl mero mero +++++++++++++++++++++++++++++++++++");
        }
        public async Task sentNoti()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate");
        }

    }
}
