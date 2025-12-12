using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
