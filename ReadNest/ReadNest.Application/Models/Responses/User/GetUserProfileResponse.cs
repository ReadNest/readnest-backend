﻿using ReadNest.Application.Models.Responses.Comment;

namespace ReadNest.Application.Models.Responses.User
{
    public class GetUserProfileResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Bio { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AvatarUrl { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public ICollection<GetCommentResponse> Comments { get; set; }
        //Sau thêm Posts nữa là đủ
        public int numberOfPosts { get; set; }
        public int NumberOfComments { get; set; }
        public int RatingScores { get; set; }
    }
}
