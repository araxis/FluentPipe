namespace FluentPipe.Core.Steps;

internal class IfElseStep<TContext>(Predicate<TContext> predicate, Step<TContext> ifStep, Step<TContext> elseStep)
    : Step<TContext>
{
    private readonly IStep<TContext> _ifStep = ifStep ?? throw new ArgumentNullException(nameof(ifStep));
    private readonly IStep<TContext> _elseStep = elseStep ?? throw new ArgumentNullException(nameof(elseStep));
    private readonly Predicate<TContext> _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

    protected override async ValueTask ExecuteInternalAsync(TContext context, CancellationToken cancellationToken)
    {
        if (_predicate(context))
        {
            await _ifStep.ExecuteAsync(context, cancellationToken);
        }
        else
        {
            await _elseStep.ExecuteAsync(context, cancellationToken);
        }
    }
}
