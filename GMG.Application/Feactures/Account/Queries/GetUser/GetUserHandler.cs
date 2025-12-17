using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Application.Feactures.Account.Dtos;
using GMG.Domain.Branches.Entities;
using GMG.Domain.Users.Entities;
using MediatR;

namespace GMG.Application.Feactures.Account.Queries.GetUser
{
    public class GetUserHandler(IUserRepository userRepository, IBranchRepository branchRepository, IUnitOfWork unitOfWork) : IRequestHandler<GetUserQuery, UserDto?>
    {
        async Task<UserDto?> IRequestHandler<GetUserQuery, UserDto?>.Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);

            if (user is not null && VerifyPassword(request.Password, user.PasswordHash))
            {
                var branch = await branchRepository.GetMainBranch(user.Id);
                if (branch is null)
                {
                    var branchResult = Branch.CreateDefaultBranchForUser(user.Id, "Main Branch", "MAIN");
                    if (branchResult.IsSuccess && branchResult.Value is not null)
                    {
                        branch = branchResult.Value;
                        await branchRepository.AddAsync(branch);
                    }
                }

                user.SetLastLogin();

                await unitOfWork.SaveChangesAsync();

                return new UserDto(
                    user.Id,
                    user.Username,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.UserRole,
                    branch.Id.ToString(),
                    branch.Name);
            }

            return null;
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
