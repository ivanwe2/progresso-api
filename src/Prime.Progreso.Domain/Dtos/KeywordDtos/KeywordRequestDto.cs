using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Dtos.KeywordDtos
{
    public class KeywordRequestDto
    {
        public string Word { get; set; }

        public Guid LanguageId { get; set; }
    }
}
