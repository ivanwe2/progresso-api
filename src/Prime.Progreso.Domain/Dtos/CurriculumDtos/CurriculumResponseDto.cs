using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Dtos.CurriculumItemDtos;
using Prime.Progreso.Domain.Dtos.TechnologyDtos;

namespace Prime.Progreso.Domain.Dtos.CurriculumDtos
{
    public class CurriculumResponseDto : BaseResponseDto
    {
        public Guid TechnologyId { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public List<CurriculumItemResponseDto> CurriculumItems { get; set; }
    }
}
