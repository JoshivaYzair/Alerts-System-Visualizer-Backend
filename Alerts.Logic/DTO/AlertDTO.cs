using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Alerts.Persistence.Model.Enum;

namespace Alerts.Logic.DTO
{
    public class AlertDTO
    {
        public string Name { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }
        public AlertSeverity Severety { get; set; }
        [MaxLength(200)]
        public string Source { get; set; }
        [MaxLength(3000)]
        public string StackTrace { get; set; }
        public AlertStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool Active { get; set; }
        public int ApplicationCode { get; set; }
    }
}
