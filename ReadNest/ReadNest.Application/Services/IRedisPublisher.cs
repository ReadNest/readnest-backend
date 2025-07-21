namespace ReadNest.Application.Services
{
    public interface IRedisPublisher
    {
        Task PublishInvoiceEmailEventAsync<T>(T eventData);
    }
}
