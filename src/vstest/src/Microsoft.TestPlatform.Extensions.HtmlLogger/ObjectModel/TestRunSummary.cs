// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.ObjectModel;

/// <summary>
/// Test run summary collects the relevant summary information.
/// </summary>
[DataContract]
public class TestRunSummary
{
    /// <summary>
    /// Indicates the pass percentage
    /// </summary>
    [DataMember] public int PassPercentage { get; set; }

    /// <summary>
    /// Total test run time.
    /// </summary>
    [DataMember] public string? TotalRunTime { get; set; }

    /// <summary>
    /// Total tests of a test run.
    /// </summary>
    [DataMember] public int TotalTests { get; set; }

    /// <summary>
    /// Passed tests of test run.
    /// </summary>
    [DataMember] public int PassedTests { get; set; }

    /// <summary>
    /// Failed Tests of test run.
    /// </summary>
    [DataMember] public int FailedTests { get; set; }

    /// <summary>
    /// Skipped Tests of test run.
    /// </summary>
    [DataMember] public int SkippedTests { get; set; }
}
