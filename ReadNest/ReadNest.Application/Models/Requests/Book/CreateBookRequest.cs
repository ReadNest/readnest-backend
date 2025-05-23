namespace ReadNest.Application.Models.Requests.Book
{
    public class CreateBookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public List<Guid> CategoryIds { get; set; }
        public List<CreateBookImageRequest> BookImages { get; set; }
    }
}
