# 0028 Blame collector hang detection

## Summary

Blame data collector now supports a new mode meant to help detect and fix hangs in test code. This mode does not introduce any perf hit on the test run as the proc dump process is only started after the specified timeout interval has elapsed.

## Motivation

Whenever running tests in CI systems if a hang occurred it would generally lead to a timeout and abrupt cancelling of the CI pipeline without giving an oppurturnity for the test platform or other scripts to attach the necessary logs/dumps required to analyse the hang.

## Pre-Requisites

This was introduced in testplatform version 16.4.0-preview-20191007-01 (This or a higher version is required)

## Working

If the testhost does not send any messages to the datacollector for the specified duration then it is inferred as a hang and a dump is collected and the testhost process is killed to ensure any available attachments and logs are gracefully attached to the trx file and propagated up to the chain in case of a CI system for further analysis of the hang.

## Steps to configure

Set the `PROCDUMP_PATH` environment variable to the full path to the directory containing
the `procdump.exe` and `procdump64.exe` tools.
This path may be `%ProgramData%\chocolatey\bin\` if you use `choco install procdump` to acquire these tools.

Add the required settings as shown below in the .runsettings file sample below:

```xml
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="blame" enabled="True">
        <Configuration>
          <CollectDumpOnTestSessionHang TestTimeout="300000" DumpType="mini" />
          <ResultsDirectory>%AGENT_TEMPDIRECTORY%</ResultsDirectory>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

Note: This works along with (but can also be used independent of) the existing collect dump option introduced in [RFC 0024](0024-Blame-Collector-Options.md).

## Supported options

|Option|Description|
|--|--|
`DumpType`|If you choose to collect a full process dump. It takes values `mini`/`full`. By default, a mini dump will be created.
`TestTimeout`|Duration of inactivity in milliseconds (no test events from the test host) after which the data collector assumes a hang has occurred and proceeds to collect a dump and kill the test host process.
`ResultsDirectory`|The path must exist, but files will not be permanently stored there. Test results ultimately go to a `TestResults` folder under the test project directory by default, or where the `--results-directory` parameter specifies.

## Invoking the tests

Add the `--settings path-to\test.runsettings` parameter to `dotnet test` or `vstest.console.exe`.
Dump collection with Blame data collector is only supported on Windows for now. For non-Windows agents, omit the .runsettings file or use one that does not activate the Blame collector.

Use the `--results-directory` to control where the `Sequence_<guid>.xml` and `.dmp` files are placed.

## Outputs

The `Sequence_guid.xml` and .dmp files will be placed under the test project source directory in a `TestResults\<guid>\` folder.

Note that the test runner may report a failure to produce the dump file, such as the one shown below:

```
Data collector 'Blame' message: System.IO.FileNotFoundException: Collect dump was enabled but no dump file was generated.
   at Microsoft.TestPlatform.Extensions.BlameDataCollector.ProcessDumpUtility.GetDumpFile()
   at Microsoft.TestPlatform.Extensions.BlameDataCollector.BlameCollector.SessionEndedHandler(Object sender, SessionEndEventArgs args).
```

But when it is followed by an attachments list that includes the dmp file such as the one shown below, the error above is incorrect and can be disregarded:

```
Attachments:
  D:\a\_temp\7d047b47-621b-493e-9a11-ccff70000ce8\testhost.x86_7100_6e0907f276fd4b2ba0d80f1fb5332e89_hangdump.dmp
  D:\a\_temp\7d047b47-621b-493e-9a11-ccff70000ce8\Sequence_6e0907f276fd4b2ba0d80f1fb5332e89.xml
```

## Azure Pipeline considerations

The test runner can be invoked from your pipeline directly by invoking the command line runner. When using one of the Microsoft-owned tasks however, special considerations apply, as described below.

### `VsTest` task usage

As of now this can be used in the vstest task by making the above changes to runsettings and also enabling "advanced diagnostics" with collect dump set to "never" in the task UI if you just want hang dumps. (If both crash and hang detection needs to be enabled then set collect dump to "on abort only").

### `DotNetCoreCLI` task usage

The `DotNetCoreCLI` task will try to upload these files as attachments, but any dump file exceeding 75MB will fail (and full dumps are almost surely going to exceed this limit).

To ensure that dmp files are collected so you can analyze them in the event of a hang/crash, you will need to author the script to capture these `TestResults` directories as artifacts.
This will allow you to collect dumps that exceed 75MB.

The `DotNetCoreCli` task adds a `--results-directory $(Agent.TempDirectory)` switch to the `dotnet test` command, so your artifact uploading script will need to search that directory for your test outputs including .dmp files.
It actually drops *two copies* of the outputs under this directory, so upload just one copy by selecting the files directly under a `$(Agent.TempDirectory)\guid` pattern. Using Powershell this can be done like this:

```ps1
$guidRegex = '^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$'
$filesToCapture = (Get-ChildItem $env:AGENT_TEMPDIRECTORY -Directory |? { $_.Name -match $guidRegex } |% { Get-ChildItem "$($_.FullName)\testhost*.dmp","$($_.FullName)\Sequence_*.xml" -Recurse })
```

The `dotnet test` CLI will fail if two `--results-directory` switches are specified, so it is not possible to override the results directory specified by the `DotNetCoreCli` task.

## Sample

Review [a sample of a pull request](https://github.com/AArnott/Library.Template/pull/43) to a repo that activates crash and hang dump collection using the `DotNetCoreCLI` task.
