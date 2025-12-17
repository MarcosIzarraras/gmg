namespace GMG.Domain.Common
{
    public abstract class OwnBranch : OwnUser
    {
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; } // BranchUser
        public DateTime UpdatedAt { get; set; } // BranchUser
        public Guid UpdatedBy { get; set; }
        public Guid BranchId { get; set; }
    }
}
