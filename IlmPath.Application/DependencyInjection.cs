using IlmPath.Api.Mappings;
using IlmPath.Application.Email;
using Microsoft.Extensions.DependencyInjection;

namespace IlmPath.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options => 
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
        });
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddTransient<IEmailService, EmailService>();
        return services;
    }
}

