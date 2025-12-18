using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Branches.Entities;
using GMG.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure.Persistence.Repositories
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        private readonly AppDbContext _db;
        public BranchRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public Task<Branch?> GetMainBranch(Guid id)
            => _db.Branches.WithoutTenantFilter().FirstOrDefaultAsync(b => b.IsMainBranch && b.OwnerId == id);
    }
}
