using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Domain.Dtos.ActivityDtos
{
    public class ActivityResponseDto : BaseResponseDto
    {
        public string Subject { get; set; }

        public string Description { get; set; }

        public ActivityType? Type { get; set; }
    }
}
