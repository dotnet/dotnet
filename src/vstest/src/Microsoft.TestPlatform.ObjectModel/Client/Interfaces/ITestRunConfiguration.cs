// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Defines common test run configuration APIs
/// </summary>
public interface ITestRunConfiguration
{
    /// <summary>
    /// Defines the frequency of run stats test event.
    /// </summary>
    /// <remarks>
    /// Run stats change event will be raised after completion of these number of tests.
    ///
    /// Note that this event is raised asynchronously and the underlying execution process is not
    /// paused during the listener invocation. So if the event handler, you try to query the
    /// next set of results, you may get more than 'FrequencyOfRunStatsChangeEvent'.
    /// </remarks>
    long FrequencyOfRunStatsChangeEvent { get; }

    /// <summary>
    /// Returns whether the run is configured to run specific tests
    /// </summary>
    bool HasSpecificTests { get; }

    /// <summary>
    /// Returns whether the run is configured to run specific sources
    /// </summary>
    bool HasSpecificSources { get; }

    /// <summary>
    /// The specific tests for this test run if any.
    /// </summary>
    IEnumerable<TestCase>? Tests { get; }
}
