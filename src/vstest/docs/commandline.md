# vstest.console.exe command line options

This document describes the command line options intended for direct use — the ones shown by
`vstest.console.exe --Help`. It is derived from the argument processors in
[`src/vstest.console/Processors`](../src/vstest.console/Processors) and largely follows the
built-in `--Help` output. A few advanced or internal switches are left out of the main
reference and collected in [Omitted switches](#omitted-switches) instead.

Options are case-insensitive and accept either a `/Option` or a `--Option` prefix (for
example `/Parallel` and `--Parallel` are equivalent). Some options also have a short
form, which accepts a `/` or `-` prefix (for example `/lt` or `-lt` for `/ListTests`, and
`/e` or `-e` for `/Environment`). Options that take a value use a colon, for
example `/Settings:test.runsettings`.

> **Using `dotnet test`?** `dotnet test` exposes most of these capabilities through its
> own switches (for example `--filter`, `--logger`, `--collect`, `--diag`,
> `--results-directory`, `--framework`). See
> [dotnet test options](https://learn.microsoft.com/dotnet/core/tools/dotnet-test) and
> [vstest.console.exe options](https://learn.microsoft.com/visualstudio/test/vstest-console-options)
> for the complete option reference. To pass runsettings values inline from
> `dotnet test`, use the `-- [name]=[value]` syntax described in
> [RunSettingsArguments.md](./RunSettingsArguments.md).

## Usage

```shell
vstest.console.exe [TestFileNames] [Options]
```

`TestFileNames` are one or more test containers (assemblies or other sources) separated by
spaces. Wildcards are supported, for example `**/*.Tests.dll`. Use forward slashes (`/`) in
wildcard patterns so they work on Windows, Linux and macOS.

## Test selection and filtering

### `/Tests:<Test Names>`

Run tests with names that match the provided values. To provide multiple values, separate
them by commas.

```shell
vstest.console.exe MyTests.dll /Tests:TestMethod1
vstest.console.exe MyTests.dll /Tests:TestMethod1,TestMethod2
```

### `/TestCaseFilter:<Expression>`

Run tests that match the given expression. `<Expression>` is of the format
`<property>Operator<value>[|&<Expression>]` where `Operator` is one of `=`, `!=` or `~`
(`~` has *contains* semantics and applies to string properties such as `DisplayName`).
Parentheses `()` group sub-expressions.

```shell
vstest.console.exe MyTests.dll /TestCaseFilter:"Priority=1"
vstest.console.exe MyTests.dll /TestCaseFilter:"(FullyQualifiedName~Nightly|Name=MyTestMethod)"
```

See [filter.md](./filter.md) for the full filtering reference, supported properties per test
framework, and escaping rules.

### `/ListTests:<File Name>` (short form `/lt`)

Lists all discovered tests from the given test container instead of running them.

## Discovery and execution behavior

### `/Parallel`

Run the tests in parallel. By default up to all available cores on the machine may be
used. The number of cores to use may be configured with the `MaxCpuCount` element in a
settings file.

### `/InIsolation`

Runs the tests in an isolated process. This makes `vstest.console.exe` less likely to be
stopped by an error in the tests, but tests may run slower.

In practice most runs already happen in isolation — the main exception is .NET Framework
test assemblies, which can run inside the runner process when they don't run in parallel,
don't use a data collector, and don't disable app domains.

### `/Platform:<Platform type>`

Target platform architecture to be used for test execution. Values are parsed
case-insensitively; valid values are `x86`, `x64`, `ARM`, `ARM64`, `S390x`, `Ppc64le`,
`RiscV64` and `LoongArch64`.

### `/Framework:<Framework Version>`

Target .NET framework version to be used for test execution. Values are parsed with NuGet's
framework parser, so the common short forms work, for example `net48`, `net6.0` or `net10.0`
(as well as the long forms `".NETFramework,Version=v4.8"` and `".NETCoreApp,Version=v10.0"`).
The legacy aliases `Framework40`, `Framework45`, `FrameworkCore10` and `FrameworkUap10` are
also accepted.

### `/Environment:<NAME>=<VALUE>` (short form `/e`)

Sets the value of an environment variable for the test host. Creates the variable if it
does not exist, overrides it if it does. This implies `/InIsolation` and forces the tests
to run in an isolated process. Specify the option multiple times to set multiple
variables.

```shell
vstest.console.exe MyTests.dll -e:VARIABLE1=VALUE1
vstest.console.exe MyTests.dll -e:ANOTHER_VARIABLE="VALUE WITH SPACES"
```

## Adapters

### `/TestAdapterPath:<path>`

Makes `vstest.console.exe` use custom test adapters from the given path in the test run. The
value is a path to a *folder* that contains the adapter assemblies, not a path to a single
adapter `.dll`.

## Settings

### `/Settings:<Settings File>`

Settings to use when running tests. See
[configure.md](./configure.md) and the
[.runsettings reference](https://learn.microsoft.com/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file).

### RunSettings arguments (`-- [name]=[value]`)

Pass runsettings configuration inline through the command line. Arguments are specified as
`[name]=[value]` pairs after `-- ` (note the space after `--`). Use a space to separate
multiple pairs; all arguments after `--` are treated as runsettings arguments, so they must
appear at the end of the command line.

```shell
vstest.console.exe MyTests.dll -- MSTest.MapInconclusiveToFailed=True
```

See [RunSettingsArguments.md](./RunSettingsArguments.md) for the full syntax, precedence
rules, and shell-escaping guidance.

## Loggers, data collection, and results

### `/logger:<Logger Uri/FriendlyName>`

Specify a logger for test results. For example, to log results into a Visual Studio Test
Results File (TRX) use `/logger:trx`. By default the TRX file is named after the test run;
use `/logger:trx;LogFilePrefix=<prefix>` rather than `LogFileName=<name>` when you want a
timestamped file per run — `LogFileName` sets the name explicitly and overwrites the previous
file, whereas `LogFilePrefix` keeps each run's file. The console logger verbosity can be set
with `/logger:console;verbosity=<quiet|minimal|normal|detailed>`. See
[report.md](./report.md) for all loggers and their options.

### `/Collect:<DataCollector FriendlyName>`

Enables a data collector for the test run (for example `/Collect:"Code Coverage"` or
`/Collect:"XPlat Code Coverage"`). More info: [vstest collect](https://aka.ms/vstest-collect).
See [analyze.md](./analyze.md) for code coverage.

### `/Blame:[CollectDump];[CollectAlways]=[Value];[DumpType]=[Value]`

Runs the test in blame mode to isolate a problematic test that crashes the test host. It
creates a `Sequence.xml` file capturing the order of execution before the crash, and can
optionally collect a process dump.

- `CollectAlways` — collect a dump on exit even when there is no crash (`true`/`false`).
- `DumpType` — dump type (`mini`/`full`).

Collecting a crash dump on Windows requires `procdump.exe`/`procdump64.exe` on `PATH` or in
the directory pointed to by the `PROCDUMP_PATH` environment variable
([download procdump](https://learn.microsoft.com/sysinternals/downloads/procdump)).

```shell
vstest.console.exe MyTests.dll /Blame
vstest.console.exe MyTests.dll /Blame:CollectDump;CollectAlways=true;DumpType=full
```

See [extensions/blame-datacollector.md](./extensions/blame-datacollector.md) for the full
blame collector reference.

### `/ResultsDirectory:<path>`

The test results directory is created at the specified path if it does not exist.

## Diagnostics

### `/Diag:<Path to log file>`

Enable diagnostic logs for the test platform, written to the provided file. Set the trace
level with `/Diag:<path>;tracelevel=<off|error|warning|info|verbose>` (default `verbose`).

```shell
vstest.console.exe MyTests.dll /Diag:log.txt
vstest.console.exe MyTests.dll /Diag:log.txt;tracelevel=info
```

See [diagnose.md](./diagnose.md) and [troubleshooting.md](./troubleshooting.md) for more.

## General

### `/Help` (short form `/?` or `-?`)

Display the usage message.

### `@<file>`

Read additional options from a response file. The file contents are read and parsed like a
normal command line: arguments are separated by whitespace (spaces or newlines) and quoting
is supported, so options may be placed on one line or spread across multiple lines.

```shell
vstest.console.exe @options.rsp
```

## Editor/IDE integration options

These options are used by IDEs and hosting tools (such as Visual Studio and
`dotnet test`) that host `vstest.console.exe`. They are not typically used directly from a
shell.

### `/Port:<Port>`

The port for the socket connection used to receive event messages from the host.

### `/ParentProcessId:<ParentProcessId>`

Records the process id of the parent process that launched the runner. On its own this only
stores the value; the runner watches the parent and exits when it exits as part of design-mode
(IDE) integration, which is activated together with `/Port`.

## Exit codes

`vstest.console.exe` returns only two exit codes:

| Code | Meaning |
| --- | --- |
| `0` | Success — the requested operation completed and, for a test run, all executed tests passed. |
| `1` | Failure — for example one or more tests failed, a run error was reported, the command line was invalid or missing, a test source could not be loaded, or the run was aborted or canceled. |

The process never returns any other value (`Program.Main` returns `0` on success and `1`
otherwise). When invoked through `dotnet test`, the SDK surfaces a non-zero exit code when
the run fails in the same way.

### When no tests are found

When discovery finds no matching tests the runner prints a **warning** (not an error), so the
"no tests" condition by itself does not produce an `Error:` line. The message depends on the
situation:

- A filter matched nothing: ``No test matches the given testcase filter `<filter>` in <sources>`` (the filter value is shown in backticks).
- Nothing was discovered from a source: `No test is available in <sources>. Make sure that installed test discoverers & executors, platform & framework version settings are appropriate and try again.` If `/TestAdapterPath` was not provided, a suggestion to specify it is appended.
- `/Tests:<names>` matched nothing although tests were discovered: `A total of <N> tests were discovered but no test matches the specified selection criteria(<names>). Use right value(s) and try again.`

By default this is **not** treated as a failure: the run prints the warning and still returns
`0`. Set `RunConfiguration.TreatNoTestsAsError` to `true` in a `.runsettings` file to make a run
that discovers/selects zero tests return `1` instead.

## Omitted switches

`vstest.console.exe` accepts a number of additional internal, legacy, or hidden switches that
are not intended for direct use from a shell (they are driven by IDEs, the .NET SDK, or other
tooling). Some are shown by `--Help` (notably `/TestAdapterLoadingStrategy`) but are left out
of the reference above because they are advanced options; the rest are not shown by `--Help` at
all. For completeness, the full set omitted from the reference above is:

| Switch | Purpose |
| --- | --- |
| `/EnableCodeCoverage` | Enables the code coverage data collector. Prefer `/Collect:"Code Coverage"`. |
| `/UseVsixExtensions` | Enables loading of legacy VSIX-installed test adapters. Deprecated. |
| `/TestAdapterLoadingStrategy` | Controls the order/strategy used to load custom test adapters. |
| `/DisableAutoFakes` | Disables automatic Microsoft Fakes support. |
| `/AeDebugger` | Configures the just-in-time (AeDebugger) crash-debugging hook for test hosts. |
| `/ListDiscoverers` | Lists the installed test discoverers. |
| `/ListExecutors` | Lists the installed test executors. |
| `/ListLoggers` | Lists the installed loggers. |
| `/ListSettingsProviders` | Lists the installed settings providers. |
| `/ListFullyQualifiedTests` | Discovers tests and writes their fully qualified names to a file (used with `/ListTestsTargetPath`). |
| `/ListTestsTargetPath` | Path of the file that `/ListFullyQualifiedTests` writes discovered test names to. |
| `/TestSessionCorrelationId` | Correlation id used to associate a run with its artifacts across processes. |
| `/ArtifactsProcessingMode-Collect` | Marks a run so its artifacts are collected for later post-processing. |
| `/ArtifactsProcessingMode-PostProcess` | Post-processes artifacts collected from earlier runs. |
| `/ShowDeprecateDotnetVSTestMessage` | Controls whether the `dotnet vstest` deprecation message is shown. |
| `/RunTests` | Internal command used by design-mode hosts to run the provided tests. |
| `/TestSource` | Internal command used by design-mode hosts to pass a test source. |



## See also

- [filter.md](./filter.md) — test case filtering reference
- [RunSettingsArguments.md](./RunSettingsArguments.md) — passing runsettings from the command line
- [configure.md](./configure.md) — `.runsettings` configuration
- [analyze.md](./analyze.md) — code coverage
- [diagnose.md](./diagnose.md) / [troubleshooting.md](./troubleshooting.md) — diagnostics
- [environment-variables.md](./environment-variables.md) — environment variables
- [vstest.console.exe options (Microsoft Learn)](https://learn.microsoft.com/visualstudio/test/vstest-console-options)
- [dotnet test options (Microsoft Learn)](https://learn.microsoft.com/dotnet/core/tools/dotnet-test)
