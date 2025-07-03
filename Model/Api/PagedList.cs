namespace ProfiraClinic.Models.Api
{
    public class PagedList<T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public List<T> Items { get; set; }

        public PagedList(int totalCount, int page, int pageSize, List<T> items)
        {
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            Items = items ?? new List<T>();
        }

    }
}
