using GMG.Domain.Common;
using GMG.Domain.Common.Result;

namespace GMG.Domain.Branches.Entities
{
    public class Branch : OwnUser
    {
        public string Name { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public string? Address { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Email { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsMainBranch { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdateAt { get; private set; }

        // Navigation Properties
        private List<BranchUser> _branchUsers = new();
        public IReadOnlyCollection<BranchUser> BranchUsers
            => _branchUsers.AsReadOnly();

        private Branch() { }
        private Branch(
            Guid userId,
            string name,
            string code,
            string? address,
            string? phoneNumber,
            string? email,
            bool isActive,
            bool isMainBranch)
        {
            OwnerId = userId;
            Name = name;
            Code = code;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            IsActive = isActive;
            IsMainBranch = isMainBranch;
            CreatedAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;
        }

        public static Result<Branch> CreateDefaultBranchForUser(
            Guid userId,
            string name,
            string code)
        {
            return new Validation()
                .Required(!string.IsNullOrWhiteSpace(name), "Branch name cannot be empty.")
                .Required(!string.IsNullOrEmpty(code), "Branch code cannot be empty.")
                .Build(() => new Branch(
                    userId,
                    name,
                    code,
                    null,
                    null,
                    null,
                    true,
                    true));
        }
    }
}
