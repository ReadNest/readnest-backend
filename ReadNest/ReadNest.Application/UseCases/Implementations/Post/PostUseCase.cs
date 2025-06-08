using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Requests.Post;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.Post;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Post;
using ReadNest.Application.Validators.Post;
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

        public async Task<ApiResponse<PagingResponse<GetPostResponse>>> GetAllPostsAsync(PagingRequest request)
        {
            var posts = await _postRepository.FindWithIncludePagedAsync(
                predicate: p => !p.IsDeleted,
                include: query => query
                    .Include(p => p.Creator)
                    .Include(p => p.Book)
                    .Include(p => p.Likes),
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                asNoTracking: true);

            var postResponse = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Book = new Domain.Entities.Book
                {
                    Id = p.Book.Id,
                    Title = p.Book.Title,
                    Author = p.Book.Author,
                    ImageUrl = p.Book.ImageUrl,
                },
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

            if (postResponse.Count == 0)
            {
                return ApiResponse<PagingResponse<GetPostResponse>>.Fail(MessageId.E0005);
            }

            var pagingResponse = new PagingResponse<GetPostResponse>
            {
                Items = postResponse,
                TotalItems = await _postRepository.CountAsync(b => !b.IsDeleted),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return ApiResponse<PagingResponse<GetPostResponse>>.Ok(pagingResponse);
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

            _ = await _postRepository.AddAsync(post);

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
                Book = new Domain.Entities.Book
                {
                    Id = post.Book.Id,
                    Title = post.Book.Title,
                    Author = post.Book.Author,
                    ImageUrl = post.Book.ImageUrl,
                },
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
            var posts = await _postRepository.FindWithIncludeAsync(
                predicate: p => p.Id == postId && !p.IsDeleted,
                include: query => query
                    .Include(p => p.Creator)
                    .Include(p => p.Book)
                    .Include(p => p.Likes),
                asNoTracking: true);

            var post = posts.FirstOrDefault();

            if (post == null)
            {
                return ApiResponse<GetPostResponse>.Fail(MessageId.E0005);
            }

            var response = new GetPostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                BookId = post.BookId,
                UserId = post.UserId,
                Book = new Domain.Entities.Book
                {
                    Id = post.Book.Id,
                    Title = post.Book.Title,
                    Author = post.Book.Author,
                    ImageUrl = post.Book.ImageUrl,
                },
                Creator = new GetUserResponse
                {
                    UserId = post.Creator.Id,
                    FullName = post.Creator.FullName,
                    UserName = post.Creator.UserName,
                    Email = post.Creator.Email,
                    AvatarUrl = post.Creator.AvatarUrl
                },
                Views = post.Views,
                LikesCount = post.Likes.Count(),
                UserLikes = post.Likes.Select(l => l.UserName).ToList()
            };

            return ApiResponse<GetPostResponse>.Ok(response);
        }

        public async Task<ApiResponse<PagingResponse<GetPostResponse>>> GetPostsByUserIdAsync(Guid userId, PagingRequest request)
        {
            var posts = await _postRepository.FindWithIncludePagedAsync( 
                predicate: p => p.UserId == userId && !p.IsDeleted,
                include: query => query
                    .Include(p => p.Creator)
                    .Include(p => p.Book)
                    .Include(p => p.Likes),
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                asNoTracking: true);

            var postResponse = posts.Select(p => new GetPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                BookId = p.BookId,
                UserId = p.UserId,
                Book = new Domain.Entities.Book
                {
                    Id = p.Book.Id,
                    Title = p.Book.Title,
                    Author = p.Book.Author,
                    ImageUrl = p.Book.ImageUrl,
                },
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

            if (postResponse.Count == 0)
            {
                return ApiResponse<PagingResponse<GetPostResponse>>.Fail("No posts found for this user.");
            }

            var pagingResponse = new PagingResponse<GetPostResponse>
            {
                Items = postResponse,
                TotalItems = await _postRepository.GetPostCountByUserIdAsync(userId),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return ApiResponse<PagingResponse<GetPostResponse>>.Ok(pagingResponse);
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
                Book = new Domain.Entities.Book
                {
                    Id = p.Book.Id,
                    Title = p.Book.Title,
                    Author = p.Book.Author,
                    ImageUrl = p.Book.ImageUrl,
                },
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
                Book = new Domain.Entities.Book
                {
                    Id = p.Book.Id,
                    Title = p.Book.Title,
                    Author = p.Book.Author,
                    ImageUrl = p.Book.ImageUrl,
                },
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
                Book = new Domain.Entities.Book
                {
                    Id = p.Book.Id,
                    Title = p.Book.Title,
                    Author = p.Book.Author,
                    ImageUrl = p.Book.ImageUrl,
                },
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
                Book = new Domain.Entities.Book
                {
                    Id = p.Book.Id,
                    Title = p.Book.Title,
                    Author = p.Book.Author,
                    ImageUrl = p.Book.ImageUrl,
                },
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
                _ = post.Likes.Remove(user);
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

        public async Task<ApiResponse<GetPostResponse>> UpdateAsync(UpdatePostRequest request)
        {
            var posts = await _postRepository.FindWithIncludeAsync(
                predicate: p => p.Id == request.Id && !p.IsDeleted,
                include: query => query
                    .Include(p => p.Creator)
                    .Include(p => p.Book)
                    .Include(p => p.Likes),
                asNoTracking: true);

            var post = posts.FirstOrDefault();

            if (post == null)
            {
                return ApiResponse<GetPostResponse>.Fail(MessageId.E0005);
            }

            post.Title = request.Title;
            post.Content = request.Content;
            post.BookId = request.BookId;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post);

            var response = new GetPostResponse
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                BookId = post.BookId,
                UserId = post.UserId,
                Book = new Domain.Entities.Book
                {
                    Id = post.Book.Id,
                    Title = post.Book.Title,
                    Author = post.Book.Author,
                    ImageUrl = post.Book.ImageUrl,
                },
                Creator = new GetUserResponse
                {
                    UserId = post.Creator.Id,
                    FullName = post.Creator.FullName,
                    UserName = post.Creator.UserName,
                    Email = post.Creator.Email,
                    AvatarUrl = post.Creator.AvatarUrl
                },
                Views = post.Views,
                LikesCount = post.Likes.Count(),
                UserLikes = post.Likes.Select(l => l.UserName).ToList()
            };

            await _postRepository.SaveChangesAsync();

            return ApiResponse<GetPostResponse>.Ok(response);
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid postId)
        {
            var post = (await _postRepository.FindAsync(predicate: query => query.Id == postId && !query.IsDeleted)).FirstOrDefault();
            if (post == null)
            {
                return ApiResponse<string>.Fail(MessageId.E0001);
            }

            await _postRepository.SoftDeleteAsync(post);
            await _postRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("Post deleted successfully");
        }
    }
}
