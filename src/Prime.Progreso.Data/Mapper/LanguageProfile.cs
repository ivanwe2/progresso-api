using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.LanguageDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile() 
        {
            CreateMap<Language, LanguageRequestDto>().ReverseMap();
            CreateMap<Language, LanguageResponseDto>().ReverseMap();
        }
    }
}
