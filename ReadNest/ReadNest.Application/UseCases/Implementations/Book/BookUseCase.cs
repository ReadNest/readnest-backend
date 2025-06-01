using System.Linq.Expressions;
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
using ReadNest.Shared.Utils;

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
                TitleNormalized = HtmlUtil.NormalizeTextWithoutHtml(request.Title),
                Author = request.Author,
                AuthorNormalized = HtmlUtil.NormalizeTextWithoutHtml(request.Author),
                Description = request.Description,
                DescriptionNormalized = HtmlUtil.NormalizeDescription(request.Description),
                AvarageRating = request.Rating,
                ISBN = request.ISBN,
                ImageUrl = request.ImageUrl,
                Language = request.Language,
                Categories = (await _categoryRepository.FindAsync(
                            predicate: query => request.CategoryIds.Contains(query.Id) && query.IsDeleted == false,
                            asNoTracking: false))
                            .ToList(),
                BookImages = request.BookImages.Select(x => new Domain.Entities.BookImage
                {
                    ImageUrl = x.ImageUrl,
                    Order = x.Order
                }).ToList(),
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

        public async Task<ApiResponse<PagingResponse<GetBookSearchResponse>>> FilterBooksAsync(BookFilterRequest request)
        {

            var response = await _bookRepository.FilterBooks(request);
            return ApiResponse<PagingResponse<GetBookSearchResponse>>.Ok(response, MessageId.I0000);
        }

        public async Task<ApiResponse<PagingResponse<GetBookResponse>>> GetAllAsync(PagingRequest request)
        {
            var books = await _bookRepository.FindWithIncludePagedAsync(
                predicate: b => !b.IsDeleted,
                include: query => query
                    .Include(b => b.Categories)
                    .Include(b => b.AffiliateLinks)
                    .Include(b => b.BookImages),
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
                AffiliateLinks = book.AffiliateLinks.Select(a => new GetAffiliateLinkResponse
                {
                    Id = a.Id,
                    AffiliateLink = a.Link,
                    PartnerName = a.PartnerName
                }).ToList(),
                BookImages = book.BookImages.Select(bi => new GetBookImageResponse
                {
                    Id = bi.Id,
                    Order = bi.Order,
                    ImageUrl = bi.ImageUrl,
                }).OrderBy(bi => bi.Order).ToList(),
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
                    .Include(b => b.AffiliateLinks)
                    .Include(b => b.BookImages),
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
                    AffiliateLink = a.Link
                }).ToList(),
                BookImages = book.BookImages.Select(bi => new GetBookImageResponse
                {
                    Id = bi.Id,
                    Order = bi.Order,
                    ImageUrl = bi.ImageUrl,
                }).OrderBy(bi => bi.Order).ToList(),
            };

            return ApiResponse<GetBookResponse>.Ok(response);
        }

        public async Task<ApiResponse<PagingResponse<GetBookSearchResponse>>> SearchBooksAsync(PagingRequest paging, string? keyword)
        {
            var normalizedKeyword = StringUtil.NormalizeKeyword(keyword);

            Expression<Func<Domain.Entities.Book, bool>> filter = b =>
                                    !b.IsDeleted &&
                                    (normalizedKeyword == null
                                     || b.TitleNormalized.Contains(normalizedKeyword)
                                     || b.AuthorNormalized.Contains(normalizedKeyword));

            Func<IQueryable<Domain.Entities.Book>, IOrderedQueryable<Domain.Entities.Book>> orderBy = q => q.OrderByDescending(b => b.AvarageRating);

            Func<IQueryable<Domain.Entities.Book>, IQueryable<Domain.Entities.Book>> include = q => q
                .Include(b => b.Categories)
                .Include(b => b.AffiliateLinks)
                .Include(b => b.BookImages)
                .Include(b => b.FavoriteBooks);

            var pagingResult = await _bookRepository.FindPagedAsync(
                                predicate: filter,
                                pageNumber: paging.PageIndex,
                                pageSize: paging.PageSize,
                                include: include,
                                orderBy: orderBy,
                                asNoTracking: true);

            var searchResponses = pagingResult.Items.Select(book => new GetBookSearchResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ShortDescription = book.Description.Length > 50 ? book.Description.Substring(0, 50) + "..." : book.Description,
                AverageRating = book.AvarageRating,
                ImageUrl = book.ImageUrl
            });

            var pagingResponse = new PagingResponse<GetBookSearchResponse>(
                items: searchResponses,
                totalCount: pagingResult.TotalItems,
                pageIndex: pagingResult.PageIndex,
                pageSize: pagingResult.PageSize);

            return ApiResponse<PagingResponse<GetBookSearchResponse>>.Ok(pagingResponse);
        }
    }
}
