namespace FluentPipe.Core;

public interface IStep<in T>
{
    ValueTask ExecuteAsync(T context, CancellationToken cancellationToken = default);
}