namespace FluentPipe.Core;

public interface IPipeline<in TContext,TResult>
{
    ValueTask<TResult> InvokeAsync(TContext context,CancellationToken cancellationToken = default);
}