using System.Net;
using FluentValidation;

namespace WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = ex.Errors.GroupBy(e=>e.PropertyName).ToDictionary(g=>g.Key,g=>g.Select(e=>e.ErrorMessage).ToArray());
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "Validation Failed",
                    Errors = errors
                });

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(
                    new
                    {
                    Message = "Internal Server Error",
                    Detail = ex.Message
                    }
                    );
            }
        }
    }
}
