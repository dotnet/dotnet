// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

[ExtensionUri(ExtensionUri)]
[FriendlyName(FriendlyName)]
internal class MSBuildLogger : ITestLoggerWithParameters
{
    public const string ExtensionUri = "logger://Microsoft/TestPlatform/MSBuildLogger/v1";

    // This name is not so friendly on purpose, because MSBuild seems like a name that someone might have
    // already claimed, and we will use this just programmatically.
    public const string FriendlyName = "Microsoft.TestPlatform.MSBuildLogger";

    /// <summary>
    /// Gets instance of IOutput used for sending output.
    /// </summary>
    /// <remarks>Protected so this can be detoured for testing purposes.</remarks>
    protected static IOutput? Output
    {
        get;
        private set;
    }

    // Default constructor is needed for a logger to be able to activate it.
    public MSBuildLogger() { }

    /// <summary>
    /// Constructor added for testing purpose
    /// </summary>
    internal MSBuildLogger(IOutput output)
    {
        Output = output;
    }

    [MemberNotNull(nameof(Output))]
    public void Initialize(TestLoggerEvents events, string testRunDirectory)
    {
        ValidateArg.NotNull(events, nameof(events));

        Output ??= ConsoleOutput.Instance;

        // Register for the events.
        events.TestRunMessage += TestMessageHandler;
        events.TestResult += TestResultHandler;
        events.TestRunComplete += TestRunCompleteHandler;
    }

    public void Initialize(TestLoggerEvents events, Dictionary<string, string?> parameters)
    {
        Initialize(events, string.Empty);
    }

    private void TestMessageHandler(object? sender, TestRunMessageEventArgs e)
    {
        switch (e.Level)
        {
            case TestMessageLevel.Informational:
                SendMessage($"output-info", e.Message);
                break;
            case TestMessageLevel.Warning:
                SendMessage($"output-warning", e.Message);
                break;
            case TestMessageLevel.Error:
                SendMessage($"output-error", e.Message);
                break;
        }
    }

    private void TestRunCompleteHandler(object? sender, TestRunCompleteEventArgs e)
    {
        TPDebug.Assert(Output != null, "Initialize should have been called");

        if (e.IsCanceled)
        {
            SendMessage("run-cancel", CommandLineResources.TestRunCanceled);
        }
        else if (e.IsAborted)
        {
            if (e.Error == null)
            {
                SendMessage("run-abort", CommandLineResources.TestRunAborted);
            }
            else
            {
                SendMessage("run-abort", string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunAbortedWithError, e.Error));
            }
        }
        else
        {
            var total = e.TestRunStatistics?.ExecutedTests ?? 0;
            var passed = e.TestRunStatistics?[TestOutcome.Passed] ?? 0;
            var skipped = e.TestRunStatistics?[TestOutcome.Skipped] ?? 0;
            var failed = e.TestRunStatistics?[TestOutcome.Failed] ?? 0;
            var time = e.ElapsedTimeInRunningTests.TotalMilliseconds;

            var summary = string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummary,
                        (failed > 0 ? CommandLineResources.FailedTestIndicator : CommandLineResources.PassedTestIndicator) + "!",
                        failed,
                        passed,
                        skipped,
                        total,
                        $"[{GetFormattedDurationString(e.ElapsedTimeInRunningTests)}]"
                        );

