using OfficeMicroService.Application.Extensions;

namespace OfficeMicroService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.ConfigureJwtAuthentication(configuration);
        builder.Services.ConfigureDbConnection(configuration);
        builder.Services.ConfigureServices();
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCors();
        builder.Services.ConfigureSwagger();
        builder.Services.ConfigureSerilog(builder.Host);
        var app = builder.Build();
        app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

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