// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

/// <summary>
/// Summary of test results per source.
/// </summary>
internal class SourceSummary
{
    /// <summary>
    /// Total tests of a test run.
    /// </summary>
    public int TotalTests { get; set; }

    /// <summary>
    /// Passed tests of a test run.
    /// </summary>
    public int PassedTests { get; set; }

    /// <summary>
    /// Failed tests of a test run.
    /// </summary>
    public int FailedTests { get; set; }

    /// <summary>
    /// Skipped tests of a test run.
    /// </summary>
    public int SkippedTests { get; set; }

    /// <summary>
    /// Duration of the test run.
    /// </summary>
    public TimeSpan Duration { get; set; }
}
