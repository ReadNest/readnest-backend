using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Requests.User;
using ReadNest.Application.Models.Responses.Comment;
using ReadNest.Application.Models.Responses.Post;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Application.Models.Responses.UserBadge;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.User;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;
using ReadNest.Shared.Enums;

namespace ReadNest.Application.UseCases.Implementations.User
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserBadgeRepository _userBadgeRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        public UserUseCase(IUserRepository userRepository, IUserBadgeRepository userBadgeRepository)
        {
            _userRepository = userRepository;
            _userBadgeRepository = userBadgeRepository;
        }

        public Task<ApiResponse<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> CreateUserAsync(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> DeleteAccountAsync(Guid userId)
        {
            await _userRepository.SoftDeleteByIdAsync(userId);
            await _userRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok(string.Empty);
        }

        public async Task<ApiResponse<PagingResponse<GetUserResponse>>> GetAllAsync(PagingRequest request)
        {
            var pagingResponse = await _userRepository.FindPagedAsync(
                predicate: query => !query.IsDeleted,
                include: query => query.Include(x => x.Role),
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                asNoTracking: true,
                orderBy: query => query.OrderByDescending(x => x.CreatedAt)
                                       .ThenByDescending(x => x.UpdatedAt)
                                       .ThenByDescending(x => x.UserName)
                                       .ThenByDescending(x => x.Email)
            );

            if (pagingResponse.TotalItems == 0)
            {
                return ApiResponse<PagingResponse<GetUserResponse>>.Fail(MessageId.E0005);
            }

            var data = new PagingResponse<GetUserResponse>
            {
                Items = pagingResponse.Items.Select(x => new GetUserResponse
                {
                    FullName = x.FullName,
                    Address = x.Address,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Email,
                    RoleId = x.RoleId,
                    RoleName = x.Role.RoleName,
                    UserId = x.Id,
                    UserName = x.UserName,
                }),
                PageIndex = pagingResponse.PageIndex,
                PageSize = pagingResponse.PageSize,
                TotalItems = pagingResponse.TotalItems,
            };

            return ApiResponse<PagingResponse<GetUserResponse>>.Ok(data); ;
        }

        public async Task<ApiResponse<GetUserResponse>> GetByIdAsync(Guid userId)
        {
            var user = (await _userRepository.FindWithIncludeAsync(
                                       predicate: query => query.Id == userId && !query.IsDeleted,
                                       include: query => query.Include(x => x.Role),
                                       asNoTracking: true)).FirstOrDefault();

            if (user == null)
            {
                return ApiResponse<GetUserResponse>.Fail(MessageId.E0005);
            }

            var selectedBadge = await _userBadgeRepository.GetSelectedBadgeByUserIdAsync(userId);

            var data = new GetUserResponse
            {
                FullName = user.FullName,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                AvatarUrl = user.AvatarUrl,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                UserId = user.Id,
                UserName = user.UserName,
                SelectedBadgeCode = selectedBadge is null ? "DEFAULT" : selectedBadge.Badge.Code,
            };

            return ApiResponse<GetUserResponse>.Ok(data); ;
        }

        public async Task<ApiResponse<GetUserProfileResponse>> GetByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null)
            {
                return ApiResponse<GetUserProfileResponse>.Fail(MessageId.E0005);
            }

            if (user.Role.RoleName == RoleEnum.Admin.ToString())
            {
                return ApiResponse<GetUserProfileResponse>.Fail("");
            }
            var ownedBadges = await _userBadgeRepository.GetAvailableByUserIdAsync(user.Id);

            var data = new GetUserProfileResponse
            {
                FullName = user.FullName,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                AvatarUrl = user.AvatarUrl,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                UserId = user.Id,
                UserName = user.UserName,
                Bio = user.Bio,
                Comments = user.Comments.Select(x => new GetCommentResponse
                {
                    CommentId = x.Id,
                    Content = x.Content,
                    //CreatedAt = x.CreatedAt,
                    //UpdatedAt = x.UpdatedAt,
                    //PostId = x.PostId,
                    UserId = x.UserId,
                    BookId = x.BookId,
                    NumberOfLikes = x.Likes.Count,
                    CreatorName = x.Creator.FullName,
                    //UserName = x.User.UserName,
                }).ToList(),
                Posts = user.Posts.Select(x => new GetPostResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                    //UpdatedAt = x.UpdatedAt,
                    //UserId = x.UserId,
                    //BookId = x.BookId,
                    LikesCount = x.Likes.Count,
                }).ToList(),
                NumberOfComments = user.Comments.Count,
                numberOfPosts = user.Posts.Count,
                RatingScores = 0, // TODO: Add logic to get rating scores
                OwnedBadges = ownedBadges.Select(x => new UserBadgeResponse
                {
                    UserBadgeId = x.Id,
                    UserId = x.UserId,
                    BadgeId = x.BadgeId,
                    BadgeCode = x.Badge.Code,
                    BadgeName = x.Badge.Name,
                    BadgeDescription = x.Badge.Description,
                    IsSelected = x.IsSelected,
                }).ToList(),
            };

            return ApiResponse<GetUserProfileResponse>.Ok(data);
        }

        public async Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<string>.Fail(MessageId.E0005);
            }
            if (string.IsNullOrEmpty(request.AvatarUrl))
            {
                user.FullName = request.FullName;
                user.Address = request.Address;
                user.DateOfBirth = request.DateOfBirth.Value;
                user.Bio = request.Bio;
            }
            else
            {
                user.AvatarUrl = request.AvatarUrl;
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            if (string.IsNullOrEmpty(request.AvatarUrl))
            {
                return ApiResponse<string>.Ok("Update profile info successfully!");
            }
            else
            {
                return ApiResponse<string>.Ok("Update avatar successfully!");
            }
        }
    }
}
