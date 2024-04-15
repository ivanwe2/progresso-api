using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.Milestones;

namespace Prime.Progreso.Data.Mapper
{
    public class MilestoneProfile : Profile
    {
        public MilestoneProfile()
        {
            CreateMap<MilestoneRequestDto, Milestone>();
            CreateMap<Milestone, MilestoneResponseDto>().ReverseMap();
        }
    }
}
