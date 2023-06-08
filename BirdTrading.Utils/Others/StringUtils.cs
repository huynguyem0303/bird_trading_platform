using System.Globalization;

namespace BirdTrading.Utils.Others
{
    public static class StringUtils
    {
        public static string FormatCurrency(decimal currency)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            return currency.ToString("#,### ₫", cul.NumberFormat);
        }
    }
}
