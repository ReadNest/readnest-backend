namespace ReadNest.Application.Models.Requests.CommentReport
{
    public class CreateCommentReportRequest
    {
        public Guid CommentId { get; set; }
        public Guid ReporterId { get; set; }
        public string Reason { get; set; }
    }
}
