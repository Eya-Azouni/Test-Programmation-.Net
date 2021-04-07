using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProgrammationConformit.Entities;
using TestProgrammationConformit.Entities.Pagination;
using TestProgrammationConformit.Models;

namespace TestProgrammationConformit.Services
{
    public interface IEventCommentRepository
    {
        // ************ Comments part ************
        IEnumerable<Comment> GetComments(Guid eventId);
        Comment GetComment(Guid eventId, Guid commentId);
        void AddComment(Guid eventId, Comment comment);
        bool UpdateComment(Guid eventId, Comment comment);
        bool DeleteComment(Guid eventId, Guid commentId);
        bool CommentExists(Guid commentId);

        // ************ Events part ************
        Page<EventDto> GetEvents(PagingParameters pagingParameters);
        IEnumerable<Event> GetEvents(IEnumerable<Guid> eventIds);
        Event GetEvent(Guid eventId);
        void AddEvent(Event occasion);
        bool UpdateEvent(Event occasion);
        bool DeleteEvent(Guid eventId);
        bool EventExists(Guid eventId);

        bool Save();
    }
}
