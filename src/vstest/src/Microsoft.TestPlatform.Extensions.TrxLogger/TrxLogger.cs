// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

using Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using NuGet.Frameworks;

using ObjectModelConstants = Microsoft.VisualStudio.TestPlatform.ObjectModel.Constants;
using TrxLoggerConstants = Microsoft.TestPlatform.Extensions.TrxLogger.Utility.Constants;
using TrxLoggerObjectModel = Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using TrxLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.Resources.TrxResource;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

/// <summary>
/// Logger for Generating TRX
/// </summary>
[FriendlyName(TrxLoggerConstants.FriendlyName)]
[ExtensionUri(TrxLoggerConstants.ExtensionUri)]
public class TrxLogger : ITestLoggerWithParameters
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrxLogger"/> class.
    /// </summary>
    public TrxLogger() : this(new Utilities.Helpers.FileHelper()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrxLogger"/> class.
    /// Constructor with Dependency injection. Used for unit testing.
    /// </summary>
    /// <param name="fileHelper">The file helper interface.</param>
    protected TrxLogger(IFileHelper fileHelper) : this(fileHelper, new TrxFileHelper()) { }

    internal TrxLogger(IFileHelper fileHelper, TrxFileHelper trxFileHelper)
    {
        _converter = new Converter(fileHelper, trxFileHelper);
        _trxFileHelper = trxFileHelper;
    }

    /// <summary>
    /// Cache the TRX file path
    /// </summary>
    private string? _trxFilePath;

    // The converter class
    private readonly Converter _converter;
    private ConcurrentDictionary<Guid, ITestResult>? _results;
    private ConcurrentDictionary<Guid, ITestElement>? _testElements;
    private ConcurrentDictionary<Guid, TestEntry>? _entries;

    // Caching results and inner test entries for constant time lookup for inner parents.
    private ConcurrentDictionary<Guid, ITestResult>? _innerResults;
    private ConcurrentDictionary<Guid, TestEntry>? _innerTestEntries;

    private readonly TrxFileHelper _trxFileHelper;

    /// <summary>
    /// Specifies the run level "out" messages
    /// </summary>
    private StringBuilder? _runLevelStdOut;

    // List of run level errors and warnings generated. These are logged in the Trx in the Results Summary.
    private List<RunInfo>? _runLevelErrorsAndWarnings;
    private readonly string _trxFileExtension = ".trx";

    /// <summary>
    /// Parameters dictionary for logger. Ex: {"LogFileName":"TestResults.trx"}.
    /// </summary>
    private Dictionary<string, string?>? _parametersDictionary;

    /// <summary>
    /// Gets the directory under which default trx file and test results attachments should be saved.
    /// </summary>
    private string? _testResultsDirPath;


    #region ITestLogger

    [MemberNotNullWhen(true, nameof(_testResultsDirPath), nameof(_results), nameof(_innerResults), nameof(_testElements), nameof(_entries), nameof(_innerTestEntries), nameof(_runLevelErrorsAndWarnings), nameof(_runLevelStdOut))]
    private bool IsInitialized { get; set; }

    /// <inheritdoc/>
    [MemberNotNull(nameof(_testResultsDirPath), nameof(_results), nameof(_innerResults), nameof(_testElements), nameof(_entries), nameof(_innerTestEntries), nameof(_runLevelErrorsAndWarnings), nameof(_runLevelStdOut))]
    public void Initialize(TestLoggerEvents events, string testResultsDirPath)
    {
        ValidateArg.NotNull(events, nameof(events));
        ValidateArg.NotNullOrEmpty(testResultsDirPath, nameof(testResultsDirPath));

        // Register for the events.
        events.TestRunMessage += TestMessageHandler;
        events.TestResult += TestResultHandler;
        events.TestRunComplete += TestRunCompleteHandler;

        _testResultsDirPath = testResultsDirPath;
        _results = new ConcurrentDictionary<Guid, ITestResult>();
        _innerResults = new ConcurrentDictionary<Guid, ITestResult>();
        _testElements = new ConcurrentDictionary<Guid, ITestElement>();
        _entries = new ConcurrentDictionary<Guid, TestEntry>();
        _innerTestEntries = new ConcurrentDictionary<Guid, TestEntry>();
        _runLevelErrorsAndWarnings = new List<RunInfo>();
        LoggerTestRun = null;
        TotalTestCount = 0;
        PassedTestCount = 0;
        FailedTestCount = 0;
        _runLevelStdOut = new StringBuilder();
        TestRunStartTime = DateTime.UtcNow;

        IsInitialized = true;
    }

    /// <inheritdoc/>
    [MemberNotNull(nameof(_parametersDictionary))]
    public void Initialize(TestLoggerEvents events, Dictionary<string, string?> parameters)
    {
        ValidateArg.NotNull(parameters, nameof(parameters));
        if (parameters.Count == 0)
        {
            throw new ArgumentException("No default parameters added", nameof(parameters));
        }

        var isLogFilePrefixParameterExists = parameters.TryGetValue(TrxLoggerConstants.LogFilePrefixKey, out _);
        var isLogFileNameParameterExists = parameters.TryGetValue(TrxLoggerConstants.LogFileNameKey, out _);

        if (isLogFilePrefixParameterExists && isLogFileNameParameterExists)
        {
            var trxParameterErrorMsg = TrxLoggerResources.PrefixAndNameProvidedError;

            EqtTrace.Error(trxParameterErrorMsg);
            throw new ArgumentException(trxParameterErrorMsg);
        }

        _parametersDictionary = parameters;
        Initialize(events, _parametersDictionary[DefaultLoggerParameterNames.TestRunDirectory]!);
    }
    #endregion

    #region ForTesting

    internal string GetRunLevelInformationalMessage()
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        return _runLevelStdOut.ToString();
    }

    internal List<RunInfo> GetRunLevelErrorsAndWarnings()
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        return _runLevelErrorsAndWarnings;
    }

    internal DateTime TestRunStartTime { get; private set; }

    internal TestRun? LoggerTestRun { get; private set; }

    internal int TotalTestCount { get; private set; }

    internal int PassedTestCount { get; private set; }

    internal int FailedTestCount { get; private set; }

    internal int TestResultCount
    {
        get
        {
            TPDebug.Assert(IsInitialized, "Logger is not initialized");
            return _results.Count;
        }
    }

    internal int UnitTestElementCount
    {
        get
        {
            TPDebug.Assert(IsInitialized, "Logger is not initialized");
            return _testElements.Count;
        }
    }

    internal int TestEntryCount
    {
        get
        {
            TPDebug.Assert(IsInitialized, "Logger is not initialized");
            return _entries.Count;
        }
    }

    internal TrxLoggerObjectModel.TestOutcome TestResultOutcome { get; private set; } = TrxLoggerObjectModel.TestOutcome.Passed;

    #endregion

    #region Event Handlers

    /// <summary>
    /// Called when a test message is received.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// Event args
    /// </param>
    internal void TestMessageHandler(object? sender, TestRunMessageEventArgs e)
    {
        ValidateArg.NotNull(sender, nameof(sender));
        ValidateArg.NotNull(e, nameof(e));
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        RunInfo runMessage;

        switch (e.Level)
        {
            case TestMessageLevel.Informational:
                AddRunLevelInformationalMessage(e.Message);
                break;
            case TestMessageLevel.Warning:
                runMessage = new RunInfo(e.Message, null, Environment.MachineName, TrxLoggerObjectModel.TestOutcome.Warning);
                _runLevelErrorsAndWarnings.Add(runMessage);
                break;
            case TestMessageLevel.Error:
                TestResultOutcome = TrxLoggerObjectModel.TestOutcome.Failed;
                runMessage = new RunInfo(e.Message, null, Environment.MachineName, TrxLoggerObjectModel.TestOutcome.Error);
                _runLevelErrorsAndWarnings.Add(runMessage);
                break;
            default:
                Debug.Fail("TrxLogger.TestMessageHandler: The test message level is unrecognized: {0}", e.Level.ToString());
                break;
        }
    }

    /// <summary>
    /// Called when a test result is received.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The eventArgs.
    /// </param>
    internal void TestResultHandler(object? sender, TestResultEventArgs e)
    {
        // Create test run
        if (LoggerTestRun == null)
            CreateTestRun();

        // Convert skipped test to a log entry as that is the behavior of mstest.
        if (e.Result.Outcome == ObjectModel.TestOutcome.Skipped)
            HandleSkippedTest(e.Result);

        var testType = Converter.GetTestType(e.Result);
        var executionId = Converter.GetExecutionId(e.Result);

        // Setting parent properties like parent result, parent test element, parent execution id.
        var parentExecutionId = _converter.GetParentExecutionId(e.Result);
        var parentTestResult = GetTestResult(parentExecutionId);
        var parentTestElement = parentTestResult != null ? GetTestElement(parentTestResult.Id.TestId) : null;

        // Switch to flat test results in case any parent related information is missing.
        if (parentTestResult == null || parentTestElement == null || parentExecutionId == Guid.Empty)
        {
            parentTestResult = null;
            parentTestElement = null;
            parentExecutionId = Guid.Empty;
        }

        // Create trx test element from rocksteady test case
        var testElement = GetOrCreateTestElement(executionId, parentExecutionId, testType, parentTestElement, e.Result);

        // Update test links. Test Links are updated in case of Ordered test.
        UpdateTestLinks(testElement, parentTestElement);

        // Convert the rocksteady result to trx test result
        var testResult = CreateTestResult(executionId, parentExecutionId, testType, testElement, parentTestElement, parentTestResult, e.Result);

        // Update test entries
        UpdateTestEntries(executionId, parentExecutionId, testElement, parentTestElement);

        // Set various counts (passed tests, failed tests, total tests)
        TotalTestCount++;
        if (testResult.Outcome == TrxLoggerObjectModel.TestOutcome.Failed)
        {
            TestResultOutcome = TrxLoggerObjectModel.TestOutcome.Failed;
            FailedTestCount++;
        }
        else if (testResult.Outcome == TrxLoggerObjectModel.TestOutcome.Passed)
        {
            PassedTestCount++;
        }
    }

    /// <summary>
    /// Called when a test run is completed.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// Test run complete events arguments.
    /// </param>
    internal void TestRunCompleteHandler(object? sender, TestRunCompleteEventArgs e)
    {
        // Create test run
        // If abort occurs there is no call to TestResultHandler which results in testRun not created.
        // This happens when some test aborts in the first batch of execution.
        if (LoggerTestRun == null)
            CreateTestRun();

        TPDebug.Assert(IsInitialized, "Logger is not initialized");

        XmlPersistence helper = new();
        XmlTestStoreParameters parameters = XmlTestStoreParameters.GetParameters();
        XmlElement rootElement = helper.CreateRootElement("TestRun");

        // Save runId/username/creation time etc.
        LoggerTestRun.Finished = DateTime.UtcNow;
        helper.SaveSingleFields(rootElement, LoggerTestRun, parameters);

        // Save test settings
        helper.SaveObject(LoggerTestRun.RunConfiguration, rootElement, "TestSettings", parameters);

        // Save test results
        helper.SaveIEnumerable(_results.Values, rootElement, "Results", ".", null, parameters);

        // Save test definitions
        helper.SaveIEnumerable(_testElements.Values, rootElement, "TestDefinitions", ".", null, parameters);

        // Save test entries
        helper.SaveIEnumerable(_entries.Values, rootElement, "TestEntries", ".", "TestEntry", parameters);

        // Save default categories
        List<TestListCategory> categories = new()
        {
            TestListCategory.UncategorizedResults,
            TestListCategory.AllResults
        };
        helper.SaveList(categories, rootElement, "TestLists", ".", "TestList", parameters);

        // Save summary
        if (TestResultOutcome == TrxLoggerObjectModel.TestOutcome.Passed)
        {
            TestResultOutcome = TrxLoggerObjectModel.TestOutcome.Completed;
        }

        TestResultOutcome = ChangeTestOutcomeIfNecessary(TestResultOutcome);

        List<string> errorMessages = new();
        List<CollectorDataEntry> collectorEntries = _converter.ToCollectionEntries(e.AttachmentSets, LoggerTestRun, _testResultsDirPath);
        IList<string> resultFiles = _converter.ToResultFiles(e.AttachmentSets, LoggerTestRun, _testResultsDirPath, errorMessages);

        if (errorMessages.Count > 0)
        {
            // Got some errors while attaching files, report them and set the outcome of testrun to be Error...
            TestResultOutcome = TrxLoggerObjectModel.TestOutcome.Error;
            foreach (string msg in errorMessages)
            {
                RunInfo runMessage = new(msg, null, Environment.MachineName, TrxLoggerObjectModel.TestOutcome.Error);
                _runLevelErrorsAndWarnings.Add(runMessage);
            }
        }

        TestRunSummary runSummary = new(
            TotalTestCount,
            PassedTestCount + FailedTestCount,
            PassedTestCount,
            FailedTestCount,
            TestResultOutcome,
            _runLevelErrorsAndWarnings,
            _runLevelStdOut.ToString(),
            resultFiles,
            collectorEntries);

        helper.SaveObject(runSummary, rootElement, "ResultSummary", parameters);

        ReserveTrxFilePath();
        PopulateTrxFile(_trxFilePath!, rootElement);
    }

    /// <summary>
    /// populate trx file from the xml element
    /// </summary>
    /// <param name="trxFileName">
    /// Trx full path
    /// </param>
    /// <param name="rootElement">
    /// XmlElement.
    /// </param>
    internal virtual void PopulateTrxFile(string trxFileName, XmlElement rootElement)
    {
        try
        {
            using (var fs = File.Open(trxFileName, FileMode.Truncate))
            {
                using XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings { NewLineHandling = NewLineHandling.Entitize, Indent = true });
                rootElement.OwnerDocument.Save(writer);
                writer.Flush();
            }

            string resultsFileMessage = string.Format(CultureInfo.CurrentCulture, TrxLoggerResources.TrxLoggerResultsFile, trxFileName);
            ConsoleOutput.Instance.Information(false, resultsFileMessage);
            EqtTrace.Info(resultsFileMessage);
        }
        catch (UnauthorizedAccessException fileWriteException)
        {
            ConsoleOutput.Instance.Error(false, fileWriteException.Message);
        }
    }

    /// <summary>
    /// Add run level informational message
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    private void AddRunLevelInformationalMessage(string message)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        _runLevelStdOut.AppendLine(message);
    }

    // Handle the skipped test result
    private void HandleSkippedTest(ObjectModel.TestResult rsTestResult)
    {
        TPDebug.Assert(rsTestResult.Outcome == ObjectModel.TestOutcome.Skipped, "Test Result should be skipped but it is " + rsTestResult.Outcome);

        TestCase testCase = rsTestResult.TestCase;
        string testCaseName = !string.IsNullOrEmpty(testCase.DisplayName) ? testCase.DisplayName : testCase.FullyQualifiedName;
        string message = string.Format(CultureInfo.CurrentCulture, TrxLoggerResources.MessageForSkippedTests, testCaseName);
        AddRunLevelInformationalMessage(message);
    }

    private void ReserveTrxFilePath()
    {
        for (short retries = 0; retries != short.MaxValue; retries++)
        {
            var filePath = AcquireTrxFileNamePath(out var shouldOverwrite);

            if (shouldOverwrite && File.Exists(filePath))
            {
                var overwriteWarningMsg = string.Format(CultureInfo.CurrentCulture, TrxLoggerResources.TrxLoggerResultsFileOverwriteWarning, filePath);
                ConsoleOutput.Instance.Warning(false, overwriteWarningMsg);
                EqtTrace.Warning(overwriteWarningMsg);
            }
            else
            {
                try
                {
                    using var fs = File.Open(filePath, FileMode.CreateNew);
                }
                catch (IOException)
                {
                    // File already exists, try again!
                    continue;
                }
            }

            _trxFilePath = filePath;
            return;
        }
    }

    private string AcquireTrxFileNamePath(out bool shouldOverwrite)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        TPDebug.Assert(_parametersDictionary is not null, "_parametersDictionary is null");

        shouldOverwrite = false;
        var isLogFileNameParameterExists = _parametersDictionary.TryGetValue(TrxLoggerConstants.LogFileNameKey, out string? logFileNameValue) && !logFileNameValue.IsNullOrWhiteSpace();
        var isLogFilePrefixParameterExists = _parametersDictionary.TryGetValue(TrxLoggerConstants.LogFilePrefixKey, out string? logFilePrefixValue) && !logFilePrefixValue.IsNullOrWhiteSpace();

        string? filePath = null;

        if (isLogFilePrefixParameterExists)
        {
            if (_parametersDictionary.TryGetValue(DefaultLoggerParameterNames.TargetFramework, out var framework) && framework != null)
            {
                framework = NuGetFramework.Parse(framework).GetShortFolderName();
                logFilePrefixValue = logFilePrefixValue + "_" + framework;
            }

            filePath = _trxFileHelper.GetNextTimestampFileName(_testResultsDirPath, logFilePrefixValue + _trxFileExtension, "_yyyyMMddHHmmss");
        }
        else if (isLogFileNameParameterExists)
        {
            filePath = Path.Combine(_testResultsDirPath, logFileNameValue!);
            shouldOverwrite = true;
        }

        filePath ??= SetDefaultTrxFilePath();

        var trxFileDirPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(trxFileDirPath))
        {
            Directory.CreateDirectory(trxFileDirPath!);
        }

        return filePath;
    }

    /// <summary>
    /// Returns an auto generated Trx file name under test results directory.
    /// </summary>
    private string SetDefaultTrxFilePath()
    {
        TPDebug.Assert(LoggerTestRun != null, "LoggerTestRun is null");
        TPDebug.Assert(LoggerTestRun.RunConfiguration != null, "LoggerTestRun.RunConfiguration is null");
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        var defaultTrxFileName = LoggerTestRun.RunConfiguration.RunDeploymentRootDirectory + ".trx";

        return TrxFileHelper.GetNextIterationFileName(_testResultsDirPath, defaultTrxFileName, false);
    }

    /// <summary>
    /// Creates test run.
    /// </summary>
    [MemberNotNull(nameof(LoggerTestRun))]
    private void CreateTestRun()
    {
        // Skip run creation if already exists.
        if (LoggerTestRun != null)
            return;

        Guid runId = Guid.NewGuid();
        LoggerTestRun = new TestRun(runId);

        // We cannot rely on the StartTime for the first test result
        // In case of parallel, first test result is the fastest test and not the one which started first.
        // Setting Started to DateTime.Now in Initialize will make sure we include the startup cost, which was being ignored earlier.
        // This is in parity with the way we set this.testRun.Finished
        LoggerTestRun.Started = TestRunStartTime;

        // Save default test settings
        string runDeploymentRoot = TrxFileHelper.ReplaceInvalidFileNameChars(LoggerTestRun.Name);
        TestRunConfiguration testrunConfig = new("default", _trxFileHelper);
        testrunConfig.RunDeploymentRootDirectory = runDeploymentRoot;
        LoggerTestRun.RunConfiguration = testrunConfig;
    }

    /// <summary>
    /// Gets test result from stored test results.
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns>Test result</returns>
    private ITestResult? GetTestResult(Guid executionId)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        ITestResult? testResult = null;

        if (executionId != Guid.Empty)
        {
            _results.TryGetValue(executionId, out testResult);

            if (testResult == null)
                _innerResults.TryGetValue(executionId, out testResult);
        }

        return testResult;
    }

    /// <summary>
    /// Gets test element from stored test elements.
    /// </summary>
    /// <param name="testId"></param>
    /// <returns></returns>
    private ITestElement? GetTestElement(Guid testId)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        _testElements.TryGetValue(testId, out var testElement);
        return testElement;
    }

    /// <summary>
    /// Gets or creates test element.
    /// </summary>
    /// <param name="executionId"></param>
    /// <param name="parentExecutionId"></param>
    /// <param name="testType"></param>
    /// <param name="parentTestElement"></param>
    /// <param name="rockSteadyTestCase"></param>
    /// <returns>Trx test element</returns>
    private ITestElement GetOrCreateTestElement(Guid executionId, Guid parentExecutionId, TestType testType, ITestElement? parentTestElement, ObjectModel.TestResult rockSteadyTestResult)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        ITestElement? testElement = parentTestElement;

        // For scenarios like data driven tests, test element is same as parent test element.
        if (parentTestElement != null && !parentTestElement.TestType.Equals(TrxLoggerConstants.OrderedTestType))
        {
            return testElement!;
        }

        TestCase testCase = rockSteadyTestResult.TestCase;
        Guid testId = Converter.GetTestId(testCase);

        // Scenario for inner test case when parent test element is not present.
        string? testName = testCase.DisplayName;
        var adapter = testCase.ExecutorUri.ToString();
        if (adapter.Contains(TrxLoggerConstants.MstestAdapterString) &&
            parentTestElement == null &&
            !string.IsNullOrEmpty(rockSteadyTestResult.DisplayName))
        {
            // Note: For old mstest adapters hierarchical support was not present. Thus inner result of data driven was identified using test result display name.
            // Non null test result display name means its a inner result of data driven/ordered test.
            // Changing GUID to keep supporting old mstest adapters.
            testId = Guid.NewGuid();
            testName = rockSteadyTestResult.DisplayName;
        }

        // Get test element
        testElement = GetTestElement(testId);

        // Create test element
        if (testElement == null)
        {
            testElement = Converter.ToTestElement(testId, executionId, parentExecutionId, testName!, testType, testCase);
            _testElements.TryAdd(testId, testElement);
        }

        return testElement;
    }

    /// <summary>
    /// Update test links
    /// </summary>
    /// <param name="testElement"></param>
    /// <param name="parentTestElement"></param>
    private static void UpdateTestLinks(ITestElement testElement, ITestElement? parentTestElement)
    {
        if (parentTestElement == null
            || !parentTestElement.TestType.Equals(TrxLoggerConstants.OrderedTestType))
        {
            return;
        }

        var orderedTest = (OrderedTestElement)parentTestElement;
        if (!orderedTest.TestLinks.ContainsKey(testElement.Id.Id))
        {
            orderedTest.TestLinks.Add(testElement.Id.Id, new TestLink(testElement.Id.Id, testElement.Name, testElement.Storage));
        }
    }

    /// <summary>
    /// Creates test result
    /// </summary>
    /// <param name="executionId"></param>
    /// <param name="parentExecutionId"></param>
    /// <param name="testType"></param>
    /// <param name="testElement"></param>
    /// <param name="parentTestElement"></param>
    /// <param name="parentTestResult"></param>
    /// <param name="rocksteadyTestResult"></param>
    /// <returns>Trx test result</returns>
    private ITestResult CreateTestResult(Guid executionId, Guid parentExecutionId, TestType testType,
        ITestElement testElement, ITestElement? parentTestElement, ITestResult? parentTestResult, ObjectModel.TestResult rocksteadyTestResult)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        // Create test result
        TrxLoggerObjectModel.TestOutcome testOutcome = Converter.ToOutcome(rocksteadyTestResult.Outcome);
        TPDebug.Assert(LoggerTestRun != null, "LoggerTestRun is null");
        var testResult = _converter.ToTestResult(testElement.Id.Id, executionId, parentExecutionId, testElement.Name,
            _testResultsDirPath, testType, testElement.CategoryId, testOutcome, LoggerTestRun, rocksteadyTestResult);

        // Normal result scenario
        if (parentTestResult == null)
        {
            _results.TryAdd(executionId, testResult);
            return testResult;
        }

        // Ordered test inner result scenario
        if (parentTestElement != null && parentTestElement.TestType.Equals(TrxLoggerConstants.OrderedTestType))
        {
            TPDebug.Assert(parentTestResult is TestResultAggregation, "parentTestResult is not of type TestResultAggregation");
            ((TestResultAggregation)parentTestResult).InnerResults.Add(testResult);
            _innerResults.TryAdd(executionId, testResult);
            return testResult;
        }

        // Data driven inner result scenario
        if (parentTestElement != null && parentTestElement.TestType.Equals(TrxLoggerConstants.UnitTestType))
        {
            TPDebug.Assert(parentTestResult is TestResultAggregation, "parentTestResult is not of type TestResultAggregation");
            var testResultAggregation = (TestResultAggregation)parentTestResult;
            testResultAggregation.InnerResults.Add(testResult);
            testResult.DataRowInfo = testResultAggregation.InnerResults.Count;
            testResult.ResultType = TrxLoggerConstants.InnerDataDrivenResultType;
            parentTestResult.ResultType = TrxLoggerConstants.ParentDataDrivenResultType;
            return testResult;
        }

        return testResult;
    }

    /// <summary>
    /// Update test entries
    /// </summary>
    /// <param name="executionId"></param>
    /// <param name="parentExecutionId"></param>
    /// <param name="testElement"></param>
    /// <param name="parentTestElement"></param>
    private void UpdateTestEntries(Guid executionId, Guid parentExecutionId, ITestElement testElement, ITestElement? parentTestElement)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        TestEntry te = new(testElement.Id, TestListCategory.UncategorizedResults.Id);
        te.ExecutionId = executionId;

        if (parentTestElement == null)
        {
            _entries.TryAdd(executionId, te);
        }
        else if (parentTestElement.TestType.Equals(TrxLoggerConstants.OrderedTestType))
        {
            te.ParentExecutionId = parentExecutionId;

            var parentTestEntry = GetTestEntry(parentExecutionId);
            if (parentTestEntry != null)
                parentTestEntry.TestEntries.Add(te);

            _innerTestEntries.TryAdd(executionId, te);
        }
    }

    /// <summary>
    /// Gets test entry from stored test entries.
    /// </summary>
    /// <param name="executionId"></param>
    /// <returns>Test entry</returns>
    private TestEntry? GetTestEntry(Guid executionId)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        TestEntry? testEntry = null;

        if (executionId != Guid.Empty)
        {
            _entries.TryGetValue(executionId, out testEntry);

            if (testEntry == null)
                _innerTestEntries.TryGetValue(executionId, out testEntry);
        }

        return testEntry;
    }

    private TrxLoggerObjectModel.TestOutcome ChangeTestOutcomeIfNecessary(TrxLoggerObjectModel.TestOutcome outcome)
    {
        TPDebug.Assert(IsInitialized, "Logger is not initialized");
        TPDebug.Assert(_parametersDictionary is not null, "_parametersDictionary is null");

        // If no tests discovered/executed and TreatNoTestsAsError was set to True
        // We will return ResultSummary as Failed
        // Note : we only send the value of TreatNoTestsAsError if it is "True"
        if (TotalTestCount == 0 && _parametersDictionary.ContainsKey(ObjectModelConstants.TreatNoTestsAsError))
        {
            outcome = TrxLoggerObjectModel.TestOutcome.Failed;
        }

        return outcome;
    }

    #endregion
}
