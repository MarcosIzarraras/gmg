using GMG.Application.Feactures.Account.Dtos;
using MediatR;

namespace GMG.Application.Feactures.Account.Queries.GetUser
{
    public record GetUserQuery(string Email, string Password) : IRequest<UserDto?>;
}
