// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// The test run changed event args that provides the test results available.
/// </summary>
[DataContract]
public class TestRunChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestRunChangedEventArgs"/> class.
    /// </summary>
    /// <param name="stats"> The stats. </param>
    /// <param name="newTestResults"> The new test results. </param>
    /// <param name="activeTests"> The active tests. </param>
    public TestRunChangedEventArgs(ITestRunStatistics? stats, IEnumerable<TestResult>? newTestResults, IEnumerable<TestCase>? activeTests)
    {
        TestRunStatistics = stats;
        NewTestResults = newTestResults;
        ActiveTests = activeTests;
    }

    /// <summary>
    /// Gets the new test results.
    /// </summary>
    [DataMember]
    public IEnumerable<TestResult>? NewTestResults { get; private set; }

    /// <summary>
    /// Gets the test run statistics.
    /// </summary>
    [DataMember]
    public ITestRunStatistics? TestRunStatistics { get; private set; }

    /// <summary>
    /// Gets the active tests.
    /// </summary>
    [DataMember]
    public IEnumerable<TestCase>? ActiveTests { get; private set; }
}
