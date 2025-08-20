namespace InventoryManagement.Domain.Common
{
    public class PaginationResponse<T>
    {
        public PaginationResponse(IEnumerable<T> items, int totalItemsCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalItemsCount;
            TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
        }

        public int TotalPages { get; set; }
        public int TotalItemsCount { get; }
        public int ItemsFrom { get; }
        public int ItemsTo { get; }
        public IEnumerable<T> Items { get; set; }
    }
}
