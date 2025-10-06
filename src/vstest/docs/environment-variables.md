# VSTest Environment Variables

This document lists all environment variables that are understood and handled by the Visual Studio Test Platform (VSTest). These variables can be used to configure various aspects of test execution, debugging, diagnostics, and feature behavior.

## Connection and Timeout Variables

### VSTEST_CONNECTION_TIMEOUT
- **Description**: Sets the timeout in seconds for establishing connections between various test platform components (vstest.console, testhost, datacollector).
- **Default**: 90 seconds
- **Example**: `VSTEST_CONNECTION_TIMEOUT=120`
- **Usage**: Useful on slow machines or when network latency causes connection timeouts.

### VSTEST_TESTHOST_SHUTDOWN_TIMEOUT
- **Description**: Sets the timeout in milliseconds to wait for testhost to safely shut down.
- **Default**: 100 milliseconds
- **Example**: `VSTEST_TESTHOST_SHUTDOWN_TIMEOUT=500`
- **Usage**: Allows testhost more time to clean up resources before forceful termination.

## Diagnostics and Logging Variables

### VSTEST_DIAG
- **Description**: Enables diagnostic logging and specifies the path to the log file.
- **Format**: Path to log directory and log file (e.g., "logs\log.txt")
- **Example**: `VSTEST_DIAG=C:\temp\logs\vstest.log`
- **Usage**: Equivalent to the `--diag` command line parameter.

### VSTEST_DIAG_VERBOSITY
- **Description**: Sets the verbosity level for diagnostic logging when VSTEST_DIAG is enabled.
- **Valid Values**: `Verbose`, `Info`, `Warning`, `Error`
- **Default**: `Verbose`
- **Example**: `VSTEST_DIAG_VERBOSITY=Info`

### VSTEST_LOGFOLDER
- **Description**: Specifies the folder where test logs should be written.
- **Example**: `VSTEST_LOGFOLDER=C:\TestLogs`

## Debug Variables

### VSTEST_HOST_DEBUG
- **Description**: Enables debugging of the testhost process.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_HOST_DEBUG=1`

### VSTEST_HOST_DEBUG_ATTACHVS
- **Description**: Enables debugging of the testhost process and attempts to attach Visual Studio debugger. Requires AttachVS tool (that can be built in this repo) on PATH.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_HOST_DEBUG_ATTACHVS=1`

### VSTEST_HOST_NATIVE_DEBUG
- **Description**: Enables native debugging of the testhost process.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_HOST_NATIVE_DEBUG=1`

### VSTEST_RUNNER_DEBUG
- **Description**: Enables debugging of the test runner (vstest.console).
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_RUNNER_DEBUG=1`

### VSTEST_RUNNER_DEBUG_ATTACHVS
- **Description**: Enables debugging of the test runner and attempts to attach Visual Studio debugger. Requires AttachVS tool (that can be built in this repo) on PATH.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_RUNNER_DEBUG_ATTACHVS=1`

### VSTEST_RUNNER_NATIVE_DEBUG
- **Description**: Enables native debugging of the test runner.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_RUNNER_NATIVE_DEBUG=1`

### VSTEST_DATACOLLECTOR_DEBUG
- **Description**: Enables debugging of data collector processes.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_DATACOLLECTOR_DEBUG=1`

### VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS
- **Description**: Enables debugging of data collector processes and attempts to attach Visual Studio debugger.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_DATACOLLECTOR_DEBUG_ATTACHVS=1`

### VSTEST_BLAMEDATACOLLECTOR_DEBUG
- **Description**: Enables debugging of the blame data collector.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_BLAMEDATACOLLECTOR_DEBUG=1`

### VSTEST_DUMPTOOL_DEBUG
- **Description**: Enables debugging of dump collection tools.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_DUMPTOOL_DEBUG=1`

### VSTEST_DEBUG_ATTACHVS_PATH
- **Description**: Specifies the path for AttachVS tool, when not found on PATH. AttachVS tool can be built from this repo.
- **Example**: `VSTEST_DEBUG_ATTACHVS_PATH=C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\devenv.exe`

### VSTEST_DEBUG_NOBP
- **Description**: Disables breakpoints on executable entry points, to for more seemless debugging when using AttachVS.
- **Values**: Set to "1" to disable breakpoints
- **Example**: `VSTEST_DEBUG_NOBP=1`

## Crash Dump and Blame Collection Variables

### VSTEST_DUMP_PATH
- **Description**: Overrides the default directory where crash dumps are stored. This disables automatic dump upload via attachments.
- **Example**: `VSTEST_DUMP_PATH=C:\CrashDumps`

