using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Account.Queries.GetBranchUser
{
    public class GetBranchUserHandler(IBranchUserRepository branchUserRepository, IUnitOfWork unitOfWork) : IRequestHandler<GetBranchUserQuery, BranchUserDto?>
    {
        public async Task<BranchUserDto?> Handle(GetBranchUserQuery request, CancellationToken cancellationToken)
        {
            var branchUser = await branchUserRepository.GetByEmailAsync(request.Email);
            if (branchUser is not null && VerifyPassword(request.Password, branchUser.PasswordHash))
            {
                branchUser.SetLastLogin();
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return new BranchUserDto
                (
                    branchUser.Id.ToString(),
                    branchUser.OwnerId.ToString(),
                    branchUser.BranchId.ToString(),
                    branchUser.Email,
                    branchUser.Username,
                    branchUser.FirstName,
                    branchUser.LastName,
                    branchUser.Branch?.Name ?? "",
                    branchUser.Role.ToString()
                );
            }

            return null;
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
