using Microsoft.AspNetCore.Http;
using Prime.Progreso.Domain.Abstractions.Providers;
using Prime.Progreso.Domain.Exceptions;

namespace Prime.Progreso.Domain.Providers
{
    public class UserDetailsProvider : IUserDetailsProvider
    {
        private const string ROLE_HEADER_NAME = "logged-user-role";
        private const string USERID_HEADER_NAME = "logged-user-id";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDetailsProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            string value = _httpContextAccessor.HttpContext?.Request?.Headers[USERID_HEADER_NAME];
            bool parse = int.TryParse(value, out int result);
            if (string.IsNullOrEmpty(value) || !parse)
            {
                throw new InvalidHeaderException("Authorization failed! User ID header is missing or invalid!");
            }
            return result;
        }

        public string GetUserRole()
        {
            string value = _httpContextAccessor.HttpContext?.Request?.Headers[ROLE_HEADER_NAME];
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidHeaderException("Authorization failed! User ROLE header is missing or invalid!");
            }
            return value;
        }
    }
}
