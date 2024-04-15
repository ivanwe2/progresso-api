using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.ActivityDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile() 
        {
            CreateMap<Activity, ActivityRequestDto>();

            CreateMap<ActivityRequestDto, Activity>()
                .ForMember(
                    dest => dest.Type,
                    opt => opt.AllowNull());

            CreateMap<Activity, ActivityResponseDto>().ReverseMap();
        }
    }
}
