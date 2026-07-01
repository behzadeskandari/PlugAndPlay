using System.Reflection;
using Framework.Application.Behaviors;
using Framework.Application.Mapping;
using Framework.Application.Requests;
using Framework.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.Application;

public static class ApplicationLayerRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        services.AddHttpContextAccessor();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddLogging();
        services.AddAuthorization();
        services.AddSingleton<ServiceFactory>(p => p.GetRequiredService);
        services.AddSingleton<IMediator, Mediator>();
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
        services.Scan(selector => selector
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.Where(type =>
                type is { IsClass: true, IsAbstract: false } &&
                (typeof(IRequestHandler<,>).IsAssignableFrom(type) ||
                 typeof(IRequestHandler<>).IsAssignableFrom(type) ||
                 typeof(ICommandHandler).IsAssignableFrom(type) ||
                 typeof(IQueryHandler).IsAssignableFrom(type) ||
                 typeof(IMapProfile).IsAssignableFrom(type) ||
                 typeof(IValidator).IsAssignableFrom(type) ||
                 typeof(IValidator<>).IsAssignableFrom(type))))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(selector => selector
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.Where(type =>
                type is { IsClass: true, IsAbstract: false } &&
                (type.GetInterfaces().Any(i => i.Name.EndsWith("Repository", StringComparison.Ordinal)) ||
                 typeof(IService).IsAssignableFrom(type) ||
                 typeof(ITransientDependency).IsAssignableFrom(type) ||
                 typeof(IScopedDependency).IsAssignableFrom(type))))
            .AsSelf()
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddScoped<IRequestContext, RequestContext>();

        return services;
    }

    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, params Type[] assemblyMarkerTypes)
    {
        var assemblies = assemblyMarkerTypes.Select(type => type.Assembly).Distinct().ToArray();
        return services.AddApplicationLayer(assemblies);
    }
}

public interface IService
{
}

public interface ITransientDependency
{
}

public interface IScopedDependency
{
}
