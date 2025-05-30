using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.Comment;
using ReadNest.Application.Models.Responses.Comment;
using ReadNest.Application.Models.Responses.CommentReport;
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
        private readonly ICommentReportRepository _commentReportRepository;

        public CommentUseCase(ICommentRepository commentRepository, IBookRepository bookRepository, IUserRepository userRepository, ICommentReportRepository commentReportRepository)
        {
            _commentRepository = commentRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _commentReportRepository = commentReportRepository;
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
                Creator = new GetUserResponse
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

        public async Task<ApiResponse<string>> DeleteCommentAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment is null)
            {
                return ApiResponse<string>.Fail("No comments found");
            }
            await _commentRepository.SoftDeleteAsync(comment);
            await _commentRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok(string.Empty);
        }

        public async Task<ApiResponse<List<GetCommentResponse>>> GetPublishedCommentsByBookIdAsync(Guid bookId)
        {
            var comments = await _commentRepository.GetPublishCommentsByBookIdAsync(bookId);
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
                UserLikes = c.Likes?.Select(l => l.Id.ToString()).ToList() ?? new List<string>()
            }).ToList();

            return ApiResponse<List<GetCommentResponse>>.Ok(response);

        }

        public async Task<ApiResponse<string>> LikeCommentAsync(Guid commentId, Guid userId)
        {
            var comment = await _commentRepository.GetCommentWithLikesByIdAsync(commentId);
            if (comment == null)
                return ApiResponse<string>.Fail("Comment not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return ApiResponse<string>.Fail("User not found");


            if (comment.Likes.Any(u => u.Id == userId))
            {
                // Handle unlike logic
                comment.Likes.Remove(user);
                await _commentRepository.SaveChangesAsync();
                return ApiResponse<string>.Ok("Unlike successfully");
            }
            else
            {
                comment.Likes.Add(user);
                await _commentRepository.SaveChangesAsync();
                return ApiResponse<string>.Ok("Like successfully");
            }
        }

        public async Task<ApiResponse<string>> UpdateCommentAsync(UpdateCommentRequest request)
        {
            var comment = await _commentRepository.GetCommentWithLikesByIdAsync(request.CommentId);
            if (comment is null)
            {
                return ApiResponse<string>.Fail("Comment not found");
            }
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return ApiResponse<string>.Fail("Empty Content!");
            }

            if (request.Content.Length > 255)
            {
                return ApiResponse<string>.Fail("Content is too long!");
            }

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow;
            await _commentRepository.UpdateAsync(comment);
            await _commentRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("Update comment successfully");
        }

        public async Task<ApiResponse<List<GetReportedCommentsResponse>>> GetAllPendingReportedCommentsAsync()
        {
            var reportedComments = await _commentRepository.GetAllReportedCommentsAsync();

            if (reportedComments == null || !reportedComments.Any())
            {
                return ApiResponse<List<GetReportedCommentsResponse>>.Fail("No reported comments found");
            }

            var response = new List<GetReportedCommentsResponse>();

            foreach (var comment in reportedComments)
            {
                var reports = await _commentReportRepository.GetPendingReportsByCommentIdAsync(comment.Id);

                var mappedReports = reports.Select(r => new CommentReportReponse
                {
                    CommentId = r.Id,
                    Reason = r.Reason,
                    CreatedAt = r.CreatedAt,
                    Reporter = r.Reporter != null ? new GetUserResponse
                    {
                        UserId = r.Reporter.Id,
                        FullName = r.Reporter.FullName,
                        AvatarUrl = r.Reporter.AvatarUrl
                    } : null
                }).ToList();

                response.Add(new GetReportedCommentsResponse
                {
                    CommentId = comment.Id,
                    Content = comment.Content,
                    Commenter = comment.Creator != null ? new GetUserResponse
                    {
                        UserId = comment.Creator.Id,
                        FullName = comment.Creator.FullName,
                        AvatarUrl = comment.Creator.AvatarUrl
                    } : null,
                    Reports = mappedReports
                });
            }

            return ApiResponse<List<GetReportedCommentsResponse>>.Ok(response);
        }
    }
}
