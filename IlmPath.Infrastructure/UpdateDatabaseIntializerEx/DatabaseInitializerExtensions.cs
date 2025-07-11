﻿using IlmPath.Infrastructure.Data;
using IlmPath.Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Infrastructure.UpdateDatabaseIntializerEx
{
    public static class DatabaseInitializerExtensions
    {
        // This is an extension method for IApplicationBuilder, which is available in Program.cs
        public static async Task UseDatabaseMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    var IdentitySeeder = services.GetRequiredService<IdentitySeeder>();
                    var dataSeeder = services.GetRequiredService<DataSeeder>();

                    await dbContext.Database.MigrateAsync();
                    await IdentitySeeder.SeedAsync();
                    await dataSeeder.SeedAsync();

                }
                catch (Exception ex)
                {
                    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("DatabaseInitializer");
                    logger.LogError(ex, "An error occurred while migrating the database.");
                   
                    throw;
                }
            }
        }
    }
}
