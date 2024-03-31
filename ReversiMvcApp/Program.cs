using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Data;
using ReversiMvcApp.Data.Context;
using ReversiMvcApp.Hangfire;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using ReversiMvcApp.SignalR;
using System;
using System.IO;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development,
    WebRootPath = "wwwroot"
});


#region Logging
builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
#endregion

builder.WebHost.UseUrls("http://localhost:5000");

#region Services
var services = builder.Services;

//Certificate handling
services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();

//Db Connections
services.AddDbContext<ReversiDbIdentityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReversiConnection"));
});

services.AddIdentity<Speler, IdentityRole>(options => {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 10;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ReversiDbIdentityContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// MVC
services.AddControllersWithViews().AddRazorRuntimeCompilation();
services.AddRazorPages();

//SignalR
services.AddSignalR();

//Custom AccessLayers
services.AddScoped<ISpelRepository, SpelAccessLayer>();
services.AddScoped<IUserRepository, UserAccessLayer>();
services.AddScoped<ISpelJob, SpelJob>();

//Hangfire
services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true, // Migration to Schema 7 is required
    }));

services.AddHangfireServer();

// CORS
string AllowEverythingPolicy = "AllowAllOrigins";

builder.Services.AddCors((options) =>
{
    options.AddPolicy(name: AllowEverythingPolicy, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

if (builder.Environment.IsDevelopment())
{
    services.AddDatabaseDeveloperPageExceptionFilter();
}

#endregion

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

// Setup Hangfire Jobs
using (var scope = app.Services.CreateScope())
{
    IRecurringJobManager recurringJobManager = app.Services.GetService<IRecurringJobManager>();
    var spelJob = scope.ServiceProvider.GetService<ISpelJob>();

    recurringJobManager.AddOrUpdate("Update AFK Games", () => spelJob.UpdateAFKGames(), Cron.Minutely());
}

#region Routing & Authentication

//This is required for portforwarding to work
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Routing
app.UseRouting();

//Authorization
app.UseCors(AllowEverythingPolicy);
app.UseAuthentication();
app.UseAuthorization();


//Mapping
app.MapControllers();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<SpelHub>("/spelHub")
    .RequireCors(AllowEverythingPolicy);

app.MapHangfireDashboard(new DashboardOptions
{
    Authorization = new[] { new AuthorizationFilter() }
});

#endregion
app.Run();
