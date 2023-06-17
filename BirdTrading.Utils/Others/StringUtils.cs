using System.Globalization;
using System.Text;

namespace BirdTrading.Utils.Others
{
    public static class StringUtils
    {
        public static string FormatCurrency(decimal currency)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("en-US");
            return currency.ToString("C", cul.NumberFormat);
        }

        public static string FormatEnumString(this string enumString)
        {
            if (string.IsNullOrWhiteSpace(enumString))
                return "";
            StringBuilder newText = new StringBuilder(enumString.Length * 2);
            newText.Append(enumString[0]);
            for (int i = 1; i < enumString.Length; i++)
            {
                if (char.IsUpper(enumString[i]) && enumString[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(enumString[i]);
            }
            return newText.ToString();
        }

        public static string FormatPhoneNumber(this string phoneNumber)
        {
            var result = phoneNumber.Substring(1);
            result = result.Substring(0, 3) + " " + result.Substring(3, 3) + " " + result.Substring(6);
            return "(+84) " + result;
        }
    }
}
