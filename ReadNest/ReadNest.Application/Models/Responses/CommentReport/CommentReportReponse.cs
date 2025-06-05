using ReadNest.Application.Models.Responses.User;

namespace ReadNest.Application.Models.Responses.CommentReport
{
    public class CommentReportReponse
    {
        public Guid CommentReportId { get; set; }
        public Guid CommentId { get; set; }
        public GetUserResponse Reporter { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
