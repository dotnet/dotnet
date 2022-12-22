// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.TestPlatform.Extensions.TrxLogger.Utility;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using TrxLoggerResources = Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger.Resources.TrxResource;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// Class to uniquely identify test results
/// </summary>
internal sealed class TestResultId : IXmlTestStore
{
    private readonly Guid _runId;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestResultId"/> class.
    /// </summary>
    /// <param name="runId">
    /// The run id.
    /// </param>
    /// <param name="executionId">
    /// The execution id.
    /// </param>
    /// <param name="parentExecutionId">
    /// The parent execution id.
    /// </param>
    /// <param name="testId">
    /// The test id.
    /// </param>
    public TestResultId(Guid runId, Guid executionId, Guid parentExecutionId, Guid testId)
    {
        _runId = runId;
        ExecutionId = executionId;
        ParentExecutionId = parentExecutionId;
        TestId = testId;
    }

    /// <summary>
    /// Gets the execution id.
    /// </summary>
    public Guid ExecutionId { get; }

    /// <summary>
    /// Gets the parent execution id.
    /// </summary>
    public Guid ParentExecutionId { get; }

    /// <summary>
    /// Gets the test id.
    /// </summary>
    public Guid TestId { get; }


    #region Overrides

    /// <summary>
    /// Override function for Equals
    /// </summary>
    /// <param name="obj">
    /// The object to compare
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is TestResultId tmpId && _runId.Equals(tmpId._runId) && ExecutionId.Equals((object)tmpId.ExecutionId);
    }

    /// <summary>
    /// Override function for GetHashCode.
    /// </summary>
    /// <returns>
    /// The <see cref="int"/>.
    /// </returns>
    public override int GetHashCode()
    {
        return _runId.GetHashCode() ^ ExecutionId.GetHashCode();
    }

    /// <summary>
    /// Override function for ToString.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public override string ToString()
    {
        return ExecutionId.ToString("B");
    }
    #endregion

    #region IXmlTestStore Members

    /// <summary>
    /// Saves the class under the XmlElement..
    /// </summary>
    /// <param name="element">
    /// The parent xml.
    /// </param>
    /// <param name="parameters">
    /// The parameter
    /// </param>
    public void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence helper = new();

        if (ExecutionId != Guid.Empty)
            helper.SaveGuid(element, "@executionId", ExecutionId);
        if (ParentExecutionId != Guid.Empty)
            helper.SaveGuid(element, "@parentExecutionId", ParentExecutionId);

        helper.SaveGuid(element, "@testId", TestId);
    }

    #endregion
}

/// <summary>
/// The test result error info class.
/// </summary>
internal sealed class TestResultErrorInfo : IXmlTestStore
{
    [StoreXmlSimpleField("Message", "")]
    private string? _message;

    [StoreXmlSimpleField("StackTrace", "")]
    private string? _stackTrace;


    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string? Message
    {
        get { return _message; }
        set { _message = value; }
    }

    /// <summary>
    /// Gets or sets the stack trace.
    /// </summary>
    public string? StackTrace
    {
        get { return _stackTrace; }
        set { _stackTrace = value; }
    }

    #region IXmlTestStore Members

    /// <summary>
    /// Saves the class under the XmlElement..
    /// </summary>
    /// <param name="element">
    /// The parent xml.
    /// </param>
    /// <param name="parameters">
    /// The parameter
    /// </param>
    public void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence.SaveUsingReflection(element, this, typeof(TestResultErrorInfo), parameters);
    }

    #endregion
}

/// <summary>
/// Class for test result.
/// </summary>
internal class TestResult : ITestResult, IXmlTestStore
{
    private readonly string _resultName;
    private string? _stdOut;
    private string? _stdErr;
    private string? _debugTrace;
    private TimeSpan _duration;
    private readonly TestType _testType;
    private TestRun? _testRun;
    private TestResultErrorInfo? _errorInfo;
    private readonly TestListCategoryId _categoryId;
    private ArrayList _textMessages;
    private readonly TrxFileHelper _trxFileHelper;

