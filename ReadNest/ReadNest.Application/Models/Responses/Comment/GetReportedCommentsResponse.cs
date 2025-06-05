using ReadNest.Application.Models.Responses.CommentReport;
using ReadNest.Application.Models.Responses.User;

namespace ReadNest.Application.Models.Responses.Comment
{
    public class GetReportedCommentsResponse
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public GetUserResponse Commenter { get; set; }
        public List<CommentReportReponse> Reports { get; set; }
    }
}
