using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Odontio.Application.Common.Behaviors;
using Odontio.Application.Common.Mapping;

namespace Odontio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            options.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.AddOpenBehavior(typeof(WorkspaceValidationBehavior<,>));
            options.AddOpenBehavior(typeof(PatientValidationBehavior<,>));
        });
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddMapping(); // add mapster
        services.AddSignalR();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        return services;
    }
}