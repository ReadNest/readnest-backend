using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadNest.Application.Models.Requests.Comment
{
    public class ReportCommentRequest
    {
        public Guid CommentId { get; set; }
        public string ModerationReason { get; set; }
    }
}
