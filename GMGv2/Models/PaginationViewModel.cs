namespace GMGv2.Models
{
    public class PaginationViewModel
    {
        private const int radius = 2;
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int Start => Math.Max(1, Page - radius);
        public int End => Math.Min(TotalPages, Page + radius);

        public PaginationViewModel(int page, int totalPages)
        {
            Page = page;
            TotalPages = totalPages;
        }
    }
}
