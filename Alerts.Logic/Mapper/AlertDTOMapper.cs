using Alerts.Logic.DTO;
using Alerts.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Mapper
{
    /// <summary>
    /// Clase para mapear objetos DTO de Alert a la entidad Alert.
    /// </summary>
    public class AlertDTOMapper
    {
         
        /// <summary>
        /// Mapea un objeto AlertDTO a la entidad Alert, incluyendo la aplicación asociada.
        /// </summary>
        /// <param name="alertDTO">DTO de Alert a mapear.</param>
        /// <param name="app">Aplicación asociada a la alerta.</param>
        /// <returns>La entidad Alert mapeada.</returns>
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

        /// <summary>
        /// Mapea las propiedades actualizadas de un objeto AlertUpdateDTO a una instancia existente de Alert.
        /// </summary>
        /// <param name="existingAlert">Instancia existente de Alert a actualizar.</param>
        /// <param name="alertUpdateDTO">DTO de Alert con las propiedades actualizadas.</param>
        /// <returns>La instancia actualizada de Alert.</returns>
        public static Alert MapUpdateToAlert(Alert existingAlert,AlertUpdateDTO alertUpdateDTO)
        {
            existingAlert.Severety = alertUpdateDTO.Severety;
            existingAlert.Status = alertUpdateDTO.Status;
            existingAlert.Active = alertUpdateDTO.Active;

            return existingAlert;
        }
    }
}
