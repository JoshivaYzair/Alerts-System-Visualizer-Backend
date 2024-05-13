using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.EventController
{
    /// <summary>
    /// Clase que gestiona la suscripción y publicación de eventos.
    /// </summary>
    public class EventAggregator
    {
        private readonly Dictionary<string, List<Func<Task>>> _subscriptions = new();

        /// <summary>
        /// Método para suscribir un controlador (handler) a un evento.
        /// </summary>
        /// <param name="eventName">Nombre del evento al que suscribirse.</param>
        /// <param name="handler">Función que se ejecutará cuando se publique el evento.</param>
        public void SubscribeEvent(string eventName, Func<Task> handler)
        {
            // Si el evento no está en el diccionario, se crea una nueva lista para almacenar los handlers
            if (!_subscriptions.ContainsKey(eventName))
            {
                _subscriptions[eventName] = new List<Func<Task>>();
            }
            // Se añade el handler a la lista correspondiente al evento
            _subscriptions[eventName].Add(handler);
        }

        /// <summary>
        /// Método para publicar un evento, ejecutando todos los handlers asociados a ese evento.
        /// </summary>
        /// <param name="eventName">Nombre del evento a publicar.</param>
        /// <returns>Una tarea asincrónica.</returns>
        public async Task PublishEvent(string eventName)
        {
            // Si hay handlers registrados para el evento, se ejecuta cada uno
            if (_subscriptions.ContainsKey(eventName))
            {
                foreach (var handler in _subscriptions[eventName])
                {
                    await handler();
                }
            }
        }
    }
}
