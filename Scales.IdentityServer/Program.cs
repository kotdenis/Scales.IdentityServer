using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scales.IdentityServer.Data;
using Scales.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");
var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptionsAction => npgsqlOptionsAction.MigrationsAssembly(migrationsAssembly)));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var serverBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
    .AddAspNetIdentity<AppUser>()
    .AddConfigurationStore(options =>
        options.ConfigureDbContext = b => b.UseNpgsql(connectionString, npgsqlOptionsAction => npgsqlOptionsAction.MigrationsAssembly(migrationsAssembly)))
    .AddOperationalStore(options =>
        options.ConfigureDbContext = b => b.UseNpgsql(connectionString, npgsqlOptionsAction => npgsqlOptionsAction.MigrationsAssembly(migrationsAssembly)));

// not recommended for production - you need to store your key material somewhere secure
serverBuilder.AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseForwardedHeaders();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

await app.PopulateAsync(builder.Configuration);

app.Run();
