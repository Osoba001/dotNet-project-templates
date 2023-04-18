using KO.WebAPI.Models;
using System.Net;

namespace KO.WebAPI.Middlewares
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new ErrorDetail
                {
                    StatusCode = context.Response.StatusCode,
                    Message = $"Internal Serval Error: {ex.Message}."
                }.ToString());
               
            }
        }
    }
}
