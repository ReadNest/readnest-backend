using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class UserBadge : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid BadgeId { get; set; }
        public Badge Badge { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
