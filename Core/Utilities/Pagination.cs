using System.Collections.Generic;

namespace Core.Utilities
{
    /// <summary>
    /// Paging utility
    /// <param name="Page">Current page.</param>
    /// <param name="PageCount">Number of items per page.</param>
    /// <param name="Count">Total number of items.</param>
    /// <param name="Data">Data we wish to present in the UI.</param>
    /// </summary>
    public class Pagination<T> where T : class
    {
        public Pagination(int page, int pageCount, int count, IEnumerable<T> data)
        {
            Page = page;
            PageCount = pageCount;
            Count = count;
            Data = data;
        }

        public int Page { get; set; }
        public int PageCount { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}