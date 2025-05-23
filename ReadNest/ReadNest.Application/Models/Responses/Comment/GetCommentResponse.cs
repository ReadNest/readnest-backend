namespace ReadNest.Application.Models.Responses.Comment
{
    public class GetCommentResponse
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public virtual Domain.Entities.Book Book { get; set; }
        //[JsonIgnore]
        //public virtual GetUserResponse Creator { get; set; }
        public virtual string CreatorName { get; set; }
        //[JsonIgnore]
        //public ICollection<GetUserResponse> Likes { get; set; }
        public int NumberOfLikes { get; set; }

    }
}
