using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Role : BaseEntity<Guid>
    {
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
