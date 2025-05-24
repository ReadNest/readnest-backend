using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.Comment;
using ReadNest.Application.Models.Responses.Comment;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Comment;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Comment
{
    public class CommentUseCase : ICommentUseCase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public CommentUseCase(ICommentRepository commentRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<GetCommentResponse>> CreateAsync(CreateCommentRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<GetCommentResponse>.Fail("Invalid User");
            }
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                return ApiResponse<GetCommentResponse>.Fail("Invalid Book");
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return ApiResponse<GetCommentResponse>.Fail("Empty Content!");
            }

            if (request.Content.Length > 255)
            {
                return ApiResponse<GetCommentResponse>.Fail("Content is too long!");
            }

            var cmt = await _commentRepository.AddAsync(new Domain.Entities.Comment
            {
                BookId = request.BookId,
                UserId = request.UserId,
                Content = request.Content,
                Status = "Published", // Assuming "Published" is the default status for a new comment
                CreatedAt = DateTime.UtcNow
            });

            var creator = await _userRepository.GetByIdAsync(cmt.UserId);

            var response = new GetCommentResponse
            {
                CommentId = cmt.Id,
                BookId = cmt.BookId,
                UserId = cmt.UserId,
                Content = cmt.Content,
                Creator =  new GetUserResponse
                {
                    UserId = creator.Id,
                    FullName = creator.FullName,
                    UserName = creator.UserName,
                    Email = creator.Email,
                    AvatarUrl = creator.AvatarUrl,
                },
                NumberOfLikes = cmt.Likes?.Count ?? 0,
                CreatedAt = cmt.CreatedAt,
            };
            await _commentRepository.SaveChangesAsync();
            return ApiResponse<GetCommentResponse>.Ok(response);
        }

        public async Task<ApiResponse<List<GetCommentResponse>>> GetPublishedCommentsByBookIdAsync(Guid bookId)
        {
            var comments = await _commentRepository.GetPublishedCommentsByBookIdAsync(bookId);
            if (comments == null || !comments.Any())
            {
                return ApiResponse<List<GetCommentResponse>>.Fail("No comments found for this book.");
            }
            var response = comments.Select(c => new GetCommentResponse
            {
                CommentId = c.Id,
                BookId = c.BookId,
                UserId = c.UserId,
                Content = c.Content,
                Creator = c.Creator != null ? new Models.Responses.User.GetUserResponse
                {
                    UserId = c.Creator.Id,
                    FullName = c.Creator.FullName,
                    UserName = c.Creator.UserName,
                    Email = c.Creator.Email,
                    AvatarUrl = c.Creator.AvatarUrl,
                } : null,
                NumberOfLikes = c.Likes?.Count ?? 0,
                CreatedAt = c.CreatedAt,
            }).ToList();

            return ApiResponse<List<GetCommentResponse>>.Ok(response);

        }
    }
}
