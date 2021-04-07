using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Entities.Pagination
{
    public class PagingParameters
    {
        const int MaxPageSize = 100;

        [FromQuery]
        [Range(1, int.MaxValue, ErrorMessage = "Page Number must be greater than 0.")]
        public int PageNumber { get; set; }

        [FromQuery]
        [Range(1, MaxPageSize, ErrorMessage = "Page Size must be greater than 0 and less than 100.")]
        public int PageSize { get; set; }
    }
}
