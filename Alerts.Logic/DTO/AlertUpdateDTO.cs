using Alerts.Persistence.Model.Enum;

namespace Alerts.Logic.DTO
{
    public class AlertUpdateDTO
    {
        public AlertStatus Status { get; set; }
        public bool Active { get; set; }
        public AlertSeverity Severety { get; set; }
    }
}