            SendMessage("run-finish",
                summary,
                total.ToString(CultureInfo.InvariantCulture),
                passed.ToString(CultureInfo.InvariantCulture),
                skipped.ToString(CultureInfo.InvariantCulture),
                failed.ToString(CultureInfo.InvariantCulture),
                time.ToString(CultureInfo.InvariantCulture));
        }
    }

    private void TestResultHandler(object? sender, TestResultEventArgs e)
    {
        ValidateArg.NotNull(sender, nameof(sender));
        ValidateArg.NotNull(e, nameof(e));
        TPDebug.Assert(Output != null, "Initialize should have been called.");
        switch (e.Result.Outcome)
        {
            case TestOutcome.Passed:
                SendMessage("test-passed",
                    CommandLineResources.PassedTestIndicator,
                    e.Result.TestCase.DisplayName,
                    e.Result.Duration.TotalMilliseconds.ToString(CultureInfo.InvariantCulture),
                    FormatOutputs(e.Result));
                break;
            case TestOutcome.Skipped:
                SendMessage("test-skipped",
                    CommandLineResources.PassedTestIndicator,
                    e.Result.TestCase.DisplayName,
                    e.Result.Duration.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
                break;
            case TestOutcome.Failed:
                var result = e.Result;
                Debug.WriteLine(">>>>ERR:" + result.ErrorMessage);
                Debug.WriteLine(">>>>STK:" + result.ErrorStackTrace);
                if (!StringUtils.IsNullOrWhiteSpace(result.ErrorStackTrace))
                {
                    string? stackFrame = null;
                    var stackFrames = Regex.Split(result.ErrorStackTrace, Environment.NewLine);
                    string? line = null;
                    string? file = null;
                    string? place = null;
                    if (stackFrames.Length > 0)
                    {
                        foreach (var frame in stackFrames.Take(20))
                        {
                            if (TryGetStackFrameLocation(frame, out line, out file, out place))
                            {
                                break;
                            }
                        }
                    }

                    // We did not find any stack frame with location in the first 20 frames.
                    // Try getting location of the test.
                    if (file == null)
                    {
                        if (!StringUtils.IsNullOrEmpty(result.TestCase.CodeFilePath))
                        {
                            // if there are no symbols but we collect source info, us the source info.
                            file = result.TestCase.CodeFilePath;
                            line = result.TestCase.LineNumber > 0 ? result.TestCase.LineNumber.ToString(CultureInfo.InvariantCulture) : null;
                            place = stackFrame;
                        }
                        else
                        {
                            // if there are no symbols and no source info use the dll
                            place = result.TestCase.DisplayName;
                            file = result.TestCase.Source;
                        }
                    }

                    var outputs = FormatOutputs(result);
                    SendMessage("test-failed", result.DisplayName, result.ErrorMessage, result.ErrorStackTrace, outputs, file, line, place);
                    return;
                }
                else
                {
                    SendMessage("test-failed", result.DisplayName, result.ErrorMessage);
                }

                break;
        }
    }

    private static bool TryGetStackFrameLocation(string stackFrame, out string? line, out string? file, out string? place)
    {
        // stack frame looks like this '   at Program.<Main>$(String[] args) in S:\t\ConsoleApp81\ConsoleApp81\Program.cs:line 9'
        var match = Regex.Match(stackFrame, @"^\s+at (?<code>.+) in (?<file>.+):line (?<line>\d+)$?");

        line = null;
        file = null;
        place = null;

        if (match.Success)
        {
            // get the exact info from stack frame.
            place = match.Groups["code"].Value;
            file = match.Groups["file"].Value;
            line = match.Groups["line"].Value;
        }

        return match.Success;
    }

    /// <summary>
    /// Writes message to standard output, with the name of the message followed by the number of
    /// parameters. With each parameter delimited by '||||', and newlines replaced with ~~~~ and !!!!.
    /// Such as:
    ///  ||||run-start1||||s:\t\mstest97\bin\Debug\net8.0\mstest97.dll
    ///  ||||test-failed6||||TestMethod5||||Assert.IsTrue failed. ||||   at mstest97.UnitTest1.TestMethod5() in s:\t\mstest97\UnitTest1.cs:line 27~~~~!!!!   at Syste...
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    private static void SendMessage(string name, params string?[] data)
    {
        TPDebug.Assert(Output != null, "Initialize should have been called");

        var message = FormatMessage(name, data);
        Debug.WriteLine($"MSBUILDLOGGER: {message}");
        Output.Information(appendPrefix: false, FormatMessage(name, data));
    }

    private static string FormatMessage(string name, params string?[] data)
    {
        return $"||||{name}{data.Length}||||{string.Join("||||", data.Select(Escape))}";
    }

    private static string? Escape(string? input)
    {
        if (input == null)
        {
            return null;
        }

        return input
            // Cleanup characters that we are using ourselves to delimit the message
            .Replace("||||", "____").Replace("~~~~", "____").Replace("!!!!", "____")
            // Replace new line characters that would change how the message is consumed.
            .Replace("\r", "~~~~").Replace("\n", "!!!!");
    }

    /// <summary>
    /// Collects all the messages of a particular category(Standard Output/Standard Error/Debug Traces) and returns a collection.
    /// </summary>
    private static Collection<TestResultMessage> GetTestMessages(Collection<TestResultMessage> messages, string requiredCategory)
    {
        var selectedMessages = messages.Where(msg => msg.Category.Equals(requiredCategory, StringComparison.OrdinalIgnoreCase));
        var requiredMessageCollection = new Collection<TestResultMessage>(selectedMessages.ToList());
        return requiredMessageCollection;
    }

    private static string FormatOutputs(TestResult result)
    {
        var stringBuilder = new StringBuilder();
        var testResultPrefix = "  ";
        TPDebug.Assert(result != null, "a null result can not be displayed");

        var stdOutMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.StandardOutCategory);
        if (stdOutMessagesCollection.Count > 0)
        {
            stringBuilder.AppendLine(testResultPrefix + CommandLineResources.StdOutMessagesBanner);
            AddFormattedOutput(stdOutMessagesCollection, stringBuilder);
        }

        var stdErrMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.StandardErrorCategory);
        if (stdErrMessagesCollection.Count > 0)
        {
            stringBuilder.AppendLine(testResultPrefix + CommandLineResources.StdErrMessagesBanner);
            AddFormattedOutput(stdErrMessagesCollection, stringBuilder);

        }

        var dbgTrcMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.DebugTraceCategory);
        if (dbgTrcMessagesCollection.Count > 0)
        {
            stringBuilder.AppendLine(testResultPrefix + CommandLineResources.DbgTrcMessagesBanner);
            AddFormattedOutput(dbgTrcMessagesCollection, stringBuilder);
        }

        var addnlInfoMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.AdditionalInfoCategory);
        if (addnlInfoMessagesCollection.Count > 0)
        {
            stringBuilder.AppendLine(testResultPrefix + CommandLineResources.AddnlInfoMessagesBanner);
            AddFormattedOutput(addnlInfoMessagesCollection, stringBuilder);
        }

        return stringBuilder.ToString();
    }

    private static void AddFormattedOutput(Collection<TestResultMessage> testMessageCollection, StringBuilder stringBuilder)
    {
        string testMessageFormattingPrefix = " ";
        if (testMessageCollection == null)
        {
            return;
        }

        foreach (var message in testMessageCollection)
        {
            var prefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}", Environment.NewLine, testMessageFormattingPrefix);
            var messageText = message.Text?.Replace(Environment.NewLine, prefix).TrimEnd(testMessageFormattingPrefix.ToCharArray());

            if (!messageText.IsNullOrWhiteSpace())
            {
                stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0}{1}", testMessageFormattingPrefix, messageText);
            }
        }
    }

    /// <summary>
    /// Converts the time span format to readable string.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    internal static string GetFormattedDurationString(TimeSpan duration)
    {
        if (duration == default)
        {
            return "< 1ms";
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
