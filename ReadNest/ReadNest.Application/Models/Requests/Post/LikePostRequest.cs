﻿namespace ReadNest.Application.Models.Requests.Post
{
    public class LikePostRequest
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
