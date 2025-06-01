namespace ReadNest.Application.Models.Requests.Post
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
