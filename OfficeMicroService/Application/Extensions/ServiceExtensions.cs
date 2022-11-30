using Microsoft.Extensions.Options;
using OfficeMicroService.Application.Middlewares;
using OfficeMicroService.Application.Services;
using OfficeMicroService.Data.Mapper;
using OfficeMicroService.Data.Settings;
using Serilog;

namespace OfficeMicroService.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services)
        {
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
            services.AddSwaggerGen();
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static void ConfigureSerilog(IHostBuilder host)
        {
            host.UseSerilog((hostingContext, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
