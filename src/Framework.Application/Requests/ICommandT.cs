using FluentResults;
using MediatR;

namespace Framework.Application.Requests;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
