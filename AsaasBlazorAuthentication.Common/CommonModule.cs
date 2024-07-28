using Microsoft.Extensions.DependencyInjection;

using AsaasBlazorAuthentication.Common.MessageBus;

namespace AsaasBlazorAuthentication.Common;

public static class CommonModule
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageBusProducerService, MessageBusProducerService>();

        return services;
    }
}