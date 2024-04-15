using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Prime.Progreso.Api.Policies.ApiKeyPolicy
{
    public class ApiKeyAccessRequirementHandler : AuthorizationHandler<ApiKeyAccessRequirement>
    {
        public const string API_KEY_HEADER_NAME = "X-API-KEY";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyAccessRequirement requirement)
        {
            SucceedRequirementIfApiKeyPresentAndValid(context, requirement);
            return Task.CompletedTask;
        }

        private void SucceedRequirementIfApiKeyPresentAndValid(AuthorizationHandlerContext context, ApiKeyAccessRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                var apiKey = httpContext.Request.Headers[API_KEY_HEADER_NAME].FirstOrDefault();
                if (apiKey is null || !requirement.ApiKeys.Any(requiredApiKey => apiKey == requiredApiKey))
                {
                    context.Fail();
                    return;
                }
                context.Succeed(requirement);
            }
        }
    }
}
