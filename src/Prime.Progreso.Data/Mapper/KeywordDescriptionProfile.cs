using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class KeywordDescriptionProfile : Profile
    {
        public KeywordDescriptionProfile()
        {
            CreateMap<KeywordDescriptionRequestDto, KeywordDescription>();
            CreateMap<KeywordDescription, KeywordDescriptionResponseDto>().ReverseMap();
        }
    }
}
