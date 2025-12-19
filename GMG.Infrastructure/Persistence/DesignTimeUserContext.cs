using GMG.Application.Common.Interfaces;

namespace GMG.Infrastructure.Persistence
{
    public class DesignTimeUserContext : IUserContext
    {
        public Guid UserId => Guid.Empty;
        public Guid OwnerId => Guid.Empty;
        public Guid? BranchId => null;
        public Guid? BranchUserId => null;
        public bool IsAuthenticated => true;

        public string UserName => string.Empty;

        public string UserType => string.Empty;

        public string Email => string.Empty;

        bool IsOwner => true;

        bool IUserContext.IsOwner => IsOwner;

        bool IsBranchUser => false;

        bool IUserContext.IsBranchUser => IsBranchUser;
    }
}
