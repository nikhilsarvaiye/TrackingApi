namespace Api
{
    using Common.Middleware;
    using Configuration.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Web;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Services;
    using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
    using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Configuration = configuration;

            var builder = new ConfigurationBuilder().AddConfiguration(configuration);

            /* Add Default configurations */
            builder.AddConfiguration(configuration);

            /* Add Custom configurations */
            // adding cache.json which contains cachemanager configuration(s)
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
            if (true || appOptions.Cache)
            {
                builder.AddJsonFile("cache.json");
            }
            Configuration = builder.Build();

            /*
            // For manual for like console app.            
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder().AddJsonFile("cache.json");
            Configuration = builder.Build();
            
            // var cacheConfiguration = Configuration.GetCacheConfiguration();
            var cacheConfiguration =
                Configuration.GetCacheConfiguration("retaileasy_cache")
                    .Builder
                    .WithMicrosoftLogging(f =>
                    {
                        f.AddSerilog();
                        // f.AddDebug(LogLevel.Information);
                    })
                    .Build();
            */

            SetupLogging(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ------ Authentication --------

            // -- Direct web api authentication --
            // services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            // services.AddControllersWithViews(options =>
            // {
            // var policy = new AuthorizationPolicyBuilder()
            //         .RequireAuthenticatedUser()
            //         .Build();
            // options.Filters.Add(new AuthorizeFilter(policy));
            // });
            // -- End direct web api authentication --

            // -- JWT web api authentication --
            // Use below for JWT web api authentication from authentication header
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            // Azure AD Auth
            // services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureAd", "Bearer", true);

            /* Customization */

            //services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //    var existingOnTokenValidatedHandler = options.Events.OnTokenValidated;
            //    options.Events.OnMessageReceived = async context =>
            //    {
            //    };
            //    options.Events.OnTokenValidated = async context =>
            //    {
            //        await existingOnTokenValidatedHandler(context);
            //        // Your code to add extra configuration that will be executed after the current event implementation.
            //        // options.TokenValidationParameters.ValidIssuers = new List<string>() { /* list of valid issuers */ };
            //        // options.TokenValidationParameters.ValidAudiences = new List<string>() { /* list of valid audiences */ };
            //    };
            //    options.Events.OnAuthenticationFailed = async context =>
            //    {
            //    };
            //});
            /* End Customization */

            // -- End JWT web api authentication --

            // ------ End Authentication --------

            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            // Configurations
            var appOptionsSection = Configuration.GetSection(nameof(AppOptions));
            var appOptions = appOptionsSection.Get<AppOptions>();
            services.Configure<AppOptions>(appOptionsSection);
            services.Configure<SqlDbOptions>(Configuration.GetSection(nameof(SqlDbOptions)));
            services.AddSingleton<IAppOptions>(options => options.GetRequiredService<IOptions<AppOptions>>().Value);
            services.AddSingleton<IDbOptions>(options => options.GetRequiredService<IOptions<SqlDbOptions>>().Value);

            services.AddLogging(c => c.AddConsole().AddDebug().AddConfiguration(Configuration));

            // Enable Caching
            if (true || appOptions.Cache)
            {
                // Cache manager
                // using the new overload which adds a singleton of the configuration to services and the configure method to add logging
                // TODO: still not 100% happy with the logging part
                // services.AddCacheManagerConfiguration(Configuration, cfg => cfg.WithMicrosoftLogging(services));
                services.AddCacheManagerConfiguration(Configuration);

                // uses a refined configurastion (this will not log, as we added the MS Logger only to the configuration above
                // services.AddCacheManager<int>(Configuration, configure: builder => builder.WithJsonSerializer());
                // creates a completely new configuration for this instance (also not logging)
                // services.AddCacheManager<DateTime>(inline => inline.WithDictionaryHandle());

                // any other type will be  Configurastion used will be the one defined by AddCacheManagerConfiguration earlier.
                services.AddCacheManager();
            }

            // Services/Repositories Configurations
            services.ConfigureServices(appOptions);

            // OData
            // services.AddOData();
            // services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            loggerFactory.AddSerilog();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            app.UseHttpsRedirection();

            app.UseRouting();

            // CORS
            // app.UseCors();
            
            /* Enable
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                      builder =>
                      {
                          builder.
                               AllowAnyHeader().
                               AllowAnyMethod().
                               AllowCredentials().

                          WithOrigins(Configuration.GetSection("CORS:ConnectionString").Get<string>());

                          builder.Build();
                      });
            });
            */

            // Logging
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // odata
                // endpoints.Select().Filter().OrderBy().Count().MaxTop(10);
                // endpoints.EnableDependencyInjection();//This guy solves the problem
                // endpoints.MapODataRoute("odata", "odata", GetEdmModel());
            });
        }

        private static void SetupLogging(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();
        }
    }
}
