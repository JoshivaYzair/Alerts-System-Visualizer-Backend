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
    /// <summary>
    /// Clase para mapear objetos DTO de Application a la entidad Application.
    /// </summary>
    public class ApplicationDTOMapper
    {
        /// <summary>
        /// Mapea un objeto ApplicationDTO a la entidad Application, inicializando la lista de alertas.
        /// </summary>
        /// <param name="appDTO">DTO de Application a mapear.</param>
        /// <returns>La entidad Application mapeada.</returns>
        public static Application MapToApplication(ApplicationDTO appDTO)
        {
            return new Application
            {
                Code = appDTO.Code,
                Name = appDTO.Name,
                Description = appDTO.Description,
                Url = appDTO.Url,
                SupportEmail = appDTO.SupportEmail,
                Alerts = new List<Alert>()
            };
        }

        /// <summary>
        /// Mapea las propiedades actualizadas de un objeto ApplicationUpdateDTO a una instancia existente de Application.
        /// </summary>
        /// <param name="existingApp">Instancia existente de Application a actualizar.</param>
        /// <param name="appDTO">DTO de Application con las propiedades actualizadas.</param>
        /// <returns>La instancia actualizada de Application.</returns>
        public static Application MapUpdateToApplication(Application existingApp, ApplicationUpdateDTO AppDTO)
        {
            existingApp.Name = AppDTO.Name;
            existingApp.Description = AppDTO.Description;
            existingApp.Url = AppDTO.Url;
            existingApp.SupportEmail = AppDTO.SupportEmail;

            return existingApp;
        }
    }
}
