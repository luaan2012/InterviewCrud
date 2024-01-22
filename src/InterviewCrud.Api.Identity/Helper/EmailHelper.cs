using System.Text.RegularExpressions;

namespace InterviewCrud.Api.Identity.Helper
{
    public class EmailHelper
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            return Regex.IsMatch(email, pattern);
        }
    }
}
