// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

[DataContract]
public class TestRunAttachmentsProcessingProgressEventArgs : EventArgs
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="currentAttachmentProcessorIndex">Specifies current attachment processor index.</param>
    /// <param name="currentAttachmentProcessorUris">Specifies current processor Uris.</param>
    /// <param name="currentAttachmentProcessorProgress">Specifies current processor progress.</param>
    /// <param name="attachmentProcessorsCount">Specifies the overall number of processors.</param>
    public TestRunAttachmentsProcessingProgressEventArgs(long currentAttachmentProcessorIndex, ICollection<Uri> currentAttachmentProcessorUris, long currentAttachmentProcessorProgress, long attachmentProcessorsCount)
    {
        CurrentAttachmentProcessorIndex = currentAttachmentProcessorIndex;
        CurrentAttachmentProcessorUris = currentAttachmentProcessorUris;
        CurrentAttachmentProcessorProgress = currentAttachmentProcessorProgress;
        AttachmentProcessorsCount = attachmentProcessorsCount;
    }

    /// <summary>
    /// Gets a current attachment processor index.
    /// </summary>
    [DataMember]
    public long CurrentAttachmentProcessorIndex { get; private set; }

    /// <summary>
    /// Gets a current attachment processor URI.
    /// </summary>
    [DataMember]
    public ICollection<Uri> CurrentAttachmentProcessorUris { get; private set; }

    /// <summary>
    /// Gets a current attachment processor progress.
    /// </summary>
    [DataMember]
    public long CurrentAttachmentProcessorProgress { get; private set; }

    /// <summary>
    /// Gets the overall number of attachment processors.
    /// </summary>
    [DataMember]
    public long AttachmentProcessorsCount { get; private set; }
}
