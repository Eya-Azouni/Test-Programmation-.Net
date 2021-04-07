using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Profiles
{
    public class CommentsProfile : Profile
    {
        public CommentsProfile()
        {
            CreateMap<Entities.Comment, Models.CommentDto>();
            CreateMap<Models.CommentForPOSTDto, Entities.Comment>();
            CreateMap<Models.CommentForPUTDto, Entities.Comment>();
        }
    }
}
