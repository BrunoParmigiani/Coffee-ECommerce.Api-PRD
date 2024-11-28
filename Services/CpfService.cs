using System.Text.RegularExpressions;

namespace Coffee_Ecommerce.API.Services
{
    internal static class CpfService
    {
        public static bool CpfIsValid(string cpf)
        {
            string formatedCpf = FormatCpf(cpf);

            if (string.IsNullOrWhiteSpace(formatedCpf))
                return false;

            if (formatedCpf.Length != 11)
                return false;

            if (!NumberIsValid(formatedCpf))
                return false;

            return true;
        }

        public static string FormatCpf(string rawCpf)
        {
            return Regex.Replace(rawCpf, @"[^\d]+", "");
        }

        private static bool NumberIsValid(string cpf)
        {
            char[] splitNumber = cpf.ToArray();
            int repeatCount = 0;

            for (int i = 0; i <= 9; i++)
            {
                if (splitNumber[i] == splitNumber[i + 1])
                {
                    repeatCount++;
                }
            }

            if (repeatCount == 10)
                return false;

            int[] digits = splitNumber.Select(c => int.Parse(c.ToString())).ToArray();

            int[] firstCheck = new int[9];
            int[] secondCheck = new int[10];
            int firstDigit;
            int secondDigit;

            firstCheck[0] = digits[0] * 10;
            firstCheck[1] = digits[1] * 9;
            firstCheck[2] = digits[2] * 8;
            firstCheck[3] = digits[3] * 7;
            firstCheck[4] = digits[4] * 6;
            firstCheck[5] = digits[5] * 5;
            firstCheck[6] = digits[6] * 4;
            firstCheck[7] = digits[7] * 3;
            firstCheck[8] = digits[8] * 2;

            firstDigit = firstCheck.Sum() * 10 % 11;
            firstDigit = firstDigit == 10 ? 0 : firstDigit;

            secondCheck[0] = digits[0] * 11;
            secondCheck[1] = digits[1] * 10;
            secondCheck[2] = digits[2] * 9;
            secondCheck[3] = digits[3] * 8;
            secondCheck[4] = digits[4] * 7;
            secondCheck[5] = digits[5] * 6;
            secondCheck[6] = digits[6] * 5;
            secondCheck[7] = digits[7] * 4;
            secondCheck[8] = digits[8] * 3;
            secondCheck[9] = digits[9] * 2;

            secondDigit = secondCheck.Sum() * 10 % 11;
            secondDigit = secondDigit == 10 ? 0 : secondDigit;

            if (firstDigit != digits[9] || secondDigit != digits[10])
                return false;

            return true;
        }
    }
}
