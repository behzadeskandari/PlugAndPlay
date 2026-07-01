using FluentValidation;
using FluentResults;
using Framework.Application;
using Framework.Application.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Plugin.Tests;

public sealed class ApplicationLayerRegistrationTests
{
    [Fact]
    public async Task AddApplicationLayer_RegistersValidatorsAndHandlersFromAssembly()
    {
        var services = new ServiceCollection();
        services.AddApplicationLayer(typeof(ApplicationLayerRegistrationTests).Assembly);

        using var provider = services.BuildServiceProvider();

        var mediator = provider.GetRequiredService<IMediator>();
        var validator = provider.GetService<IValidator<TestCommand>>();

        Assert.NotNull(validator);

        var result = await mediator.Send(new TestCommand());

        Assert.True(result.IsSuccess);
        Assert.Equal("validated", result.Value);
    }

    public sealed class TestCommand : ICommand<string>
    {
    }

    public sealed class TestCommandValidator : AbstractValidator<TestCommand>
    {
        public TestCommandValidator()
        {
            RuleFor(x => x).NotNull();
        }
    }

    public sealed class TestCommandHandler : ICommandHandler<TestCommand, string>
    {
        public Task<Result<string>> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result.Ok("validated"));
        }
    }
}
