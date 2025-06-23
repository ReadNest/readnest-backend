using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Event;
using ReadNest.Application.Models.Responses.Event;
using ReadNest.Application.Models.Responses.EventReward;
using ReadNest.Application.UseCases.Interfaces.Event;
using ReadNest.Application.UseCases.Interfaces.EventReward;
using ReadNest.Shared.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventUseCase _eventUseCase;
        private readonly IEventRewardUseCase _eventRewardUseCase;

        public EventController(IEventUseCase eventUseCase, IEventRewardUseCase eventRewardUseCase)
        {
            _eventUseCase = eventUseCase;
            _eventRewardUseCase = eventRewardUseCase;
        }

        [HttpGet("current")]
        [ProducesResponseType(typeof(ApiResponse<EventResponse?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCurrentEvent()
        {
            var response = await _eventUseCase.GetCurrentEventAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EventResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllEvents()
        {
            var response = await _eventUseCase.GetAllEventsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("all-paging")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<EventResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllEventsWithPagingAsync([FromQuery] PagingRequest request)
        {
            var response = await _eventUseCase.GetAllEventsWithPagingAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EventResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {
            var response = await _eventUseCase.CreateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<EventResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request)
        {
            var response = await _eventUseCase.UpdateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var response = await _eventUseCase.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{eventId}/rewards")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EventRewardResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRewardsByEventId(Guid eventId)
        {
            var response = await _eventRewardUseCase.GetRewardsByEventIdAsync(eventId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
