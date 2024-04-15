using Microsoft.AspNetCore.Authorization;

namespace Prime.Progreso.Api.Policies.RolePolicy
{
    public class ValidRoleHandler : AuthorizationHandler<ValidRoleAuthorizationRequirement>
    {
        public const string AUTHORIZATION_KEY_HEADER_NAME = "logged-user-role";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidRoleAuthorizationRequirement requirement)
        {
            SucceedRequirementIfRoleAuthorizationPresentAndValid(context, requirement);

            return Task.CompletedTask;
        }

        private void SucceedRequirementIfRoleAuthorizationPresentAndValid(AuthorizationHandlerContext context, ValidRoleAuthorizationRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                var role = httpContext.Request.Headers[AUTHORIZATION_KEY_HEADER_NAME].FirstOrDefault();
                var roleFromRequest = GetRoleFromRequest(httpContext.Request);

                if (requirement.Role.Any(r => r == roleFromRequest))
                {
                    context.Succeed(requirement);
                    return;
                }

                context.Fail();
                return;
            }
        }
        public string GetRoleFromRequest(HttpRequest request)
        {
            var re = request;
            var headers = re.Headers;
            string role = "";

            if (headers.ContainsKey("logged-user-role"))
            {
                role = headers["logged-user-role"];
            }

            return role;
        }
    }
}
