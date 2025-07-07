namespace ProfiraClinic.Models.Api
{
    public class Pagination<T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public List<T> Items { get; set; } = new();
    }

}
