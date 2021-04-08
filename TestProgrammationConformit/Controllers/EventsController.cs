using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using TestProgrammationConformit.Entities;
using TestProgrammationConformit.Entities.Pagination;
using TestProgrammationConformit.Models;
using TestProgrammationConformit.Services;

namespace TestProgrammationConformit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventCommentRepository _eventCommentRepository;
        private readonly IMapper _mapper;
        public EventsController(IEventCommentRepository eventCommentRepository, IMapper mapper)
        {
            _eventCommentRepository = eventCommentRepository ?? throw new ArgumentNullException(nameof(eventCommentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Get all events paginated
        [HttpGet]
        public ActionResult<Page<EventDto>> GetEvents([FromQuery] PagingParameters pagingParameters)
        {
            var eventsFromRepo = _eventCommentRepository.GetEvents(pagingParameters);
            return Ok(eventsFromRepo);
        }

        // Get an event by given Id
        [HttpGet("{eventId}", Name ="GetEventById")]
        public ActionResult<EventDto> GetEventById(Guid eventId)
        {
            // 1. getting the object by given id from DB
            var eventsFromRepo = _eventCommentRepository.GetEvent(eventId);
            if (null == eventsFromRepo)
                return NotFound();
            // 2. mapping the output
            var eventDto = _mapper.Map<Models.EventDto>(eventsFromRepo);

            return Ok(eventDto);
        }

        // Create a new event
        [HttpPost]
        public ActionResult<EventDto> CreateEvent([FromBody] EventForPOSTDto eventDto)
        {
            // 1. mapping the input
            var eventEntity = _mapper.Map<Entities.Event>(eventDto);
            // 2. saving the new object
            _eventCommentRepository.AddEvent(eventEntity);
            _eventCommentRepository.Save();
            // 3. mapping the output
            var eventToReturn = _mapper.Map<Models.EventDto>(eventEntity);

            return CreatedAtRoute("GetEventById", new { eventId = eventToReturn.Id }, eventToReturn);
        }

        // Update an existing event
        [HttpPut]
        public ActionResult<EventDto> UpdateComment([FromBody] EventForPUTDto eventDto)
        {
            // 1. mapping the input
            var eventEntity = _mapper.Map<Event>(eventDto);
            // 2. updating the object
            var updated = _eventCommentRepository.UpdateEvent(eventEntity);

            // 3.1 in case the old object was found and updated successfully
            if (updated)
            {
            // save changes & return success message
             _eventCommentRepository.Save();
             return Ok(new MessageDto("Event successfully updated !"));
            }
            // 3.2 in case the old object was not found
            return NotFound();
        }

        // Delete and event by given Id
        [HttpDelete("{eventId}")]
        public ActionResult DeleteEvent(Guid eventId)
        {
            // 1. deleting the object by given Id
            var deleted = _eventCommentRepository.DeleteEvent(eventId);

            // 2.1 in case the object was found and deleted successfully
            if (deleted)
            {
             // save changes & return success message
             _eventCommentRepository.Save();
             return Ok(new MessageDto("Event successfully deleted !"));
            }
            // 2.2 in case the object was not found
            return NotFound();
        }
    }
}
