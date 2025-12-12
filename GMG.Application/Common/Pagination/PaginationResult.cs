using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Common.Pagination
{
    public class PaginationResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
