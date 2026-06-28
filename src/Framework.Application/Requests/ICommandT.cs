namespace Framework.Application.Requests;

public interface ICommand<TResponse> : MediatR.IRequest<FluentResults.Result<TResponse>>
{
}
