namespace Prime.Progreso.Domain.Abstractions.Helpers
{
    public interface IJavaApiHelper
    {
        public string ApiKey { get; }
        public string LoggedUserEmail { get; }
        public string XApiKeyValue { get; init; }
        public string AdminEmail { get; init; }
        public string GetSeasonsUrl { get; }
    }
}
