// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.TestPlatform.Build.Trace;
using Microsoft.TestPlatform.Build.Utils;

namespace Microsoft.TestPlatform.Build.Tasks;

public class VSTestTask : Task, ICancelableTask
{
    // The process which is invoking vstest.console
    private VSTestForwardingApp? _vsTestForwardingApp;

    private const string CodeCoverageString = "Code Coverage";

    public string? TestFileFullPath { get; set; }

    public string? VSTestSetting { get; set; }

    public string[]? VSTestTestAdapterPath { get; set; }

    public string? VSTestFramework { get; set; }

    public string? VSTestPlatform { get; set; }

    public string? VSTestTestCaseFilter { get; set; }

    public string[]? VSTestLogger { get; set; }

    public string? VSTestListTests { get; set; }

    public string? VSTestDiag { get; set; }

    public string[]? VSTestCLIRunSettings { get; set; }

    [Required]
    // Initialized to empty string to allow declaring as non-nullable, the property is marked as
    // required so we can ensure that the property is set to non-null before the task is executed.
    public string VSTestConsolePath { get; set; } = "";

    public string? VSTestResultsDirectory { get; set; }

    public string? VSTestVerbosity { get; set; }

    public string[]? VSTestCollect { get; set; }

    public string? VSTestBlame { get; set; }

    public string? VSTestBlameCrash { get; set; }

    public string? VSTestBlameCrashDumpType { get; set; }

    public string? VSTestBlameCrashCollectAlways { get; set; }

    public string? VSTestBlameHang { get; set; }

    public string? VSTestBlameHangDumpType { get; set; }

    public string? VSTestBlameHangTimeout { get; set; }

    public string? VSTestTraceDataCollectorDirectoryPath { get; set; }

    public string? VSTestNoLogo { get; set; }

    public string? VSTestArtifactsProcessingMode { get; set; }

    public string? VSTestSessionCorrelationId { get; set; }

    public override bool Execute()
    {
        var traceEnabledValue = Environment.GetEnvironmentVariable("VSTEST_BUILD_TRACE");
        Tracing.traceEnabled = !traceEnabledValue.IsNullOrEmpty() && traceEnabledValue.Equals("1", StringComparison.OrdinalIgnoreCase);

        var debugEnabled = Environment.GetEnvironmentVariable("VSTEST_BUILD_DEBUG");
        if (!debugEnabled.IsNullOrEmpty() && debugEnabled.Equals("1", StringComparison.Ordinal))
        {
            Console.WriteLine("Waiting for debugger attach...");

            var currentProcess = Process.GetCurrentProcess();
            Console.WriteLine($"Process Id: {currentProcess.Id}, Name: {currentProcess.ProcessName}");

            while (!Debugger.IsAttached)
            {
                Thread.Sleep(1000);
            }

            Debugger.Break();
        }

        // Avoid logging "Task returned false but did not log an error." on test failure, because we don't
        // write MSBuild error. https://github.com/dotnet/msbuild/blob/51a1071f8871e0c93afbaf1b2ac2c9e59c7b6491/src/Framework/IBuildEngine7.cs#L12
        var allowfailureWithoutError = BuildEngine.GetType().GetProperty("AllowFailureWithoutError");
        allowfailureWithoutError?.SetValue(BuildEngine, true);

        _vsTestForwardingApp = new VSTestForwardingApp(VSTestConsolePath, CreateArgument());
        if (!VSTestFramework.IsNullOrEmpty())
        {
            Console.WriteLine(Resources.Resources.TestRunningSummary, TestFileFullPath, VSTestFramework);
        }

        return _vsTestForwardingApp.Execute() == 0;
    }

    public void Cancel()
    {
        Tracing.Trace("VSTest: Killing the process...");
        _vsTestForwardingApp?.Cancel();
    }

    internal IEnumerable<string> CreateArgument()
    {
        var allArgs = AddArgs();

        // VSTestCLIRunSettings should be last argument in allArgs as vstest.console ignore options after "--"(CLIRunSettings option).
        AddCliRunSettingsArgs(allArgs);

        return allArgs;
    }

    private void AddCliRunSettingsArgs(List<string> allArgs)
    {
        if (VSTestCLIRunSettings != null && VSTestCLIRunSettings.Length > 0)
        {
            allArgs.Add("--");
            foreach (var arg in VSTestCLIRunSettings)
            {
                allArgs.Add(ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(arg));
            }
        }
    }

