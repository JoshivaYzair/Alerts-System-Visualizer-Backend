using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Alerts.Persistence.Model
{
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(1000)]
        public string Url { get; set; }
        [MaxLength (100)]
        public string SupportEmail { get; set; }
        public bool Active { get; set; }
        [JsonIgnore]
        public List<Alert> Alerts { get; set; }
    }
}
