using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Branches.Entities;
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

        public Task<Branch?> GetMainBranch(Guid ownerId)
            => _db.Branches.FirstOrDefaultAsync(b => b.OwnerId == ownerId && b.IsMainBranch);
    }
}
