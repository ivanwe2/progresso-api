namespace Prime.Progreso.Domain.Dtos.CurriculumItemDtos
{
    public class CurriculumItemRequestDto
    {
        public Guid ActivityId { get; set; }

        public Guid CurriculumId { get; set; }

        public int DayOfInternship { get; set; }
    }
}
