using Prime.Progreso.Domain.Dtos.BaseDtos;
using Prime.Progreso.Domain.Dtos.KeywordDescriptionDtos;
using Prime.Progreso.Domain.Dtos.LanguageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Dtos.KeywordDtos
{
    public class KeywordResponseDto : BaseResponseDto
    {
        public string Word { get; set; }

        public Guid LanguageId { get; set; }

        public List<KeywordDescriptionResponseDto> KeywordDescriptions { get; set; }
    }
}
