using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace FluentPipe.Core.Steps;

internal abstract class Step<TContext> : IStep<TContext>
{
    readonly ILogger _logger;

    protected Step(ILoggerFactory? loggerFactory = null)
    {
        var loggerFactory1 = loggerFactory ?? NullLoggerFactory.Instance;
        _logger = loggerFactory1.CreateLogger<Step<TContext>>();
    }

    public Step<TContext>? NextStep { get; set; }
    public Step<TContext>? UndoStep { get; set; }
    private bool? ErrorHandledSucceed { get; set; }
    public async ValueTask ExecuteAsync(TContext context, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecuteInternalAsync(context, cancellationToken);
            if (NextStep is not null)
            {
                await NextStep.ExecuteAsync(context, cancellationToken);
            }
        }
        catch (Exception e) when (e is PipelineExecutionException or OperationCanceledException)
        {
            ErrorHandledSucceed = false;
            throw; // Re-throws the current exception without losing stack trace
        }
        catch (Exception e)
        {
            ErrorHandledSucceed = await HandleExceptionAsync(context,cancellationToken);

            if (!ErrorHandledSucceed.HasValue || !ErrorHandledSucceed.Value)
            {

                throw new PipelineExecutionException(e);
            }
        }
        finally
        {
            // Cleanup resources if needed
        }

    }

    private async Task<bool> HandleExceptionAsync(TContext context, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected virtual ValueTask ExecuteInternalAsync(TContext context, CancellationToken cancellationToken) =>
        ValueTask.CompletedTask;
}