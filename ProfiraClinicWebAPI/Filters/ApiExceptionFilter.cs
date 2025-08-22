using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProfiraClinic.Models.Core;
using ProfiraClinicWebAPI.Model;

namespace ProfiraClinicWebAPI.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            System.Diagnostics.Debug.WriteLine(exception, "An unhandled exception occurred.");

            // Default values
            var statusCode = 200;
            var errorType = ErrorType.SERVER_ERROR;
            var message = exception.Message;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = 403;
                    errorType = ErrorType.FORBIDDEN;
                    message = "You do not have permission to perform this action.";
                    break;

                case ArgumentException:
                    statusCode = 400;
                    errorType = ErrorType.WRONG_STRUCTURE;
                    message = "Invalid request structure.";
                    break;

                case KeyNotFoundException:
                    statusCode = 404;
                    errorType = ErrorType.NOT_FOUND;
                    message = "Resource not found.";
                    break;

                case FormatException:
                    statusCode = 400;
                    errorType = ErrorType.VALIDATION_ERROR;
                    message = "Invalid format.";
                    break;

                default:
                    errorType = ErrorType.SERVER_ERROR;
                    break;
            }

            var apiResponse = new ApiResponse<object>(
                1,
                message,
                new { innerException = exception.InnerException?.StackTrace, stackTrace = exception.StackTrace },
                errorType
            );

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = statusCode,
            };

            context.ExceptionHandled = true;
        }
    }
}