### VSTEST_DUMP_TEMP_PATH
- **Description**: Specifies the temporary directory for dump collection operations.
- **Fallback**: Falls back to AGENT_TEMPDIRECTORY, then system temp directory
- **Example**: `VSTEST_DUMP_TEMP_PATH=C:\temp\dumps`

### VSTEST_DUMP_FORCEPROCDUMP
- **Description**: Forces the use of ProcDump for crash dump collection.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_DUMP_FORCEPROCDUMP=1`

### VSTEST_DUMP_FORCENETDUMP
- **Description**: Forces the use of dotnet-dump for crash dump collection.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_DUMP_FORCENETDUMP=1`

### VSTEST_DUMP_PROCDUMPARGUMENTS
- **Description**: Specifies custom arguments for ProcDump when collecting crash dumps.
- **Example**: `VSTEST_DUMP_PROCDUMPARGUMENTS=-e 1 -g -t -ma`

### VSTEST_DUMP_PROCDUMPADDITIONALARGUMENTS
- **Description**: Specifies additional arguments to append to ProcDump command line.
- **Example**: `VSTEST_DUMP_PROCDUMPADDITIONALARGUMENTS=-r`

## Feature Control Variables (Disable Features)

### VSTEST_DISABLE_ARTIFACTS_POSTPROCESSING
- **Description**: Disables artifact post-processing functionality.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_ARTIFACTS_POSTPROCESSING=1`
- **Added**: Version 17.2-preview, 7.0-preview

### VSTEST_DISABLE_ARTIFACTS_POSTPROCESSING_NEW_SDK_UX
- **Description**: Disables new SDK UX for artifact post-processing, showing old output format.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_ARTIFACTS_POSTPROCESSING_NEW_SDK_UX=1`
- **Usage**: Useful when parsing console output and need to maintain compatibility
- **Added**: Version 17.2-preview, 7.0-preview

### VSTEST_DISABLE_FASTER_JSON_SERIALIZATION
- **Description**: Disables the faster JSON serialization mechanism and falls back to standard serialization.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_FASTER_JSON_SERIALIZATION=1`

### VSTEST_DISABLE_MULTI_TFM_RUN
- **Description**: Forces vstest.console to run all sources using the same target framework (TFM) and architecture instead of allowing multiple different TFMs and architectures to run simultaneously.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_MULTI_TFM_RUN=1`

### VSTEST_DISABLE_SERIALTESTRUN_DECORATOR
- **Description**: Disables the SerialTestRun decorator.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_SERIALTESTRUN_DECORATOR=1`

### VSTEST_DISABLE_SHARING_NETFRAMEWORK_TESTHOST
- **Description**: Disables sharing of .NET Framework testhosts, returning to the behavior of sharing testhosts when they are running .NET Framework DLLs and are not disabling appdomains or running in parallel.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_SHARING_NETFRAMEWORK_TESTHOST=1`

### VSTEST_DISABLE_STANDARD_OUTPUT_CAPTURING
- **Description**: Disables capturing standard output from testhost processes.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_STANDARD_OUTPUT_CAPTURING=1`

### VSTEST_DISABLE_STANDARD_OUTPUT_FORWARDING
- **Description**: Disables forwarding standard output from testhost processes.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_STANDARD_OUTPUT_FORWARDING=1`

### VSTEST_DISABLE_THREADPOOL_SIZE_INCREASE
- **Description**: Disables setting a higher value for ThreadPool.SetMinThreads. The higher value allows testhost to connect back faster by avoiding waits for ThreadPool to start more threads.
- **Values**: Set to any non-zero value to disable
- **Example**: `VSTEST_DISABLE_THREADPOOL_SIZE_INCREASE=1`

### VSTEST_DISABLE_UTF8_CONSOLE_ENCODING
- **Description**: Disables setting UTF-8 encoding in console output.
- **Values**: Set to "1" to disable
- **Example**: `VSTEST_DISABLE_UTF8_CONSOLE_ENCODING=1`

## Build and MSBuild Integration Variables

### VSTEST_BUILD_DEBUG
- **Description**: Enables debug output for VSTest build tasks.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_BUILD_DEBUG=1`

### VSTEST_BUILD_TRACE
- **Description**: Enables trace output for VSTest build tasks.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_BUILD_TRACE=1`

### VSTEST_MSBUILD_NOLOGO
- **Description**: Suppresses the display of the copyright banner when running tests via MSBuild.
- **Values**: Set to "1" to suppress logo
- **Example**: `VSTEST_MSBUILD_NOLOGO=1`

## Telemetry Variables

### VSTEST_TELEMETRY_OPTEDIN
- **Description**: Controls whether telemetry data is collected and sent.
- **Values**: Set to "1" to opt in, any other value opts out
- **Example**: `VSTEST_TELEMETRY_OPTEDIN=1`

