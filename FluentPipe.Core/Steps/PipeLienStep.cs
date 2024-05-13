namespace FluentPipe.Core.Steps;


internal class PipeLienStep<TContext, TResult>(Predicate<TContext> predicate, PipelineBuilder<TContext, TResult> pipelineBuilder)
    : Step<TContext>
    where TContext : PipelineContext<TResult>
{
    private readonly Predicate<TContext> _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
    private readonly PipelineBuilder<TContext, TResult> _pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));

    protected override async ValueTask ExecuteInternalAsync(TContext context, CancellationToken cancellationToken)
    {
        if (_predicate(context))
        {
            await _pipelineBuilder.Build().InvokeAsync(context,cancellationToken);
        }
    }
}
