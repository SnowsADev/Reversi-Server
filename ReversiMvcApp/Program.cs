using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

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

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 443;
});


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

services.AddIdentity<Speler, IdentityRole>(options =>
{
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
services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
    options.Events.OnRedirectToLogout = context =>
    {
        context.Response.Redirect("/Account/Logout");
        return Task.CompletedTask;
    };
});

services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Rate-Limit", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 50;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

// CORS
string CORSPolicy = "CORSPolicy";

builder.Services.AddCors((options) =>
{
    options.AddPolicy(name: CORSPolicy, policy =>
    {
        if (Debugger.IsAttached)
        {
            policy.WithOrigins("https://localhost:44378/", "http://localhost:52463/")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins("https://*.hbo-ict.org/")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});


if (builder.Environment.IsDevelopment())
{
    services.AddDatabaseDeveloperPageExceptionFilter();
}

services.AddAntiforgery(x => x.SuppressXFrameOptionsHeader = true);

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
app.UseHttpsRedirection();

//Authorization
app.UseCors(CORSPolicy);
app.UseAuthentication();
app.UseAuthorization();

var policyCollection = new HeaderPolicyCollection()
        .AddDefaultSecurityHeaders()
        .AddContentTypeOptionsNoSniff()
        .AddReferrerPolicySameOrigin()
        .AddFrameOptionsDeny()
        .AddStrictTransportSecurityNoCache()
        .AddContentSecurityPolicy(builder =>
        {
            builder.AddUpgradeInsecureRequests(); // upgrade-insecure-requests
            builder.AddBlockAllMixedContent(); // block-all-mixed-content

            if (Debugger.IsAttached)
            {
                builder.AddDefaultSrc() // default-src 'self' http://testUrl.com
                .Self()
                .From("newsapi.org");

                builder.AddConnectSrc() // connect-src 'self' http://testUrl.com
                    .Self()
                    .From("newsapi.org")
                    .From("localhost:*")
                    .From("wss:");

                builder.AddImgSrc() // img-src https:
                    .OverHttps();

                builder.AddScriptSrc() // script-src 'self' 'unsafe-inline' 'unsafe-eval' 'report-sample'
                    .Self()
                    .From("cdn.jsdelivr.net")
                    .From("ajax.googleapis.com")
                    .WithNonce();

                builder.AddStyleSrc() // style-src 'self' 'strict-dynamic'
                    .Self()
                    .From("cdn.jsdelivr.net")
                    .UnsafeInline()
                    .UnsafeEval();

                builder.AddFontSrc()
                    .Self()
                    .From("data:");
            }
            else
            {

                builder.AddDefaultSrc()
                    .Self()
                    .From("newsapi.org");

                builder.AddConnectSrc()
                    .Self()
                    .From("newsapi.org");

                builder.AddScriptSrc()
                    .Self()
                    .From("cdn.jsdelivr.net")
                    .From("ajax.googleapis.com")
                    .WithNonce();

                builder.AddStyleSrc()
                    .Self()
                    .From("cdn.jsdelivr.net")
                    .WithNonce();

                builder.AddImgSrc()
                    .Self()
                    .OverHttps();
            }
        });

app.UseSecurityHeaders(policyCollection);
app.UseRateLimiter();
app.UseSecurityHeaders(policyCollection);

//Mapping
app.MapControllers()
    .RequireCors(CORSPolicy)
    .RequireRateLimiting("Rate-Limit");

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireCors(CORSPolicy)
    .RequireRateLimiting("Rate-Limit");

app.MapRazorPages()
    .RequireCors(CORSPolicy)
    .RequireRateLimiting("Rate-Limit");

app.MapHub<SpelHub>("/spelHub")
    .RequireRateLimiting("Rate-Limit");

app.MapHangfireDashboard(new DashboardOptions
{
    Authorization = new[] { new AuthorizationFilter() }
});

#endregion
app.Run();
