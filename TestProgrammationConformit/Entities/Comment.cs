using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTimeOffset DateOfCreation { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public Guid EventId { get; set; }
    }
}
