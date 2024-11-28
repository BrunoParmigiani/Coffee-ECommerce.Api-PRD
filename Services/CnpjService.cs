using System.Text.RegularExpressions;

namespace Coffee_Ecommerce.API.Services
{
    internal static class CnpjService
    {
        public static bool CnpjIsValid(string cnpj)
        {
            string formatedCnpj = FormatCnpj(cnpj);

            if (string.IsNullOrWhiteSpace(formatedCnpj))
                return false;

            if (formatedCnpj.Length != 14)
                return false;

            if (!NumberIsValid(formatedCnpj))
                return false;

            return true;
        }

        public static string FormatCnpj(string rawCnpj)
        {
            return Regex.Replace(rawCnpj, @"[^\d]+", "");
        }

        private static bool NumberIsValid(string cnpj)
        {
            char[] splitNumber = cnpj.ToArray();
            int[] digits = splitNumber.Select(c => int.Parse(c.ToString())).ToArray();

            int[] firstCheck = new int[12];
            int[] secondCheck = new int[13];
            int firstDigit;
            int secondDigit;

            firstCheck[0] = digits[0] * 5;
            firstCheck[1] = digits[1] * 4;
            firstCheck[2] = digits[2] * 3;
            firstCheck[3] = digits[3] * 2;
            firstCheck[4] = digits[4] * 9;
            firstCheck[5] = digits[5] * 8;
            firstCheck[6] = digits[6] * 7;
            firstCheck[7] = digits[7] * 6;
            firstCheck[8] = digits[8] * 5;
            firstCheck[9] = digits[9] * 4;
            firstCheck[10] = digits[10] * 3;
            firstCheck[11] = digits[11] * 2;

            firstDigit = firstCheck.Sum() % 11;
            firstDigit = firstDigit < 2 ? 0 : 11 - firstDigit;

            secondCheck[0] = digits[0] * 6;
            secondCheck[1] = digits[1] * 5;
            secondCheck[2] = digits[2] * 4;
            secondCheck[3] = digits[3] * 3;
            secondCheck[4] = digits[4] * 2;
            secondCheck[5] = digits[5] * 9;
            secondCheck[6] = digits[6] * 8;
            secondCheck[7] = digits[7] * 7;
            secondCheck[8] = digits[8] * 6;
            secondCheck[9] = digits[9] * 5;
            secondCheck[10] = digits[10] * 4;
            secondCheck[11] = digits[11] * 3;
            secondCheck[12] = digits[12] * 2;

            secondDigit = secondCheck.Sum() % 11;
            secondDigit = secondDigit < 2 ? 0 : 11 - secondDigit;

            if (firstDigit != digits[12] || secondDigit != digits[13])
                return false;

            return true;
        }
    }
}