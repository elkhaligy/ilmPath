using IlmPath.Api.Mappings;
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
        return services;
    }
}

