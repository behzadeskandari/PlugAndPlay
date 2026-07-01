using FluentResults;
using MediatR;

namespace Framework.Application.Requests;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
