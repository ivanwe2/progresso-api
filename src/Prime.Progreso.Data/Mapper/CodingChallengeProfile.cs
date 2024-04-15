using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.CodingChallengeDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class CodingChallengeProfile : Profile
    {
        public CodingChallengeProfile()
        {
            CreateMap<CodingChallengeRequestDto, CodingChallenge>();
            CreateMap<CodingChallenge, CodingChallengeResponseDto>().ReverseMap();
        }
    }
}
