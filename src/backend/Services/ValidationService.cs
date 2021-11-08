using backend.DTO.Address;
using backend.Models;
using System.Text.RegularExpressions;

namespace backend.Services
{
    public static class ValidationService
    {
        public static bool ValidEmail(string text)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$");
            return regex.IsMatch(text);
        }
        public static bool ValidPassword(string text)
        {
            var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            return regex.IsMatch(text);
        }

        public static bool ValidUsername(string text)
        {
            var regex = new Regex(@"^[\w\d.-]{4,}$");
            return regex.IsMatch(text);
        }

        public static bool ValidAddressDto(AddressDto address)
        {
            string streetText = address.Street;
            var streetRegex = new Regex(@"^([A-Z][a-z]*\.? )+(g\.|gatvė)$");
            string cityText = address.City;
            var cityRegex = new Regex(@"^(Vilnius|Kaunas|Klaipeda)$");
            return streetRegex.IsMatch(streetText) && cityRegex.IsMatch(cityText);
        }
    }
}