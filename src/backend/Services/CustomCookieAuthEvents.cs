using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace backend.Services
{
    public class CustomCookieAuthEvents : CookieAuthenticationEvents
    {
        private readonly IUsersService _usersService;

        public CustomCookieAuthEvents(IUsersService usersService)
        {
            _usersService = usersService;
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

            var user = _usersService.FindById(userId);

            if (user == null)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync();
            }
            else
            {
                context.HttpContext.Items.Add("user", user);
            }
        }
    }
}