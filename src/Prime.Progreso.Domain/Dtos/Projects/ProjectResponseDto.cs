using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.Projects
{
    public class ProjectResponseDto : BaseResponseDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Guid> Milestones { get; set; }
    }
}
