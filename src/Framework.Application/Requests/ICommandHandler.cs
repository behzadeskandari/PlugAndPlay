using FluentResults;
using MediatR;

namespace Framework.Application.Requests;

public interface ICommandHandler
{
}

public interface ICommandHandler<in TCommand, TResponse> : ICommandHandler, IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
    new Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> : ICommandHandler, IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
    new Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}
