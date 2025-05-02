using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Requests.User;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.User;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.User
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            if(user == null)
            {
                return ApiResponse<GetUserResponse>.Fail(MessageId.E0005);
            }

            var data = new GetUserResponse
            {
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role.RoleName,
                UserId = user.Id,
                UserName = user.UserName,
            };

            return ApiResponse<GetUserResponse>.Ok(data); ;
        }

        public Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
