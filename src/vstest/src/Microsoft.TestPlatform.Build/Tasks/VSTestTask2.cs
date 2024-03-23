// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.TestPlatform.Build.Tasks;

public class VSTestTask2 : ToolTask, ITestTask
{
    [Required]
    public ITaskItem? TestFileFullPath { get; set; }
    public string? VSTestSetting { get; set; }
    public ITaskItem[]? VSTestTestAdapterPath { get; set; }
    public string? VSTestFramework { get; set; }
    public string? VSTestPlatform { get; set; }
    public string? VSTestTestCaseFilter { get; set; }
    public string[]? VSTestLogger { get; set; }
    public bool VSTestListTests { get; set; }
    public string? VSTestDiag { get; set; }
    public string[]? VSTestCLIRunSettings { get; set; }
    [Required]
    public ITaskItem? VSTestConsolePath { get; set; }
    public ITaskItem? VSTestResultsDirectory { get; set; }
    public string? VSTestVerbosity { get; set; }
    public string[]? VSTestCollect { get; set; }
    public bool VSTestBlame { get; set; }
    public bool VSTestBlameCrash { get; set; }
    public string? VSTestBlameCrashDumpType { get; set; }
    public bool VSTestBlameCrashCollectAlways { get; set; }
    public bool VSTestBlameHang { get; set; }
    public string? VSTestBlameHangDumpType { get; set; }
    public string? VSTestBlameHangTimeout { get; set; }
    public ITaskItem? VSTestTraceDataCollectorDirectoryPath { get; set; }
    public bool VSTestNoLogo { get; set; }
    public string? VSTestArtifactsProcessingMode { get; set; }
    public string? VSTestSessionCorrelationId { get; set; }

    protected override Encoding StandardErrorEncoding => _disableUtf8ConsoleEncoding ? base.StandardErrorEncoding : Encoding.UTF8;
    protected override Encoding StandardOutputEncoding => _disableUtf8ConsoleEncoding ? base.StandardOutputEncoding : Encoding.UTF8;

    private readonly string _messageSplitter = "||||";
    private readonly string[] _messageSplitterArray = new[] { "||||" };

    private readonly bool _disableUtf8ConsoleEncoding;

    protected override string? ToolName => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "dotnet.exe" : "dotnet";

    public VSTestTask2()
    {
        // Unless user opted out, use UTF encoding, which we force in vstest.console.
        _disableUtf8ConsoleEncoding = Environment.GetEnvironmentVariable("VSTEST_DISABLE_UTF8_CONSOLE_ENCODING") == "1";
        LogStandardErrorAsError = false;
        StandardOutputImportance = "Normal";
    }

    protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
    {
        var useTerminalLogger = true;
        Debug.WriteLine($"VSTESTTASK2: Received output {singleLine}, importance {messageImportance}");
        if (TryGetMessage(singleLine, out string name, out string?[] data))
        {
            // See MSBuildLogger.cs for the messages produced.
            // The number suffix is the amount of parameters that are sent along with the message.
            switch (name)
            {
                // Forward the output we receive as messages.
                case "output-info1":
                    Log.LogMessage(MessageImportance.Low, data[0]);
                    break;
                case "output-warning1":
                    Log.LogWarning(data[0]);
                    break;
                case "output-error1":
                    Log.LogError(data[0]);
                    break;

                case "run-cancel1":
                case "run-abort1":
                    Log.LogError(data[0]);
                    break;
                case "run-finish6":
                    // 0 - Localized summary
                    // 1 - total tests
                    // 2 - passed tests
                    // 3 - skipped tests
                    // 4 - failed tests
                    // 5 - duration
                    var summary = data[0];
                    if (useTerminalLogger)
                    {
                        var message = new ExtendedBuildMessageEventArgs("TLTESTFINISH", summary, null, null, MessageImportance.High)
                        {
                            ExtendedMetadata = new Dictionary<string, string?>
                            {
                                ["total"] = data[1],
                                ["passed"] = data[2],
                                ["skipped"] = data[3],
                                ["failed"] = data[4],
                                ["duration"] = data[5],
                            }
                        };

                        BuildEngine.LogMessageEvent(message);
                    }
                    else
                    {
                        Log.LogMessage(MessageImportance.Low, summary);
                    }
                    break;
                case "test-passed4":
                    {
                        // 0 - localized result indicator
                        // 1 - display name
                        // 2 - duration
                        // 3 - outputs
                        var indicator = data[0];
                        var displayName = data[1];
                        var duration = data[2];
                        var outputs = data[3];

                        double durationNumber = 0;
                        var _ = duration != null && double.TryParse(duration, out durationNumber);

                        string? formattedDuration = GetFormattedDurationString(TimeSpan.FromMilliseconds(durationNumber));
                        var testResultWithTime = !formattedDuration.IsNullOrEmpty() ? $"{indicator} {displayName} [{formattedDuration}]" : $"{indicator} {displayName}";
                        var n = Environment.NewLine;

                        var testPassed = StringUtils.IsNullOrWhiteSpace(outputs)
                            ? testResultWithTime
                            : $"{testResultWithTime}{n}Outputs:{n}{outputs}";

                        if (useTerminalLogger)
                        {
                            var message = new ExtendedBuildMessageEventArgs("TLTESTPASSED", testPassed, null, null, MessageImportance.High)
                            {
                                ExtendedMetadata = new Dictionary<string, string?>
                                {
                                    ["localizedResult"] = data[0],
                                    ["displayName"] = data[1],
                                }
                            };
                            BuildEngine.LogMessageEvent(message);
                        }
                        else
                        {
                            Log.LogMessage(MessageImportance.Low, testPassed);
                        }
                    }
                    break;
                case "test-skipped2":
                    {
                        // 0 - localized result indicator
                        // 1 - display name
                        var indicator = data[0];
                        var displayName = data[1];

                        var testSkipped = $"{indicator} {displayName}";
                        if (useTerminalLogger)
                        {
                            var message = new ExtendedBuildMessageEventArgs("TLTESTSKIPPED", testSkipped, null, null, MessageImportance.High)
                            {
                                ExtendedMetadata = new Dictionary<string, string?>
                                {
                                    ["localizedResult"] = data[0],
                                    ["displayName"] = data[1],
                                }
                            };
                            BuildEngine.LogMessageEvent(message);
                        }
                        else
                        {
                            Log.LogMessage(MessageImportance.Low, testSkipped);
                        }
                    }
                    break;
                case "test-failed7":
                    {
                        // 0 - display name
                        // 1 - error message
                        // 2 - error stack trace
                        // 3 - outputs
                        // 4 - file
                        // 5 - line
                        // 6 - place
                        var displayName = data[0]; // Display name
                        var fullErrorMessage = data[1];
                        var fullStackTrace = data[2];
                        var outputs = data[3];
                        var file = data[4];
                        var line = data[5];
                        var place = data[6];
                        var lineNumber = 0;
                        var _ = !StringUtils.IsNullOrWhiteSpace(place) && int.TryParse(line, out lineNumber);

                        string? nameAndPlace = place == displayName ? place : $"{displayName}: {place}";
                        string? singleLineError = JoinSingleLineAndShorten(nameAndPlace, fullErrorMessage);

                        file ??= string.Empty;

                        // Report error to msbuild.
                        Log.LogError(null, "VSTEST1", null, file, lineNumber, 0, 0, 0, singleLineError, null);

                        // Write the full error to verbose log.
                        //
                        // Log without location information, because it will give better experience in binary log viewer. By default you will see the output shortened followed by "Space: view, Ctrl+C: copy).
                        // Pressing space will navigate to code, instead of showing the full output. To show full output you have to right-click and select View full text.
                        // So we avoid providing source info
                        // Log.LogMessage(null, "VSTEST1", null, file, lineNumber, 0, 0, 0, MessageImportance.Low, $"{displayName}: {fullErrorMessage}{n}Stack Trace:{n}{fullStackTrace}");
                        var n = Environment.NewLine;
                        Log.LogMessage(MessageImportance.Low, $"{displayName}: {fullErrorMessage}{n}Stack Trace:{n}{fullStackTrace}{n}Outputs:{n}{outputs}");
                    }
                    break;
                case "test-failed3":
                    {
                        // 0 - display name
                        // 1 - error message
                        // 2 - outputs
                        var displayName = data[0];
                        var fullErrorMessage = data[1];
                        var outputs = data[2];

                        var singleLineError = JoinSingleLineAndShorten(displayName, fullErrorMessage);
                        Log.LogError(null, "VSTEST1", null, string.Empty, 0, 0, 0, 0, singleLineError);
                        // Write the full error to verbose log.
                        //
                        // Log without location information, because it will give better experience in binary log viewer. By default you will see the output shortened followed by "Space: view, Ctrl+C: copy).
                        // Pressing space will navigate to code, instead of showing the full output. To show full output you have to right-click and select View full text.
                        // So we avoid providing source info
                        // var n = Environment.NewLine;
                        // Log.LogMessage(null, "VSTEST1", null, string.Empty, 0, 0, 0, 0, MessageImportance.Low, $"{displayName}: {fullErrorMessage}");
                        var n = Environment.NewLine;
                        Log.LogMessage(MessageImportance.Low, $"{displayName}: {fullErrorMessage}{n}Outputs:{n}{outputs}");
                    }
                    break;
                default:
                    // If we get other message, forward it to binary log. In the future we can ignore this or remove the prefix, but now I want to see it.
                    Log.LogMessage(MessageImportance.Low, $"Unhandled message: {singleLine}");
                    break;
            }
        }
        else
        {
            // We will receive output, such as vstest version, forward it to msbuild log.

            // DO NOT call the base, it parses out the output, and if it sees "error" in any place it will log it as error
            // we don't want this, we only want to log errors from the text messages we receive that start error splitter.
            // base.LogEventsFromTextOutput(singleLine, messageImportance);

            if (!StringUtils.IsNullOrWhiteSpace(singleLine))
            {
                Log.LogMessage(MessageImportance.Low, singleLine);
            }
        }
    }

