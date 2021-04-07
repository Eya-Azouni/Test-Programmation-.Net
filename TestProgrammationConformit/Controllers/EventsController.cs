﻿using AutoMapper;
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
        public ActionResult<IEnumerable<EventDto>> GetEvents([FromQuery] PagingParameters pagingParameters)
        {
            var eventsFromRepo = _eventCommentRepository.GetEvents(pagingParameters);
            return Ok(_mapper.Map<IEnumerable<EventDto>>(eventsFromRepo));
        }
    }
}
