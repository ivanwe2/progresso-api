using System.ComponentModel.DataAnnotations;

namespace Prime.Progreso.Data.Entities
{
    public class AnswerChoice : BaseEntity
    {
        [Required]
        public Guid QuizExecutionId { get; set; }

        public QuizExecution QuizExecution { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        public Question Question { get; set; }

        [Required]
        public Guid ChoiceId { get; set; }

        public Answer Choice { get; set; }
    }
}
