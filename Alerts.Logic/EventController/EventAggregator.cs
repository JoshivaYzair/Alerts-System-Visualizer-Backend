using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.EventController
{
    public class EventAggregator
    {
        private readonly Dictionary<string, List<Func<Task>>> _subscriptions = new();

        public void SubscribeEvent(string eventName, Func<Task> handler)
        {
            if (!_subscriptions.ContainsKey(eventName))
            {
                _subscriptions[eventName] = new List<Func<Task>>();
            }
            _subscriptions[eventName].Add(handler);
        }

        public async Task PublishEvent(string eventName)
        {
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
