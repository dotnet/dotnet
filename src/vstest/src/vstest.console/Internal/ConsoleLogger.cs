// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using NuGet.Frameworks;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

/// <summary>
/// Logger for sending output to the console.
/// All the console logger messages prints to Standard Output with respective color, except OutputLevel.Error messages
/// from adapters and test run result on failed.
/// </summary>
[FriendlyName(FriendlyName)]
[ExtensionUri(ExtensionUri)]
internal class ConsoleLogger : ITestLoggerWithParameters
{
    private const string TestMessageFormattingPrefix = " ";

    /// <summary>
    /// Prefix used for formatting the result output
    /// </summary>
    private const string TestResultPrefix = "  ";

    /// <summary>
    /// Suffix used for formatting the result output
    /// </summary>
    private const string TestResultSuffix = " ";

    /// <summary>
    /// Bool to decide whether Verbose level should be added as prefix or not in log messages.
    /// </summary>
    internal static bool AppendPrefix;

    /// <summary>
    /// Bool to decide whether progress indicator should be enabled.
    /// </summary>
    internal static bool EnableProgress;

    /// <summary>
    /// Uri used to uniquely identify the console logger.
    /// </summary>
    public const string ExtensionUri = "logger://Microsoft/TestPlatform/ConsoleLogger/v1";

    /// <summary>
    /// Alternate user friendly string to uniquely identify the console logger.
    /// </summary>
    public const string FriendlyName = "Console";

    /// <summary>
    /// Parameter for Verbosity
    /// </summary>
    public const string VerbosityParam = "verbosity";

    /// <summary>
    /// Parameter for log message prefix
    /// </summary>
    public const string PrefixParam = "prefix";

    /// <summary>
    /// Parameter for disabling progress
    /// </summary>
    public const string ProgressIndicatorParam = "progress";

    /// <summary>
    ///  Property Id storing the ParentExecutionId.
    /// </summary>
    public const string ParentExecutionIdPropertyIdentifier = "ParentExecId";

    /// <summary>
    ///  Property Id storing the ExecutionId.
    /// </summary>
    public const string ExecutionIdPropertyIdentifier = "ExecutionId";

    // Figure out the longest result string (+1 for ! where applicable), so we don't
    // get misaligned output on non-english systems
    private static readonly int LongestResultIndicator = new[]
    {
        CommandLineResources.FailedTestIndicator.Length + 1,
        CommandLineResources.PassedTestIndicator.Length + 1,
        CommandLineResources.SkippedTestIndicator.Length + 1,
        CommandLineResources.None.Length
    }.Max();

    internal enum Verbosity
    {
        Quiet,
        Minimal,
        Normal,
        Detailed
    }

    private bool _testRunHasErrorMessages;

    /// <summary>
    /// Framework on which the test runs.
    /// </summary>
    private string? _targetFramework;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ConsoleLogger()
    {
    }

    /// <summary>
    /// Constructor added for testing purpose
    /// </summary>
    internal ConsoleLogger(IOutput output, IProgressIndicator progressIndicator, IFeatureFlag featureFlag)
    {
        Output = output;
        _progressIndicator = progressIndicator;
        _featureFlag = featureFlag;
    }

    /// <summary>
    /// Gets instance of IOutput used for sending output.
    /// </summary>
    /// <remarks>Protected so this can be detoured for testing purposes.</remarks>
    protected static IOutput? Output
    {
        get;
        private set;
    }

    private IProgressIndicator? _progressIndicator;

    private readonly IFeatureFlag _featureFlag = FeatureFlag.Instance;

    /// <summary>
    /// Get the verbosity level for the console logger
    /// </summary>

    public Verbosity VerbosityLevel { get; private set; } =
#if NETFRAMEWORK
        Verbosity.Normal;
#else
        // Keep default verbosity for x-plat command line as minimal
        Verbosity.Minimal;
#endif

    /// <summary>
    /// Tracks leaf test outcomes per source. This is needed to correctly count hierarchical tests as well as
    /// tracking counts per source for the minimal and quiet output.
    /// </summary>
    private ConcurrentDictionary<Guid, MinimalTestResult>? LeafTestResults { get; set; }

    #region ITestLoggerWithParameters

