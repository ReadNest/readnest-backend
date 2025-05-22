using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.FavoriteBook;
using ReadNest.Application.Models.Responses.FavoriteBook;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.FavoriteBook;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.FavoriteBook
{
    public class FavoriteBookUseCase : IFavoriteBookUseCase
    {
        private readonly IFavoriteBookRepository _favoriteBookRepository;

        public FavoriteBookUseCase(IFavoriteBookRepository favoriteBookRepository)
        {
            _favoriteBookRepository = favoriteBookRepository;
        }

        public async Task<ApiResponse<ToggleFavoriteBookResponse>> ToggleFavoriteAsync(ToggleFavoriteBookRequest request)
        {
            var existing = await _favoriteBookRepository.GetByUserAndBookAsync(request.UserId, request.BookId);
            bool isFavorited;

            if (existing == null)
            {
                var newFavorite = new Domain.Entities.FavoriteBook
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    BookId = request.BookId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _favoriteBookRepository.AddAsync(newFavorite);
                isFavorited = true;
            }
            else
            {
                await _favoriteBookRepository.SoftDeleteAsync(existing);
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

        public async Task<ApiResponse<List<Guid>>> GetFavoriteBookIdsByUserAsync(Guid userId)
        {
            // Implementation for getting favorite book IDs by user
            throw new NotImplementedException();
        }
    }
}
