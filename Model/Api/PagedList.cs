namespace ProfiraClinic.Models.Api
{
    public class PagedList<T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public List<T> Items { get; set; } = [];

    }
}
