using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionSinglePlayerResultDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class KeywordDescriptionSinglePlayerResultProfile : Profile
    {
        public KeywordDescriptionSinglePlayerResultProfile()
        {
            CreateMap<KeywordDescriptionSinglePlayerResultWithIsCorrectDto, KeywordDescriptionSinglePlayerResult>();
            CreateMap<KeywordDescriptionSinglePlayerResultRequestDto, KeywordDescriptionSinglePlayerResultWithIsCorrectDto>();
            CreateMap<KeywordDescriptionSinglePlayerResult, KeywordDescriptionSinglePlayerResultResponseDto>();
        }
    }
}
