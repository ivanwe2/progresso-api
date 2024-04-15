namespace Prime.Progreso.Domain.Dtos.CurriculumDtos
{
    public class CurriculumRequestDto
    {
        public Guid TechnologyId { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }
    }
}
