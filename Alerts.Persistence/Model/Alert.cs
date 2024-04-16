using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Alerts.Persistence.Model.Enum;

namespace Alerts.Persistence.Model
{
    public class Alert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }
        [Required]
        public AlertSeverity Severety { get; set; }
        [MaxLength(200)]
        [Required]
        public string Source { get; set; }
        [MaxLength(3000)]
        [Required]
        public string StackTrace { get; set; }
        [Required]
        public AlertStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        [Required]
        public bool Active { get; set; } = true;
        [MaxLength(20)]
        [ForeignKey("Application")]
        public string ApplicationCode { get; set; }
        [JsonIgnore]
        public virtual Application Application { get; set; }
    }
}
