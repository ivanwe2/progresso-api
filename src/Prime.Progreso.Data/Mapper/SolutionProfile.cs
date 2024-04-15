using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.SolutionDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class SolutionProfile : Profile
    {
        public SolutionProfile()
        {
            CreateMap<SolutionRequestDto, AssignmentToCodingChallenge>()
                .ForMember(dest => dest.SolutionPath,
                           opts => opts.MapFrom(src => src.SolutionCode));
            CreateMap<AssignmentToCodingChallenge, SolutionResponseDto>()
                .ForMember(dest => dest.AssignmentId,
                           opts => opts.MapFrom(src => src.Id));
        }
    }
}
