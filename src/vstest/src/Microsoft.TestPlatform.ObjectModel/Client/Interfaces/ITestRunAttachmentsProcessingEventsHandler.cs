// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Interface contract for handling test run attachments processing events
/// </summary>
public interface ITestRunAttachmentsProcessingEventsHandler : ITestMessageEventHandler
{
    /// <summary>
    /// Dispatch TestRunAttachmentsProcessingComplete event to listeners.
    /// </summary>
    /// <param name="attachmentsProcessingCompleteEventArgs">AttachmentsProcessing Complete event args.</param>
    /// <param name="attachments">Last set of processed attachment sets.</param>
    void HandleTestRunAttachmentsProcessingComplete(TestRunAttachmentsProcessingCompleteEventArgs attachmentsProcessingCompleteEventArgs, IEnumerable<AttachmentSet>? lastChunk);

    /// <summary>
    /// Dispatch ProcessedAttachmentsChunk event to listeners.
    /// </summary>
    /// <param name="attachments">Processed attachment sets.</param>
    void HandleProcessedAttachmentsChunk(IEnumerable<AttachmentSet> attachments);


    /// <summary>
    /// Dispatch TestRunAttachmentsProcessingProgress event to listeners.
    /// </summary>
    /// <param name="AttachmentsProcessingProgressEventArgs">AttachmentsProcessing Progress event args.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Part of the public API.")]
    void HandleTestRunAttachmentsProcessingProgress(TestRunAttachmentsProcessingProgressEventArgs AttachmentsProcessingProgressEventArgs);
}
