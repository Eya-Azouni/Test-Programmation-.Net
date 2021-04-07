using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Models
{
    public class EventForPOSTDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string InvolvedPerson { get; set; }

        public ICollection<CommentForPOSTDto> Comments { get; set; } = new List<CommentForPOSTDto>();
    }
}
