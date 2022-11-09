// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;

/// <summary>
/// The ProxyDataCollectionManager interface.
/// </summary>
public interface IProxyDataCollectionManager : IDisposable
{
    /// <summary>
    /// Initializes proxy datacollection manager.
    /// </summary>
    void Initialize();

    /// <summary>
    /// The settings xml
    /// </summary>
    string? SettingsXml { get; }

    /// <summary>
    /// List of test sources
    /// </summary>
    IEnumerable<string> Sources { get; }

    /// <summary>
    /// Invoked before starting of test run
    /// </summary>
    /// <param name="resetDataCollectors">
    /// Bool value to reset and reinitialize datacollectors.
    /// </param>
    /// <param name="isRunStartingNow">
    /// Bool value to specify if the test execution has started or not.
    /// </param>
    /// <param name="runEventsHandler">
    /// The run Events Handler.
    /// </param>
    /// <returns>
    /// BeforeTestRunStartResult object
    /// </returns>
    DataCollectionParameters BeforeTestRunStart(
        bool resetDataCollectors,
        bool isRunStartingNow,
        ITestMessageEventHandler runEventsHandler);

    /// <summary>
    /// Invoked after ending of test run
    /// </summary>
    /// <param name="isCanceled">
    /// The is Canceled.
    /// </param>
    /// <param name="runEventsHandler">
    /// The run Events Handler.
    /// </param>
    /// <returns>
    /// The <see cref="DataCollectionResult"/>.
    /// </returns>
    DataCollectionResult AfterTestRunEnd(bool isCanceled, ITestMessageEventHandler runEventsHandler);

    /// <summary>
    /// Invoked after initialization of test host
    /// </summary>
    /// <param name="processId">
    /// Process ID of test host
    /// </param>
    void TestHostLaunched(int processId);
}
