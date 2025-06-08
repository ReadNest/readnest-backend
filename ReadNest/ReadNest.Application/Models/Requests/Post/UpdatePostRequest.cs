namespace ReadNest.Application.Models.Requests.Post
{
    public class UpdatePostRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }

    }
}
