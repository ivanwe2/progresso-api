using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.CurriculumDtos;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;
using Prime.Progreso.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Mapper
{
    public class CurriculumProfile : Profile
    {
        public CurriculumProfile()
        {
            CreateMap<Curriculum, CurriculumRequestDto>().ReverseMap();
            CreateMap<Curriculum, CurriculumResponseDto>().ReverseMap();
        }
    }
}
