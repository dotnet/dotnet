# 0030 Specifying Environment Variables In RunSettings File

## Summary
Specifying environment variables in the runsettings file. The environment variables can be set which can directly interact with the test host.

## Motivation
Specifying environment variables in the runsettings file is necessary to support non-trivial projects that require settings env vars like DOTNET_ROOT. These variables are set while spawning the test host process, thus will be available in the host.

## Usage
The runsettings contains a "EnvironmentVariables" node in the RunConfiguration section.
The different environment variables can be specified as element name and it's value.
Below is a sample runsettings for passing environment variables.

```csharp

<?xml version="1.0" encoding="utf-8"?>
<!-- File name extension must be .runsettings -->
<RunSettings>
  <RunConfiguration>
    <EnvironmentVariables>
      <!-- List of environment variables we want to set-->
      <DOTNET_ROOT>C:\ProgramFiles\dotnet</DOTNET_ROOT>
      <SDK_PATH>C:\Codebase\Sdk</SDK_PATH>
    </EnvironmentVariables>
  </RunConfiguration>
</RunSettings>


```
Since these environment variables should always be set when the test host is started, the tests should always run in a separate process.
For this, the `/InIsolation` flag will be set when there are environment variables so that the test host is always invoked.

