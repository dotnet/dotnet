using System.Reflection;
using BenchmarkDotNet.Running;

// Auto-discovers and runs all classes implementing IBenchmark in this assembly.
// To write benchmarks: add a new *.cs file (git-ignored by .gitignore in this directory)
// with a class that implements IBenchmark and annotates benchmark methods with [Benchmark].
// No changes to this file are needed. See README.md for an example.

Assembly assembly = Assembly.GetExecutingAssembly();
Type[] benchmarkTypes = [.. assembly.GetTypes()
    .Where(t => typeof(IBenchmark).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })];

if (benchmarkTypes.Length > 0)
{
    BenchmarkRunner.Run(benchmarkTypes, config: null, args: args);
}
else
{
    Console.WriteLine("No benchmark classes found.");
    Console.WriteLine("Create a *.cs file in this directory with a class that implements IBenchmark.");
    Console.WriteLine("See README.md for an example.");
}

/// <summary>
/// Marker interface for benchmark classes that are automatically discovered and run.
/// Implement this interface in a class and annotate methods with [Benchmark].
/// </summary>
public interface IBenchmark { }
