using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class QuizExecution : BaseEntity
    {
        [Required]
        public Guid QuizId { get; set; }

        public Quiz Quiz { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
