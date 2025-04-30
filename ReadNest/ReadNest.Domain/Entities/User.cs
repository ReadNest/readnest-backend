using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string HashPasswod { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