    /// <summary>
    /// Paths to test result files, relative to the test results folder, sorted in increasing order
    /// </summary>
    private readonly SortedList<string, object?> _resultFiles = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Information provided by data collectors for the test case
    /// </summary>
    private readonly List<CollectorDataEntry> _collectorDataEntries = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestResult"/> class.
    /// </summary>
    /// <param name="computerName">
    /// The computer name.
    /// </param>
    /// <param name="runId">
    /// The run id.
    /// </param>
    /// <param name="test">
    /// The test.
    /// </param>
    /// <param name="outcome">
    /// The outcome.
    /// </param>
    public TestResult(
        Guid runId,
        Guid testId,
        Guid executionId,
        Guid parentExecutionId,
        string resultName,
        string computerName,
        TestOutcome outcome,
        TestType testType,
        TestListCategoryId testCategoryId,
        TrxFileHelper trxFileHelper)
    {
        TPDebug.Assert(computerName != null, "computername is null");
        TPDebug.Assert(!Guid.Empty.Equals(executionId), "ExecutionId is empty");
        TPDebug.Assert(!Guid.Empty.Equals(testId), "TestId is empty");

        _textMessages = new ArrayList();
        DataRowInfo = -1;

        Id = new TestResultId(runId, executionId, parentExecutionId, testId);
        _resultName = resultName;
        _testType = testType;
        ComputerName = computerName;
        Outcome = outcome;
        _categoryId = testCategoryId;
        RelativeTestResultsDirectory = TestRunDirectories.GetRelativeTestResultsDirectory(executionId);
        _trxFileHelper = trxFileHelper;
    }

    /// <summary>
    /// Gets or sets the end time.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets or sets the start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public TimeSpan Duration
    {
        get { return _duration; }

        set
        {
            // On some hardware the Stopwatch.Elapsed can return a negative number.  This tends
            // to happen when the duration of the test is very short and it is hardware dependent
            // (seems to happen most on virtual machines or machines with AMD processors).  To prevent
            // reporting a negative duration, use TimeSpan.Zero when the elapsed time is less than zero.
            EqtTrace.WarningIf(value < TimeSpan.Zero, "TestResult.Duration: The duration is being set to {0}.  Since the duration is negative the duration will be updated to zero.", value);
            _duration = value > TimeSpan.Zero ? value : TimeSpan.Zero;
        }
    }

    /// <summary>
    /// Gets the computer name.
    /// </summary>
    public string ComputerName { get; }

