using ReadNest.Application.Models.Requests.FavoriteBook;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.Category;
using ReadNest.Application.Models.Responses.FavoriteBook;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.FavoriteBook;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.FavoriteBook
{
    public class FavoriteBookUseCase : IFavoriteBookUseCase
    {
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IUserRepository _userRepository;

        public FavoriteBookUseCase(IFavoriteBookRepository favoriteBookRepository, IUserRepository userRepository)
        {
            _favoriteBookRepository = favoriteBookRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<ToggleFavoriteBookResponse>> ToggleFavoriteAsync(ToggleFavoriteBookRequest request)
        {
            var existing = await _favoriteBookRepository.GetByUserAndBookAsync(request.UserId, request.BookId);
            bool isFavorited;

            if (existing == null)
            {
                var numFav = await _favoriteBookRepository.CountFavByUser(request.UserId);
                if (numFav >= 5)
                {
                    var hasActive = await _userRepository.isActivePremium(request.UserId);
                    if (hasActive == false)
                    {
                        //return ApiResponse<ToggleFavoriteBookResponse>.Fail("Cannot add more favorite book.");
                        return new ApiResponse<ToggleFavoriteBookResponse>
                        {
                            Success = false,
                            Message = "Cannot add more favorite book.",
                            Data = new ToggleFavoriteBookResponse()
                        };
                    }
                }


                var newFavorite = new Domain.Entities.FavoriteBook
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    BookId = request.BookId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _ = await _favoriteBookRepository.AddAsync(newFavorite);
                isFavorited = true;
            }
            else
            {
                await _favoriteBookRepository.HardDeleteAsync(existing);
                isFavorited = false;
            }

            await _favoriteBookRepository.SaveChangesAsync();

            var response = new ToggleFavoriteBookResponse
            {
                BookId = request.BookId,
                IsFavorited = isFavorited
            };

            return ApiResponse<ToggleFavoriteBookResponse>.Ok(response);
        }

        public async Task<ApiResponse<PagingResponse<GetBookResponse>>> GetFavoriteBooksPagedByUserAsync(Guid userId, PagingRequest request)
        {
            var pagingResponse = await _favoriteBookRepository.GetFavoriteBooksByUserPagedAsync(userId, request.PageIndex, request.PageSize);

            if (pagingResponse.TotalItems == 0)
            {
                return ApiResponse<PagingResponse<GetBookResponse>>.Fail("No favorite books found.");
            }

            var data = new PagingResponse<GetBookResponse>
            {
                Items = pagingResponse.Items.Select(b => new GetBookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    AverageRating = b.AvarageRating,
                    Description = b.Description,
                    ISBN = b.ISBN,
                    Language = b.Language,
                    Categories = b.Categories.Select(c => new GetCategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }).ToList(),
                }).ToList(),
                TotalItems = pagingResponse.TotalItems,
                PageIndex = pagingResponse.PageIndex,
                PageSize = pagingResponse.PageSize
            };

            return ApiResponse<PagingResponse<GetBookResponse>>.Ok(data);
        }
    }
}
