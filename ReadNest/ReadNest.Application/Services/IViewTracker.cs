namespace ReadNest.Application.Services
{
    public interface IViewTracker
    {
        Task<bool> ShouldIncreaseViewAsync(string key, TimeSpan ttl);
    }
}