    /// <summary>
    /// Gets or sets the outcome.
    /// </summary>
    public TestOutcome Outcome { get; set; }


    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    public TestResultId Id { get; internal set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string ErrorMessage
    {
        get { return _errorInfo?.Message ?? string.Empty; }
        set
        {
            _errorInfo ??= new TestResultErrorInfo();

            _errorInfo.Message = value;
        }
    }

    /// <summary>
    /// Gets or sets the error stack trace.
    /// </summary>
    public string ErrorStackTrace
    {
        get { return _errorInfo?.StackTrace ?? string.Empty; }

        set
        {
            _errorInfo ??= new TestResultErrorInfo();

            _errorInfo.StackTrace = value;
        }
    }

    /// <summary>
    /// Gets the text messages.
    /// </summary>
    /// <remarks>
    /// Additional information messages from TestTextResultMessage, e.g. generated by TestOutcome.WriteLine.
    /// Avoid using this property in the following way: for (int i=0; i&lt;prop.Length; i++) { ... prop[i] ...}
    /// </remarks>
    [NotNull]
    public string[]? TextMessages
    {
        get { return (string[])_textMessages.ToArray(typeof(string)); }

        set
        {
            if (value != null)
                _textMessages = new ArrayList(value);
            else
                _textMessages.Clear();
        }
    }

    /// <summary>
    /// Gets or sets the standard out.
    /// </summary>
    public string StdOut
    {
        get { return _stdOut ?? string.Empty; }
        set { _stdOut = value; }
    }

    /// <summary>
    /// Gets or sets the standard err.
    /// </summary>
    public string StdErr
    {
        get { return _stdErr ?? string.Empty; }
        set { _stdErr = value; }
    }

    /// <summary>
    /// Gets or sets the debug trace.
    /// </summary>
    public string DebugTrace
    {
        get { return _debugTrace ?? string.Empty; }
        set { _debugTrace = value; }
    }

    /// <summary>
    /// Gets the path to the test results directory
    /// </summary>
    public string TestResultsDirectory
    {
        get
        {
            if (_testRun == null)
            {
                Debug.Fail("'m_testRun' is null");
                throw new InvalidOperationException(TrxLoggerResources.Common_MissingRunInResult);
            }

            return _testRun.GetResultFilesDirectory(this);
        }
    }

    /// <summary>
    /// Gets the directory containing the test result files, relative to the root results directory
    /// </summary>
    public string RelativeTestResultsDirectory { get; }

    /// <summary>
    /// Gets or sets the data row info.
    /// </summary>
    public int DataRowInfo { get; set; }

    /// <summary>
    /// Gets or sets the result type.
    /// </summary>
    public string? ResultType { get; set; }


    #region Overrides
    public override bool Equals(object? obj)
    {
        if (obj is not TestResult trm)
        {
            return false;
        }
        TPDebug.Assert(Id != null, "id is null");
        TPDebug.Assert(trm.Id != null, "test result message id is null");
        return Id.Equals(trm.Id);
    }

    public override int GetHashCode()
    {
        TPDebug.Assert(Id != null, "id is null");
        return Id.GetHashCode();
    }

    #endregion

    /// <summary>
    /// Helper function to add a text message info to the test result
    /// </summary>
    /// <param name="text">Message to be added</param>
    public void AddTextMessage(string text)
    {
        EqtAssert.ParameterNotNull(text, nameof(text));
        _textMessages.Add(text);
    }

    /// <summary>
    /// Sets the test run the test was executed in
    /// </summary>
    /// <param name="testRun">The test run the test was executed in</param>
    internal virtual void SetTestRun(TestRun testRun)
    {
        TPDebug.Assert(testRun != null, "'testRun' is null");
        _testRun = testRun;
    }

    /// <summary>
    /// Adds result files to the <see cref="_resultFiles"/> collection
    /// </summary>
    /// <param name="resultFileList">Paths to the result files</param>
    internal void AddResultFiles(IEnumerable<string> resultFileList)
    {
        TPDebug.Assert(resultFileList != null, "'resultFileList' is null");

        string testResultsDirectory = TestResultsDirectory;
        foreach (string resultFile in resultFileList)
        {
            TPDebug.Assert(!string.IsNullOrEmpty(resultFile), "'resultFile' is null or empty");
            TPDebug.Assert(resultFile.Trim() == resultFile, "'resultFile' has whitespace at the ends");

            _resultFiles[TrxFileHelper.MakePathRelative(resultFile, testResultsDirectory)] = null;
        }
    }

    /// <summary>
    /// Adds collector data entries to the <see cref="_collectorDataEntries"/> collection
    /// </summary>
    /// <param name="collectorDataEntryList">The collector data entry to add</param>
    internal void AddCollectorDataEntries(IEnumerable<CollectorDataEntry> collectorDataEntryList)
    {
        TPDebug.Assert(collectorDataEntryList != null, "'collectorDataEntryList' is null");

        string testResultsDirectory = TestResultsDirectory;
        foreach (CollectorDataEntry collectorDataEntry in collectorDataEntryList)
        {
            TPDebug.Assert(collectorDataEntry != null, "'collectorDataEntry' is null");
            TPDebug.Assert(!_collectorDataEntries.Contains(collectorDataEntry), "The collector data entry already exists in the collection");

            _collectorDataEntries.Add(collectorDataEntry.Clone(testResultsDirectory, false));
        }
    }


    #region IXmlTestStore Members

    /// <summary>
    /// Saves the class under the XmlElement..
    /// </summary>
    /// <param name="element">
    /// The parent xml.
    /// </param>
    /// <param name="parameters">
    /// The parameter
    /// </param>
    public virtual void Save(System.Xml.XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence helper = new();

        helper.SaveObject(Id, element, ".", parameters);
        helper.SaveSimpleField(element, "@testName", _resultName, string.Empty);
        helper.SaveSimpleField(element, "@computerName", ComputerName, string.Empty);
        helper.SaveSimpleField(element, "@duration", _duration, default(TimeSpan));
        helper.SaveSimpleField(element, "@startTime", StartTime, default(DateTime));
        helper.SaveSimpleField(element, "@endTime", EndTime, default(DateTime));
        helper.SaveGuid(element, "@testType", _testType.Id);

        if (_stdOut != null)
            _stdOut = _stdOut.Trim();

        if (_stdErr != null)
            _stdErr = _stdErr.Trim();

        helper.SaveSimpleField(element, "@outcome", Outcome, default(TestOutcome));
        helper.SaveSimpleField(element, "Output/StdOut", _stdOut, string.Empty);
        helper.SaveSimpleField(element, "Output/StdErr", _stdErr, string.Empty);
        helper.SaveSimpleField(element, "Output/DebugTrace", _debugTrace, string.Empty);
        helper.SaveObject(_errorInfo, element, "Output/ErrorInfo", parameters);
        helper.SaveGuid(element, "@testListId", _categoryId.Id);
        helper.SaveIEnumerable(_textMessages, element, "Output/TextMessages", ".", "Message", parameters);
        helper.SaveSimpleField(element, "@relativeResultsDirectory", RelativeTestResultsDirectory, null);
        helper.SaveIEnumerable(_resultFiles.Keys, element, "ResultFiles", "@path", "ResultFile", parameters);
        helper.SaveIEnumerable(_collectorDataEntries, element, "CollectorDataEntries", ".", "Collector", parameters);

        if (DataRowInfo >= 0)
            helper.SaveSimpleField(element, "@dataRowInfo", DataRowInfo, -1);

        if (!string.IsNullOrEmpty(ResultType))
            helper.SaveSimpleField(element, "@resultType", ResultType, string.Empty);
    }

    #endregion
}
