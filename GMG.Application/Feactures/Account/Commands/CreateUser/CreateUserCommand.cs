
using GMG.Application.Feactures.Account.Dtos;
using GMG.Domain.Common.Result;
using MediatR;

namespace GMG.Application.Feactures.Account.Commands.CreateUser
{
    public record CreateUserCommand(
        string Username, 
        string Email, 
        string Password,
        string FirstName,
        string LastName) : IRequest<Result<UserDto>>;
}
