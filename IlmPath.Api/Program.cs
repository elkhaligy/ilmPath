
using IlmPath.Api.Middleware;
using IlmPath.Application;
using IlmPath.Infrastructure;
using IlmPath.Infrastructure.UpdateDatabaseIntializerEx;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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
        // Add cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        

        // AddSwaggerGen registers the Swagger generator, defining one or more Swagger documents.
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "iLmPath API",
                Version = "v1"
            });


            // ba3ml authorization 3lshan ba test el redisCart 
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid JWT",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
        });

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();

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

        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowAllOrigins");
        app.MapControllers();

        app.Run();
    }
}
