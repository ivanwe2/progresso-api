using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionMultiPlayerResultDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class KeywordDescriptionMultiPlayerResultProfile : Profile
    {
        public KeywordDescriptionMultiPlayerResultProfile()
        {
            CreateMap<KeywordDescriptionMultiPlayerResultRequestDto, KeywordDescriptionMultiPlayerResult>();
            CreateMap<KeywordDescriptionMultiPlayerResult, KeywordDescriptionMultiPlayerResultResponseDto>();
        }
    }
}
