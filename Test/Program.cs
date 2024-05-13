// See https://aka.ms/new-console-template for more information

using FluentPipe.Core;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");
var services = new ServiceCollection();
services.AddTransient<Step1>();
services.AddTransient<Step2>();
services.AddTransient<Step3>();
services.AddTransient<Step4>();
services.AddTransient<Step5>();
services.AddTransient<Step6>();
services.AddTransient<Step7>();
services.AddTransient<Step8>();
var sp = services.BuildServiceProvider();
var factory = new StepFactory(sp);
var pipelineBuilder = new PipelineBuilder<Context, string>(factory);
var pipeline = pipelineBuilder
    .Add<Step1>()
    .Add<Step2>()
    .AddIfElse<Step3,Step4>(c => c.Branch)
    .Fork(c=>c.Fork,b=>b.Add<Step7>())
    .Add<Step8>()
    .Build();
var result = await pipeline.InvokeAsync(new Context("test"));
Console.WriteLine(result);

public class Context(string name, bool branch = false) : PipelineContext<string>
{
    public bool Branch { get; set; } = branch;
    public bool Fork { get; set; }
    public override string GetPipelineResult()
    {
        return name;
    }
}

public class Step1 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step1");
        context.Branch = true;
        return ValueTask.CompletedTask;
    }
}

public class Step2 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step2");
        return ValueTask.CompletedTask;
    }
}
public class Step3 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step3");
        context.Fork = true;
        return ValueTask.CompletedTask;
    }
}
public class Step4 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step4");
        return ValueTask.CompletedTask;
    }
}

public class Step5 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step5");
        return ValueTask.CompletedTask;
    }
}
public class Step6 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step6");
        return ValueTask.CompletedTask;
    }
}

public class Step7 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step7");
        return ValueTask.CompletedTask;
    }
}
public class Step8 : IStep<Context>
{
    public ValueTask ExecuteAsync(Context context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Step8");
        return ValueTask.CompletedTask;
    }
}
