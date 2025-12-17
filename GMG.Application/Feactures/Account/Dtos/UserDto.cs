using GMG.Domain.Users.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GMG.Application.Feactures.Account.Dtos
{
    public record UserDto(
        Guid Id, 
        string Username,
        string Email, 
        string FirstName,
        string LastName,
        UserRole UserRole,
        string BranchId,
        string BranchName
        );
}
