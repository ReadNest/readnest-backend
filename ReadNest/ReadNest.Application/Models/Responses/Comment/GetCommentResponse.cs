using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.User;

namespace ReadNest.Application.Models.Responses.Comment
{
    public class GetCommentResponse
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public GetBookResponse Book { get; set; }
        public virtual GetUserResponse Creator { get; set; }
        public virtual string CreatorName { get; set; }
        // Lưu list Id của user đã lưu
        public ICollection<string> UserLikes { get; set; }
        public int NumberOfLikes { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
