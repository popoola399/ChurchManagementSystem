
using System;
using System.Globalization;

namespace ChurchManagementSystem.Web.Extentions
{
    public static class CurrencyExtention
    {
        public static string AsFormattedFigure(this int figure)
        {
            return string.Format("{0:n0}", figure);
        }

        public static string AsCurrency(this decimal value)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            nfi.CurrencySymbol = "$";

            var currencyValue = value.AsCurrency(CultureInfo.CurrentCulture, nfi);
            currencyValue = currencyValue.Split(".")[0];
            return currencyValue;
        }

        public static string AsCurrency(this decimal value, CultureInfo culture, NumberFormatInfo nfi)
        {
            return string.Format(nfi, "{0:C}", value);
        }
    }
}