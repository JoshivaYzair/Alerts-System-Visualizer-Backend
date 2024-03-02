using Alerts.Logic.DTO;
using Alerts.Persistence.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Mapper
{
    public class ApplicationDTOMapper
    {
        public static Application MapToApplication(ApplicationDTO appDTO)
        {
            return new Application
            {
                Code = appDTO.Code,
                Name = appDTO.Name,
                Description = appDTO.Description,
                Url = appDTO.Url,
                SupportEmail = appDTO.SupportEmail,
                Active = appDTO.Active,
                Alerts = new List<Alert>()
            };
        }
        public static Application MapUpdateToApplication(Application existingApp, ApplicationUpdateDTO AppDTO)
        {
            existingApp.Name = AppDTO.Name;
            existingApp.Description = AppDTO.Description;
            existingApp.Url = AppDTO.Url;
            existingApp.SupportEmail = AppDTO.SupportEmail;
            existingApp.Active = AppDTO.Active;

            return existingApp;
        }
    }
}
