using ReadNest.Application.Models.Responses.AffiliateLink;
using ReadNest.Application.Models.Responses.Category;

namespace ReadNest.Application.Models.Responses.Book
{
    public class GetBookResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public List<GetCategoryResponse> Categories { get; set; }
        public List<GetAffiliateLinkResponse> AffiliateLinks { get; set; }
        public List<GetBookImageResponse> BookImages { get; set; }
        public int FavoriteCount { get; set; }
        //public List<GetCommentResponse> Comments { get; set; } = new();
    }
}
