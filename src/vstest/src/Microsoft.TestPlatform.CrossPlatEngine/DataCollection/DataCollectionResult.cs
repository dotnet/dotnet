// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// Information returned after data collection.
/// </summary>
public class DataCollectionResult
{
    public DataCollectionResult(Collection<AttachmentSet>? attachments, Collection<InvokedDataCollector>? invokedDataCollectors)
    {
        Attachments = attachments;
        InvokedDataCollectors = invokedDataCollectors;
    }

    /// <summary>
    /// Get list of attachments
    /// </summary>
    public Collection<AttachmentSet>? Attachments { get; }

    /// <summary>
    /// Get the list of the invoked data collectors.
    /// </summary>
    public Collection<InvokedDataCollector>? InvokedDataCollectors { get; }
}
