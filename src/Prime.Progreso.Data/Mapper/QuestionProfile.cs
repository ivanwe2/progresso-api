using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.QuestionDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile() 
        {
            CreateMap<QuestionRequestDto, Question>()
                .ForMember(dest => dest.QuestionCategories, opt => opt.Ignore())
                .ForMember(dest => dest.CategorizedQuestions, opt => opt.MapFrom((src, dest) => MapCategorizedQuestions(src, dest)));

            CreateMap<Question, QuestionResponseDto>()
                .ForMember(dest => dest.QuestionCategoryIds,
                           opt => opt.MapFrom(src => src.CategorizedQuestions.Select(qc => qc.QuestionCategoryId).ToList()));
        }

        private List<CategorizedQuestion> MapCategorizedQuestions(QuestionRequestDto source, Question destination)
        {
            return source.QuestionCategoryIds.Select(categoryId => new CategorizedQuestion
            {
                Question = destination,
                QuestionCategoryId = categoryId
            }).ToList();
        }
    }
}
