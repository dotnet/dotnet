// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
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
        // events.TestRunMessage += TestMessageHandler;
        events.TestResult += TestResultHandler;
        events.TestRunComplete += TestRunCompleteHandler;
        // events.TestRunStart += TestRunStartHandler;

        // Register for the discovery events.
        // events.DiscoveryMessage += TestMessageHandler;
    }

    public void Initialize(TestLoggerEvents events, Dictionary<string, string?> parameters)
    {
        Initialize(events, string.Empty);
    }

    private void TestRunCompleteHandler(object? sender, TestRunCompleteEventArgs e)
    {
        TPDebug.Assert(Output != null, "Initialize should have been called");

        if (e.IsCanceled)
        {
            Output.Error(false, CommandLineResources.TestRunCanceled);
        }
        else if (e.IsAborted)
        {
            if (e.Error == null)
            {
                Output.Error(false, CommandLineResources.TestRunAborted);
            }
            else
            {
                Output.Error(false, CommandLineResources.TestRunAbortedWithError, e.Error);
            }
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
            case TestOutcome.Skipped:

                var test = e.Result.TestCase.DisplayName;
                var outcome = e.Result.Outcome == TestOutcome.Passed
                    ? CommandLineResources.PassedTestIndicator
                    : CommandLineResources.SkippedTestIndicator;
                var info = $"++++{outcome}++++{ReplacePlusSeparator(test)}";

                Debug.WriteLine(">>>>MESSAGE:" + info);
                Output.Information(false, info);
                break;
            case TestOutcome.Failed:

                var result = e.Result;
                if (!StringUtils.IsNullOrWhiteSpace(result.ErrorStackTrace))
                {
                    var maxLength = 1000;
                    string? error = null;
                    if (result.ErrorMessage != null)
                    {
                        // Do not use environment.newline here, we want to replace also \n on Windows.
                        var oneLineMessage = result.ErrorMessage.Replace("\n", " ").Replace("\r", " ");
                        error = oneLineMessage.Length > maxLength ? oneLineMessage.Substring(0, maxLength) : oneLineMessage;
                    }

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

                    place = $"({result.TestCase.DisplayName}) {place}";
                    var message = $"||||{ReplacePipeSeparator(file)}||||{line}||||{ReplacePipeSeparator(place)}||||{ReplacePipeSeparator(error)}";

                    Debug.WriteLine(">>>>MESSAGE:" + message);
                    Output.Error(false, message);

                    var fullError = $"~~~~{ReplaceTildaSeparator(result.ErrorMessage)}~~~~{ReplaceTildaSeparator(result.ErrorStackTrace)}";
                    Output.Information(false, fullError);
                    return;
                }
                else
                {
                    Output.Error(false, result.DisplayName?.Replace(Environment.NewLine, " ") ?? string.Empty);
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

        Debug.WriteLine($">>>> {(match.Success ? "MATCH" : "NOMATCH")} {stackFrame}");

        return match.Success;
    }


    private static string? ReplacePipeSeparator(string? text)
    {
        if (text == null)
        {
            return null;
        }

        // Remove any occurrence of message splitter.
        return text.Replace("||||", "____");
    }

    private static string? ReplacePlusSeparator(string? text)
    {
        if (text == null)
        {
            return null;
        }

        // Remove any occurrence of message splitter.
        return text.Replace("++++", "____");
    }

    private static string? ReplaceTildaSeparator(string? text)
    {
        if (text == null)
        {
            return null;
        }

        // Remove any occurrence of message splitter.
        text = text.Replace("~~~~", "____");
        // Clean up any occurrence of newline splitter.
        text = text.Replace("!!!!", "____");
        // Replace newlines with newline splitter.
        text = text.Replace(Environment.NewLine, "!!!!");

        return text;
    }

}
