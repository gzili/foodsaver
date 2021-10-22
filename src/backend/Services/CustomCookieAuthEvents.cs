using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace backend.Services
{
    public class CustomCookieAuthEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            // Prevent automatic redirection to non-existing login page (https://stackoverflow.com/a/39091019)
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Task.FromResult(0);
        }
    }
}