using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Mapper
{
    /// <summary>
    /// Clase interna para mapear una cadena de texto a un objeto DateTime.
    /// </summary>
    internal class StringToDateTimeMapper
    {
        /// <summary>
        /// Convierte una cadena de texto en formato de fecha a un objeto DateTime.
        /// </summary>
        /// <param name="date">Cadena de texto que representa la fecha.</param>
        /// <returns>Objeto DateTime resultante de la conversión.</returns>
        public static DateTime MapStringToDateTime(string date) {

            // Decodifica la cadena en caso de que contenga caracteres codificados
            date = WebUtility.UrlDecode(date);

            // Intenta analizar la cadena como fecha en el formato especificado
            if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                // Especifica la zona horaria como UTC
                parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }
            return parsedDate;
        }
    }
}
