namespace ReadNest.Application.Models.Requests.CommentLike
{
    public class CreateCommentLikeRequest
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
