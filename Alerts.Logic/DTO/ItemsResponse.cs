using Alerts.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.DTO
{
    public class ItemsResponse <T>
    {
        public List<T> items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
    }
}
