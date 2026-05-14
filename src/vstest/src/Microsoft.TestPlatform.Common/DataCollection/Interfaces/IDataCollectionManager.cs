// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;

/// <summary>
/// Defines the Data Collection Manager for Data Collectors.
/// </summary>
internal interface IDataCollectionManager : IDisposable
{
    /// <summary>
    /// Loads and initializes data collector plugins.
    /// </summary>
    /// <param name="settingsXml">Run Settings which has DataCollector configuration.</param>
    /// <returns>Environment variables.</returns>
    IDictionary<string, string?> InitializeDataCollectors(string settingsXml);

    /// <summary>
    /// Raises TestCaseStart event to all data collectors configured for run.
    /// </summary>
    /// <param name="testCaseStartEventArgs">TestCaseStart event.</param>
    void TestCaseStarted(TestCaseStartEventArgs testCaseStartEventArgs);

    /// <summary>
    /// Raises TestCaseEnd event to all data collectors configured for run.
    /// </summary>
    /// <param name="testCaseEndEventArgs">
    /// The test Case End Event Args.
    /// </param>
    /// <returns>
    /// Collection of  testCase attachmentSet.
    /// </returns>
    Collection<AttachmentSet> TestCaseEnded(TestCaseEndEventArgs testCaseEndEventArgs);

    /// <summary>
    /// Raises TestHostLaunched event to all data collectors configured for run.
    /// </summary>
    /// <param name="processId">
    /// Process ID of test host.
    /// </param>
    void TestHostLaunched(int processId);

    /// <summary>
    /// Raises SessionStart event to all data collectors configured for run.
    /// </summary>
    /// <param name="sessionStartEventArgs">
    /// The session start Event Args.
    /// </param>
    /// <returns>boolean value specifying whether test case events are subscribed by datacollectors. Based on this execution process will decide whether to send TestCaseStart and TestCaseEnd events to dataCollector process.</returns>
    bool SessionStarted(SessionStartEventArgs sessionStartEventArgs);

    /// <summary>
    /// Raises SessionEnd event to all data collectors configured for run.
    /// </summary>
    /// <param name="isCancelled">
    /// Boolean to specify is the test run is canceled or not.
    /// </param>
    /// <returns>
    /// Collection of session attachmentSet.
    /// </returns>
    Collection<AttachmentSet> SessionEnded(bool isCancelled);

    /// <summary>
    /// Return a collections of the invoked data collectors
    /// </summary>
    /// <returns>Collection of data collectors.</returns>
    Collection<InvokedDataCollector> GetInvokedDataCollectors();
}
