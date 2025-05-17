using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.AffiliateLink;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.Category;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Book;
using ReadNest.Application.Validators.Book;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Book
{
    public class BookUseCase : IBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly CreateBookRequestValidator _validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bookRepository"></param>
        /// <param name="categoryRepository"></param>
        /// <param name="validator"></param>
        public BookUseCase(IBookRepository bookRepository, ICategoryRepository categoryRepository, CreateBookRequestValidator validator)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _validator = validator;
        }

        public async Task<ApiResponse<GetBookResponse>> CreateBookAsync(CreateBookRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);
            var book = new Domain.Entities.Book
            {
                Title = request.Title,
                Author = request.Author,
                Description = request.Description,
                AvarageRating = request.Rating,
                ISBN = request.ISBN,
                ImageUrl = request.ImageUrl,
                Language = request.Language,
                Categories = (await _categoryRepository.FindAsync(
                            predicate: query => request.CategoryIds.Contains(query.Id) && query.IsDeleted == false,
                            asNoTracking: false))
                            .ToList()
            };

            _ = await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            var response = new GetBookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ImageUrl = book.ImageUrl,
                AverageRating = book.AvarageRating,
                Description = book.Description,
                ISBN = book.ISBN,
                Language = book.Language,
                Categories = book.Categories.Select(c => new GetCategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                AffiliateLinks = [],
                FavoriteCount = 0
            };

            return ApiResponse<GetBookResponse>.Ok(response);
        }

        public async Task<ApiResponse<PagingResponse<GetBookResponse>>> GetAllAsync(PagingRequest request)
        {
            var books = await _bookRepository.FindWithIncludePagedAsync(
                predicate: b => !b.IsDeleted,
                include: query => query
                    .Include(b => b.Categories),
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                asNoTracking: true);

            var bookResponses = books.Select(book => new GetBookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ImageUrl = book.ImageUrl,
                AverageRating = book.AvarageRating,
                Description = book.Description,
                ISBN = book.ISBN,
                Language = book.Language,
                Categories = book.Categories.Select(c => new GetCategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                AffiliateLinks = [],
                FavoriteCount = book.FavoriteBooks?.Count ?? 0
            }).ToList();

            if (bookResponses.Count == 0)
            {
                return ApiResponse<PagingResponse<GetBookResponse>>.Fail(MessageId.E0005);
            }

            var pagingResponse = new PagingResponse<GetBookResponse>
            {
                Items = bookResponses,
                TotalItems = await _bookRepository.CountAsync(b => !b.IsDeleted),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return ApiResponse<PagingResponse<GetBookResponse>>.Ok(pagingResponse);
        }

        public async Task<ApiResponse<GetBookResponse>> GetByIdAsync(Guid bookId)
        {
            var books = await _bookRepository.FindWithIncludeAsync(
                predicate: b => b.Id == bookId && !b.IsDeleted,
                include: query => query
                    .Include(b => b.Categories)
                    .Include(b => b.AffiliateLinks),
                asNoTracking: true);

            var book = books.FirstOrDefault();

            if (book == null)
            {
                return ApiResponse<GetBookResponse>.Fail(MessageId.E0005);
            }

            var response = new GetBookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ImageUrl = book.ImageUrl,
                AverageRating = book.AvarageRating,
                Description = book.Description,
                ISBN = book.ISBN,
                Language = book.Language,
                Categories = book.Categories.Select(c => new GetCategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                AffiliateLinks = book.AffiliateLinks.Select(a => new GetAffiliateLinkResponse
                {
                    Id = a.Id,
                    PartnerName = a.PartnerName,
                    Link = a.Link
                }).ToList(),
            };

            return ApiResponse<GetBookResponse>.Ok(response);
        }

    }
}