    /// <summary>
    /// Initializes the Test Logger.
    /// </summary>
    /// <param name="events">Events that can be registered for.</param>
    /// <param name="testRunDirectory">Test Run Directory</param>
    [MemberNotNull(nameof(Output), nameof(LeafTestResults))]
    public void Initialize(TestLoggerEvents events, string testRunDirectory)
    {
        ValidateArg.NotNull(events, nameof(events));

        Output ??= ConsoleOutput.Instance;

        if (_progressIndicator == null && !Console.IsOutputRedirected && EnableProgress)
        {
            // Progress indicator needs to be displayed only for cli experience.
            _progressIndicator = new ProgressIndicator(Output, new ConsoleHelper());
        }

        // Register for the events.
        events.TestRunMessage += TestMessageHandler;
        events.TestResult += TestResultHandler;
        events.TestRunComplete += TestRunCompleteHandler;
        events.TestRunStart += TestRunStartHandler;

        // Register for the discovery events.
        events.DiscoveryMessage += TestMessageHandler;
        LeafTestResults = new ConcurrentDictionary<Guid, MinimalTestResult>();

        // TODO Get changes from https://github.com/Microsoft/vstest/pull/1111/
        // events.DiscoveredTests += DiscoveredTestsHandler;
    }

    public void Initialize(TestLoggerEvents events, Dictionary<string, string?> parameters)
    {
        ValidateArg.NotNull(parameters, nameof(parameters));

        if (parameters.Count == 0)
        {
            throw new ArgumentException("No default parameters added", nameof(parameters));
        }

        var verbosityExists = parameters.TryGetValue(VerbosityParam, out var verbosity);
        if (verbosityExists && Enum.TryParse(verbosity, true, out Verbosity verbosityLevel))
        {
            VerbosityLevel = verbosityLevel;
        }

        var prefixExists = parameters.TryGetValue(PrefixParam, out var prefix);
        if (prefixExists)
        {
            _ = bool.TryParse(prefix, out AppendPrefix);
        }

        var progressArgExists = parameters.TryGetValue(ProgressIndicatorParam, out var enableProgress);
        if (progressArgExists)
        {
            _ = bool.TryParse(enableProgress, out EnableProgress);
        }

        parameters.TryGetValue(DefaultLoggerParameterNames.TargetFramework, out _targetFramework);
        _targetFramework = !_targetFramework.IsNullOrEmpty() ? NuGetFramework.Parse(_targetFramework).GetShortFolderName() : _targetFramework;

        Initialize(events, string.Empty);
    }
    #endregion

