using ReadNest.Application.Models.Responses.User;

namespace ReadNest.Application.Models.Responses.Post
{
    public class GetPostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public virtual Domain.Entities.Book Book { get; set; }
        public virtual GetUserResponse Creator { get; set; }
        public int Views { get; set; }
        public int LikesCount { get; set; }
        public ICollection<string> UserLikes { get; set; }
    }
}
