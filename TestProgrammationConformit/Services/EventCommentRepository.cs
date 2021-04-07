﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public void DeleteComment(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            if(comment.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(comment.Id));

            _context.Comments.Remove(comment);
        }

        public void DeleteEvent(Event occasion)
        {
            if (occasion == null)
                throw new ArgumentNullException(nameof(occasion));

            if (occasion.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(occasion.Id));

            _context.Events.Remove(occasion);
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

            return _context.Events.Where(e => e.Id == eventId).FirstOrDefault();
        }

        public Page<EventDto> GetEvents(PagingParameters pagingParameters)
        {
            IQueryable<Event> query = _context.Events;

            var count =  query.Count();
            var items =  query
                .Skip(pagingParameters.PageNumber)
                .Take(pagingParameters.PageSize)
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

        public void UpdateComment(Comment comment)
        {
            // not implemented
        }

        public void UpdateEvent(Event occasion)
        {
            // not implemented
        }


    }
}