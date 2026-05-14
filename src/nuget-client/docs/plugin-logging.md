# Plugin Diagnostic Logging

NuGet 5.1.0 includes opt-in diagnostic plugin logging.  This logging can help diagnose issues involving NuGet plugins.

This logging only pertains to the new plugin model that enabled [package download](https://github.com/NuGet/Home/wiki/NuGet-Package-Download-Plugin) and [cross-platform authentication](https://github.com/NuGet/Home/wiki/NuGet-cross-plat-authentication-plugin) plugins.  It does not pertain to older, Windows-only credential plugins.

## Enabling Logging

To enable diagnostic plugin logging set the environment variable `NUGET_PLUGIN_ENABLE_LOG` to `true`.

By default log files are created in the current working directory.  To override this default set the environment variable `NUGET_PLUGIN_LOG_DIRECTORY_PATH` to a fully qualified directory path.

Here is an example using both environment variables in PowerShell:

```PowerShell
$Env:NUGET_PLUGIN_ENABLE_LOG='true'
$Env:NUGET_PLUGIN_LOG_DIRECTORY_PATH='C:\logs'

.\NuGet.exe restore .\MySolution.sln
```

## Inspecting Log Files

When logging is enabled, each NuGet client and plugin process using [NuGet.Protocol](https://www.nuget.org/packages/NuGet.Protocol) 5.1.0 or later will generate its own log file, and each log file will provide a one-sided history of a NuGet-plugin session.

For instance, a NuGet client process --- whether running in NuGet.exe, dotnet.exe, or Visual Studio --- will generate one log file.  This file will contain the NuGet client's history of all interactions with every plugin throughout the NuGet client process's lifetime.  Also, each plugin process will generate its own log file, which will contain the plugin process's history of all interactions with that NuGet client process.

As diagnostic plugin logging was added in NuGet 5.1.0, versions of the NuGet.Protocol package, which both NuGet clients and plugins use, older than 5.1.0 cannot participate in diagnostic plugin logging.  That means if the NuGet client (e.g.:  NuGet.exe) is 5.1.0 or later, but a plugin is using a version of NuGet.Protocol older than 5.1.0, the NuGet client will generate a log file but the plugin will not.  While having only one and not both log files for a single NuGet-plugin session is suboptimal, that one log file may still be useful.

The [NuGet plugin log viewer](https://github.com/NuGet/Entropy/tree/master/NuGet.Protocol.Plugins.LogViewer) can combine all log files for a single NuGet-plugin session and provide a coherent view for that session.