    /// <summary>
    /// Prints the timespan onto console.
    /// </summary>
    private static void PrintTimeSpan(TimeSpan timeSpan)
    {
        TPDebug.Assert(Output is not null, "ConsoleLogger.Output is null");

        if (timeSpan.TotalDays >= 1)
        {
            Output.Information(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, timeSpan.TotalDays, CommandLineResources.Days));
        }
        else if (timeSpan.TotalHours >= 1)
        {
            Output.Information(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, timeSpan.TotalHours, CommandLineResources.Hours));
        }
        else if (timeSpan.TotalMinutes >= 1)
        {
            Output.Information(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, timeSpan.TotalMinutes, CommandLineResources.Minutes));
        }
        else
        {
            Output.Information(false, string.Format(CultureInfo.CurrentCulture, CommandLineResources.ExecutionTimeFormatString, timeSpan.TotalSeconds, CommandLineResources.Seconds));
        }
    }

    /// <summary>
    /// Constructs a well formatted string using the given prefix before every message content on each line.
    /// </summary>
    private static string GetFormattedOutput(Collection<TestResultMessage> testMessageCollection)
    {
        if (testMessageCollection == null)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        foreach (var message in testMessageCollection)
        {
            var prefix = string.Format(CultureInfo.CurrentCulture, "{0}{1}", Environment.NewLine, TestMessageFormattingPrefix);
            var messageText = message.Text?.Replace(Environment.NewLine, prefix).TrimEnd(TestMessageFormattingPrefix.ToCharArray());

            if (!messageText.IsNullOrWhiteSpace())
            {
                sb.AppendFormat(CultureInfo.CurrentCulture, "{0}{1}", TestMessageFormattingPrefix, messageText);
            }
        }
        return sb.ToString();
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

    /// <summary>
    /// outputs the Error messages, Stack Trace, and other messages for the parameter test.
    /// </summary>
    private static void DisplayFullInformation(TestResult result)
    {
        TPDebug.Assert(result != null, "a null result can not be displayed");
        TPDebug.Assert(Output != null, "Initialize should have been called.");

        // Add newline if it is not in given output data.
        var addAdditionalNewLine = false;

        if (!result.ErrorMessage.IsNullOrEmpty())
        {
            addAdditionalNewLine = true;
            Output.Information(false, ConsoleColor.Red, TestResultPrefix + CommandLineResources.ErrorMessageBanner);
            var errorMessage = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", TestResultPrefix, TestMessageFormattingPrefix, result.ErrorMessage);
            Output.Information(false, ConsoleColor.Red, errorMessage);
        }

        if (!result.ErrorStackTrace.IsNullOrEmpty())
        {
            addAdditionalNewLine = false;
            Output.Information(false, ConsoleColor.Red, TestResultPrefix + CommandLineResources.StacktraceBanner);
            var stackTrace = string.Format(CultureInfo.CurrentCulture, "{0}{1}", TestResultPrefix, result.ErrorStackTrace);
            Output.Information(false, ConsoleColor.Red, stackTrace);
        }

        var stdOutMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.StandardOutCategory);
        if (stdOutMessagesCollection.Count > 0)
        {
            addAdditionalNewLine = true;
            var stdOutMessages = GetFormattedOutput(stdOutMessagesCollection);

            if (!stdOutMessages.IsNullOrEmpty())
            {
                Output.Information(false, TestResultPrefix + CommandLineResources.StdOutMessagesBanner);
                Output.Information(false, stdOutMessages);
            }
        }

        var stdErrMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.StandardErrorCategory);
        if (stdErrMessagesCollection.Count > 0)
        {
            addAdditionalNewLine = false;
            var stdErrMessages = GetFormattedOutput(stdErrMessagesCollection);

            if (!stdErrMessages.IsNullOrEmpty())
            {
                Output.Information(false, ConsoleColor.Red, TestResultPrefix + CommandLineResources.StdErrMessagesBanner);
                Output.Information(false, ConsoleColor.Red, stdErrMessages);
            }
        }

        var dbgTrcMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.DebugTraceCategory);
        if (dbgTrcMessagesCollection.Count > 0)
        {
            addAdditionalNewLine = false;
            var dbgTrcMessages = GetFormattedOutput(dbgTrcMessagesCollection);

            if (!dbgTrcMessages.IsNullOrEmpty())
            {
                Output.Information(false, TestResultPrefix + CommandLineResources.DbgTrcMessagesBanner);
                Output.Information(false, dbgTrcMessages);
            }
        }

        var addnlInfoMessagesCollection = GetTestMessages(result.Messages, TestResultMessage.AdditionalInfoCategory);
        if (addnlInfoMessagesCollection.Count > 0)
        {
            addAdditionalNewLine = false;
            var addnlInfoMessages = GetFormattedOutput(addnlInfoMessagesCollection);

            if (!addnlInfoMessages.IsNullOrEmpty())
            {
                Output.Information(false, TestResultPrefix + CommandLineResources.AddnlInfoMessagesBanner);
                Output.Information(false, addnlInfoMessages);
            }
        }

        if (addAdditionalNewLine)
        {
            Output.WriteLine(string.Empty, OutputLevel.Information);
        }
    }

    /// <summary>
    /// Returns the parent Execution id of given test result.
    /// </summary>
    /// <param name="testResult"></param>
    /// <returns></returns>
    private static Guid GetParentExecutionId(TestResult testResult)
    {
        var parentExecutionIdProperty = testResult.Properties.FirstOrDefault(property =>
            property.Id.Equals(ParentExecutionIdPropertyIdentifier));
        return parentExecutionIdProperty == null
            ? Guid.Empty
            : testResult.GetPropertyValue(parentExecutionIdProperty, Guid.Empty);
    }

    /// <summary>
    /// Returns execution id of given test result
    /// </summary>
    /// <param name="testResult"></param>
    /// <returns></returns>
    private static Guid GetExecutionId(TestResult testResult)
    {
        var executionIdProperty = testResult.Properties.FirstOrDefault(property =>
            property.Id.Equals(ExecutionIdPropertyIdentifier));
        var executionId = Guid.Empty;

        if (executionIdProperty != null)
        {
            executionId = testResult.GetPropertyValue(executionIdProperty, Guid.Empty);
        }

        return executionId.Equals(Guid.Empty) ? Guid.NewGuid() : executionId;
    }

    /// <summary>
    /// Called when a test run start is received
    /// </summary>
    private void TestRunStartHandler(object? sender, TestRunStartEventArgs e)
    {
        ValidateArg.NotNull(sender, nameof(sender));
        ValidateArg.NotNull(e, nameof(e));
        TPDebug.Assert(Output != null, "Initialize should have been called");

        // Print all test containers.
        Output.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestSourcesDiscovered, CommandLineOptions.Instance.Sources.Count()), OutputLevel.Information);
        if (VerbosityLevel == Verbosity.Detailed)
        {
            foreach (var source in CommandLineOptions.Instance.Sources)
            {
                Output.WriteLine(source, OutputLevel.Information);
            }
        }
    }

    /// <summary>
    /// Called when a test message is received.
    /// </summary>
    private void TestMessageHandler(object? sender, TestRunMessageEventArgs e)
    {
        ValidateArg.NotNull(sender, nameof(sender));
        ValidateArg.NotNull(e, nameof(e));
        TPDebug.Assert(Output is not null, "ConsoleLogger.Output is null");

        switch (e.Level)
        {
            case TestMessageLevel.Informational:
                {
                    if (VerbosityLevel is Verbosity.Quiet or Verbosity.Minimal)
                    {
                        break;
                    }

                    // Pause the progress indicator to print the message
                    _progressIndicator?.Pause();

                    Output.Information(AppendPrefix, e.Message);

                    // Resume the progress indicator after printing the message
                    _progressIndicator?.Start();

                    break;
                }

            case TestMessageLevel.Warning:
                {
                    if (VerbosityLevel == Verbosity.Quiet)
                    {
                        break;
                    }

                    // Pause the progress indicator to print the message
                    _progressIndicator?.Pause();

                    Output.Warning(AppendPrefix, e.Message);

                    // Resume the progress indicator after printing the message
                    _progressIndicator?.Start();

                    break;
                }

            case TestMessageLevel.Error:
                {
                    // Pause the progress indicator to print the message
                    _progressIndicator?.Pause();

                    _testRunHasErrorMessages = true;
                    Output.Error(AppendPrefix, e.Message);

                    // Resume the progress indicator after printing the message
                    _progressIndicator?.Start();

                    break;
                }
            default:
                EqtTrace.Warning("ConsoleLogger.TestMessageHandler: The test message level is unrecognized: {0}", e.Level.ToString());
                break;
        }
    }

    /// <summary>
    /// Called when a test result is received.
    /// </summary>
    private void TestResultHandler(object? sender, TestResultEventArgs e)
    {
        ValidateArg.NotNull(sender, nameof(sender));
        ValidateArg.NotNull(e, nameof(e));
        TPDebug.Assert(Output != null && LeafTestResults != null, "Initialize should have been called");

        var testDisplayName = e.Result.DisplayName;

        if (e.Result.DisplayName.IsNullOrWhiteSpace())
        {
            testDisplayName = e.Result.TestCase.DisplayName;
        }

        string? formattedDuration = GetFormattedDurationString(e.Result.Duration);
        if (!formattedDuration.IsNullOrEmpty())
        {
            testDisplayName = $"{testDisplayName} [{formattedDuration}]";
        }

        var executionId = GetExecutionId(e.Result);
        var parentExecutionId = GetParentExecutionId(e.Result);

        if (parentExecutionId != Guid.Empty)
        {
            // Not checking the result value.
            // This would return false if the id did not exist,
            // or true if it did exist. In either case the id is not in the dictionary
            // which is our goal.
            LeafTestResults.TryRemove(parentExecutionId, out _);
        }

        if (!LeafTestResults.TryAdd(executionId, new MinimalTestResult(e.Result)))
        {
            // This would happen if the key already exists. This should not happen, because we are
            // inserting by GUID key, so this would mean an error in our code.
            throw new InvalidOperationException($"ExecutionId {executionId} already exists.");
        }

        switch (e.Result.Outcome)
        {
            case TestOutcome.Skipped:
                {
                    if (VerbosityLevel == Verbosity.Quiet)
                    {
                        break;
                    }

                    // Pause the progress indicator before displaying test result information
                    _progressIndicator?.Pause();

                    Output.Write(GetFormattedTestIndicator(CommandLineResources.SkippedTestIndicator), OutputLevel.Information, ConsoleColor.Yellow);
                    Output.WriteLine(testDisplayName, OutputLevel.Information);
                    if (VerbosityLevel == Verbosity.Detailed)
                    {
                        DisplayFullInformation(e.Result);
                    }

                    // Resume the progress indicator after displaying the test result information
                    _progressIndicator?.Start();

                    break;
                }

            case TestOutcome.Failed:
                {
                    if (VerbosityLevel == Verbosity.Quiet)
                    {
                        break;
                    }

                    // Pause the progress indicator before displaying test result information
                    _progressIndicator?.Pause();

                    Output.Write(GetFormattedTestIndicator(CommandLineResources.FailedTestIndicator), OutputLevel.Information, ConsoleColor.Red);
                    Output.WriteLine(testDisplayName, OutputLevel.Information);
                    DisplayFullInformation(e.Result);

                    // Resume the progress indicator after displaying the test result information
                    _progressIndicator?.Start();

                    break;
                }

            case TestOutcome.Passed:
                {
                    if (VerbosityLevel is Verbosity.Normal or Verbosity.Detailed)
                    {
                        // Pause the progress indicator before displaying test result information
                        _progressIndicator?.Pause();

                        Output.Write(GetFormattedTestIndicator(CommandLineResources.PassedTestIndicator), OutputLevel.Information, ConsoleColor.Green);
                        Output.WriteLine(testDisplayName, OutputLevel.Information);
                        if (VerbosityLevel == Verbosity.Detailed)
                        {
                            DisplayFullInformation(e.Result);
                        }

                        // Resume the progress indicator after displaying the test result information
                        _progressIndicator?.Start();
                    }

                    break;
                }

            default:
                {
                    if (VerbosityLevel == Verbosity.Quiet)
                    {
                        break;
                    }

                    // Pause the progress indicator before displaying test result information
                    _progressIndicator?.Pause();

                    Output.Write(GetFormattedTestIndicator(CommandLineResources.SkippedTestIndicator), OutputLevel.Information, ConsoleColor.Yellow);
                    Output.WriteLine(testDisplayName, OutputLevel.Information);
                    if (VerbosityLevel == Verbosity.Detailed)
                    {
                        DisplayFullInformation(e.Result);
                    }

                    // Resume the progress indicator after displaying the test result information
                    _progressIndicator?.Start();

                    break;
                }
        }

        // Local functions
        static string GetFormattedTestIndicator(string indicator) => TestResultPrefix + indicator + TestResultSuffix;
    }

    private static string? GetFormattedDurationString(TimeSpan duration)
    {
        if (duration == default)
        {
            return null;
        }

        var time = new List<string>();
        if (duration.Hours > 0)
        {
            time.Add(duration.Hours + " h");
        }

        if (duration.Minutes > 0)
        {
            time.Add(duration.Minutes + " m");
        }

        if (duration.Hours == 0)
        {
            if (duration.Seconds > 0)
            {
                time.Add(duration.Seconds + " s");
            }

            if (duration.Milliseconds > 0 && duration.Minutes == 0 && duration.Seconds == 0)
            {
                time.Add(duration.Milliseconds + " ms");
            }
        }

        return time.Count == 0 ? "< 1 ms" : string.Join(" ", time);
    }

    /// <summary>
    /// Called when a test run is completed.
    /// </summary>
    private void TestRunCompleteHandler(object? sender, TestRunCompleteEventArgs e)
    {
        TPDebug.Assert(Output != null, "Initialize should have been called");

        // Stop the progress indicator as we are about to print the summary
        _progressIndicator?.Stop();
        var passedTests = 0;
        var failedTests = 0;
        var skippedTests = 0;
        var totalTests = 0;
        Output.WriteLine(string.Empty, OutputLevel.Information);

        // Printing Run-level Attachments
        var runLevelAttachmentsCount = e.AttachmentSets == null ? 0 : e.AttachmentSets.Sum(attachmentSet => attachmentSet.Attachments.Count);
        if (runLevelAttachmentsCount > 0)
        {
            // If ARTIFACTS_POSTPROCESSING is disabled
            if (_featureFlag.IsSet(FeatureFlag.DISABLE_ARTIFACTS_POSTPROCESSING) ||
                // DISABLE_ARTIFACTS_POSTPROCESSING_NEW_SDK_UX(new UX) is disabled
                _featureFlag.IsSet(FeatureFlag.DISABLE_ARTIFACTS_POSTPROCESSING_NEW_SDK_UX) ||
                // TestSessionCorrelationId is null(we're not running through the dotnet SDK).
                CommandLineOptions.Instance.TestSessionCorrelationId is null)
            {
                Output.Information(false, CommandLineResources.AttachmentsBanner);
                TPDebug.Assert(e.AttachmentSets != null, "e.AttachmentSets should not be null when runLevelAttachmentsCount > 0.");
                foreach (var attachmentSet in e.AttachmentSets)
                {
                    foreach (var uriDataAttachment in attachmentSet.Attachments)
                    {
                        var attachmentOutput = string.Format(CultureInfo.CurrentCulture, CommandLineResources.AttachmentOutputFormat, uriDataAttachment.Uri.LocalPath);
                        Output.Information(false, attachmentOutput);
                    }
                }
            }
        }

        var leafTestResultsPerSource = LeafTestResults?.Select(p => p.Value)?.GroupBy(r => r.TestCase.Source);
        if (leafTestResultsPerSource is not null)
        {
            foreach (var sd in leafTestResultsPerSource)
            {
                var source = sd.Key;
                var sourceSummary = new SourceSummary();

                var results = sd.ToArray();
                // duration of the whole source is the difference between the test that ended last and the one that started first
                sourceSummary.Duration = !results.Any() ? TimeSpan.Zero : results.Max(r => r.EndTime) - results.Min(r => r.StartTime);
                foreach (var result in results)
                {
                    switch (result.Outcome)
                    {
                        case TestOutcome.Passed:
                            sourceSummary.TotalTests++;
                            sourceSummary.PassedTests++;
                            break;
                        case TestOutcome.Failed:
                            sourceSummary.TotalTests++;
                            sourceSummary.FailedTests++;
                            break;
                        case TestOutcome.Skipped:
                            sourceSummary.TotalTests++;
                            sourceSummary.SkippedTests++;
                            break;
                        default:
                            break;
                    }
                }

                if (VerbosityLevel is Verbosity.Quiet or Verbosity.Minimal)
                {
                    TestOutcome sourceOutcome = TestOutcome.None;
                    if (sourceSummary.FailedTests > 0)
                    {
                        sourceOutcome = TestOutcome.Failed;
                    }
                    else if (sourceSummary.PassedTests > 0)
                    {
                        sourceOutcome = TestOutcome.Passed;
                    }
                    else if (sourceSummary.SkippedTests > 0)
                    {
                        sourceOutcome = TestOutcome.Skipped;
                    }

                    string resultString = sourceOutcome switch
                    {
                        TestOutcome.Failed => (CommandLineResources.FailedTestIndicator + "!").PadRight(LongestResultIndicator),
                        TestOutcome.Passed => (CommandLineResources.PassedTestIndicator + "!").PadRight(LongestResultIndicator),
                        TestOutcome.Skipped => (CommandLineResources.SkippedTestIndicator + "!").PadRight(LongestResultIndicator),
                        _ => CommandLineResources.None.PadRight(LongestResultIndicator),
                    };
                    var failed = sourceSummary.FailedTests.ToString(CultureInfo.CurrentCulture).PadLeft(5);
                    var passed = sourceSummary.PassedTests.ToString(CultureInfo.CurrentCulture).PadLeft(5);
                    var skipped = sourceSummary.SkippedTests.ToString(CultureInfo.CurrentCulture).PadLeft(5);
                    var total = sourceSummary.TotalTests.ToString(CultureInfo.CurrentCulture).PadLeft(5);


                    var frameworkString = _targetFramework.IsNullOrEmpty()
                        ? string.Empty
                        : $"({_targetFramework})";

                    var duration = GetFormattedDurationString(sourceSummary.Duration);
                    var sourceName = Path.GetFileName(sd.Key);

                    var outputLine = string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummary,
                        resultString,
                        failed,
                        passed,
                        skipped,
                        total,
                        duration,
                        sourceName,
                        frameworkString);


                    ConsoleColor? color = null;
                    if (sourceOutcome == TestOutcome.Failed)
                    {
                        color = ConsoleColor.Red;
                    }
                    else if (sourceOutcome == TestOutcome.Passed)
                    {
                        color = ConsoleColor.Green;
                    }
                    else if (sourceOutcome == TestOutcome.Skipped)
                    {
                        color = ConsoleColor.Yellow;
                    }

                    if (color != null)
                    {
                        Output.Write(outputLine, OutputLevel.Information, color.Value);
                    }
                    else
                    {
                        Output.Write(outputLine, OutputLevel.Information);
                    }

                    Output.Information(false, CommandLineResources.TestRunSummaryAssemblyAndFramework,
                        sourceName,
                        frameworkString);
                }

                passedTests += sourceSummary.PassedTests;
                failedTests += sourceSummary.FailedTests;
                skippedTests += sourceSummary.SkippedTests;
                totalTests += sourceSummary.TotalTests;
            }
        }

        if (VerbosityLevel is Verbosity.Quiet or Verbosity.Minimal)
        {
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

            return;
        }

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
        else if (failedTests > 0 || _testRunHasErrorMessages)
        {
            Output.Error(false, CommandLineResources.TestRunFailed);
        }
        else if (totalTests > 0)
        {
            Output.Information(false, ConsoleColor.Green, CommandLineResources.TestRunSuccessful);
        }

        // Output a summary.
        if (totalTests > 0)
        {
            string totalTestsformat = (e.IsAborted || e.IsCanceled) ? CommandLineResources.TestRunSummaryForCanceledOrAbortedRun : CommandLineResources.TestRunSummaryTotalTests;
            Output.Information(false, string.Format(CultureInfo.CurrentCulture, totalTestsformat, totalTests));

            if (passedTests > 0)
            {
                Output.Information(false, ConsoleColor.Green, string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryPassedTests, passedTests));
            }
            if (failedTests > 0)
            {
                Output.Information(false, ConsoleColor.Red, string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummaryFailedTests, failedTests));
            }
            if (skippedTests > 0)
            {
                Output.Information(false, ConsoleColor.Yellow, string.Format(CultureInfo.CurrentCulture, CommandLineResources.TestRunSummarySkippedTests, skippedTests));
            }
        }

        if (totalTests > 0)
        {
            if (e.ElapsedTimeInRunningTests.Equals(TimeSpan.Zero))
            {
                EqtTrace.Info("Skipped printing test execution time on console because it looks like the test run had faced some errors");
            }
            else
            {
                PrintTimeSpan(e.ElapsedTimeInRunningTests);
            }
        }
    }
    /// <summary>
    /// Raises test run warning occurred before console logger starts listening warning events.
    /// </summary>
    /// <param name="warningMessage"></param>
    public static void RaiseTestRunWarning(string warningMessage)
    {
        Output ??= ConsoleOutput.Instance;

        Output.Warning(AppendPrefix, warningMessage);
    }

    private class MinimalTestResult
    {
        public MinimalTestResult(TestResult testResult)
        {
            TestCase = testResult.TestCase;
            Outcome = testResult.Outcome;
            StartTime = testResult.StartTime;
            EndTime = testResult.EndTime;
        }

        public TestCase TestCase { get; }
        public TestOutcome Outcome { get; }
        public DateTimeOffset StartTime { get; }
        public DateTimeOffset EndTime { get; }
    }
}
