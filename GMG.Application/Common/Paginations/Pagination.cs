using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Common.Paginations
{
    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Skip => (Page - 1) * PageSize;
        public string? Search { get; set; }
    }
}
