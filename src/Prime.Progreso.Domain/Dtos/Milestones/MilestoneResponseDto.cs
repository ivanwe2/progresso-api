using Prime.Progreso.Domain.Dtos.BaseDtos;

namespace Prime.Progreso.Domain.Dtos.Milestones
{
    public class MilestoneResponseDto : BaseResponseDto
    {
        public string Description { get; set; }
        public int Order { get; set; }
        public int Duration { get; set; }
    }
}
