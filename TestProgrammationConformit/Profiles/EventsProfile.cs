using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProgrammationConformit.Profiles
{
    public class EventsProfile : Profile
    {
        public EventsProfile()
        {
            CreateMap<Entities.Event, Models.EventDto>().ForMember(d => d.CommentsDto,
                opt => opt.MapFrom(c => c.Comments));
            CreateMap<Models.EventForPOSTDto, Entities.Event >();
            CreateMap<Models.EventForPUTDto, Entities.Event>();
        }
    }
}
