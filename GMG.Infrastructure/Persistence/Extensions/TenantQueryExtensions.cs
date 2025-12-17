using GMG.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure.Persistence.Extensions
{
    public static class TenantQueryExtensions
    {
        // Ignorar TODOS los filtros (ej: admin global del sistema)
        public static IQueryable<T> WithoutTenantFilter<T>(this IQueryable<T> query)
            where T : class
        {
            return query.IgnoreQueryFilters();
        }

        // Consultar una sucursal específica (útil para Owner que quiere ver solo una)
        public static IQueryable<T> ForSpecificBranch<T>(this IQueryable<T> query, Guid branchId)
            where T : OwnBranch
        {
            return query.IgnoreQueryFilters()
                .Where(e => e.BranchId == branchId);
        }

        // Nuevo: Filtrar múltiples sucursales específicas
        public static IQueryable<T> ForBranches<T>(this IQueryable<T> query, List<Guid> branchIds)
            where T : OwnBranch
        {
            return query.IgnoreQueryFilters()
                .Where(e => branchIds.Contains(e.BranchId));
        }
    }
}
