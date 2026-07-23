# Quickstart Guide

This guide gets you running tests from the command line with the Visual Studio Test Platform
(`vstest.console.exe`). For the exhaustive option list see
[command line options](./commandline.md).

## Which command should I use?

Most people don't run `vstest.console.exe` directly. Pick the entry point that fits your setup:

| You have… | Use | Notes |
| --- | --- | --- |
| The .NET SDK | `dotnet test` | Recommended. Builds the project and wraps `vstest.console` for you. |
| A built test assembly / CI runner | `vstest.console.exe <TestFile>` | The standalone runner. Does not build; you point it at already-built assemblies. |
| Old habit of `dotnet vstest` | Prefer `dotnet test` | `dotnet vstest` is deprecated in favor of `dotnet test`. |

## Getting the standalone runner

`vstest.console` is not a separate download; it ships inside NuGet packages (and with Visual
Studio). Which package depends on the runtime you need:

- **.NET Framework runner (Windows)** — the
  [`Microsoft.TestPlatform`](https://www.nuget.org/packages/Microsoft.TestPlatform) package.
  After restoring/extracting it the runner lives at
  `tools/net462/Common7/IDE/Extensions/TestPlatform/vstest.console.exe` and requires .NET
  Framework.
- **Cross-platform .NET runner** — the
  [`Microsoft.TestPlatform.Portable`](https://www.nuget.org/packages/Microsoft.TestPlatform.Portable)
  package. It ships `vstest.console.dll` under `tools/net8.0/`, launched with
  `dotnet path/to/vstest.console.dll`, and requires a compatible .NET runtime.

When you use `dotnet test`, the runner and a matching runtime are already provided by the .NET
SDK (pinned by your `global.json`), so there is nothing extra to install.

## Your first run

1. Build a test project (for example an MSTest, xUnit, or NUnit project) so you have a test
   assembly such as `bin\Debug\net8.0\MyProject.Tests.dll`.
2. Run the tests:

   ```shell
   vstest.console.exe bin\Debug\net8.0\MyProject.Tests.dll
   ```

   Or, to build and run in one step with the SDK:

   ```shell
   dotnet test
   ```

You can pass more than one assembly and use wildcards:

```shell
vstest.console.exe **/bin/Debug/**/*.Tests.dll
```

The runner prints a per-test summary and a final total. Test results and any collected
artifacts are written under a `TestResults` directory.

## Common tasks

| Task | Command |
| --- | --- |
| Run a subset of tests | `vstest.console.exe Tests.dll /TestCaseFilter:"Priority=1"` |
| Produce a TRX report | `vstest.console.exe Tests.dll /logger:trx` |
| Use a settings file | `vstest.console.exe Tests.dll /Settings:test.runsettings` |
| Collect code coverage | `vstest.console.exe Tests.dll /Collect:"Code Coverage"` |
| Run in parallel | `vstest.console.exe Tests.dll /Parallel` |
| Capture diagnostic logs | `vstest.console.exe Tests.dll /Diag:log.txt` |

The equivalent `dotnet test` switches are `--filter`, `--logger`, `--settings`, `--collect`,
and `--diag`.

## Exit codes

`vstest.console.exe` returns `0` when the run succeeds and all executed tests pass, and `1`
otherwise (test failure, run error, invalid command line, etc.). See
[Exit codes](./commandline.md#exit-codes) for details, including what happens when no tests
match.

## Learn more

New to running .NET tests? Start with the official .NET testing documentation:

- [Testing in .NET](https://learn.microsoft.com/dotnet/core/testing/) — overview and getting started
- [Unit testing with `dotnet test`](https://learn.microsoft.com/dotnet/core/testing/unit-testing-with-dotnet-test)
- [`dotnet test` command reference](https://learn.microsoft.com/dotnet/core/tools/dotnet-test)
- [`vstest.console.exe` command-line options](https://learn.microsoft.com/visualstudio/test/vstest-console-options)

## Next steps

- [Command line options](./commandline.md) — full option reference
- [TestCase filtering](./filter.md)
- [Configure a test run (.runsettings)](./configure.md)
- [Passing runsettings from the command line](./RunSettingsArguments.md)
- [Code coverage](./analyze.md)
- [Diagnostics](./diagnose.md) / [Troubleshooting](./troubleshooting.md)
- [Environment variables](./environment-variables.md)

For test platform internals and extensibility, see the [Overview](./Overview.md) and the
[RFCs](./RFCs).