    private static string? JoinSingleLineAndShorten(string? first, string? second)
    {
        // Join them with space if both are not null,
        // otherwise use the one that is not null.
        return first != null && second != null
            ? SingleLineAndShorten(first) + " " + SingleLineAndShorten(second)
            : SingleLineAndShorten(first) ?? SingleLineAndShorten(second);
    }

    private static string AsForwardedMessage(string?[] data)
    {
        return string.Join("||||", data.Select(CleanSeperator));
    }

    private static string? CleanSeperator(string? text)
    {
        return text == null ? null : text.Replace("||||", "___");
    }

    private static string? SingleLineAndShorten(string? text)
    {
        if (text == null)
        {
            return null;
        }

        return text.Length <= 1000 ? text : text.Substring(0, 1000).Replace('\r', ' ').Replace('\n', ' ');
    }

    private bool TryGetMessage(string singleLine, out string name, out string?[] data)
    {
        if (singleLine.StartsWith(_messageSplitter))
        {
            var parts = singleLine.Split(_messageSplitterArray, StringSplitOptions.None);
            name = parts[1];
            data = parts.Skip(2).Take(parts.Length).Select(p => p == null ? null : p.Replace("~~~~", "\r").Replace("!!!!", "\n")).ToArray();
            return true;
        }

        name = string.Empty;
        data = Array.Empty<string>();
        return false;
    }

    protected override string? GenerateCommandLineCommands()
    {
        return TestTaskUtils.CreateCommandLineArguments(this);
    }

    protected override string? GenerateFullPathToTool()
    {
        if (!ToolPath.IsNullOrEmpty())
        {
            return Path.Combine(Path.GetDirectoryName(Path.GetFullPath(ToolPath))!, ToolExe);
        }

        //TODO: https://github.com/dotnet/sdk/issues/20 Need to get the dotnet path from MSBuild?

        var dhp = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        if (!dhp.IsNullOrEmpty())
        {
            var path = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(dhp))!, ToolExe);
            if (File.Exists(path))
            {
                return path;
            }
        }

        if (File.Exists(ToolExe))
        {
            return Path.GetFullPath(ToolExe);
        }

        var values = Environment.GetEnvironmentVariable("PATH");
        foreach (var p in values!.Split(Path.PathSeparator))
        {
            var fullPath = Path.Combine(p, ToolExe);
            if (File.Exists(fullPath))
                return fullPath;
        }

        return null;
    }

    /// <summary>
    /// Converts the time span format to readable string.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    internal static string? GetFormattedDurationString(TimeSpan duration)
    {
        if (duration == default)
        {
            return null;
        }

        var time = new List<string>();
        if (duration.Days > 0)
        {
            time.Add("> 1d");
        }
        else
        {
            if (duration.Hours > 0)
            {
                time.Add(duration.Hours + "h");
            }

            if (duration.Minutes > 0)
            {
                time.Add(duration.Minutes + "m");
            }

            if (duration.Hours == 0)
            {
                if (duration.Seconds > 0)
                {
                    time.Add(duration.Seconds + "s");
                }

                if (duration.Milliseconds > 0 && duration.Minutes == 0)
                {
                    time.Add(duration.Milliseconds + "ms");
                }
            }
        }

        return time.Count == 0 ? "< 1ms" : string.Join(" ", time);
    }
}
