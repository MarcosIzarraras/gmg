using GMG.Domain.Users.Entities;

namespace GMG.Domain.Common
{
    public abstract class OwnUser
    {
        // Id of the entity
        public Guid Id { get; set; }

        // Id of the owner user
        public Guid OwnerId { get; set; }

        public User? Owner { get; set; }
    }
}
