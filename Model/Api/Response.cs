using ProfiraClinic.Models.Core;

namespace ProfiraClinic.Models.Api
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public ErrorType? ErrorType { get; set; }

        public Response(int statusCode, string message, T? data = default, ErrorType? errorType = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            ErrorType = errorType;
        }
    }
}
