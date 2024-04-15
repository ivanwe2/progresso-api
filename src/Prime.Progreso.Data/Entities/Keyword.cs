using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Data.Entities
{
    public class Keyword : BaseEntity
    {
        public string Word { get; set; }

        public Guid LanguageId { get; set; }

        public Language Language { get; set; }

        public List<KeywordDescription> KeywordDescriptions { get; set; }
    }
}
