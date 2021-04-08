using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Models
{
    public class MessageDto
    {
        public string Message { get; set; }

        public MessageDto(string message)
        {
            Message = message;
        }
    }
}
