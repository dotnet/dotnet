// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Microsoft.Build.Utilities;

namespace Microsoft.TestPlatform.Build.Tasks;

internal static class TestTaskUtils
{
    public static string CreateCommandLineArguments(ITestTask task)
    {
        const string codeCoverageString = "Code Coverage";
        const string vsTestAppName = "vstest.console.dll";

        // We have 2 tasks:
        // VSTestTask that writes directly to console output and does not write via msbuild
        // and VSTestTask2 that uses msbuild tools task to capture standard output and translates
        // all output to Error to msbuild errors.
        //
        // Here we are choosing a vstest.console logger to use. Those loggers both write to standard output
        // and error output in vstest.console process, but MSBuild task is not focusing on writing with nice colors
        // but instead on writing cohesive messages in one go, so MSBuild can capture them as single message, and not
        // as 20 different messages for 20 lines.
        var loggerToUse = task is VSTestTask ? "Console" : "Microsoft.TestPlatform.MSBuildLogger";
        // User is free to override the logging using the standard vstest.parameters
        // otherwise we just add the appropriate logger and use the msbuild verbosity
        // to forward it to the vstest.console logger.
        var isLoggerSpecifiedByUser = false;
        var isCollectCodeCoverageEnabled = false;
        var isRunSettingsEnabled = false;
        var hasConsoleLoggerVerbosityInRunSettings = false;

        var builder = new CommandLineBuilder();
        builder.AppendSwitch("exec");
        if (task.VSTestConsolePath != null && !task.VSTestConsolePath.ItemSpec.IsNullOrEmpty())
        {
            builder.AppendFileNameIfNotNull(task.VSTestConsolePath);
        }
        else
        {
            builder.AppendFileNameIfNotNull(vsTestAppName);
        }

        // TODO log arguments in task
        if (!task.VSTestSetting.IsNullOrEmpty())
        {
            isRunSettingsEnabled = true;
            hasConsoleLoggerVerbosityInRunSettings =
                task is VSTestTask && HasConsoleLoggerVerbosity(task.VSTestSetting);
            builder.AppendSwitchIfNotNull("--settings:", task.VSTestSetting);
        }

        if (task.VSTestTestAdapterPath != null)
        {
            foreach (var arg in task.VSTestTestAdapterPath)
            {
                builder.AppendSwitchIfNotNull("--testAdapterPath:", arg);
            }
        }

        builder.AppendSwitchIfNotNull("--framework:", task.VSTestFramework);

        // vstest.console only support x86 and x64 for argument platform
        if (!task.VSTestPlatform.IsNullOrEmpty() && !task.VSTestPlatform.Contains("AnyCPU"))
        {
            builder.AppendSwitchIfNotNull("--platform:", task.VSTestPlatform);
        }

        builder.AppendSwitchIfNotNull("--testCaseFilter:", task.VSTestTestCaseFilter);

        if (task.VSTestLogger != null)
        {
            foreach (var arg in task.VSTestLogger)
            {
                builder.AppendSwitchIfNotNull("--logger:", arg);

                if (arg != null && arg.StartsWith(loggerToUse, StringComparison.OrdinalIgnoreCase))
                {
                    System.Diagnostics.Trace.WriteLine($">>>> : User specified logger: {arg}");
                    isLoggerSpecifiedByUser = true;
                }
            }
        }

        builder.AppendSwitchIfNotNull("--resultsDirectory:", task.VSTestResultsDirectory);

        if (task.VSTestListTests)
        {
            builder.AppendSwitch("--listTests");
        }

        builder.AppendSwitchIfNotNull("--diag:", task.VSTestDiag);

        if (task.TestFileFullPath == null || task.TestFileFullPath.ItemSpec.IsNullOrEmpty())
        {
            task.Log.LogError(Resources.Resources.TestFilePathCannotBeEmptyOrNull);
        }
        else
        {
            builder.AppendFileNameIfNotNull(task.TestFileFullPath);
        }

        // The logger to use (console or msbuild) logger was not specified by user,
        // add the logger and verbosity, so we know what to use in vstest.console.
        if (!isLoggerSpecifiedByUser)
        {
            // For VSTestTask (Console logger): when the settings file configures the console logger
            // verbosity, don't inject Verbosity here so that the settings file takes precedence.
            // For VSTestTask2 (MSBuildLogger): always inject the MSBuild-derived verbosity. The
            // MSBuildLogger verbosity is driven by MSBuild, not by user settings, so it must always
            // reflect the MSBuild-derived value regardless of whether a settings file is present.
            if (hasConsoleLoggerVerbosityInRunSettings)
            {
                builder.AppendSwitchUnquotedIfNotNull("--logger:", loggerToUse);
            }
            else
            {
                string vsTestVerbosity = "minimal";
                if (!task.VSTestVerbosity.IsNullOrWhiteSpace())
                {
                    var normalTestLogging = new List<string>() { "n", "normal", "d", "detailed", "diag", "diagnostic" };
                    var quietTestLogging = new List<string>() { "q", "quiet" };

                    string taskVsTestVerbosity = task.VSTestVerbosity.ToLowerInvariant();
                    if (normalTestLogging.Contains(taskVsTestVerbosity))
                    {
                        vsTestVerbosity = "normal";
                    }
                    else if (quietTestLogging.Contains(taskVsTestVerbosity))
                    {
                        vsTestVerbosity = "quiet";
                    }
                }

                builder.AppendSwitchUnquotedIfNotNull("--logger:", $"{loggerToUse};Verbosity={vsTestVerbosity}");
            }
        }

        if (task.VSTestBlame || task.VSTestBlameCrash || task.VSTestBlameHang)
        {
            var dumpArgs = new List<string>();
            if (task.VSTestBlameCrash || task.VSTestBlameHang)
            {
                if (task.VSTestBlameCrash)
                {
                    dumpArgs.Add("CollectDump");
                    if (task.VSTestBlameCrashCollectAlways)
                    {
                        dumpArgs.Add($"CollectAlways={task.VSTestBlameCrashCollectAlways}");
                    }

                    if (!task.VSTestBlameCrashDumpType.IsNullOrEmpty())
                    {
                        dumpArgs.Add($"DumpType={task.VSTestBlameCrashDumpType}");
                    }
                }

                if (task.VSTestBlameHang)
                {
                    dumpArgs.Add("CollectHangDump");

                    if (!task.VSTestBlameHangDumpType.IsNullOrEmpty())
                    {
                        dumpArgs.Add($"HangDumpType={task.VSTestBlameHangDumpType}");
                    }

                    if (!task.VSTestBlameHangTimeout.IsNullOrEmpty())
                    {
                        dumpArgs.Add($"TestTimeout={task.VSTestBlameHangTimeout}");
                    }
                }
            }

            if (dumpArgs.Count != 0)
            {
                builder.AppendSwitchIfNotNull("--Blame:", string.Join(";", dumpArgs));
            }
            else
            {
                builder.AppendSwitch("--Blame");
            }
        }

        if (task.VSTestCollect != null)
        {
            foreach (var arg in task.VSTestCollect)
            {
                if (arg == null)
                {
                    continue;
                }

                // For collecting code coverage, argument value can be either "Code Coverage" or "Code Coverage;a=b;c=d".
                // Split the argument with ';' and compare first token value.
                var tokens = arg.Split(';');
                if (arg.Equals(codeCoverageString, StringComparison.OrdinalIgnoreCase) ||
                    tokens[0].Equals(codeCoverageString, StringComparison.OrdinalIgnoreCase))
                {
                    isCollectCodeCoverageEnabled = true;
                }

                builder.AppendSwitchIfNotNull("--collect:", arg);
            }
        }

        if (isCollectCodeCoverageEnabled || isRunSettingsEnabled)
        {
            // Pass TraceDataCollector path to vstest.console as TestAdapterPath if --collect "Code Coverage"
            // or --settings (User can enable code coverage from runsettings) option given.
            // Not parsing the runsettings for two reason:
            //    1. To keep no knowledge of runsettings structure in VSTestTask.
            //    2. Impact of adding adapter path always is minimal. (worst case: loads additional data collector assembly in datacollector process.)
            // This is required due to currently trace datacollector not ships with dotnet sdk, can be remove once we have
            // go code coverage x-plat.
            if (task.VSTestTraceDataCollectorDirectoryPath != null && !task.VSTestTraceDataCollectorDirectoryPath.ItemSpec.IsNullOrEmpty())
            {
                builder.AppendSwitchIfNotNull("--testAdapterPath:", task.VSTestTraceDataCollectorDirectoryPath);
            }
            else if (isCollectCodeCoverageEnabled)
            {
                // Not showing message in runsettings scenario, because we are not sure that code coverage is enabled.
                // User might be using older Microsoft.NET.Test.Sdk which don't have CodeCoverage infra.
                task.Log.LogWarning(Resources.Resources.UpdateTestSdkForCollectingCodeCoverage);
            }
        }

        if (task.VSTestNoLogo)
        {
            builder.AppendSwitch("--nologo");
            Environment.SetEnvironmentVariable("VSTEST_MSBUILD_NOLOGO", "1");
        }

        if (string.Equals(task.VSTestArtifactsProcessingMode, "collect", StringComparison.OrdinalIgnoreCase))
        {
            builder.AppendSwitch("--artifactsProcessingMode-collect");
        }

        builder.AppendSwitchIfNotNull("--testSessionCorrelationId:", task.VSTestSessionCorrelationId);

        // VSTestCLIRunSettings should be last argument as vstest.console ignore options after "--" (CLIRunSettings option).
        if (task.VSTestCLIRunSettings != null)
        {
            builder.AppendSwitch("--");
            foreach (var arg in task.VSTestCLIRunSettings)
            {
                builder.AppendSwitchIfNotNull(string.Empty, arg);
            }
        }

        return builder.ToString();
    }

