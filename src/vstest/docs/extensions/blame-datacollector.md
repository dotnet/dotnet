# Motivation
Certain execution sequences can crash the testhost process spawned by the vstest runner. However there is no easy way to diagnose such an aborted test run since there is no way to know what specific test case was running at the time. The "blame" mode in vstest tracks the tests as they are executing and, in the case of the testhost process crashing, emits the tests names in their sequence of execution up to and including the specific test that was running at the time of the crash. This makes it easier to isolate the offending test and diagnose further.

# Syntax

Blame can be enabled from either `vstest.console.exe`, `dotnet test`, or runsettings. The simplest form records the test execution sequence:

```shell
vstest.console.exe MyTests.dll /Blame
dotnet test --blame
```

Use the canonical `vstest.console.exe` switch form to collect crash or hang dumps:

```text
/Blame:[CollectDump];[CollectAlways]=[value];[DumpType]=[value]
/Blame:[CollectHangDump];TestTimeout=[value];[HangDumpType]=[value]
```

For example:

```shell
vstest.console.exe MyTests.dll /Blame:CollectDump;CollectAlways=true;DumpType=full
vstest.console.exe MyTests.dll /Blame:CollectHangDump;TestTimeout=90m;HangDumpType=mini
```

The switch name is `/Blame`; option names are separated with semicolons. Crash dump parameters apply to `CollectDump`; hang dump parameters apply to `CollectHangDump`.

# Options

## Crash dump options

| Option | Values | Default | dotnet test switch | runsettings element |
| --- | --- | --- | --- | --- |
| `CollectDump` | Present or absent | Off | `--blame-crash` | `<CollectDump />` |
| `CollectAlways` | `true`, `false` | `false` | `--blame-crash-collect-always` | `<CollectDump CollectAlways="true" />` |
| `DumpType` | `mini`, `full` | `full` when `/Blame:CollectDump` is used | `--blame-crash-dump-type full\|mini` | `<CollectDump DumpType="full" />` |

Examples:

```shell
vstest.console.exe MyTests.dll /Blame:CollectDump;CollectAlways=true;DumpType=mini
dotnet test --blame-crash --blame-crash-collect-always --blame-crash-dump-type mini
```

## Hang dump options

| Option | Values | Default | dotnet test switch | runsettings element |
| --- | --- | --- | --- | --- |
| `CollectHangDump` | Present or absent | Off | `--blame-hang` | Use `<CollectDumpOnTestSessionHang ... />` |
| `TestTimeout` | Time span such as `1.5h`, `90m`, `5400s`, `5400000ms`; unitless values are milliseconds | `1h` when `/Blame:CollectHangDump` is used | `--blame-hang-timeout <timeout>` | `<CollectDumpOnTestSessionHang TestTimeout="90m" />` |
| `HangDumpType` | `mini`, `full`, `none` | `full` when `/Blame:CollectHangDump` is used | `--blame-hang-dump-type full\|mini\|none` | `<CollectDumpOnTestSessionHang HangDumpType="mini" />` |
| `DumpType` | `mini`, `full`, `none` | Prefer `HangDumpType` | Not applicable | Backward-compatible alias on `<CollectDumpOnTestSessionHang />` |

Examples:

```shell
vstest.console.exe MyTests.dll /Blame:CollectHangDump;TestTimeout=5400s;HangDumpType=full
dotnet test --blame-hang --blame-hang-timeout 90m --blame-hang-dump-type full
```

Use `HangDumpType=none` when you want blame to abort a run after the hang timeout and still produce the sequence file, but do not want a dump file.

## Additional configuration keys

| Option | Values | Default | dotnet test switch | runsettings element |
| --- | --- | --- | --- | --- |
| `CollectDumpOnTestSessionHang` | Element name | Off | Configured by `--blame-hang` | `<CollectDumpOnTestSessionHang ... />` |
| `MonitorPostmortemDebugger` | Element name with `DumpDirectoryPath` attribute | Off | No direct equivalent | `<MonitorPostmortemDebugger DumpDirectoryPath="..." />` |
| `Framework` | Target framework moniker, for example `.NETCoreApp,Version=v8.0` | Supplied by the test platform | No direct equivalent | `<Framework>...</Framework>` |

`MonitorPostmortemDebugger` is intended for scenarios that monitor an external postmortem debugger dump directory. Most users should use `CollectDump` or `CollectDumpOnTestSessionHang` instead.

# Runsettings example

```xml
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="blame" enabled="true">
        <Configuration>
          <CollectDump CollectAlways="true" DumpType="full" />
          <CollectDumpOnTestSessionHang TestTimeout="90m" HangDumpType="mini" />
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

# Dump collection support

For managed testhost crashes on .NET 5 and later, VSTest can collect crash dumps automatically on Windows, macOS, and Linux. Native code crashes, and crashes on .NET Core 3.1 or earlier, require ProcDump:

- Windows: `procdump.exe` or `procdump64.exe`
- Linux/macOS: `procdump`

Put ProcDump on `PATH`, or set `PROCDUMP_PATH` to the directory that contains the executable. Set `VSTEST_DUMP_FORCEPROCDUMP=1` to force ProcDump-based collection on .NET 5 and later.

Hang dumps are supported for these target frameworks and platforms:

| Platform | Target framework support |
| --- | --- |
| Windows | `netcoreapp2.1` and later |
| Linux | `netcoreapp3.1` and later |
| macOS | `net5.0` and later |

# Output

Blame writes attachments under the run's results directory, typically `TestResults/<Guid>/`.

- `Sequence.xml` records the tests that started, in execution order. If a testhost crashes or hangs, the last test listed is usually the test that was running at the time.
- `*.dmp` files are written to the same run-specific results directory when dump collection is enabled.

Here is an example of the emitted sequence file.

```xml
<?xml version="1.0"?>
<TestSequence>
  <Test Name="TestProject.UnitTest1.TestMethodBBB" Source="D:\repos\TestProject\TestProject\bin\Debug\TestProject.dll" />
  <Test Name="TestProject.UnitTest1.TestMethodAAA" Source="D:\repos\TestProject\TestProject\bin\Debug\TestProject.dll" />
</TestSequence>
```

In this case, the `<Test Name>` listed last is the test that was running at the time of the crash.

# Microsoft.Testing.Platform (MTP) equivalent

Microsoft.Testing.Platform does not use `/Blame` or the VSTest blame data collector. MTP projects use `--crashdump`, `--hangdump`, and `--hangdump-timeout` from the `Microsoft.Testing.Extensions.CrashDump` and `Microsoft.Testing.Extensions.HangDump` packages.
