# Instructions

To run the benchmarks in this project, follow these steps:

- Temporarily set the `CacheCapacity` field in `SqlProcessor` to 0 to disable caching.
- Close all redundant applications to free up system resources.
- Open a terminal and navigate to the
`test/OpenTelemetry.Contrib.Shared.Benchmarks` directory.
- Run the benchmarks using the command:

  ```txt
  dotnet run -c Release -f net10.0 -- --filter *SqlProcessorBenchmark*
  ```

- Return the `CacheCapacity` field in `SqlProcessor` to its original value.
