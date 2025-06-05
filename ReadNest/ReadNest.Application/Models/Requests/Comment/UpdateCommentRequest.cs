namespace ReadNest.Application.Models.Requests.Comment
{
    public class UpdateCommentRequest
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; }
    }
}
