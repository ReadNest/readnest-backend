namespace ReadNest.Application.Models.Responses.Book
{
    public class GetBookSearchResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public string ShortDescription { get; set; }
        public bool IsFavorite { get; set; } = false;
    }
}
