using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.KeywordSinglePlayerResultDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class KeywordSinglePlayerResultProfile : Profile
    {
        public KeywordSinglePlayerResultProfile()
        {
            CreateMap<KeywordSinglePlayerResultRequestDto, KeywordSinglePlayerResult>();
            CreateMap<KeywordSinglePlayerResult, KeywordSinglePlayerResultResponseDto>().ReverseMap();
            CreateMap<KeywordSinglePlayerResultIsCorrectUpdateRequestDto, KeywordSinglePlayerResult>();
        }
    }
}
