using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProfiraClinic.Models.Api;

namespace ProfiraClinicWebChildAPI.Filters
{
    public class ApiResponseWrapperAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                int statusCode = objectResult.StatusCode ?? 200;
                var apiResponse = new Response<object>(statusCode, "Success", objectResult.Value);
                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = statusCode
                };
            }
            else if (context.Result is EmptyResult)
            {
                var apiResponse = new Response<object>(204, "Success");
                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = 204
                };
            }

            base.OnActionExecuted(context);
        }
    }
}
