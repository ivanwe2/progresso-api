using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class TechnologyProfile : Profile
    {
        public TechnologyProfile()
        {
            CreateMap<TechnologyRequestDto, Technology>();
            CreateMap<Technology, TechnologyResponseDto>();
        }
    }
}
