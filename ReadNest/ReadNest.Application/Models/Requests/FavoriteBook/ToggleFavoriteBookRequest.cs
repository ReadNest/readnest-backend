
namespace ReadNest.Application.Models.Requests.FavoriteBook
{
    public class ToggleFavoriteBookRequest
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
