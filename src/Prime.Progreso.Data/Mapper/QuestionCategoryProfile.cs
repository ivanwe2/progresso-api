using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.QuestionCategoryDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class QuestionCategoryProfile : Profile
    {
        public QuestionCategoryProfile() 
        { 
            CreateMap<QuestionCategory, QuestionCategoryResponseDto>();

            CreateMap<QuestionCategoryRequestDto, QuestionCategory>();
        }
    }
}
