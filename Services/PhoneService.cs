using System.Text.RegularExpressions;

namespace Coffee_Ecommerce.API.Services
{
    internal static class PhoneService
    {
        public static bool NumberIsValid(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return false;

            if (number.Length != 11)
                return false;

            return true;
        }

        public static string FormatPhoneNumber(string number)
        {
            return Regex.Replace(number, @"[^\d]+", "");
        }
    }
}
