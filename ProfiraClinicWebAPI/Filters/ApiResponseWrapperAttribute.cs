using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProfiraClinicWebAPI.Model;

namespace ProfiraClinicWebAPI.Filters
{
    public class ApiResponseWrapperAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                int statusCode = objectResult.StatusCode ?? 200;

                string message = "Success";

                if (statusCode >= 400)
                {
                    if (objectResult.Value is string str)
                    {
                        message = str;
                    }
                    else if (objectResult.Value is Exception ex)
                    {
                        message = ex.Message;
                    }
                    else if (objectResult.Value is IDictionary<string, object> dict && dict.TryGetValue("error", out var errorVal))
                    {
                        message = errorVal?.ToString() ?? "Error";
                    }
                    else if (objectResult.Value?.GetType().GetProperty("error") is not null)
                    {
                        var errorProp = objectResult.Value.GetType().GetProperty("error");
                        message = (errorProp?.GetValue(objectResult.Value))?.ToString() ?? "Error";
                    }
                    else
                    {
                        message = "Error";
                    }
                }

                var apiResponse = new ApiResponse<object>(statusCode, message, objectResult.Value);
                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = statusCode
                };
            }
            else if (context.Result is EmptyResult)
            {
                var apiResponse = new ApiResponse<object>(204, "Success");
                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = 204
                };
            }

            base.OnActionExecuted(context);
        }


    }
}
