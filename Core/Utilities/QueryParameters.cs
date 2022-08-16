namespace Core.Utilities
{
    /// <summary>
    /// Parameters for paging, sorting, filtering, searching
    /// <param name="MaxPageCount">Maximum number of items shown per page.</param>
    /// <param name="Page">Current page.</param>
    /// <param name="PageCount">Number of items per page.</param>
    /// <param name="BranchId">Filtering by branch id.</param>
    /// <param name="CategoryId">Filters children items by category id.</param>
    /// <param name="ManufacturerId">Filters children items by manufacturer id.</param>
    /// <param name="OrderStatusId">Filtering orders by order status id.</param>
    /// <param name="TagId">Filtering children items by tag id.</param>
    /// <param name="Sort">Sorting the data.</param>
    /// <param name="Query">Searching functionality.</param>
    /// </summary>
    public class QueryParameters
    {
        private const int MaxPageCount = 50;
        public int Page { get; set; } = 1;
        private int _pageCount = MaxPageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > MaxPageCount) ? MaxPageCount : value; }
        }
        public int? ManufacturerId { get; set; }
        public int? TagId { get; set; }
        public int? CategoryId { get; set; }
        public int? BranchId { get; set; }
        public int? OrderStatusId { get; set; }
        public int? RoleId { get; set; }
        public string Sort { get; set; }
        private string _query;
        public string Query
        {
            get => _query;
            set => _query = value.ToLower();
        }
    }
}