using Prime.Progreso.Api.Extenions;

namespace Prime.Progreso.Api.Middleware.Models
{
    public class ProblemDetails
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }

        public override string ToString()
        {
            return this.SerializeWithCamelCase();
        }
    }
}
