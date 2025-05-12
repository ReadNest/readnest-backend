using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class AffiliateLink : BaseEntity<Guid>
    {
        public string Link { get; set; }
        public string PartnerName { get; set; }
        public Guid BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
