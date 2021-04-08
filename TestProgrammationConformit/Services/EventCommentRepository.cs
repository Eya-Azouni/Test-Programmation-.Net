using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestProgrammationConformit.Entities;
using TestProgrammationConformit.Entities.Pagination;
using TestProgrammationConformit.Infrastructures;
using TestProgrammationConformit.Models;

namespace TestProgrammationConformit.Services
{
    public class EventCommentRepository : IEventCommentRepository
    {
        private readonly ConformitContext _context;
        private readonly IMapper _mapper;

        public EventCommentRepository(ConformitContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // ********** Comments Related Methods ********** //

        /// <summary>
        /// Creating a new Comment for a given Event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="comment"></param>
        public void AddComment(Guid eventId, Comment comment)
        {
            // checking parameters
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            // preparing and adding object comment
            comment.DateOfCreation = DateTimeOffset.Now;
            comment.EventId = eventId;
            _context.Comments.Add(comment);
        }

        /// <summary>
        /// Getting a Comment by given Ids.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public Comment GetComment(Guid eventId, Guid commentId)
        {
            // checking parameters
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));
            if (commentId == Guid.Empty)
                throw new ArgumentNullException(nameof(commentId));

            // returning comment object based on given Ids
            return _context.Comments.Where(c => c.Id == commentId && c.EventId == eventId).FirstOrDefault();
        }

        /// <summary>
        /// Updating a Comment.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool UpdateComment(Guid eventId, Comment comment)
        {
            // getting the old comment
            var oldComment = GetComment(eventId, comment.Id);
            if (oldComment == null)
                return false;

            // updating the old comment with new changes
            oldComment.Description = comment.Description;
            _context.Update(oldComment);
            return true;
        }

        /// <summary>
        /// Checking if the Comments exits.
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool CommentExists(Guid commentId)
        {
            // checking parameter
            if (commentId == Guid.Empty)
                throw new ArgumentNullException(nameof(commentId));

            // returning boolean based on result
            return _context.Comments.Any(c => c.Id == commentId);
        }

        /// <summary>
        /// Getting all Comments of an Event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetComments(Guid eventId)
        {
            // checking parameter
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            // returning comments object based on given event Id (+ ordering them by creation date)
            return _context.Comments.Where(c => c.EventId == eventId).OrderBy(c => c.DateOfCreation).ToList();
        }

        /// <summary>
        /// Deleting a Comment by given Ids.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteComment(Guid eventId, Guid commentId)
        {
            // checking parameters
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));
            if (commentId == Guid.Empty)
                throw new ArgumentNullException(nameof(commentId));

            // getting the comment to delete based on given Ids
            var comment = GetComment(eventId, commentId);

            // returning false in case the comment doesn't exist
            if (comment == null)
                return false;

            // removing the comment and returning true
            _context.Comments.Remove(comment);
            return true;
        }
        

        // ********** Events Related Methods ********** //

        /// <summary>
        /// Creating a new Event.
        /// </summary>
        /// <param name="occasion"></param>
        public void AddEvent(Event occasion)
        {
            // checking parameter
            if (occasion== null)
                throw new ArgumentNullException(nameof(occasion));

            // adding object event
            _context.Events.Add(occasion);
        }

        /// <summary>
        /// Getting an Event by given Id.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public Event GetEvent(Guid eventId)
        {
            // checking parameter
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            // returning event object by given Id (+ including related comments)
            return _context.Events.Where(e => e.Id == eventId).Include(x => x.Comments).FirstOrDefault();
        }

        /// <summary>
        /// Updating an existing Event.
        /// </summary>
        /// <param name="occasion"></param>
        /// <returns></returns>
        public bool UpdateEvent(Event occasion)
        {
            // getting the old event
            var oldEvent = GetEvent(occasion.Id);

            // returning false if the event doesn't exist
            if (oldEvent == null)
                return false;

            // updating old event with applying the new changes
            oldEvent.Title = occasion.Title;
            oldEvent.Description = occasion.Description;
            oldEvent.InvolvedPerson = occasion.InvolvedPerson;
            _context.Update(oldEvent);
            return true;
        }

        /// <summary>
        /// Checking if an Event exists by given Id.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public bool EventExists(Guid eventId)
        {
            // checking parameter
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            // returning boolean based on result
            return _context.Events.Any(e => e.Id == eventId);
        }

        /// <summary>
        /// Getting a page of Events.
        /// </summary>
        /// <param name="pagingParameters"></param>
        /// <returns></returns>
        public Page<EventDto> GetEvents(PagingParameters pagingParameters)
        {
            // preparing the query
            IQueryable<Event> query = _context.Events;

            // counting total objects
            var count = query.Count();

            // returning objects based on given parameters + mapping them to fit the output form
            var items = query
                .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .Include(x => x.Comments)
                .Select(ev => _mapper.Map<EventDto>(ev))
                .ToList();

            // returning a page of events including page informations
            return new Page<EventDto>(items, count, pagingParameters.PageNumber, pagingParameters.PageSize);

        }

        /// <summary>
        /// Deleting an Event by given eventId.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public bool DeleteEvent(Guid eventId)
        {
            // checking parameter
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            // getting the event to delete based on given Id
            var occasion = GetEvent(eventId);

            // returning false in case the event doesn't exist
            if (occasion == null)
                return false;

            // removing the event and returning true
            _context.Events.Remove(occasion);
            return true;
        }

        /// <summary>
        /// Saving changes.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
