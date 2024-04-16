using Alerts.Persistence.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.DTO
{
    public class ApplicationDTO
    {
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(1000)]
        public string Url { get; set; }
        [MaxLength(100)]
        public string SupportEmail { get; set; }
    }
}
