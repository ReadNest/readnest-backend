using ReadNest.Application.Models.Requests.Event;
using ReadNest.Application.Models.Responses.Event;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Event
{
    public interface IEventUseCase
    {
        Task<ApiResponse<EventResponse?>> GetCurrentEventAsync();
        Task<ApiResponse<IEnumerable<EventResponse>>> GetAllEventsAsync();
        Task<ApiResponse<PagingResponse<EventResponse>>> GetAllEventsWithPagingAsync(PagingRequest request);

        Task<ApiResponse<EventResponse>> CreateAsync(CreateEventRequest request);
        Task<ApiResponse<EventResponse>> UpdateAsync(UpdateEventRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}
