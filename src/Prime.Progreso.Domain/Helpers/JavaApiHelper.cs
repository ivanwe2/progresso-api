using Microsoft.Extensions.Options;
using Prime.Progreso.Domain.Abstractions.Helpers;

namespace Prime.Progreso.Domain.Helpers
{
    public class JavaApiHelper : IJavaApiHelper
    {
        public string ApiKey { get; }
        public string LoggedUserEmail { get; }
        public string XApiKeyValue { get; init; }
        public string AdminEmail { get; init; }
        public string GetSeasonsUrl { get; }

        public JavaApiHelper() { }

        public JavaApiHelper(IOptions<JavaApiHelper> helperOptions)
        {
            ApiKey = "x-api-key";
            LoggedUserEmail = "logged-user-email";
            XApiKeyValue = helperOptions.Value.XApiKeyValue;
            AdminEmail = helperOptions.Value.AdminEmail;
            GetSeasonsUrl = "https://int-team.protal.biz/progreso/dev/java-api/seasons";
        }
    }
}
