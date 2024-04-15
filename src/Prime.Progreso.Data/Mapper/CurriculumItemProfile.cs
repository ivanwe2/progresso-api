using AutoMapper;
using Prime.Progreso.Data.Entities;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;

namespace Prime.Progreso.Data.Mapper
{
    public class CurriculumItemProfile : Profile
    {
        public CurriculumItemProfile()
        {
            CreateMap<CurriculumItem, CurriculumItemRequestDto>().ReverseMap();
            CreateMap<CurriculumItem, CurriculumItemResponseDto>().ReverseMap();
        }
    }
}
