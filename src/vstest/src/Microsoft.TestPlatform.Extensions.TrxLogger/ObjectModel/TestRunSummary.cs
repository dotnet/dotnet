// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Xml;

using Microsoft.TestPlatform.Extensions.TrxLogger.XML;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;

/// <summary>
/// XML object for saving test summary - Outcome and counts (passed, failed etc)
/// </summary>
internal class TestRunSummary : IXmlTestStore
{
    [StoreXmlSimpleField("Counters/@total")]
    private readonly int _totalTests;
    [StoreXmlSimpleField("Counters/@executed")]
    private readonly int _executedTests;
    [StoreXmlSimpleField("Counters/@passed")]
    private readonly int _passedTests;
    [StoreXmlSimpleField("Counters/@failed")]
    private readonly int _failedTests;
    [StoreXmlSimpleField("Counters/@error")]
    private readonly int _errorTests;
    [StoreXmlSimpleField("Counters/@timeout")]
    private readonly int _timeoutTests;
    [StoreXmlSimpleField("Counters/@aborted")]
    private readonly int _abortedTests;
    [StoreXmlSimpleField("Counters/@inconclusive")]
    private readonly int _inconclusiveTests;
    [StoreXmlSimpleField("Counters/@passedButRunAborted")]
    private readonly int _passedButRunAborted;
    [StoreXmlSimpleField("Counters/@notRunnable")]
    private readonly int _notRunnable;
    [StoreXmlSimpleField("Counters/@notExecuted")]
    private readonly int _notExecuted;
    [StoreXmlSimpleField("Counters/@disconnected")]
    private readonly int _disconnected;
    [StoreXmlSimpleField("Counters/@warning")]
    private readonly int _warning;
    [StoreXmlSimpleField("Counters/@completed")]
    private readonly int _completed;
    [StoreXmlSimpleField("Counters/@inProgress")]
    private readonly int _inProgress;
    [StoreXmlSimpleField("Counters/@pending")]
    private readonly int _pending;
    [StoreXmlSimpleField("@outcome")]
    private readonly TestOutcome _outcome = TestOutcome.Pending;
    [StoreXmlSimpleField("Output/StdOut", "")]
    private readonly string _stdOut = string.Empty;

    private readonly List<RunInfo> _runLevelErrorsAndWarnings;

    private readonly List<CollectorDataEntry> _collectorDataEntries;

    private readonly IList<string> _resultFiles;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunSummary"/> class.
    /// </summary>
    /// <param name="total">
    /// The total number of tests discover in this run.
    /// </param>
    /// <param name="executed">
    /// The executed tests.
    /// </param>
    /// <param name="pass">
    /// The pass tests.
    /// </param>
    /// <param name="fail">
    /// The fail tests.
    /// </param>
    /// <param name="outcome">
    /// The outcome.
    /// </param>
    /// <param name="runMessages">
    /// The run messages.
    /// </param>
    /// <param name="stdOut">
    /// The standard out.
    /// </param>
    /// <param name="resultFiles">
    /// The result files.
    /// </param>
    /// <param name="dataCollectors">
    /// The data collectors.
    /// </param>
    public TestRunSummary(
        int total,
        int executed,
        int pass,
        int fail,
        TestOutcome outcome,
        List<RunInfo> runMessages,
        string stdOut,
        IList<string> resultFiles,
        List<CollectorDataEntry> dataCollectors)
    {
        _totalTests = total;
        _executedTests = executed;
        _passedTests = pass;
        _failedTests = fail;
        int countForNonExistingResults = 0; // if below values are assigned constants 0, compiler gives warning CS0414
        _abortedTests = countForNonExistingResults;
        _errorTests = countForNonExistingResults;
        _timeoutTests = countForNonExistingResults;
        _inconclusiveTests = countForNonExistingResults;
        _passedButRunAborted = countForNonExistingResults;
        _notRunnable = countForNonExistingResults;
        _notExecuted = countForNonExistingResults;
        _disconnected = countForNonExistingResults;
        _warning = countForNonExistingResults;
        _completed = countForNonExistingResults;
        _inProgress = countForNonExistingResults;
        _pending = countForNonExistingResults;

        _outcome = outcome;
        _stdOut = stdOut;

        _runLevelErrorsAndWarnings = runMessages;
        _resultFiles = resultFiles;
        _collectorDataEntries = dataCollectors;
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
    public void Save(XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence helper = new();
        helper.SaveSingleFields(element, this, parameters);
        helper.SaveIEnumerable(_runLevelErrorsAndWarnings, element, "RunInfos", ".", "RunInfo", parameters);
        helper.SaveIEnumerable(_resultFiles, element, "ResultFiles", "@path", "ResultFile", parameters);
        helper.SaveIEnumerable(_collectorDataEntries, element, "CollectorDataEntries", ".", "Collector", parameters);
    }

    #endregion
}
