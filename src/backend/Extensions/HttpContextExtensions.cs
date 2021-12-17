using backend.Models;
using Microsoft.AspNetCore.Http;

namespace backend.Extensions
{
    public static class HttpContextExtensions
    {
        public static User GetUser(this HttpContext context)
        {
            return (User) context.Items["user"];
        }
    }
}