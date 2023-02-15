using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scales.IdentityServer.Configuration;
using Scales.IdentityServer.Constants;
using Scales.IdentityServer.Models;
using System.Security.Claims;

namespace Scales.IdentityServer.Data
{
    public static class SeedData
    {
        public static async Task PopulateAsync(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            var configurationContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            configurationContext.Database.Migrate();
            var identityContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            identityContext.Database.Migrate();

            if (!identityContext.Users.Any())
            {
                var usrManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var user = new AppUser
                {
                    Email = "admin@test.com",
                    UserName = "Admin"
                };
                var result = await usrManager.CreateAsync(user, "Password123$");
                if (result.Succeeded)
                {
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    foreach (var roleName in RoleConstants.ROLES)
                        await roleManager.CreateAsync(new IdentityRole { Name = roleName });
                    await usrManager.AddToRoleAsync(user, RoleConstants.ADMIN_ROLE);
                    await usrManager.AddClaimsAsync(user, new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, user.UserName),
                        new Claim(JwtClaimTypes.Role, RoleConstants.ADMIN_ROLE)
                    });
                }
            }

            if (!configurationContext.Clients.Any())
            {
                foreach (var client in IdentityConfig.Clients(configuration))
                {
                    configurationContext.Clients.Add(client.ToEntity());
                }
                await configurationContext.SaveChangesAsync();
            }

            if (!configurationContext.IdentityResources.Any())
            {
                foreach (var resource in IdentityConfig.IdentityResources)
                {
                    configurationContext.IdentityResources.Add(resource.ToEntity());
                }
                await configurationContext.SaveChangesAsync();
            }

            if (!configurationContext.ApiScopes.Any())
            {
                foreach (var resource in IdentityConfig.ApiScopes)
                {
                    configurationContext.ApiScopes.Add(resource.ToEntity());
                }
                await configurationContext.SaveChangesAsync();
            }

            if (!configurationContext.ApiResources.Any())
            {
                foreach (var resource in IdentityConfig.ApiResources)
                {
                    configurationContext.ApiResources.Add(resource.ToEntity());
                }
                await configurationContext.SaveChangesAsync();
            }
        }
    }
}