    private static bool HasConsoleLoggerVerbosity(string settingsFile)
    {
        // XDocument.Load resolves the path as a URI, so a malformed path throws UriFormatException
        // (and ArgumentException on .NET Framework), neither of which derive from the exceptions
        // caught below. File.Exists never throws, so it keeps a bad path from failing the build.
        if (!File.Exists(settingsFile))
        {
            return false;
        }

        const string consoleLoggerUri = "logger://Microsoft/TestPlatform/ConsoleLogger/v1";
        XDocument document;
        try
        {
            document = XDocument.Load(settingsFile);
        }
        catch (XmlException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }

        if (document.Root is null)
        {
            return false;
        }

        foreach (var loggerRunSettings in document.Root.Elements())
        {
            if (!loggerRunSettings.Name.LocalName.Equals("LoggerRunSettings", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            foreach (var loggers in loggerRunSettings.Elements())
            {
                if (!loggers.Name.LocalName.Equals("Loggers", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                foreach (var logger in loggers.Elements())
                {
                    if (!logger.Name.LocalName.Equals("Logger", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    string? friendlyName = null;
                    string? uri = null;
                    foreach (var attribute in logger.Attributes())
                    {
                        if (attribute.Name.LocalName.Equals("friendlyName", StringComparison.OrdinalIgnoreCase))
                        {
                            friendlyName = attribute.Value;
                        }
                        else if (attribute.Name.LocalName.Equals("uri", StringComparison.OrdinalIgnoreCase))
                        {
                            uri = attribute.Value;
                        }
                    }

                    if (!string.Equals(friendlyName, "console", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(uri, consoleLoggerUri, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // Verbosity only counts as configured when it sits directly under Configuration,
                    // which is where the console logger reads it from. Searching the whole subtree
                    // would also match a Verbosity element belonging to some other logger's schema.
                    foreach (var configuration in logger.Elements())
                    {
                        if (!configuration.Name.LocalName.Equals("Configuration", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        foreach (var element in configuration.Elements())
                        {
                            if (element.Name.LocalName.Equals("Verbosity", StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Resolves the full path to the dotnet host from the DOTNET_HOST_PATH environment variable.
    /// </summary>
    /// <returns>
    /// The resolved full path to the dotnet host, or <see langword="null"/> if the variable is not set or empty.
    /// </returns>
    internal static string? ResolveDotnetPath()
    {
        var dotnetHostPath = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        return StringUtils.IsNullOrEmpty(dotnetHostPath) ? null : Path.GetFullPath(dotnetHostPath);
    }
}
