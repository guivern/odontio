using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Odontio.Application.Common.Interfaces;
using Odontio.Infrastructure.Models;
using Odontio.Infrastructure.Persistence;
using Odontio.Infrastructure.Services;

namespace Odontio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContextInitializer(configuration);
        services.AddAuth(configuration);
        services.AddServices();

        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
        //     options.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
        // });

        return services;
    }
    
    public static IServiceCollection AddDbContextInitializer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<DbContextInitializer>();

        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // AddTransient method is used to register a service with transient scope. Transient scope means that a new
        // instance of the service is created every time it is requested.
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
    
     public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        // var facebookAuthSettings = new FacebookAuthSettings();
        // configuration.Bind(nameof(FacebookAuthSettings), facebookAuthSettings);
        // services.AddSingleton(Options.Create(facebookAuthSettings));
        // services.AddScoped<IFacebookAuthService, FacebookAuthService>();

        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));
        services.AddScoped<IAuthService, AuthService>();
        
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/api/notifications-hub"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
}