﻿namespace ReadNest.Application.Models.Responses.Badge
{
    public class GetBadgeResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
