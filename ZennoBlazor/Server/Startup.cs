using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using ZennoBlazor.Server.Data;
using ZennoBlazor.Server.Models;
using Microsoft.AspNetCore.HttpOverrides;
using System;
using Microsoft.Extensions.Logging;

namespace ZennoBlazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;
            Configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.secrets.json")
                     .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            logger.LogInformation($"env.IsDevelopment():- {env.IsDevelopment()}");
            logger.LogInformation($"env.IsProduction():- {env.IsProduction()}");
            logger.LogInformation($"env.EnvironmentName:- {env.EnvironmentName}");


            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                //RequireHeaderSymmetry = false
            };
            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();
            // ref: https://github.com/aspnet/Docs/issues/2384
            app.UseForwardedHeaders(forwardOptions);
            //app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.Use((context, next) =>
            {
                //if (Environment.GetEnvironmentVariable("SSL_OFFLOAD") == "true")
                context.Request.Scheme = "https";
                return next();
            });
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
