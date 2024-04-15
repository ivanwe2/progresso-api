using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Dtos.QuizAssignmentDtos
{
    public class QuizAssignmentRequestDto
    {
        public Guid QuizId { get; set; }

        public int AssigneeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
