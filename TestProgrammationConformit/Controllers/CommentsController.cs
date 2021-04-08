using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TestProgrammationConformit.Entities;
using TestProgrammationConformit.Models;
using TestProgrammationConformit.Services;

namespace TestProgrammationConformit.Controllers
{
    [Route("api/events/{eventId}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IEventCommentRepository _eventCommentRepository;
        private readonly IMapper _mapper;

        public CommentsController(IEventCommentRepository eventCommentRepository, IMapper mapper)
        {
            _eventCommentRepository = eventCommentRepository ?? throw new ArgumentNullException(nameof(eventCommentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommentDto>> GetCommentsForEvent(Guid eventId)
        {
            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();

            var commentsFromRepo = _eventCommentRepository.GetComments(eventId);

            if (commentsFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CommentDto>>(commentsFromRepo));

        }

        [HttpGet("{commentId}", Name = "GetCommentByIdForEvent")]
        public ActionResult<CommentDto> GetCommentForEvent(Guid eventId, Guid commentId)
        {
            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();

            var commentFromRepo = _eventCommentRepository.GetComment(eventId, commentId);

            if(commentFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<CommentDto>(commentFromRepo));

        }

        [HttpPost]
        public ActionResult<CommentDto> CreateComment(Guid eventId, [FromBody] CommentForPOSTDto comment)
        {
            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();

            var commentEntity = _mapper.Map<Comment>(comment);
            _eventCommentRepository.AddComment(eventId, commentEntity);
            _eventCommentRepository.Save();

            var commentToReturn = _mapper.Map<CommentDto>(commentEntity);

            return CreatedAtRoute("GetCommentByIdForEvent", new { eventId = eventId, commentId = commentToReturn.Id }, commentToReturn);
        }

        [HttpPut]
        public ActionResult<CommentDto> UpdateComment(Guid eventId, [FromBody] CommentForPUTDto comment)
        {
            if (eventId == null)
                throw new ArgumentNullException(nameof(eventId));

            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();

            var commentEntity = _mapper.Map<Comment>(comment);
            var updated = _eventCommentRepository.UpdateComment(eventId, commentEntity);

            if (!updated)
                return NotFound();

            _eventCommentRepository.Save();
            var commentToReturn = _mapper.Map<CommentDto>(commentEntity);

            return AcceptedAtRoute("GetCommentByIdForEvent", new { eventId = eventId, commentId = commentToReturn.Id }, commentToReturn);
        }

        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(Guid eventId, Guid commentId)
        {
            var deleted = _eventCommentRepository.DeleteComment(eventId, commentId);
            

            if (deleted)
            {
                _eventCommentRepository.Save();
                return Ok(new MessageDto("Comment successfully deleted !"));
            }     

            return NotFound();
        }
    }
}
