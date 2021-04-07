using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Models
{
    public class CommentForPUTDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
