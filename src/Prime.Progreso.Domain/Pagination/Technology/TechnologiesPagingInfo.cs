using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prime.Progreso.Domain.Pagination;

namespace Prime.Progreso.Domain.Pagination.Technology
{
    public class TechnologiesPagingInfo : PagingInfo
    {
        public string PartOfNameFilter { get; set; } = string.Empty;
    }
}
