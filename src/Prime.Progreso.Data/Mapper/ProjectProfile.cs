using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.Projects;

namespace Prime.Progreso.Data.Mapper
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectRequestDto, Project>()
                .ForMember(x => x.Milestones, option => option.Ignore());

            CreateMap<Project, ProjectResponseDto>()
                 .ForMember(x => x.Milestones, opt => opt.MapFrom(src => src.Milestones.Select(e => e.Id)));
        }
    }
}
