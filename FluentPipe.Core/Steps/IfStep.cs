namespace FluentPipe.Core.Steps;

internal class IfStep<TContext>(Predicate<TContext> predicate, IStep<TContext> step) : Step<TContext>
{
    private readonly IStep<TContext> _step = step ?? throw new ArgumentNullException(nameof(step));
    private readonly Predicate<TContext> _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

    protected override async ValueTask ExecuteInternalAsync(TContext context, CancellationToken cancellationToken)
    {
        if (_predicate(context))
        {
            await _step.ExecuteAsync(context, cancellationToken);
        }
    }

   
}