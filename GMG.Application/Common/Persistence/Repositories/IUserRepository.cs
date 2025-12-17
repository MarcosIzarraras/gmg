using GMG.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Common.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
