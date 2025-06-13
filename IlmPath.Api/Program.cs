
using IlmPath.Api.Middleware;
using IlmPath.Application;
using IlmPath.Infrastructure;
using IlmPath.Infrastructure.UpdateDatabaseIntializerEx;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;

namespace IlmPath.Api;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // --- Service Configuration ---
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // AddSwaggerGen registers the Swagger generator, defining one or more Swagger documents.
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "iLmPath API",
                Version = "v1"
            });
        });

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        // --- Configure the HTTP request pipeline. ---

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            // This is where you configure the Swagger UI
            app.UseSwaggerUI(options =>
            {
                // 3lshan n test bl old swagger api
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "iLmPath API V1");

                options.RoutePrefix = string.Empty;
            });

            // 3lshan t update el data base 3ala a5er migration badl ma n3mlha manual   

            await app.UseDatabaseMigrations();

        }

        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
