using Microsoft.AspNetCore.Authorization;

namespace Prime.Progreso.Api.Policies.RolePolicy
{
    public class ValidRoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public string[] Role { get; }
        
        public ValidRoleAuthorizationRequirement(params string[] role)
        {
            Role = role;
        }
    }
}
