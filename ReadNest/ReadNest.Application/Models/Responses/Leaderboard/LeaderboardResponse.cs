using ReadNest.Application.Models.Responses.User;

namespace ReadNest.Application.Models.Responses.Leaderboard
{
    public class LeaderboardResponse
    {
        public Guid UserId { get; set; }
        public virtual GetUserResponse User { get; set; }
        public int TotalPosts { get; set; }
        public int TotalLikes { get; set; }
        public int TotalViews { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }
}
