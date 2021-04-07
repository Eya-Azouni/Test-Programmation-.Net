using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Models
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateOfCreation { get; set; }
    }
}
