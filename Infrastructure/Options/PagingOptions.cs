namespace Infrastructure.Options
{
    public class PagingOptions
    {
        public const string Paging = "PagingOptions";
        public int DefaultPageSize { get; set; }
        public int DefaultPageNumber { get; set; }
    }
}