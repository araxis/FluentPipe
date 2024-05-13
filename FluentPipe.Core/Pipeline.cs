using FluentPipe.Core.Steps;

namespace FluentPipe.Core;

/// <inheritdoc/>
public class Pipeline<TContext, TResult> : IPipeline<TContext,TResult>
    where TContext : PipelineContext<TResult>
{

    private readonly IStep<TContext> _initStep;
    internal Pipeline(IReadOnlyList<Step<TContext>> steps)
    {

        _initStep = steps[0];

        SetupSteps(steps);
    }

    public async ValueTask<TResult> InvokeAsync(TContext context,CancellationToken cancellationToken)
    {
        await _initStep.ExecuteAsync(context, cancellationToken);
        return  context.GetPipelineResult() ;
    }

    private static void SetupSteps(IReadOnlyList<Step<TContext>> steps)
    {
        for (var i = 0; i < steps.Count - 1; i++)
        {
            steps[i].NextStep = steps[i + 1];
        }
    }
}
