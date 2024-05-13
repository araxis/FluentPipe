using FluentPipe.Core.Steps;

namespace FluentPipe.Core;

public class PipelineBuilder<TContext, TResult>(IStepFactory stepFactory)
    where TContext : PipelineContext<TResult>
{
    private readonly IStepFactory _stepFactory = stepFactory ?? throw new ArgumentNullException(nameof(stepFactory));

    internal List<Step<TContext>> Steps { get; } = [];
    private Step<TContext> LastStep => Steps[^1];

    public PipelineBuilder<TContext, TResult> Add<T>()
        where T : IStep<TContext>
    {
        var step = new LazyStep<TContext>(_stepFactory.Create<T, TContext>);
        Steps.Add(step);
        return this;
    }

    public PipelineBuilder<TContext, TResult> AddIf<T>(Predicate<TContext> predicate)
        where T : IStep<TContext>
    {
        var step = new IfStep<TContext>(predicate, new LazyStep<TContext>(_stepFactory.Create<T, TContext>));
        Steps.Add(step);
        return this;
    }
    public PipelineBuilder<TContext, TResult> AddIfElse<TIf, TElse>(Predicate<TContext> predicate)
        where TIf : IStep<TContext>
        where TElse : IStep<TContext>
    {
        var step = new IfElseStep<TContext>(predicate, new LazyStep<TContext>(_stepFactory.Create<TIf, TContext>), new LazyStep<TContext>(_stepFactory.Create<TElse, TContext>));
        Steps.Add(step);
        return this;
    }
    public PipelineBuilder<TContext, TResult> AddIf(Predicate<TContext> predicate, Func<PipelineBuilder<TContext, TResult>, PipelineBuilder<TContext, TResult>> action)
    {
        var internalBuilder = action(new PipelineBuilder<TContext, TResult>(_stepFactory));

        Steps.Add(new PipeLienStep<TContext,TResult>(predicate, internalBuilder));

        return this;
    }

    public PipelineBuilder<TContext, TResult> CompensateWith<T>()
        where T : IStep<TContext>
    {
        LastStep.UndoStep = new LazyStep<TContext>(_stepFactory.Create<T, TContext>);
        return this;
    }

    public PipelineBuilder<TContext, TResult> Parallel(Func<PipelineBuilder<TContext, TResult>,
        PipelineBuilder<TContext, TResult>> action, int maxDegreeOfParallelism = -1)
    {
        var internalBuilder = action(new PipelineBuilder<TContext, TResult>(_stepFactory));
        var step =new ParallelStep<TContext, TResult>(maxDegreeOfParallelism, internalBuilder);
        Steps.Add(step);
        return this;
    }

    public IPipeline<TContext,TResult> Build()
    {
        var step =new EndStep<TContext>();
        Steps.Add(step);
        return new Pipeline<TContext, TResult>(Steps);
    }
}