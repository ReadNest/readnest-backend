using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadNest.Application.Models.Requests.CommentReport
{
    public class CreateCommentReportRequest
    {
        public Guid CommentId { get; set; }
        public Guid ReporterId { get; set; }
        public string Reason { get; set; }
    }
}
