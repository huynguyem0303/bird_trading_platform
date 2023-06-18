namespace BirdTrading.Utils.Pagination
{
    public class Pagination<T>
    {
        public int TotalItemsCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPagesCount
        {
            get
            {
                return (int)Math.Ceiling((double)(TotalItemsCount / PageSize));
            }
        }
        public int PageIndex { get; set; }

        public bool HasNextPage { get { return PageIndex + 1 <= TotalPagesCount; } }
        public bool HasPreviousPage { get { return PageIndex > 0; } }

        public ICollection<T> Items { get; set; }
    }
}
