using Framework.Application.Requests;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IAuthorizationRequest authorizationRequest)
        {
            return await next();
        }

        var authorizationService = _serviceProvider.GetService<IAuthorizationService>();
        if (authorizationService is null)
        {
            return await next();
        }

        foreach (var policy in authorizationRequest.RequiredPolicies)
        {
            var result = await authorizationService.AuthorizeAsync(_serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new System.Security.Claims.ClaimsPrincipal(), policy);
            if (!result.Succeeded)
            {
                return CreateFailure<TResponse>("Authorization failed");
            }
        }

        foreach (var role in authorizationRequest.RequiredRoles)
        {
            var user = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.User;
            if (user is null || !user.IsInRole(role))
            {
                return CreateFailure<TResponse>("Authorization failed");
            }
        }

        return await next();
    }

    private static TResponse CreateFailure<TResponseResult>(string message)
    {
        if (typeof(TResponseResult) == typeof(Result))
        {
            return (TResponse)(object)Result.Fail(message);
        }

        if (typeof(TResponseResult).IsGenericType && typeof(TResponseResult).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericType = typeof(TResponseResult).GetGenericArguments()[0];
            var failure = typeof(Result).GetMethod(nameof(Result.Fail), new[] { typeof(string) })?.MakeGenericMethod(genericType);
            var response = failure?.Invoke(null, new object[] { message });
            return (TResponse)(object)response!;
        }

        throw new InvalidOperationException("Authorization behavior requires a Result-based response.");
    }
}
