using System.Text.RegularExpressions;

namespace backend.Services
{
    public static class ValidationService
    {
        public static bool IsEmail(string text)
        {
            var regex = new Regex(@"^[A-Za-z\d]{1,15}@[a-zA-Z\d]{1,12}\.(com|org|net|lt|us|eu)$");
            return regex.IsMatch(text);
        }
    }
}