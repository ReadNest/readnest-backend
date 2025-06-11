namespace ReadNest.Application.Models.Requests.Book
{
    public class UpdateBookImageRequest
    {
        public string? BookImageId { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
