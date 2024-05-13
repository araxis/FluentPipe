namespace FluentPipe.Core.Steps;


internal class ParallelStep<TContext, TResult>(int maxDegreeOfParallelism, PipelineBuilder<TContext, TResult> pipelineBuilder)
    : Step<TContext>
    where TContext : PipelineContext<TResult>
{
    private readonly PipelineBuilder<TContext, TResult> _pipelineBuilder = pipelineBuilder ?? throw new ArgumentNullException(nameof(pipelineBuilder));
    protected override async ValueTask ExecuteInternalAsync(TContext context, CancellationToken cancellationToken)
    {
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism,
            CancellationToken = cancellationToken,
        };

        await Parallel.ForEachAsync(_pipelineBuilder.Steps, parallelOptions, async (step, token) => await step.ExecuteAsync(context, token));

    }
}
