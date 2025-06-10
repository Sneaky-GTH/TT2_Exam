using Microsoft.AspNetCore.Authorization;

namespace TT2_Exam.Utility
{
    public static class AuthorizationPolicies
    {
        public const string RequireAdmin = "RequireAdmin";
        public const string RequireDeveloper = "RequireDeveloper";

        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(RequireAdmin, policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy(RequireDeveloper, policy =>
                policy.RequireRole("Admin", "Developer"));
        }
    }
}