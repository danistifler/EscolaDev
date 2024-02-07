using EscolaParaDevs.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json;

namespace EscolaParaDevs.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = error switch
                {
                    BadRequestException => (int)HttpStatusCode.BadRequest, //custon application error
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,  //not found error
                    ForbiddenException => (int)HttpStatusCode.Forbidden,   // unhandled error
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var result = JsonSerializer.Serialize(new { message = error?.Message});
                await response.WriteAsync(result);
            }
        }
    }
}
