namespace ReadNest.Application.Models.Responses.Book
{
    public class GetBookImageResponse
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
