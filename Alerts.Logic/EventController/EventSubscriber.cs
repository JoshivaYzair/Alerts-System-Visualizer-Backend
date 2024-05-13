using Alerts.Logic.HubController;
using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.SignalR;

namespace Alerts.Logic.EventController
{
    /// <summary>
    /// Clase que se encarga de suscribirse a eventos y enviar notificaciones a través de SignalR.
    /// </summary>
    public class EventSubscriber
    {
        private EventAggregator _eventAggregator;
        private readonly IHubContext<NotificationHub> _hubContext;

        /// <summary>
        /// Constructor de la clase EventSubscriber.
        /// </summary>
        /// <param name="eventAggregator">Agente de eventos para gestionar la suscripción y publicación de eventos.</param>
        /// <param name="hubContext">Contexto del hub de SignalR para enviar notificaciones.</param>
        public EventSubscriber(EventAggregator eventAggregator, IHubContext<NotificationHub> hubContext)
        {
            _eventAggregator = eventAggregator;
            _hubContext = hubContext;
            // Suscribe el método "sentNoti" al evento "UserCreated"
            _eventAggregator.SubscribeEvent(Event.UserCreated.ToString(), sentNoti);
        }
        /// <summary>
        /// Método que envía una notificación a todos los clientes conectados cuando se dispara el evento.
        /// </summary>
        /// <returns>Una tarea asincrónica.</returns>
        public async Task sentNoti()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate");
        }
    }
}
