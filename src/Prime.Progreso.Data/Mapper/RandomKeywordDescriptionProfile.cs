using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.RandomKeywordDescriptionDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class RandomKeywordDescriptionProfile : Profile
    {
        public RandomKeywordDescriptionProfile() 
        {
            CreateMap<KeywordDescription, RandomKeywordDescriptionResponseDto>()
                .ForMember(dest => dest.Id,
                           opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description,
                           opts => opts.MapFrom(src => src.Description));
        }
    }
}
