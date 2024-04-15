using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.QuestionCategoryDtos
{
    public class QuestionCategoryResponseDto : BaseResponseDto
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
