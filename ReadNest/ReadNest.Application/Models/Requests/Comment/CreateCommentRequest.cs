namespace ReadNest.Application.Models.Requests.Comment
{
    public class CreateCommentRequest
    {
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
