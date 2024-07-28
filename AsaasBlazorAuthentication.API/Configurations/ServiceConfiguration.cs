using System.Text;
using System.Text.Json.Serialization;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Serilog;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

using AsaasBlazorAuthentication.Common;
using AsaasBlazorAuthentication.Common.Swagger;
using AsaasBlazorAuthentication.Common.Options;
using AsaasBlazorAuthentication.Application;
using AsaasBlazorAuthentication.Infrastructure;
using AsaasBlazorAuthentication.API.Middlewares;

namespace AsaasBlazorAuthentication.API.Configurations;

public static class ServiceConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.ConfigureOptions(configuration);

        // Add modules
        builder.Services.AddCommon();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(configuration);

        builder.Services.ConfigureSerilog(configuration);

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(s =>
        {
            s.UseCommonSwaggerDoc("AsaasBlazorAuthentication.API", "v1");

            s.UseCommonAuthorizationBearer();

            s.AddEnumsWithValuesFixFilters();
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Authentication:Issuer"],
                    ValidAudience = configuration["Authentication:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Key"]!))
                };
            });

        return builder;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.UseStaticFiles();

        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    public static void ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateLogger();

        // Add Serilog as the log provider.
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AsaasApiOptions>(options => configuration.GetSection(OptionsConstants.AsaasApiSection).Bind(options));
        services.Configure<WebMailApiOptions>(options => configuration.GetSection(OptionsConstants.WebMailApiSection).Bind(options));
        services.Configure<AuthenticationOptions>(options => configuration.GetSection(OptionsConstants.AuthenticationSection).Bind(options));
        services.Configure<RabbitMqConfigurationOptions>(options => configuration.GetSection(OptionsConstants.RabbitMQConfigurationSection).Bind(options));
    }
}