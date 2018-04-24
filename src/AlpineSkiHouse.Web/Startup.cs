using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Serilog;
using Microsoft.Extensions.Options;
using System.Globalization;
using AlpineSkiHouse.Web.Configuration.Models;
using AlpineSkiHouse.Web.Conventions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AlpineSkiHouse.Web.Data;
using AlpineSkiHouse.Web.Models;
using AlpineSkiHouse.Web.Security;
using AlpineSkiHouse.Web.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace AlpineSkiHouse.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = AlpineConfigurationBuilder.Build(env);
            CurrentEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }

        public IHostingEnvironment CurrentEnvironment {get ;}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddScoped<SingleInstanceFactory>(p => t => p.GetRequiredService(t));
            services.AddScoped<MultiInstanceFactory>(p => t => p.GetServices(t));

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddDbContext<ApplicationUserContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<SkiCardContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<PassContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<PassTypeContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ResortContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationUserContext>()
                .AddDefaultTokenProviders();

            services.Configure<AzureStorageSettings>(Configuration.GetSection("MicrosoftAzureStorage"));
            services.AddTransient<IBlobFileUploadService, BlobFileUploadService>();

            services.AddSingleton<IAuthorizationHandler, EditSkiCardAuthorizationHandler>();

            services.AddOptions();
            services.Configure<CsrInformationOptions>(Configuration.GetSection("CsrInformationOptions"));
            services.AddScoped<ICsrInformationService, CsrInformationService>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc(mvcOptions =>
            {
                mvcOptions.Conventions.Add(new AutoValidateAntiForgeryTokenModelConvention());
            })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();


            var supportedCultures = new[]
            {
                new CultureInfo("en-CA"),
                new CultureInfo("fr-CA")
            };
            
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-CA");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            if (!CurrentEnvironment.IsDevelopment())
            {
                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                });
            }

            services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions()
            {
                DeveloperMode = true
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = new PathString("/login"))
                .AddFacebook(o =>
                {
                    o.AppId = Configuration["Authentication:Facebook:AppId"];
                    o.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddTwitter(o =>
                {
                    o.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                    o.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
        {
            // Console logging
            // uncomment to use the default console logger
            //var loggingConfig = Configuration.GetSection("Logging");
            //loggerFactory.AddConsole(loggingConfig);
            // end of Console logging

            loggerFactory.AddDebug((className, logLevel) =>
            {
                if (className.StartsWith("AlpineSkiHouse."))
                    return true;
                return false;
            });

            // Serilog config
            // comment out if using the default console logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()                
                .MinimumLevel.Override("AlpineSkiHouse", Serilog.Events.LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            loggerFactory.AddSerilog();
            applicationLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            // end of Serilog config

            
            //app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseRequestLocalization(requestLocalizationOptions.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
