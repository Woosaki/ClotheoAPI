using ClotheoAPI.Application.Auth.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ClotheoAPI.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
    }
}
