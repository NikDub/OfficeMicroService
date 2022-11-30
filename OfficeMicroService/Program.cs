using Microsoft.Extensions.Options;
using OfficeMicroService.Mapper;
using OfficeMicroService.Services;
using OfficeMicroService.Settings;

namespace OfficeMicroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<OfficeStoreDatabaseSettings>(builder.Configuration.GetSection(nameof(OfficeStoreDatabaseSettings)));

            builder.Services.AddSingleton<IOfficeStoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<OfficeStoreDatabaseSettings>>().Value);
            builder.Services.AddSingleton<IOfficeServices, OfficeServices>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();
            app.MapControllers();

            app.Run();
        }
    }
}