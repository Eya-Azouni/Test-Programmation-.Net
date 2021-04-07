using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public ActionResult<Page<EventDto>> GetEvents([FromQuery] PagingParameters pagingParameters)
        {
            var eventsFromRepo = _eventCommentRepository.GetEvents(pagingParameters);
            return Ok(eventsFromRepo);
        }

        [HttpGet("{eventId}", Name ="GetEventById")]
        public ActionResult<EventDto> GetEventById(Guid eventId)
        {
            var eventsFromRepo = _eventCommentRepository.GetEvent(eventId);
            if (null == eventsFromRepo)
                return NotFound();

            var eventDto = _mapper.Map<Models.EventDto>(eventsFromRepo);
            return Ok(eventDto);
        }

        [HttpPost]
        public ActionResult<EventDto> CreateEvent([FromBody] EventForPOSTDto eventDto)
        {
            var eventEntity = _mapper.Map<Entities.Event>(eventDto);
            _eventCommentRepository.AddEvent(eventEntity);
            _eventCommentRepository.Save();

            var eventToReturn = _mapper.Map<Models.EventDto>(eventEntity);

            return CreatedAtRoute("GetEventById", new { eventId = eventToReturn.Id }, eventToReturn);
        }
    }
}
