using GMG.Application.Common.Persistence.Repositories;
using GMG.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public Task<User?> GetByEmailAsync(string email)
            => _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }
}
