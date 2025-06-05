namespace ReadNest.Application.Models.Requests.Comment
{
    public class ReportCommentRequest
    {
        public Guid CommentId { get; set; }
        public string ModerationReason { get; set; }
    }
}
