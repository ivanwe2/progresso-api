using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.CurriculumItemDtos
{
    public class CurriculumItemResponseDto : BaseResponseDto
    {
        public Guid ActivityId { get; set; }

        public Guid CurriculumId { get; set; }

        public int DayOfInternship { get; set; }
    }
}
