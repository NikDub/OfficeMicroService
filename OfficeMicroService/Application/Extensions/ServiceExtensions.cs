using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeMicroService.Application.Middlewares;
using OfficeMicroService.Application.Services;
using OfficeMicroService.Data.Mapper;
using OfficeMicroService.Data.Settings;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace OfficeMicroService.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   {
                       options.Authority = configuration.GetValue<string>("Routes:AuthorityRoute");
                       options.Audience = "TestsAPI";
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateAudience = false
                       };
                   });
        }
        public static void ConfigureDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OfficeStoreDatabaseSettings>(configuration.GetSection(nameof(OfficeStoreDatabaseSettings)));

            services.AddSingleton<IOfficeStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<OfficeStoreDatabaseSettings>>().Value);
        }
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IOfficeServices, OfficeServices>();
            services.AddAutoMapper(typeof(MappingProfile));
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        { Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                        },
                        new List<string>()
                    }
                });

                setup.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Comments.xml"));
            });
        }
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static void ConfigureSerilog(this IServiceCollection services, IHostBuilder host)
        {
            host.UseSerilog((hostingContext, loggerConfiguration) =>
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
        }
    }
}
