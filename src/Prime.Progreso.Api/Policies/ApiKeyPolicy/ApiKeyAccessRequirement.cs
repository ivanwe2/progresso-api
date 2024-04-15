using Microsoft.AspNetCore.Authorization;

namespace Prime.Progreso.Api.Policies.ApiKeyPolicy
{
    public class ApiKeyAccessRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> ApiKeys { get; set; }

        public ApiKeyAccessRequirement(IEnumerable<string> apiKeys)
        {
            ApiKeys = apiKeys?.ToList() ?? new List<string>();
        }
    }
}
