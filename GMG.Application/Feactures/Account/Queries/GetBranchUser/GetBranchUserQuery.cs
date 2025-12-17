using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Account.Queries.GetBranchUser
{
    public record GetBranchUserQuery(string Email, string Password) : IRequest<BranchUserDto?>;
}
