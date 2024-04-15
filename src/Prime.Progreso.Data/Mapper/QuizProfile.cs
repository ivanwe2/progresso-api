using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.QuizDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class QuizProfile :  Profile
    {
        public QuizProfile() 
        {
            CreateMap<QuizRequestDto, Quiz>().ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.QuizQuestionLinks, opt => opt.MapFrom((src, dest) => MapQuizQuestionLinks(src, dest)));
            
            CreateMap<Quiz, QuizResponseDto>().ForMember(
                dest => dest.QuestionIds,
                opt => opt.MapFrom(src => src.QuizQuestionLinks.Select(l => l.QuestionId).ToList()));
        }

        private static List<QuizQuestionLink> MapQuizQuestionLinks(QuizRequestDto source, Quiz destination)
        {
            return source.QuestionIds.Select(questionId => new QuizQuestionLink
            {
                Quiz = destination,
                QuestionId = questionId
            }).ToList();
        }
    }
}
