using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class QuizExecutionProfile : Profile
    {
        public QuizExecutionProfile()
        {
            CreateMap<QuizExecutionRequestDto, QuizExecution>();
            CreateMap<QuizExecution, QuizExecutionResponseDto>();
        }
    }
}
