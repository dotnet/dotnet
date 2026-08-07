# Benchmarking

Use the `NuGet.Benchmarks` project at `test\TestExtensions\NuGet.Benchmarks` for performance measurements.

1. Create a `.cs` file in that directory. The file is git-ignored.
2. Add a class that implements `IBenchmark`.
3. Annotate benchmark methods with `[Benchmark]`.
4. Do not modify `Program.cs`; it discovers `IBenchmark` implementations automatically.
5. Run:

   ```powershell
   dotnet run -c Release --project test\TestExtensions\NuGet.Benchmarks\NuGet.Benchmarks.csproj
   ```

See the [benchmark project README](../../test/TestExtensions/NuGet.Benchmarks/README.md) for an example.
