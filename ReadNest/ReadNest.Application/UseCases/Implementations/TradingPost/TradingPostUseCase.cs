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
        private readonly ITradingRequestRepository _tradingRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly CreateTradingPostRequestValidator _validator;
        private readonly CreateTradingPostRequestV2Validator _validatorV2;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tradingPostRepository"></param>
        /// <param name="tradingRequestRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="validator"></param>
        /// <param name="validatorV2"></param>
        public TradingPostUseCase(ITradingPostRepository tradingPostRepository, ITradingRequestRepository tradingRequestRepository, IUserRepository userRepository, CreateTradingPostRequestValidator validator, CreateTradingPostRequestV2Validator validatorV2)
        {
            _tradingPostRepository = tradingPostRepository;
            _tradingRequestRepository = tradingRequestRepository;
            _userRepository = userRepository;
            _validator = validator;
            _validatorV2 = validatorV2;
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

        public async Task<ApiResponse<string>> CreateTradingPostV2Async(CreateTradingPostRequestV2 request)
        {
            await _validatorV2.ValidateAndThrowAsync(request);

            var tradingPost = new Domain.Entities.TradingPost
            {
                OwnerId = request.UserId,
                Condition = string.Empty,
                MessageToRequester = string.Empty,
                Title = string.Empty,
                ShortDesc = string.Empty,
                Status = StatusEnum.InProgress.ToString(),
                ExternalBookUrl = request.ExternalBookUrl,
                Message = request.Message,
            };

            _ = await _tradingPostRepository.AddAsync(tradingPost);
            // await _tradingPostRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(string.Empty);
        }

        public async Task<ApiResponse<string>> CreateTradingRequestAsync(CreateTradingRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<string>.Fail(messageId: Message.E0000);
            }

            var tradingPost = await _tradingPostRepository.GetByIdAsync(request.TradingPostId);
            if (tradingPost == null)
            {
                return ApiResponse<string>.Fail(messageId: Message.E0000);
            }

            var tradingRequest = new TradingRequest
            {
                TradingPostId = request.TradingPostId,
                RequesterId = request.UserId,
                Status = StatusEnum.InProgress.ToString()
            };

            _ = await _tradingRequestRepository.AddAsync(tradingRequest);
            await _tradingRequestRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(string.Empty);
        }

        public async Task<ApiResponse<string>> DeleteTradingPostAsync(Guid tradingPostId)
        {
            var tradingPost = await _tradingPostRepository.GetByIdAsync(tradingPostId);
            if (tradingPost == null)
            {
                return ApiResponse<string>.Fail(messageId: Message.E0005);
            }

            await _tradingPostRepository.SoftDeleteAsync(tradingPost);
            await _tradingPostRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(data: tradingPost.Id.ToString());
        }

        public async Task<ApiResponse<List<GetBookTradingPostV2Response>>> GetTopTradingPostsAsync(int? limit)
        {
            var tradingPosts = await _tradingPostRepository.FindWithIncludePagedAsync(
                predicate: query => !query.IsDeleted && query.Status == StatusEnum.InProgress.ToString(),
                include: query => query.Include(x => x.Owner)
                                       .Include(x => x.OfferedBook)
                                       .Include(x => x.TradingRequests)
                                       .Include(x => x.Images),
                pageNumber: 1,
                pageSize: limit.GetValueOrDefault(),
                orderBy: query => query.OrderByDescending(x => x.TradingRequests.Count())
                                       .ThenByDescending(x => x.CreatedAt));

            if (!tradingPosts.Any())
            {
                return ApiResponse<List<GetBookTradingPostV2Response>>.Fail(messageId: Message.E0005);
            }

            var response = tradingPosts.Select(x => new GetBookTradingPostV2Response
            {
                Id = x.Id,
                OwnerName = x.Owner.FullName,
                UserName = x.Owner.UserName,
                Author = x.OfferedBook.Author,
                ImageUrl = x.OfferedBook.ImageUrl,
                Title = x.OfferedBook.Title,
                Condition = x.Condition,
                MessageToRequester = x.MessageToRequester,
                ShortDesc = x.ShortDesc,
                NumberOfTradingRequests = x.TradingRequests.Count(),
                Images = x.Images.Select(x => new GetTradingPostImageResponse
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Order = x.Order,
                }).ToList()
            }).ToList();

            return ApiResponse<List<GetBookTradingPostV2Response>>.Ok(response);
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

        public async Task<ApiResponse<List<GetUserRequestResponse>>> GetUserRequestsByIdAsync(Guid tradingPostId)
        {
            var tradingRequest = (await _tradingRequestRepository.FindWithIncludeAsync(
                predicate: query => !query.IsDeleted && query.TradingPostId == tradingPostId,
                include: query => query.Include(x => x.Requester)));

            if (!tradingRequest.Any())
            {
                return ApiResponse<List<GetUserRequestResponse>>.Fail(messageId: Message.E0005);
            }

            var response = tradingRequest.Select(x => new GetUserRequestResponse
            {
                TradingRequestId = x.Id,
                UserId = x.RequesterId,
                FullName = x.Requester.FullName,
                UserName = x.Requester.UserName,
                AvatarUrl = x.Requester.AvatarUrl,
                Status = x.Status,
            }).ToList();

            return ApiResponse<List<GetUserRequestResponse>>.Ok(data: response);
        }

        public async Task<ApiResponse<string>> UpdateStatusTradingRequestAsync(Guid tradingPostId, Guid tradingRequestId, UpdateStatusTradingRequest request)
        {
            var tradingRequest = (await _tradingRequestRepository.FindWithIncludeAsync(
                predicate: query => !query.IsDeleted && query.TradingPostId == tradingPostId && query.Id == tradingRequestId,
                asNoTracking: false)).FirstOrDefault();

            if (tradingRequest == null)
            {
                return ApiResponse<string>.Fail(MessageId.E0000);
            }

            tradingRequest.Status = StatusEnum.Completed.ToString();

            await _tradingRequestRepository.UpdateAsync(tradingRequest);
            await _tradingRequestRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(string.Empty);
        }
    }
}
