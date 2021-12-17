using System.Net;
using System.Threading.Tasks;
using backend.Exceptions;
using Microsoft.AspNetCore.Http;

namespace backend.Middleware
{
    public class ErrorResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (EntityNotFoundException e)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                await context.Response.WriteAsync($"{e.EntityName} with ID = {e.EntityId} could not be found");
            }
        }
    }
}