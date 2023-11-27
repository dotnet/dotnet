// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

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
}
