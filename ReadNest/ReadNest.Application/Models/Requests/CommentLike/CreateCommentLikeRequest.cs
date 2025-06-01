using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadNest.Application.Models.Requests.CommentLike
{
    public class CreateCommentLikeRequest
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
