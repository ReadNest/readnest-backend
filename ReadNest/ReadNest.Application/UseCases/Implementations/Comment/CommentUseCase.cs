using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.Comment;
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

        public async Task<ApiResponse<string>> CreateAsync(CreateCommentRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<string>.Fail("Invalid User");
            }
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                return ApiResponse<string>.Fail("Invalid Book");
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return ApiResponse<string>.Fail("Empty Content!");
            }

            if (request.Content.Length > 255)
            {
                return ApiResponse<string>.Fail("Content is too long!");
            }

            await _commentRepository.AddAsync(new Domain.Entities.Comment
            {
                BookId = request.BookId,
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            });

            await _commentRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok(string.Empty);
        }
    }
}
