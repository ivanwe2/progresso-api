namespace Prime.Progreso.Data.Entities
{
    public class Curriculum : BaseEntity
    {
        public Guid TechnologyId { get; set; }

        public Technology Technology { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public virtual List<CurriculumItem> CurriculumItems { get; set; }
    }
}
