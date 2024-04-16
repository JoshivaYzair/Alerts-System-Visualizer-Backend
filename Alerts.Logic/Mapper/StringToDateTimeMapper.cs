using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Logic.Mapper
{
    internal class StringToDateTimeMapper
    {
        public static DateTime MapStringToDateTime(string date) {
            date = WebUtility.UrlDecode(date);
            if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
            }
            return parsedDate;
        }
    }
}
