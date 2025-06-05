using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Book : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string TitleNormalized { get; set; }
        public string Author { get; set; }
        public string AuthorNormalized { get; set; }
        public string ImageUrl { get; set; }
        public double AvarageRating { get; set; }
        public string Description { get; set; }
        public string DescriptionNormalized { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public ICollection<FavoriteBook> FavoriteBooks { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<AffiliateLink> AffiliateLinks { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<BookImage> BookImages { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<TradingPost> TradingPosts { get; set; }
    }
}
