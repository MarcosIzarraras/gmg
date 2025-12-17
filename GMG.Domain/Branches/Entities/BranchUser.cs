using GMG.Domain.Common;

namespace GMG.Domain.Branches.Entities
{
    public class BranchUser : OwnBranch
    {
        public string Username { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public BranchUserRole Role { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        public Branch? Branch { get; private set; }

        public void SetLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
}
