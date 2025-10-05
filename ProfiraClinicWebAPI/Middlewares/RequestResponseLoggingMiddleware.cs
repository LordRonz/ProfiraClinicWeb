using Serilog;
using System.Text;

namespace ProfiraClinicWebAPI.Middlewares
{
   
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log request
            context.Request.EnableBuffering(); // allow multiple reads
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            Log.Information("HTTP Request {Method} {Path} | Body: {Body}",
                context.Request.Method,
                context.Request.Path,
                requestBody);

            // Copy original response stream
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Continue pipeline
            await _next(context);

            // Log response
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            Log.Information("HTTP Response {StatusCode} | Body: {Body}",
                context.Response.StatusCode,
                responseText);

            // Copy response back to original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

}
