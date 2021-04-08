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
        public void AddComment(Guid eventId, Comment comment)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            comment.DateOfCreation = DateTimeOffset.Now;
            comment.EventId = eventId;
            _context.Comments.Add(comment);
        }

        public void AddEvent(Event occasion)
        {
            if(occasion== null)
                throw new ArgumentNullException(nameof(occasion));

            _context.Events.Add(occasion);
        }

        public bool DeleteComment(Guid eventId, Guid commentId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            if(commentId == Guid.Empty)
                throw new ArgumentNullException(nameof(commentId));

            var comment = GetComment(eventId, commentId);

            if (comment == null)
                return false;

            _context.Comments.Remove(comment);
            return true;
        }

        public bool DeleteEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            var occasion = GetEvent(eventId);

            if (occasion == null)
                return false;

            _context.Events.Remove(occasion);
            return true;
        }

        public bool CommentExists(Guid commentId)
        {
            if (commentId == Guid.Empty)
                throw new ArgumentNullException(nameof(commentId));

            return _context.Comments.Any(c => c.Id == commentId);
        }

        public bool EventExists(Guid eventId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            return _context.Events.Any(e => e.Id == eventId);
        }

        public Comment GetComment(Guid eventId, Guid commentId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            if (commentId == Guid.Empty)
                throw new ArgumentNullException(nameof(commentId));

            return _context.Comments.Where(c => c.Id == commentId && c.EventId == eventId).FirstOrDefault();
        }

        public IEnumerable<Comment> GetComments(Guid eventId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            return _context.Comments.Where(c => c.EventId == eventId).OrderBy(c => c.DateOfCreation).ToList();
        }

        public Event GetEvent(Guid eventId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentNullException(nameof(eventId));

            return _context.Events.Where(e => e.Id == eventId).Include(x => x.Comments).FirstOrDefault();
        }

        public Page<EventDto> GetEvents(PagingParameters pagingParameters)
        {
            IQueryable<Event> query = _context.Events;

            var count =  query.Count();
            var items =  query
                .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .Include(x => x.Comments)
                .Select(ev => _mapper.Map<EventDto>(ev))
                .ToList();

            return new Page<EventDto>(items, count, pagingParameters.PageNumber, pagingParameters.PageSize);

        }

        IEnumerable<Event> IEventCommentRepository.GetEvents(IEnumerable<Guid> eventIds)
        {
            if(eventIds == null)
                throw new ArgumentNullException(nameof(eventIds));

            return _context.Events.Where(e => eventIds.Contains(e.Id)).OrderBy(e => e.Title).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool UpdateComment(Guid eventId, Comment comment)
        {
            var oldComment = GetComment(eventId, comment.Id);
            if (oldComment == null)
                return false;

            oldComment.Description = comment.Description;
            _context.Update(oldComment);
            return true;
        }

        public bool UpdateEvent(Event occasion)
        {
            var oldEvent = GetEvent(occasion.Id);
            if (oldEvent == null)
                return false;

            oldEvent.Title = occasion.Title;
            oldEvent.Description = occasion.Description;
            oldEvent.InvolvedPerson = occasion.InvolvedPerson;
            _context.Update(oldEvent);
            return true;
        }


    }
}
