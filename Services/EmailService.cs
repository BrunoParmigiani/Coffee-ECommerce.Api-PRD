using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Coffee_Ecommerce.API.Services
{
    internal static class EmailService
    {
        public static bool EmailIsValid(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.[\w]+$";

            if (!Regex.IsMatch(email, regex))
                return false;

            try
            {
                var emailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool EmailIsValid(string email, string domain)
        {
            string regex = @"^[^@\s]+@" + domain + @"\.[\w]+$";

            if (!Regex.IsMatch(email, regex))
                return false;
            
            return true;
        }
    }
}
