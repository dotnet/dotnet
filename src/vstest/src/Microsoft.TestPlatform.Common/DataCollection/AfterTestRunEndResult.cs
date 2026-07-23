// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollection;

/// <summary>
/// Payload object that is used to exchange data between datacollector process and runner process.
/// </summary>
[DataContract]
public class AfterTestRunEndResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AfterTestRunEndResult"/> class.
    /// </summary>
    /// <param name="attachmentSets">
    /// The collection of attachment sets.
    /// </param>
    /// <param name="metrics">
    /// The metrics.
    /// </param>
    public AfterTestRunEndResult(Collection<AttachmentSet> attachmentSets, IDictionary<string, object> metrics)
        : this(attachmentSets, new Collection<InvokedDataCollector>(), metrics)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AfterTestRunEndResult"/> class.
    /// </summary>
    /// <param name="attachmentSets">
    /// The collection of attachment sets.
    /// </param>
    /// <param name="invokedDataCollectors">
    /// The collection of the DataCollectors invoked during test session
    /// </param>
    /// <param name="metrics">
    /// The metrics.
    /// </param>
    public AfterTestRunEndResult(Collection<AttachmentSet> attachmentSets,
        Collection<InvokedDataCollector>? invokedDataCollectors,
        IDictionary<string, object> metrics)
    {
        AttachmentSets = attachmentSets;
        InvokedDataCollectors = invokedDataCollectors;
        Metrics = metrics;
    }

    [DataMember]
    public Collection<AttachmentSet> AttachmentSets { get; private set; }

    [DataMember]
    public Collection<InvokedDataCollector>? InvokedDataCollectors { get; private set; }

    [DataMember]
    public IDictionary<string, object> Metrics { get; private set; }
}
