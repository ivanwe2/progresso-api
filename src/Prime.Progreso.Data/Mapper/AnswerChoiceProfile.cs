using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.AnswerChoiceDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class AnswerChoiceProfile : Profile
    {
        public AnswerChoiceProfile()
        {
            CreateMap<AnswerChoiceRequestDto, AnswerChoice>();
            CreateMap<AnswerChoice, AnswerChoiceResponseDto>();
        }
    }
}
