namespace ReadNest.Application.Services
{
    public interface IRedisUserTrackingService
    {
        Task TrackBookClickAsync(Guid userId, Guid bookId);
        Task<List<Guid>> GetTopBookClicksAsync(Guid userId, int top = 5);
        Task TrackKeywordSearchAsync(Guid userId, string keyword);
        Task<List<string>> GetTopKeywordsAsync(Guid userId, int top = 5);
        Task ClearUserTrackingAsync(Guid userId);
        Task<List<string>> GetTopGlobalKeywordsAsync(int top = 10);
    }
}
