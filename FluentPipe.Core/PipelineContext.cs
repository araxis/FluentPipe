namespace FluentPipe.Core;

public abstract class PipelineContext<TResult>
{
    public abstract TResult GetPipelineResult();
}
