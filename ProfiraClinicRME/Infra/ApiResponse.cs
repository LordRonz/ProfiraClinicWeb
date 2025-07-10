namespace ProfiraClinicRME.Infra
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public ResponseType? ErrorType { get; set; }

        public ApiResponse(int statusCode, string message, T data = default, ResponseType? errorType = null)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            ErrorType = errorType;
        }
    }

}
