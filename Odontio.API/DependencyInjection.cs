using Microsoft.AspNetCore.Mvc;
using Odontio.API.Common.Mapping;

namespace Odontio.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // disable automatic model validation because we use FluentValidation in the application layer
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Odontio.API", Version = "v1" });
        });

        services.AddCors();
        services.AddControllers();
        services.AddMapping();

        return services;
    }
}