using Alerts.Logic.DTO;
using Alerts.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Mapper
{
    public class AlertDTOMapper
    {
        public static Alert MapToAlert(AlertDTO alertDTO, Application app)
        {
            return new Alert
            {
                Name = alertDTO.Name,
                Description = alertDTO.Description,
                Severety = alertDTO.Severety,
                Source = alertDTO.Source,
                StackTrace = alertDTO.StackTrace,
                Status = alertDTO.Status,
                CreationDate = alertDTO.CreationDate,
                ModificationDate = alertDTO.ModificationDate,
                Active = alertDTO.Active,
                Application = app
            };
        }

        public static Alert MapUpdateToAlert(Alert existingAlert,AlertUpdateDTO alertUpdateDTO)
        {
            existingAlert.Severety = alertUpdateDTO.Severety;
            existingAlert.Status = alertUpdateDTO.Status;
            existingAlert.Active = alertUpdateDTO.Active;

            return existingAlert;
        }
    }
}
