using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProgrammationConformit.Entities;
using TestProgrammationConformit.Entities.Pagination;

namespace TestProgrammationConformit.Services
{
    public interface IEventCommentRepository
    {
        // Comments part
        IEnumerable<Comment> GetComments(Guid eventId);
        Comment GetComment(Guid eventId, Guid commentId);
        void AddComment(Guid eventId, Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(Comment comment);
        bool CommentExists(Guid commentId);

        // Events part
        Task<PagedList<Event>> GetEvents(PagingParameters pagingParameters);
        IEnumerable<Event> GetEvents(IEnumerable<Guid> eventIds);
        Event GetEvent(Guid eventId);
        void AddEvent(Event occasion);
        void UpdateEvent(Event occasion);
        void DeleteEvent(Event occasion);
        bool EventExists(Guid eventId);

        bool save();
    }
}
