using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AsaasBlazorAuthentication.Common.Auth;
using AsaasBlazorAuthentication.Common.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Common.Persistence.DbConnectionFactories;

using AsaasBlazorAuthentication.Domain.Users;
using AsaasBlazorAuthentication.Domain.Enrollments;
using AsaasBlazorAuthentication.Domain.Subscribers;
using AsaasBlazorAuthentication.Domain.Subscriptions;
using AsaasBlazorAuthentication.Domain.EnrollmentPayments;

using AsaasBlazorAuthentication.Application.Abstractions.PaymentGateway;
using AsaasBlazorAuthentication.Infrastructure.Auth;
using AsaasBlazorAuthentication.Infrastructure.Interceptors;
using AsaasBlazorAuthentication.Infrastructure.BackgroundJobs;
using AsaasBlazorAuthentication.Infrastructure.Persistence.Contexts;
using AsaasBlazorAuthentication.Infrastructure.Persistence.UnitOfWork;
using AsaasBlazorAuthentication.Infrastructure.Integrations.Asaas.Apis;
using AsaasBlazorAuthentication.Infrastructure.Persistence.Repositories;

namespace AsaasBlazorAuthentication.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContexts(configuration)
                .AddInterceptors()
                .AddRepositories()
                .AddUnitOfWork()
                .AddAuthentication()
                .AddBackgroundJobs()
                .AddIntegrations();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString();

        services.AddDbContext<AsaasBlazorAuthenticationDbContext>((sp, opts) =>
        {
            opts.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .AddInterceptors(
                    sp.GetRequiredService<PublishDomainEventsToOutBoxMessagesInterceptor>());
        });

        return services;
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<PublishDomainEventsToOutBoxMessagesInterceptor>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ISubscriberRepository, SubscriberRepository>();
        services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
        services.AddTransient<IEnrollmentRepository, EnrollmentRepository>();
        services.AddTransient<IEnrollmentPaymentRepository, EnrollmentPaymentRepository>();

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddHostedService<ProcessOutboxMessagesJob>();

        return services;
    }

    private static IServiceCollection AddIntegrations(this IServiceCollection services)
    {
        services.AddTransient<IPaymentGateway, AsaasPaymentGatewayApi>();

        return services;
    }
}