using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Branches.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure.Persistence.Repositories
{
    public class BranchUserRepository(AppDbContext db) : IBranchUserRepository
    {
        public Task<BranchUser?> GetByEmailAsync(string email)
            => db.BranchUsers.FirstOrDefaultAsync(bu => bu.Email == email && bu.IsActive);
    }
}
