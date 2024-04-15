namespace Prime.Progreso.Data.Entities
{
    public class CurriculumItem : BaseEntity
    {
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }

        public Guid CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }

        public int DayOfInternship { get; set; }
    }
}
