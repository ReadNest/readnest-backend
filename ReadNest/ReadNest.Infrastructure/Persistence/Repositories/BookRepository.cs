using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;
using ReadNest.Shared.Common;
using ReadNest.Shared.Utils;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class BookRepository(AppDbContext context) : GenericRepository<Book, Guid>(context), IBookRepository
    {
        public async Task<PagingResponse<GetBookSearchResponse>> FilterBooks(BookFilterRequest request)
        {
            var query = _context.Books
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Include(b => b.Categories)
                .AsQueryable();

            var normalizedKeyword = StringUtil.NormalizeKeyword(request.Keyword);

            query = query.Where(b => normalizedKeyword == null
                                     || b.TitleNormalized.Contains(normalizedKeyword)
                                     || b.AuthorNormalized.Contains(normalizedKeyword));

            if (request.CategoryIds != null && request.CategoryIds.Count != 0)
            {
                query = query.Where(b =>
                    request.CategoryIds.All(filterCatId => b.Categories.Any(c => c.Id == filterCatId)));
            }

            if (request.LanguageIds != null && request.LanguageIds.Any())
            {
                query = query.Where(b => request.LanguageIds.Contains(b.Language));
            }

            var totalRecords = query.Count(x => !x.IsDeleted);

            var books = await query
                .OrderByDescending(b => b.AvarageRating)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var data = books.Select(book => new GetBookSearchResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ShortDescription = book.Description.Length > 50 ? book.Description.Substring(0, 50) + "..." : book.Description,
                AverageRating = book.AvarageRating,
                ImageUrl = book.ImageUrl
            });

            return new PagingResponse<GetBookSearchResponse>
            {
                Items = data,
                TotalItems = totalRecords,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        public async Task<IEnumerable<Category>> GetCategoriesByBookIds(List<Guid> bookIds)
        {
            return await _context.Categories.AsNoTracking().Where(x => bookIds.Contains(x.Id) && !x.IsDeleted).ToListAsync();
        }

        public async Task<List<Book>> RecommendFromFavoritesBooksAsync(Guid userId)
        {
            var sqlQuery = @"
                                WITH table1 AS (
                                    SELECT bc.category_id, bc.book_id 
                                    FROM favorite_books fb 
                                    JOIN book_categories bc ON (fb.book_id = bc.book_id)
                                    WHERE fb.user_id = @UserId
                                ), table2 AS (
                                    SELECT bc.book_id
                                    FROM book_categories bc 
                                    JOIN table1 t1 ON (bc.category_id = t1.category_id AND bc.book_id <> t1.book_id)
                                )
                                SELECT *
                                FROM books b
                                JOIN table2 t ON (b.id = t.book_id)
                                WHERE b.""IsDeleted"" = false";

            var books = await _context.Books
                    .FromSqlRaw(sqlQuery, new NpgsqlParameter("@UserId", userId))
                    .AsNoTracking()
                    .ToListAsync();

            return books;
        }
    }
}
