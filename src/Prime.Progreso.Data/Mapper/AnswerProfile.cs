using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.AnswerDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile() 
        {
            CreateMap<AnswerRequestDto, Answer>();
            CreateMap<Answer, AnswerResponseDto>();
        }
    }
}
