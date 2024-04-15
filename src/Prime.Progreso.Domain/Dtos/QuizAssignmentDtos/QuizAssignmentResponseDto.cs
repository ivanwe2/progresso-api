using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Dtos.QuizDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Dtos.QuizAssignmentDtos
{
    public class QuizAssignmentResponseDto : BaseResponseDto
    {
        public QuizResponseDto Quiz { get; set; }

        public int AssigneeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
