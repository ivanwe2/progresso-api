using Swashbuckle.AspNetCore.Annotations;

namespace Prime.Progreso.Domain.Pagination
{
    public class PagingInfo
    {
        [SwaggerParameter("Zero-based page index (0..N)")]
        public int Page { get; set; } = 0;

        [SwaggerParameter("The size of the page to be returned")]
        public int Size { get; set; } = 10;
    }
}
