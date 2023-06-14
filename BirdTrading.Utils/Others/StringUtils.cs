using System.Globalization;

namespace BirdTrading.Utils.Others
{
    public static class StringUtils
    {
        public static string FormatCurrency(decimal currency)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("en-US");
            return currency.ToString("C", cul.NumberFormat);
        }
    }
}
