# NuGet.Benchmarks

A micro-benchmark project based on [BenchmarkDotNet](https://benchmarkdotnet.org/) for quick performance experiments inside the NuGet.Client repository.

## Purpose

This project exists to make it easy to write and run micro-benchmarks against NuGet source code without having to set up a separate repository or solution. It is intended for **local use only** — benchmark class files are git-ignored by the `.gitignore` in this folder, so you can create them freely without worrying about accidentally committing them.

Use it for:

- Profiling allocations and throughput while developing a feature or fix.
- Comparing the performance of two implementation approaches before committing.
- Reproducing a performance regression reported in an issue.

## How to run

```bash
dotnet run -c Release --project test/TestExtensions/NuGet.Benchmarks/NuGet.Benchmarks.csproj
```

> **Note:** Always run benchmarks in `Release` configuration. Debug / unoptimised builds produce misleading results.

## Writing benchmarks

`Program.cs` auto-discovers all classes that implement `IBenchmark` in the assembly — you never need to edit it.

1. Create a new `*.cs` file in this directory (it will be automatically git-ignored).
2. Add a `public class` that implements `IBenchmark`.
3. Annotate the methods you want to measure with `[Benchmark]`.
4. Run the project in Release mode (see above).

### Example

```csharp
// MyBenchmarks.cs  (git-ignored, safe to create locally)
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class LambdaAllocationBenchmarks : IBenchmark
{
    [Benchmark]
    public void StaticLambda()
    {
        Create(static path => new Item(path));
    }

    [Benchmark]
    public void Lambda()
    {
        string pre = "my prefix";
        Create(path => new Item(path + pre));
    }

    private static void Create<T>(Func<string, T> factory) { }
}

public class Item
{
    public Item(string value) => Value = value;
    public string Value { get; }
}
```

For full documentation on available attributes (`[MemoryDiagnoser]`, `[Params]`, `[GlobalSetup]`, etc.) see the [BenchmarkDotNet docs](https://benchmarkdotnet.org/articles/overview.html).
