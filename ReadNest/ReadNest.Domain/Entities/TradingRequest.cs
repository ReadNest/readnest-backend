using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class TradingRequest : BaseEntity<Guid>
    {
        public Guid TradingPostId { get; set; }
        public TradingPost TradingPost { get; set; }
        public Guid RequesterId { get; set; }
        public User Requester { get; set; }
        public string Status { get; set; } // e.g., Pending, Accepted, Rejected
    }
}
