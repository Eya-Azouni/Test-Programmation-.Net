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

        // Get all the comments of a given event
        [HttpGet]
        public ActionResult<IEnumerable<CommentDto>> GetCommentsForEvent(Guid eventId)
        {
            // 1. checking if the event exists
            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();
            // 2. getting the related comments
            var commentsFromRepo = _eventCommentRepository.GetComments(eventId);
            // 3.1 if there are no comments to be found
            if (commentsFromRepo == null)
                return NotFound();
            // 3.2 if there were comments related to the event => mapping and returning them
            return Ok(_mapper.Map<IEnumerable<CommentDto>>(commentsFromRepo));

        }

        // Get a comment by given Id 
        [HttpGet("{commentId}", Name = "GetCommentByIdForEvent")]
        public ActionResult<CommentDto> GetCommentForEvent(Guid eventId, Guid commentId)
        {
            // 1. checking if the event exists
            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();
            // 2. getting the comment
            var commentFromRepo = _eventCommentRepository.GetComment(eventId, commentId);
            // 3.1 if there is no comment to be found with givent Ids
            if (commentFromRepo == null)
                return NotFound();
            // 3.2 if there was a comment with givent Ids => mapping and returning it
            return Ok(_mapper.Map<CommentDto>(commentFromRepo));

        }

        // Create a new comment
        [HttpPost]
        public ActionResult<CommentDto> CreateComment(Guid eventId, [FromBody] CommentForPOSTDto comment)
        {
            // 1. checking if the event exists
            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();
            // 2. mapping the input
            var commentEntity = _mapper.Map<Comment>(comment);
            // 3. saving the object
            _eventCommentRepository.AddComment(eventId, commentEntity);
            _eventCommentRepository.Save();
            // 4. mapping the output
            var commentToReturn = _mapper.Map<CommentDto>(commentEntity);

            return CreatedAtRoute("GetCommentByIdForEvent", new { eventId = eventId, commentId = commentToReturn.Id }, commentToReturn);
        }

        // Update an existing comment
        [HttpPut]
        public ActionResult<CommentDto> UpdateComment(Guid eventId, [FromBody] CommentForPUTDto comment)
        {
            // 1. checking if the event exists
            if (!_eventCommentRepository.EventExists(eventId))
                return NotFound();
            // 2. mapping the input
            var commentEntity = _mapper.Map<Comment>(comment);
            // 3. updating the object
            var updated = _eventCommentRepository.UpdateComment(eventId, commentEntity);
            // 3.1 in case the old object was found and updated successfully
            if (updated)
            {
                 // save changes & return success message
                 _eventCommentRepository.Save();
                 return Ok(new MessageDto("Comment successfully updated !"));
            }
            // 3.2 in case the old object was not found
            return NotFound();
        }

        // Delete and event by given Id
        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(Guid eventId, Guid commentId)
        {
            // 1. deleting the object by given Id
            var deleted = _eventCommentRepository.DeleteComment(eventId, commentId);
            // 2.1 in case the object was found and deleted successfully
            if (deleted)
            {
                // save changes & return success message
                _eventCommentRepository.Save();
                return Ok(new MessageDto("Comment successfully deleted !"));
            }
            // 2.2 in case the object was not found
            return NotFound();
        }
    }
}
