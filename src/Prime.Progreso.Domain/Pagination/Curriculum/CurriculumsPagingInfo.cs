using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Progreso.Domain.Pagination.Curriculum
{
    public class CurriculumsPagingInfo : PagingInfo
    {
        public List<Guid> technologyIds = new List<Guid>();

        public string TechnologyFilter { get; set; } = string.Empty;
    }
}
