namespace FluentPipe.Core;

public interface IStepFactory
{
    IStep<TContext> Create<TStep, TContext>() where TStep : IStep<TContext>;
}