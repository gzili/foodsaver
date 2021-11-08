using System.Linq;
using System.Net;
using System.Threading.Tasks;
using backend.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace backend.Services
{
    public class CustomCookieAuthEvents : CookieAuthenticationEvents
    {
        private readonly UsersRepository _usersRepository;

        public CustomCookieAuthEvents(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            // Prevent automatic redirection to non-existing login page (https://stackoverflow.com/a/39091019)
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Task.FromResult(0);
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var principal = context.Principal;

            var userIdString = (
                from claim in principal.Claims
                where claim.Type == "id"
                select claim.Value
            ).FirstOrDefault();

            var userId = int.Parse(userIdString);

            var user = _usersRepository.FindByCondition(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync();
            }
            
            context.HttpContext.Items.Add("user", user);
        }
    }
}