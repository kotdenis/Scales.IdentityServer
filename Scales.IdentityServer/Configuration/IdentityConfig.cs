using Duende.IdentityServer.Models;
using Duende.IdentityServer;

namespace Scales.IdentityServer.Configuration
{
    public class IdentityConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("journalAPI"),
                new ApiScope("referenceAPI")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("journalAPI", "Journal API") { Scopes = {"journalAPI"}},
                new ApiResource("referenceAPI", "Allows the access to reference books") { Scopes = {"referenceAPI"} }
            };

        public static IEnumerable<Client> Clients (IConfiguration configuration) =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "blazorWASM",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowOfflineAccess = true,
                    AllowedCorsOrigins = { configuration.GetSection("BlazorOrigin").Value },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "journalAPI",
                        "referenceAPI"
                    },
                    RedirectUris = { $"{configuration.GetSection("BlazorOrigin").Value}/authentication/login-callback" },
                    PostLogoutRedirectUris = { $"{configuration.GetSection("BlazorOrigin").Value}/authentication/logout-callback" }
                }
            };
    }
}
