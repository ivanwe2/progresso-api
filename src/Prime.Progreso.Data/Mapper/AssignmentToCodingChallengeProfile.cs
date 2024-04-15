using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.AssignmentToChallengeDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class AssignmentToCodingChallengeProfile : Profile
    {
        public AssignmentToCodingChallengeProfile()
        {
            CreateMap<AssignmentRequestDto, AssignmentToCodingChallenge>();

            CreateMap<UnassignmentRequestDto, AssignmentToCodingChallenge>()
                .ForMember(dest => dest.InternId, opt => opt.Ignore())
                .ForMember(dest => dest.CodingChallengeId, opt => opt.Ignore());

            CreateMap<AssignmentToCodingChallenge, AssignmentResponseDto>();
        }
    }
}