    private List<string> AddArgs()
    {
        var isConsoleLoggerSpecifiedByUser = false;
        var isCollectCodeCoverageEnabled = false;
        var isRunSettingsEnabled = false;
        var allArgs = new List<string>();

        // TODO log arguments in task
        if (!VSTestSetting.IsNullOrEmpty())
        {
            isRunSettingsEnabled = true;
            allArgs.Add("--settings:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(VSTestSetting));
        }

        if (VSTestTestAdapterPath != null && VSTestTestAdapterPath.Length > 0)
        {
            foreach (var arg in VSTestTestAdapterPath)
            {
                allArgs.Add("--testAdapterPath:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(arg));
            }
        }

        if (!VSTestFramework.IsNullOrEmpty())
        {
            allArgs.Add("--framework:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(VSTestFramework));
        }

        // vstest.console only support x86 and x64 for argument platform
        if (!VSTestPlatform.IsNullOrEmpty() && !VSTestPlatform.Contains("AnyCPU"))
        {
            allArgs.Add("--platform:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(VSTestPlatform));
        }

        if (!VSTestTestCaseFilter.IsNullOrEmpty())
        {
            allArgs.Add("--testCaseFilter:" +
                        ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(VSTestTestCaseFilter));
        }

        if (VSTestLogger != null && VSTestLogger.Length > 0)
        {
            foreach (var arg in VSTestLogger)
            {
                allArgs.Add("--logger:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(arg));

                if (arg.StartsWith("console", StringComparison.OrdinalIgnoreCase))
                {
                    isConsoleLoggerSpecifiedByUser = true;
                }
            }
        }

        if (!VSTestResultsDirectory.IsNullOrEmpty())
        {
            allArgs.Add("--resultsDirectory:" +
                        ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(VSTestResultsDirectory));
        }

        if (!VSTestListTests.IsNullOrEmpty())
        {
            allArgs.Add("--listTests");
        }

        if (!VSTestDiag.IsNullOrEmpty())
        {
            allArgs.Add("--Diag:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(VSTestDiag));
        }

        if (TestFileFullPath.IsNullOrEmpty())
        {
            Log.LogError("Test file path cannot be empty or null.");
        }
        else
        {
            allArgs.Add(ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(TestFileFullPath));
        }

        // Console logger was not specified by user, but verbosity was, hence add default console logger with verbosity as specified
        if (!VSTestVerbosity.IsNullOrWhiteSpace() && !isConsoleLoggerSpecifiedByUser)
        {
            var normalTestLogging = new List<string>() { "n", "normal", "d", "detailed", "diag", "diagnostic" };
            var quietTestLogging = new List<string>() { "q", "quiet" };

            string vsTestVerbosity = "minimal";
            if (normalTestLogging.Contains(VSTestVerbosity.ToLowerInvariant()))
            {
                vsTestVerbosity = "normal";
            }
            else if (quietTestLogging.Contains(VSTestVerbosity.ToLowerInvariant()))
            {
                vsTestVerbosity = "quiet";
            }

            allArgs.Add("--logger:Console;Verbosity=" + vsTestVerbosity);
        }

        var blameCrash = !VSTestBlameCrash.IsNullOrEmpty();
        var blameHang = !VSTestBlameHang.IsNullOrEmpty();
        if (!VSTestBlame.IsNullOrEmpty() || blameCrash || blameHang)
        {
            var blameArgs = "--Blame";

            var dumpArgs = new List<string>();
            if (blameCrash || blameHang)
            {
                if (blameCrash)
                {
                    dumpArgs.Add("CollectDump");
                    if (!VSTestBlameCrashCollectAlways.IsNullOrEmpty())
                    {
                        dumpArgs.Add($"CollectAlways={VSTestBlameCrashCollectAlways}");
                    }

                    if (!VSTestBlameCrashDumpType.IsNullOrEmpty())
                    {
                        dumpArgs.Add($"DumpType={VSTestBlameCrashDumpType}");
                    }
                }

                if (blameHang)
                {
                    dumpArgs.Add("CollectHangDump");

                    if (!VSTestBlameHangDumpType.IsNullOrEmpty())
                    {
                        dumpArgs.Add($"HangDumpType={VSTestBlameHangDumpType}");
                    }

                    if (!VSTestBlameHangTimeout.IsNullOrEmpty())
                    {
                        dumpArgs.Add($"TestTimeout={VSTestBlameHangTimeout}");
                    }
                }

                if (dumpArgs.Count != 0)
                {
                    blameArgs += $":\"{string.Join(";", dumpArgs)}\"";
                }
            }

            allArgs.Add(blameArgs);
        }

        if (VSTestCollect != null && VSTestCollect.Length > 0)
        {
            foreach (var arg in VSTestCollect)
            {
                // For collecting code coverage, argument value can be either "Code Coverage" or "Code Coverage;a=b;c=d".
                // Split the argument with ';' and compare first token value.
                var tokens = arg.Split(';');

                if (arg.Equals(CodeCoverageString, StringComparison.OrdinalIgnoreCase) ||
                    tokens[0].Equals(CodeCoverageString, StringComparison.OrdinalIgnoreCase))
                {
                    isCollectCodeCoverageEnabled = true;
                }

                allArgs.Add("--collect:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(arg));
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
            if (!VSTestTraceDataCollectorDirectoryPath.IsNullOrEmpty())
            {
                allArgs.Add("--testAdapterPath:" +
                            ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(
                                VSTestTraceDataCollectorDirectoryPath));
            }
            else
            {
                if (isCollectCodeCoverageEnabled)
                {
                    // Not showing message in runsettings scenario, because we are not sure that code coverage is enabled.
                    // User might be using older Microsoft.NET.Test.Sdk which don't have CodeCoverage infra.
                    Console.WriteLine(Resources.Resources.UpdateTestSdkForCollectingCodeCoverage);
                }
            }
        }

        if (!VSTestNoLogo.IsNullOrWhiteSpace())
        {
            allArgs.Add("--nologo");
        }

        if (!VSTestArtifactsProcessingMode.IsNullOrEmpty() && VSTestArtifactsProcessingMode.Equals("collect", StringComparison.OrdinalIgnoreCase))
        {
            allArgs.Add("--artifactsProcessingMode-collect");
        }

        if (!VSTestSessionCorrelationId.IsNullOrEmpty())
        {
            allArgs.Add("--testSessionCorrelationId:" + ArgumentEscaper.HandleEscapeSequenceInArgForProcessStart(VSTestSessionCorrelationId));
        }

        return allArgs;
    }
}
