namespace ProfiraClinicWeb.Helpers
{
    public class ApiResponse<T>
    {
        public ApiResponse(int statusCode, string v)
        {
            StatusCode = statusCode;
            this.Message = v;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

}
