using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadNest.Application.Models.Requests.Comment
{
    public class CreateCommentRequest
    {
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
