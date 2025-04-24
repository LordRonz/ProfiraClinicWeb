using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinicWebAPI.Model;

namespace ProfiraClinicWebAPI.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Optionally log the exception here
            System.Diagnostics.Debug.WriteLine(context.Exception, "An unhandled exception occurred.");

            var apiResponse = new ApiResponse<object>(
                500,
                context.Exception.Message // In production, consider a generic error message
            );

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = 500,
            };

            context.ExceptionHandled = true;
        }
    }
}
