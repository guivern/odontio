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
        services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); // add all handlers
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); // add validation behavior
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); // add validators
        services.AddMapping(); // add mapster
        services.AddSignalR();

        return services;
    } 
}