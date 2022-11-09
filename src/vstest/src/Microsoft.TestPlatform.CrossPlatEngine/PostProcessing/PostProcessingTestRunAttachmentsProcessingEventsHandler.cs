// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;

internal class PostProcessingTestRunAttachmentsProcessingEventsHandler : ITestRunAttachmentsProcessingEventsHandler
{
    private readonly IOutput _consoleOutput;
    private readonly ConcurrentBag<AttachmentSet> _attachmentsSet = new();

    public PostProcessingTestRunAttachmentsProcessingEventsHandler(IOutput consoleOutput)
    {
        _consoleOutput = consoleOutput ?? throw new ArgumentNullException(nameof(consoleOutput));
    }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    { }

    public void HandleRawMessage(string rawMessage)
    { }

    public void HandleTestRunAttachmentsProcessingProgress(TestRunAttachmentsProcessingProgressEventArgs attachmentsProcessingProgressEventArgs)
    { }

    public void HandleProcessedAttachmentsChunk(IEnumerable<AttachmentSet> attachments)
    {
        if (attachments is null)
        {
            EqtTrace.Warning($"PostProcessingTestRunAttachmentsProcessingEventsHandler.HandleProcessedAttachmentsChunk: Unexpected null attachments");
            return;
        }

        foreach (var attachment in attachments)
        {
            _attachmentsSet.Add(attachment);
        }
    }

    public void HandleTestRunAttachmentsProcessingComplete(TestRunAttachmentsProcessingCompleteEventArgs attachmentsProcessingCompleteEventArgs, IEnumerable<AttachmentSet>? lastChunk)
    {
        foreach (var attachment in lastChunk ?? Enumerable.Empty<AttachmentSet>())
        {
            _attachmentsSet.Add(attachment);
        }

        if (!_attachmentsSet.IsEmpty)
        {
            // Make an empty line
            _consoleOutput.WriteLine("", OutputLevel.Information);
        }

        _consoleOutput.Information(false, ConsoleColor.Gray, CommandLineResources.AttachmentsBanner);
        foreach (var attachmentSet in _attachmentsSet)
        {
            foreach (var uriDataAttachment in attachmentSet.Attachments)
            {
                _consoleOutput.Information(false, ConsoleColor.Gray, CommandLineResources.AttachmentOutputFormat, uriDataAttachment.Uri.LocalPath);
            }
        }
    }
}
