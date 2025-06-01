using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class CommentReport : BaseEntity<Guid>
    {
        public Guid ReporterId { get; set; } // User thực hiện báo cáo
        public Guid CommentId { get; set; } // Comment bị báo cáo
        public string Reason { get; set; }   // Lý do báo cáo
        public string Status { get; set; }   // Pending, NotViolated, Violated.
        public virtual Comment Comment { get; set; }
        public virtual User Reporter { get; set; }
    }
}
