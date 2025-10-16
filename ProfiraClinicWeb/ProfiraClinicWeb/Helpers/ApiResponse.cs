// ProfiraClinicWeb/Helpers/ApiResponse.cs
namespace ProfiraClinicWeb.Helpers
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; }
        public string Message { get; }
        public T Data { get; }
        public string? Title { get; }

        public ApiResponse(int statusCode, string message, T data = default)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }

    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
}
