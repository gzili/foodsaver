using System.Text.RegularExpressions;
using backend.DTO.Users;

namespace backend.Services
{
    public static class ValidationService
    {
        private static bool isEmail(string text)
        {
            var regex = new Regex(@"^[A-Za-z\d]{1,15}@[a-zA-Z\d]{1,12}.(com|org|net|lt|us|eu)$");
            return regex.IsMatch(text);
        }

        private static bool NotBlank(string text)
        {
            return text.Length > 0;
        }

        public static bool validateUser(CreateUserDto createUserDto)
        {
            return isEmail(createUserDto.Email) && NotBlank(createUserDto.Password) && NotBlank(createUserDto.Name);
        }
    }
}