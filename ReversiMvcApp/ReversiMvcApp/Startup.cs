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
using Microsoft.Extensions.Options;
using ReversiMvcApp.Data;
using ReversiMvcApp.Data.Context;
using ReversiMvcApp.Hangfire;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using ReversiMvcApp.SignalR;
using System;

namespace ReversiMvcApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        private string AllowEverythingPolicy = "AllowAllOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Certificate handling
            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate();

            //Db Connections
            services.AddDbContext<ReversiDbIdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ReversiConnection"));
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
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
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
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true, // Migration to Schema 7 is required
                }));
            
            services.AddHangfireServer();

            services.AddCors((options) =>
            {
                options.AddPolicy(name: AllowEverythingPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            ISpelJob spelJob)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            //Hangire Jobs
            recurringJobManager.AddOrUpdate("Update AFK Games", () => spelJob.UpdateAFKGames(), Cron.Minutely());

            app.UseRouting();

            //Authorization
            app.UseCors(AllowEverythingPolicy);
            app.UseAuthentication();
            app.UseAuthorization();

            //This is required for portforwarding to work
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
                endpoints.MapHub<SpelHub>("/spelHub")
                    .RequireCors(AllowEverythingPolicy);

                var options = new DashboardOptions
                {
                    Authorization = new[] { new AuthorizationFilter() }
                };
                endpoints.MapHangfireDashboard(options);
            });
        }
    }
}
