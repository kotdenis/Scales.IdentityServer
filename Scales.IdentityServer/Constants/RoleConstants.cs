namespace Scales.IdentityServer.Constants
{
    public class RoleConstants
    {
        public const string ADMIN_ROLE = "admin";
        public const string USER_ROLE = "user";
        public const string MANAGER_ROLE = "manager";
        public static IReadOnlyCollection<string> ROLES = new[] { ADMIN_ROLE, USER_ROLE, MANAGER_ROLE };
    }
}
