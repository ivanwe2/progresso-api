namespace Prime.Progreso.Domain.Dtos.Projects
{
    public class ProjectRequestDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Guid> Milestones { get; set; }
    }
}
