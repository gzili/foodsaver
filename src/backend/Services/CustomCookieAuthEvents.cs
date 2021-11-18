using System.Linq;
using System.Net;
using System.Security.Claims;
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

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            // Prevent automatic redirection to non-existing access denied page (https://stackoverflow.com/a/39091019)
            context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            return Task.CompletedTask;
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            // Prevent automatic redirection to non-existing login page (https://stackoverflow.com/a/39091019)
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userId = int.Parse(context.Principal.Identity.Name);

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