using FluentResults;
using MediatR;

namespace Framework.Application.Requests;

public interface ICommand : IRequest<Result>
{
}
