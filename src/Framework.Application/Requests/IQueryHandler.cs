using FluentResults;
using MediatR;

namespace Framework.Application.Requests;

public interface IQueryHandler
{
}

public interface IQueryHandler<in TQuery, TResponse> : IQueryHandler, IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
    new Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
