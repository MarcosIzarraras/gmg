using GMG.Application.Common.Persistence;
using GMG.Application.Common.Persistence.Repositories;
using GMG.Application.Feactures.Account.Dtos;
using GMG.Domain.Branches.Entities;
using GMG.Domain.Common.Result;
using GMG.Domain.Users.Entities;
using GMG.Domain.Users.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Account.Commands.CreateUser
{
    public class CreateUserHandler(IUserRepository userRepository, IRepository<Branch> branchRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = User.Create(
                request.Username, 
                request.Email, 
                BCrypt.Net.BCrypt.HashPassword(request.Password), 
                request.FirstName, 
                request.LastName, 
                UserRole.User);

            if (user.IsFailure)
                return Result<UserDto>.Failure(user.Error);

            var branch = Branch.CreateDefaultBranchForUser(user.Value.Id, "Default Branch", "0001");

            if (branch.IsFailure)
                return Result<UserDto>.Failure(branch.Error);

            await userRepository.AddAsync(user.Value);
            await branchRepository.AddAsync(branch.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var userDto = new UserDto
            (
                user.Value.Id,
                user.Value.Username,
                user.Value.Email,
                user.Value.FirstName,
                user.Value.LastName,
                user.Value.UserRole,
                branch.Value.Id.ToString(),
                branch.Value.Name
            );

            return Result<UserDto>.Success(userDto);
        }
    }
}
