// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Client;

internal class InProcessTestRunAttachmentsProcessingEventsHandler : ITestRunAttachmentsProcessingEventsHandler
{
    private readonly ITestRunAttachmentsProcessingEventsHandler _eventsHandler;

    public InProcessTestRunAttachmentsProcessingEventsHandler(
        ITestRunAttachmentsProcessingEventsHandler eventsHandler)
    {
        _eventsHandler = eventsHandler;
    }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        _eventsHandler.HandleLogMessage(level, message);
    }

    public void HandleProcessedAttachmentsChunk(IEnumerable<AttachmentSet> attachments)
    {
        // Not implemented by design, keep in sync with the same named method from
        // TestRunAttachmentsProcessingEventsHandler.cs.
        throw new NotImplementedException();
    }

    public void HandleRawMessage(string rawMessage)
    {
        // No-op by design.
        //
        // For out-of-process vstest.console, raw messages are passed to the translation layer but
        // they are never read and don't get passed to the actual events handler in TW. If they
        // were (as it happens for in-process vstest.console since there is no more translation
        // layer) a NotImplemented exception would be raised as per the time this of writing this
        // note.
        //
        // Consider changing this logic in the future if TW changes the handling logic for raw
        // messages.
    }

    public void HandleTestRunAttachmentsProcessingComplete(
        TestRunAttachmentsProcessingCompleteEventArgs attachmentsProcessingCompleteEventArgs,
        IEnumerable<AttachmentSet>? lastChunk)
    {
        _eventsHandler.HandleTestRunAttachmentsProcessingComplete(
            attachmentsProcessingCompleteEventArgs,
            lastChunk);
    }

    public void HandleTestRunAttachmentsProcessingProgress(
        TestRunAttachmentsProcessingProgressEventArgs attachmentsProcessingProgressEventArgs)
    {
        _eventsHandler.HandleTestRunAttachmentsProcessingProgress(
            attachmentsProcessingProgressEventArgs);
    }
}
