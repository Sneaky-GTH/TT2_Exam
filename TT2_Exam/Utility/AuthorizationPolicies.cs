using Microsoft.AspNetCore.Authorization;

namespace TT2_Exam.Utility
{
    public static class AuthorizationPolicies
    {
        public const string RequireAdmin = "RequireAdmin";
        public const string RequirePublisher = "RequirePublisher";

        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(RequireAdmin, policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy(RequirePublisher, policy =>
                policy.RequireRole("Admin", "Publisher"));
            
            options.AddPolicy("IsPublisher", policy =>
                policy.Requirements.Add(new IsPublisherRequirement()));
        }
    }
}