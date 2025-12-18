using GMG.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure.Persistence.Extensions
{
    public static class TenantQueryExtensions
    {
        public static IQueryable<T> WithoutTenantFilter<T>(this IQueryable<T> query)
            where T : class
        {
            return query.IgnoreQueryFilters();
        }

        public static IQueryable<T> ForSpecificBranch<T>(this IQueryable<T> query, Guid branchId)
            where T : OwnBranch
        {
            return query.IgnoreQueryFilters()
                .Where(e => e.BranchId == branchId);
        }

        public static IQueryable<T> ForBranches<T>(this IQueryable<T> query, List<Guid> branchIds)
            where T : OwnBranch
        {
            return query.IgnoreQueryFilters()
                .Where(e => branchIds.Contains(e.BranchId));
        }
    }
}
