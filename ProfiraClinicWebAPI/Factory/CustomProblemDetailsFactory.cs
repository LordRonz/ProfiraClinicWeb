using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace ProfiraClinicWebAPI.Factory
{

    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;

        public CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
        {
            _options = options.Value;
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            statusCode ??= 500;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title ?? ReasonPhrases.GetReasonPhrase(statusCode.Value),
                Type = type ?? $"https://tools.ietf.org/html/rfc9110#section-15.{statusCode}",
                Detail = detail,
                Instance = instance ?? httpContext?.Request?.Path
            };

            problemDetails.Extensions["errorType"] = MapErrorType(statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            statusCode ??= 400;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Title = title ?? ReasonPhrases.GetReasonPhrase(statusCode.Value),
                Type = type ?? $"https://tools.ietf.org/html/rfc9110#section-15.{statusCode}",
                Detail = detail,
                Instance = instance ?? httpContext?.Request?.Path
            };

            problemDetails.Extensions["errorType"] = MapErrorType(statusCode.Value);

            return problemDetails;
        }

        private string MapErrorType(int statusCode) => statusCode switch
        {
            400 => "WRONG_STRUCTURE",
            401 => "UNAUTHORIZED",
            403 => "FORBIDDEN",
            404 => "NOT_FOUND",
            500 => "INTERNAL_ERROR",
            _ => "UNKNOWN"
        };
    }


}
