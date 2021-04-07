using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Models
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InvolvedPerson { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
            = new List<CommentDto>();
    }
}
