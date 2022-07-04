
using System;
using System.Globalization;

namespace ChurchManagementSystem.Web.Extentions
{
    public static class DateExtension
    {
        public static string AsFormattedDate(this DateTime date)
        {
            return date.ToString("d");
        }

        public static string AsFormattedDateWithTime(this DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return "-";
            }
            return date.ToString("MM/dd/yyyy hh:mm tt");
        }

        public static string AsFormattedDateWithoutTime(this DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return "-";
            }
            return date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
