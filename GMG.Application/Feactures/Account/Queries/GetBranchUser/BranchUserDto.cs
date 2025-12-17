using GMG.Domain.Branches.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Account.Queries.GetBranchUser
{
    public record BranchUserDto(
        string Id,
        string OwnerId,
        string BranchId,
        string Email,
        string Username,
        string FirstName, 
        string LastName,
        string BranchName,
        string BranchUserRole);
}
