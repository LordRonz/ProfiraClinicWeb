using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ProfiraClinic.Models.Core;

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
                Title = title ?? ReasonPhrases.GetReasonPhrase(statusCode.Value),
                Type = type ?? $"https://tools.ietf.org/html/rfc9110#section-15.{statusCode}",
                Detail = detail,
                Instance = instance ?? httpContext?.Request?.Path
            };

            problemDetails.Extensions["errorType"] = MapErrorType(statusCode.Value);
            problemDetails.Extensions["statusCode"] = statusCode;

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
                Title = title ?? ReasonPhrases.GetReasonPhrase(statusCode.Value),
                Type = type ?? $"https://tools.ietf.org/html/rfc9110#section-15.{statusCode}",
                Detail = detail,
                Instance = instance ?? httpContext?.Request?.Path
            };

            problemDetails.Extensions["errorType"] = MapErrorType(statusCode.Value).ToString();
            problemDetails.Extensions["statusCode"] = statusCode;

            return problemDetails;
        }

        private ErrorType MapErrorType(int statusCode) => statusCode switch
        {
            400 => ErrorType.WRONG_STRUCTURE,
            401 => ErrorType.UNAUTHORIZED,
            403 => ErrorType.FORBIDDEN,
            404 => ErrorType.NOT_FOUND,
            500 => ErrorType.SERVER_ERROR,
            _ => ErrorType.UNKNOWN
        };
    }


}
