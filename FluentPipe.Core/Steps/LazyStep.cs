namespace FluentPipe.Core.Steps;

internal class LazyStep<TContext> : Step<TContext>
{
    private readonly Lazy<IStep<TContext>> _step;
    public LazyStep(Func<IStep<TContext>> factory)
    {
         if(factory is null) throw new ArgumentNullException(nameof(factory));
        _step = new Lazy<IStep<TContext>>(() =>
        {
            var instance = factory();
            return instance;
        });
    }
    protected override async ValueTask ExecuteInternalAsync(TContext context, CancellationToken cancellationToken)
    {
        await _step.Value.ExecuteAsync(context, cancellationToken);

    }
}