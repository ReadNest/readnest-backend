using ReadNest.Application.Models.Requests.Event;
using ReadNest.Application.Models.Responses.Event;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Event;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Event
{
    public class EventUseCase : IEventUseCase
    {
        private readonly IEventRepository _eventRepository;

        public EventUseCase(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<ApiResponse<EventResponse?>> GetCurrentEventAsync()
        {
            var entity = await _eventRepository.GetCurrentEventAsync();
            if (entity == null)
                return ApiResponse<EventResponse?>.Fail("No current event found.");

            var result = new EventResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Type = entity.Type,
                Status = entity.Status
            };

            return ApiResponse<EventResponse?>.Ok(result);
        }

        public async Task<ApiResponse<IEnumerable<EventResponse>>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            var result = events.Select(e => new EventResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Type = e.Type,
                Status = e.Status
            });

            return ApiResponse<IEnumerable<EventResponse>>.Ok(result);
        }

        public async Task<ApiResponse<EventResponse>> CreateAsync(CreateEventRequest request)
        {
            var entity = new Domain.Entities.Event
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Type = request.Type,
                Status = request.Status
            };

            await _eventRepository.AddAsync(entity);

            var response = new EventResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Type = entity.Type,
                Status = entity.Status
            };

            await _eventRepository.SaveChangesAsync();

            return ApiResponse<EventResponse>.Ok(response);
        }

        public async Task<ApiResponse<EventResponse>> UpdateAsync(UpdateEventRequest request)
        {
            var entity = (await _eventRepository.FindAsync(predicate: query => query.Id == request.Id && !query.IsDeleted)).FirstOrDefault();
            if (entity == null)
                return ApiResponse<EventResponse>.Fail("Event not found.");

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Type = request.Type;
            entity.Status = request.Status;

            await _eventRepository.UpdateAsync(entity);
            await _eventRepository.SaveChangesAsync();

            var response = new EventResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Type = entity.Type,
                Status = entity.Status
            };

            return ApiResponse<EventResponse>.Ok(response);
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var entity = (await _eventRepository.FindAsync(predicate: query => query.Id == id && !query.IsDeleted)).FirstOrDefault();
            if (entity == null)
                return ApiResponse<string>.Fail("Event not found.");

            await _eventRepository.SoftDeleteAsync(entity);
            await _eventRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("Event delete successfully!");
        }
    }
}
