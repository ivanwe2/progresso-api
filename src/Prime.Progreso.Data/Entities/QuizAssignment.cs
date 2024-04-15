using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Entities
{
    public class QuizAssignment : BaseEntity
    {
        [Required]
        public Guid QuizId { get; set; }

        public Quiz Quiz { get; set; }

        [Required]
        public int AssigneeId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
