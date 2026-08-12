// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.ObjectModel;

/// <summary>
/// It stores the all relevant information of the test run.
/// </summary>
[DataContract]
[SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "Part of the public API")]
public sealed class TestRunDetails
{
    /// <summary>
    /// Guards the run level message lists, which are created on first use, and
    /// <see cref="ResultCollectionList"/>. All of them are mutated from multiple threads while
    /// tests execute in parallel.
    /// </summary>
    private readonly object _syncLock = new();

    /// <summary>
    /// Test run summary of all test results.
    /// </summary>
    [DataMember] public TestRunSummary? Summary { get; set; }

    /// <summary>
    /// List of informational run level messages.
    /// </summary>
    [DataMember] public List<string>? RunLevelMessageInformational;

    /// <summary>
    /// List of error and warning messages.
    /// </summary>
    [DataMember] public List<string>? RunLevelMessageErrorAndWarning;

    /// <summary>
    /// List of all the results
    /// </summary>
    [DataMember] public List<TestResultCollection>? ResultCollectionList = new();

    /// <summary>
    /// Adds an informational run level message. Safe to call concurrently from multiple threads.
    /// </summary>
    internal void AddInformationalMessage(string message)
    {
        lock (_syncLock)
        {
            RunLevelMessageInformational ??= new List<string>();
            RunLevelMessageInformational.Add(message);
        }
    }

    /// <summary>
    /// Adds an error or warning run level message. Safe to call concurrently from multiple threads.
    /// </summary>
    internal void AddErrorOrWarningMessage(string message)
    {
        lock (_syncLock)
        {
            RunLevelMessageErrorAndWarning ??= new List<string>();
            RunLevelMessageErrorAndWarning.Add(message);
        }
    }

    /// <summary>
    /// Adds a result collection. Safe to call concurrently from multiple threads.
    /// </summary>
    internal void AddResultCollection(TestResultCollection resultCollection)
    {
        lock (_syncLock)
        {
            ResultCollectionList ??= new List<TestResultCollection>();
            ResultCollectionList.Add(resultCollection);
        }
    }
}
