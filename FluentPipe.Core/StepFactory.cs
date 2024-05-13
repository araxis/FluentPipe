using Microsoft.Extensions.DependencyInjection;

namespace FluentPipe.Core;

public class StepFactory(IServiceProvider serviceProvider) : IStepFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public IStep<TContext> Create<TStep, TContext>() where TStep : IStep<TContext>
    {
       return _serviceProvider.GetRequiredService<TStep>();
    }
}