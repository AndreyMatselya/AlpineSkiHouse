using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AlpineSkiHouse.Web.Middleware
{
    public class CSPMiddleware
    {
        private readonly RequestDelegate _next;
                
        public CSPMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
            await _next.Invoke(context);
        }

    }
}
