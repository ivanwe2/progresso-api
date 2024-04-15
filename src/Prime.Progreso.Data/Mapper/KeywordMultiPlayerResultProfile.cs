using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.KeywordMultiPlayerResultDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class KeywordMultiPlayerResultProfile : Profile
    {
        public KeywordMultiPlayerResultProfile()
        {
            CreateMap<KeywordMultiPlayerResultRequestDto, KeywordMultiPlayerResult>();
            CreateMap<KeywordMultiPlayerResult, KeywordMultiPlayerResultResponseDto>();
        }
    }
}
