using FluentValidation;
using ReadNest.Application.Models.Requests.Post;
using ReadNest.Application.Models.Responses.Comment;
using ReadNest.Application.Models.Responses.Post;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Post;
using ReadNest.Application.Validators.Post;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Post
{
    public class PostUseCase : IPostUseCase
    {
        public readonly IPostRepository _postRepository;
        public readonly IBookRepository _bookRepository;
        public readonly IUserRepository _userRepository;
        public readonly CreatePostRequestValidator _validator;

        public PostUseCase(IPostRepository postRepository, CreatePostRequestValidator validator, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _validator = validator;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<List<GetPostResponse>>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            if (posts == null || !posts.Any())
            {
                return ApiResponse<List<GetPostResponse>>.Fail("No posts found.");
            }

            var response = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Creator = new GetUserResponse
                {
                    UserId = p.Creator.Id,
                    FullName = p.Creator.FullName,
                    UserName = p.Creator.UserName,
                    Email = p.Creator.Email,
                    AvatarUrl = p.Creator.AvatarUrl
                },
                Views = p.Views,
                LikesCount = p.Likes.Count(),
                UserLikes = p.Likes.Select(l => l.UserName).ToList()
            }).ToList();

            return ApiResponse<List<GetPostResponse>>.Ok(response);
        }

        public async Task<ApiResponse<GetPostResponse>> CreateAsync(CreatePostRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<GetPostResponse>.Fail("Invalid User");
            }
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                return ApiResponse<GetPostResponse>.Fail("Invalid Book");
            }

            var post = new Domain.Entities.Post
            {
                Title = request.Title,
                Content = request.Content,
                BookId = request.BookId,
                UserId = request.UserId
            };

            await _postRepository.AddAsync(post);

            var creator = await _userRepository.GetByIdAsync(request.UserId);

            var response = new GetPostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                BookId = post.BookId,
                UserId = post.UserId,
                Creator = new GetUserResponse
                {
                    UserId = creator.Id,
                    FullName = creator.FullName,
                    UserName = creator.UserName,
                    Email = creator.Email,
                    AvatarUrl = creator.AvatarUrl
                },
                Views = post.Views,
                LikesCount = 0
            };

            await _postRepository.SaveChangesAsync();
            return ApiResponse<GetPostResponse>.Ok(response);
        }

        public async Task<ApiResponse<GetPostResponse>> GetPostByIdAsync(Guid postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null || post.IsDeleted)
            {
                return ApiResponse<GetPostResponse>.Fail("Post not found");
            }

            var creator = await _userRepository.GetByIdAsync(post.UserId);

            var response = new GetPostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                BookId = post.BookId,
                UserId = post.UserId,
                Creator = new GetUserResponse
                {
                    UserId = creator.Id,
                    FullName = creator.FullName,
                    UserName = creator.UserName,
                    Email = creator.Email,
                    AvatarUrl = creator.AvatarUrl
                },
                Views = post.Views,
                LikesCount = post.Likes.Count(),
                UserLikes = post.Likes.Select(l => l.UserName).ToList()
            };

            return ApiResponse<GetPostResponse>.Ok(response);
        }

        public async Task<ApiResponse<List<GetPostResponse>>> GetPostsByUserIdAsync(Guid userId)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(userId);
            if (posts == null || !posts.Any())
            {
                return ApiResponse<List<GetPostResponse>>.Fail("No posts found for this user.");
            }

            var response = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Creator = new GetUserResponse
                {
                    UserId = p.Creator.Id,
                    FullName = p.Creator.FullName,
                    UserName = p.Creator.UserName,
                    Email = p.Creator.Email,
                    AvatarUrl = p.Creator.AvatarUrl
                },
                Views = p.Views,
                LikesCount = p.Likes.Count(),
                UserLikes = p.Likes.Select(l => l.UserName).ToList()
            }).ToList();

            return ApiResponse<List<GetPostResponse>>.Ok(response);
        }

        public async Task<ApiResponse<List<GetPostResponse>>> GetPostsByBookIdAsync(Guid bookId)
        {
            var posts = await _postRepository.GetPostsByBookIdAsync(bookId);
            if (posts == null || !posts.Any())
            {
                return ApiResponse<List<GetPostResponse>>.Fail("No posts found for this book.");
            }

            var response = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Creator = new GetUserResponse
                {
                    UserId = p.Creator.Id,
                    FullName = p.Creator.FullName,
                    UserName = p.Creator.UserName,
                    Email = p.Creator.Email,
                    AvatarUrl = p.Creator.AvatarUrl
                },
                Views = p.Views,
                LikesCount = p.Likes.Count(),
                UserLikes = p.Likes.Select(l => l.UserName).ToList()
            }).ToList();

            return ApiResponse<List<GetPostResponse>>.Ok(response);
        }

        public async Task<ApiResponse<List<GetPostResponse>>> GetTopMostLikedPostsAsync(int count)
        {
            var posts = await _postRepository.GetTopMostLikedPostsAsync(count);
            if (posts == null || !posts.Any())
            {
                return ApiResponse<List<GetPostResponse>>.Fail("No posts found.");
            }

            var response = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Creator = new GetUserResponse
                {
                    UserId = p.Creator.Id,
                    FullName = p.Creator.FullName,
                    UserName = p.Creator.UserName,
                    Email = p.Creator.Email,
                    AvatarUrl = p.Creator.AvatarUrl
                },
                Views = p.Views,
                LikesCount = p.Likes.Count(),
                UserLikes = p.Likes.Select(l => l.UserName).ToList()
            }).ToList();

            return ApiResponse<List<GetPostResponse>>.Ok(response);
        }

        public async Task<ApiResponse<List<GetPostResponse>>> GetTopMostViewedPostsAsync(int count)
        {
            var posts = await _postRepository.GetTopMostViewedPostsAsync(count);
            if (posts == null || !posts.Any())
            {
                return ApiResponse<List<GetPostResponse>>.Fail("No posts found.");
            }

            var response = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Creator = new GetUserResponse
                {
                    UserId = p.Creator.Id,
                    FullName = p.Creator.FullName,
                    UserName = p.Creator.UserName,
                    Email = p.Creator.Email,
                    AvatarUrl = p.Creator.AvatarUrl
                },
                Views = p.Views,
                LikesCount = p.Likes.Count(),
                UserLikes = p.Likes.Select(l => l.UserName).ToList()
            }).ToList();

            return ApiResponse<List<GetPostResponse>>.Ok(response);
        }

        public async Task<ApiResponse<List<GetPostResponse>>> SearchByTitleAsync(string keyword)
        {
            var posts = await _postRepository.SearchByTitleAsync(keyword);
            if (posts == null || !posts.Any())
            {
                return ApiResponse<List<GetPostResponse>>.Fail("No posts found matching the search criteria.");
            }

            var response = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Creator = new GetUserResponse
                {
                    UserId = p.Creator.Id,
                    FullName = p.Creator.FullName,
                    UserName = p.Creator.UserName,
                    Email = p.Creator.Email,
                    AvatarUrl = p.Creator.AvatarUrl
                },
                Views = p.Views,
                LikesCount = p.Likes.Count(),
                UserLikes = p.Likes.Select(l => l.UserName).ToList()
            }).ToList();

            return ApiResponse<List<GetPostResponse>>.Ok(response);
        }

        public async Task<ApiResponse<string>> LikePostAsync(Guid postId, Guid userId)
        {
            var post = await _postRepository.GetPostWithLikesByIdAsync(postId);
            if (post == null || post.IsDeleted)
            {
                return ApiResponse<string>.Fail("Post not found");
            }
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<string>.Fail("User not found");
            }

            if (post.Likes.Any(l => l.Id == userId))
            {
                // Handle unlike logic
                post.Likes.Remove(user);
                await _postRepository.SaveChangesAsync();
                return ApiResponse<string>.Ok("Unlike successfully");
            }
            else
            {
                post.Likes.Add(user);
                await _postRepository.SaveChangesAsync();
                return ApiResponse<string>.Ok("Like successfully");
            }
        }
    }
}
