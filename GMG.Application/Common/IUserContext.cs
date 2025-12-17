namespace GMG.Application.Common
{
    public interface IUserContext
    {
        string UserName { get; }
        string UserType { get; }
        string Email { get; }
        Guid UserId { get; }
        Guid OwnerId { get; }
        Guid? BranchId { get; }
        Guid? BranchUserId { get; }
        bool IsOwner { get; }
        bool IsBranchUser { get; }
        bool IsAuthenticated { get; }
    }
}
