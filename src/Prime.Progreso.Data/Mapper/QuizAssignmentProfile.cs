using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.QuizAssignmentDtos;
using Prime.Progreso.Domain.Dtos.QuizExecutionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Mapper
{
    public class QuizAssignmentProfile : Profile
    {
        public QuizAssignmentProfile()
        {
            CreateMap<QuizAssignmentRequestDto, QuizAssignment>();
            CreateMap<QuizAssignment, QuizAssignmentResponseDto>();
        }
    }
}
