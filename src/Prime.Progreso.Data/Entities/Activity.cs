using Prime.Progreso.Domain.Enums;

namespace Prime.Progreso.Data.Entities
{
    public class Activity : BaseEntity
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public ActivityType? Type { get; set; }
    }
}
