// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.Extensions.HtmlLogger.ObjectModel;

/// <summary>
/// Stores the list of failed results and list of all results corresponding to the source.
/// </summary>
[DataContract]
public class TestResultCollection
{
    private readonly string _source;

    /// <summary>
    /// Guards <see cref="ResultList"/> and <see cref="FailedResultList"/>, which are appended to
    /// from multiple threads while tests execute in parallel.
    /// </summary>
    private readonly object _resultsLock = new();

    public TestResultCollection(string source) => _source = source ?? throw new ArgumentNullException(nameof(source));

    /// <summary>
    /// Source of the test dll.
    /// </summary>
    [DataMember]
    public string Source
    {
        get => _source;
        private set { }
    }

    /// <summary>
    /// Hash id of source.
    /// </summary>
    [DataMember]
    public int Id
    {
        get => _source.GetHashCode();
        private set { }
    }

    /// <summary>
    /// List of test results.
    /// </summary>
    [DataMember] public List<TestResult>? ResultList { get; set; }

    /// <summary>
    /// List of failed test results.
    /// </summary>
    [DataMember] public List<TestResult>? FailedResultList { get; set; }

    /// <summary>
    /// Adds a result to <see cref="ResultList"/>, and to <see cref="FailedResultList"/> when the
    /// result is a failure. Safe to call concurrently from multiple threads.
    /// </summary>
    internal void AddResult(TestResult testResult, bool isFailed)
    {
        lock (_resultsLock)
        {
            if (isFailed)
            {
                FailedResultList ??= new List<TestResult>();
                FailedResultList.Add(testResult);
            }

            ResultList ??= new List<TestResult>();
            ResultList.Add(testResult);
        }
    }
}
