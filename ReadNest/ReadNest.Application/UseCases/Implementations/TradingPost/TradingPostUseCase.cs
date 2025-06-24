using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Requests.TradingPost;
using ReadNest.Application.Models.Responses.TradingPost;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.TradingPost;
using ReadNest.Application.Validators.TradingPost;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;
using ReadNest.Shared.Enums;

namespace ReadNest.Application.UseCases.Implementations.TradingPost
{
    public class TradingPostUseCase : ITradingPostUseCase
    {
        private readonly ITradingPostRepository _tradingPostRepository;
        private readonly CreateTradingPostRequestValidator _validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tradingPostRepository"></param>
        /// <param name="validator"></param>
        public TradingPostUseCase(ITradingPostRepository tradingPostRepository, CreateTradingPostRequestValidator validator)
        {
            _tradingPostRepository = tradingPostRepository;
            _validator = validator;
        }

        public async Task<ApiResponse<string>> CreateTradingPostAsync(CreateTradingPostRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);

            var tradingPost = new Domain.Entities.TradingPost
            {
                OwnerId = request.UserId,
                OfferedBookId = request.BookId,
                Condition = request.Condition ?? string.Empty,
                MessageToRequester = request.MessageToRequester ?? string.Empty,
                Title = request.Title ?? string.Empty,
                ShortDesc = request.ShortDescription ?? string.Empty,
                Status = StatusEnum.InProgress.ToString(),
                Images = request.Images.Select(x => new TradingPostImage
                {
                    ImageUrl = x.ImageUrl,
                    Order = x.Order,
                }).ToList()
            };

            _ = await _tradingPostRepository.AddAsync(tradingPost);
            await _tradingPostRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(string.Empty);
        }

        public async Task<ApiResponse<PagingResponse<GetBookTradingPostResponse>>> GetTradingPostByUserIdAsync(GetTradingPostPagingRequest request)
        {
            var tradingPosts = await _tradingPostRepository.FindWithIncludePagedAsync(
                predicate: query => !query.IsDeleted && query.OwnerId == request.UserId,
                include: query => query.Include(x => x.OfferedBook).Include(x => x.TradingRequests),
                asNoTracking: true,
                pageNumber: request.PageIndex,
                pageSize: request.PageSize);

            if (!tradingPosts.Any())
            {
                return ApiResponse<PagingResponse<GetBookTradingPostResponse>>.Fail(MessageId.E0005);
            }

            var response = new PagingResponse<GetBookTradingPostResponse>
            {
                Items = tradingPosts.Select(x => new GetBookTradingPostResponse
                {
                    Id = x.Id,
                    Author = x.OfferedBook.Author,
                    ImageUrl = x.OfferedBook.ImageUrl,
                    Title = x.OfferedBook.Title,
                    Condition = x.Condition,
                    TradingRequestIds = x.TradingRequests.Select(x => x.Id).ToList(),
                }),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = await _tradingPostRepository.CountAsync(x => !x.IsDeleted && x.OwnerId == request.UserId)
            };

            return ApiResponse<PagingResponse<GetBookTradingPostResponse>>.Ok(response);
        }
    }
}
