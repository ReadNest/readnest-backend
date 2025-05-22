
namespace ReadNest.Application.Models.Responses.FavoriteBook
{
    public class ToggleFavoriteBookResponse
    {
        public Guid BookId { get; set; }
        public bool IsFavorited { get; set; }
    }

}