### VSTEST_LOGTELEMETRY
- **Description**: Enables logging of telemetry data to files.
- **Values**: Set to any non-empty value to enable
- **Example**: `VSTEST_LOGTELEMETRY=1`

### VSTEST_LOGTELEMETRY_PATH
- **Description**: Specifies the directory where telemetry log files should be written.
- **Example**: `VSTEST_LOGTELEMETRY_PATH=C:\TelemetryLogs`

## Performance and Parallelization Variables

### VSTEST_HOSTPRESTART_COUNT
- **Description**: Sets the number of testhosts to pre-start for improved performance in parallel test execution.
- **Format**: Integer value
- **Example**: `VSTEST_HOSTPRESTART_COUNT=4`

## Configuration and Path Variables

### VSTEST_CONSOLE_PATH
- **Description**: Specifies the path to the vstest.console executable.
- **Example**: `VSTEST_CONSOLE_PATH=C:\Tools\VSTest\vstest.console.exe`

### VSTEST_IGNORE_DOTNET_ROOT
- **Description**: When set to a non-zero value, ignores the DOTNET_ROOT environment variable during testhost selection.
- **Values**: Set to any non-zero value to ignore DOTNET_ROOT
- **Default**: "0" (respects DOTNET_ROOT)
- **Example**: `VSTEST_IGNORE_DOTNET_ROOT=1`

### VSTEST_SKIP_FAKES_CONFIGURATION
- **Description**: Skips Microsoft Fakes configuration during test execution.
- **Values**: Set to "1" to skip
- **Example**: `VSTEST_SKIP_FAKES_CONFIGURATION=1`

## UWP (Universal Windows Platform) Variables

### VSTEST_UWP_DEPLOY_LOCAL_PATH
- **Description**: Overrides the local path for UWP application deployment.
- **Example**: `VSTEST_UWP_DEPLOY_LOCAL_PATH=C:\UWPApps\LocalDeploy`

### VSTEST_UWP_DEPLOY_REMOTE_PATH
- **Description**: Overrides the remote path for UWP application deployment.
- **Example**: `VSTEST_UWP_DEPLOY_REMOTE_PATH=\\RemoteDevice\Deploy`

## Windows App Host Variables

### VSTEST_WINAPPHOST_*
- **Description**: Various environment variables related to Windows App Host configuration.
- **Pattern**: Variables following the pattern `VSTEST_WINAPPHOST_{VARIABLE_NAME}`
- **Usage**: Used internally for Windows App Host test execution scenarios

## Legacy/Experimental Variables

### VSTEST_EXPERIMENTAL_FORWARD_OUTPUT_FEATURE
- **Description**: (Deprecated) Previously used to enable output forwarding feature.
- **Status**: Replaced by VSTEST_DISABLE_STANDARD_OUTPUT_CAPTURING and VSTEST_DISABLE_STANDARD_OUTPUT_FORWARDING
- **Note**: This variable is no longer used as the feature is now enabled by default

### VSTEST_DISABLE_PROTOCOL_3_VERSION_DOWNGRADE
- **Description**: Disables automatic downgrade to protocol version 3 for compatibility.
- **Values**: Set to any non-empty value to disable downgrade
- **Example**: `VSTEST_DISABLE_PROTOCOL_3_VERSION_DOWNGRADE=1`

## Usage Examples

### Debugging a Test Run
```bash
# Enable testhost debugging and increase connection timeout
set VSTEST_HOST_DEBUG=1
set VSTEST_CONNECTION_TIMEOUT=300
dotnet test MyTests.dll
```

### Collecting Diagnostics
```bash
# Enable detailed diagnostic logging
set VSTEST_DIAG=C:\temp\vstest.log
set VSTEST_DIAG_VERBOSITY=Verbose
dotnet test MyTests.dll
```

### Performance Optimization
```bash
# Pre-start testhosts for better parallel performance
set VSTEST_HOSTPRESTART_COUNT=4
dotnet test MyTests.dll --parallel
```

### Crash Dump Collection
```bash
# Configure crash dump collection
set VSTEST_DUMP_PATH=C:\CrashDumps
set VSTEST_DUMP_FORCEPROCDUMP=1
dotnet test MyTests.dll --collect:"blame;collectdump=true"
```

## Notes

- Most debug variables require only being set to any non-empty value to be enabled.
- Feature disable variables typically use "1" or any non-zero value to disable the feature.
- Connection timeout values are in seconds unless otherwise specified.
- Paths should use the appropriate path separator for your operating system.
- Some variables are only effective in specific scenarios (e.g., UWP variables only apply to UWP test projects).

For more information about VSTest and its features, see the [VSTest documentation](https://github.com/Microsoft/vstest/tree/main/docs).