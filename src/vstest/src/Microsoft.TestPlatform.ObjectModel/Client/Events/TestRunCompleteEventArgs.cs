// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Event arguments used when a test run has completed.
/// </summary>
[DataContract]
public class TestRunCompleteEventArgs : EventArgs
{
    // We have more than one ctor for backward-compatibility reason but we don't want to add dependency on Newtosoft([JsonConstructor])
    // We want to fallback to the non-public default constructor https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_ConstructorHandling.htm during deserialization
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private TestRunCompleteEventArgs()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        AttachmentSets = new Collection<AttachmentSet>();
        InvokedDataCollectors = new Collection<InvokedDataCollector>();
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="stats">The final stats for the test run. This parameter is only set for communications between the test host and the clients (like VS)</param>
    /// <param name="isCanceled">Specifies whether the test run is canceled.</param>
    /// <param name="isAborted">Specifies whether the test run is aborted.</param>
    /// <param name="error">Specifies the error encountered during the execution of the test run.</param>
    /// <param name="attachmentSets">Attachment sets associated with the run.</param>
    /// <param name="elapsedTime">Time elapsed in just running tests</param>
    public TestRunCompleteEventArgs(
        ITestRunStatistics? stats,
        bool isCanceled,
        bool isAborted,
        Exception? error,
        Collection<AttachmentSet>? attachmentSets,
        TimeSpan elapsedTime)
        : this(
              stats,
              isCanceled,
              isAborted,
              error,
              attachmentSets,
              null,
              elapsedTime)
    { }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="stats">The final stats for the test run. This parameter is only set for communications between the test host and the clients (like VS)</param>
    /// <param name="isCanceled">Specifies whether the test run is canceled.</param>
    /// <param name="isAborted">Specifies whether the test run is aborted.</param>
    /// <param name="error">Specifies the error encountered during the execution of the test run.</param>
    /// <param name="attachmentSets">Attachment sets associated with the run.</param>
    /// <param name="InvokedDataCollectors">Invoked data collectors</param>
    /// <param name="elapsedTime">Time elapsed in just running tests</param>
    public TestRunCompleteEventArgs(
        ITestRunStatistics? stats,
        bool isCanceled,
        bool isAborted,
        Exception? error,
        Collection<AttachmentSet>? attachmentSets,
        Collection<InvokedDataCollector>? invokedDataCollectors,
        TimeSpan elapsedTime)
    {
        TestRunStatistics = stats;
        IsCanceled = isCanceled;
        IsAborted = isAborted;
        Error = error;
        AttachmentSets = attachmentSets ?? new Collection<AttachmentSet>(); // Ensuring attachmentSets are not null, so that new attachmentSets can be combined whenever required.
        InvokedDataCollectors = invokedDataCollectors ?? new Collection<InvokedDataCollector>(); // Ensuring that invoked data collectors are not null.
        ElapsedTimeInRunningTests = elapsedTime;

        DiscoveredExtensions = new Dictionary<string, HashSet<string>>();
    }

    /// <summary>
    /// Gets the statistics on the state of the test run.
    /// </summary>
    [DataMember]
    public ITestRunStatistics? TestRunStatistics { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the test run is canceled or not.
    /// </summary>
    [DataMember]
    public bool IsCanceled { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the test run is aborted.
    /// </summary>
    [DataMember]
    public bool IsAborted { get; private set; }

    /// <summary>
    /// Gets the error encountered during the execution of the test run. Null if there is no error.
    /// </summary>
    [DataMember]
    public Exception? Error { get; private set; }

    /// <summary>
    /// Gets the attachment sets associated with the test run.
    /// </summary>
    [DataMember]
    public Collection<AttachmentSet> AttachmentSets { get; private set; }

    /// <summary>
    /// Gets the invoked data collectors for the test session.
    /// </summary>
    [DataMember]
    public Collection<InvokedDataCollector> InvokedDataCollectors { get; private set; }

    /// <summary>
    /// Gets the time elapsed in just running the tests.
    /// Value is set to TimeSpan.Zero in case of any error.
    /// </summary>
    [DataMember]
    public TimeSpan ElapsedTimeInRunningTests { get; private set; }

    /// <summary>
    /// Get or Sets the Metrics
    /// </summary>
    [DataMember]
    public IDictionary<string, object>? Metrics { get; set; }

    /// <summary>
    /// Gets or sets the collection of discovered extensions.
    /// </summary>
    [DataMember]
    public Dictionary<string, HashSet<string>>? DiscoveredExtensions { get; set; }
}
