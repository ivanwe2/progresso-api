using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;
using Prime.Progreso.Domain.Dtos.KeywordDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Mapper
{
    public class KeywordProfile : Profile
    {
        public KeywordProfile()
        {
            CreateMap<KeywordRequestDto, Keyword>();
            CreateMap<Keyword, KeywordResponseDto>();
        }
    }
}
