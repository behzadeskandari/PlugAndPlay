namespace Framework.Application.Requests;

public interface IQuery<TResponse> : MediatR.IRequest<FluentResults.Result<TResponse>>
{
}
