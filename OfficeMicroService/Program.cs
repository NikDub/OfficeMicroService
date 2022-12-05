using OfficeMicroService.Application.Extensions;

namespace OfficeMicroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureJWTAuthentication(builder.Configuration);
            builder.Services.ConfigureDbConnection(builder.Configuration);
            builder.Services.ConfigureServices();
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            builder.Services.ConfigureSerilog(builder.Host);
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}