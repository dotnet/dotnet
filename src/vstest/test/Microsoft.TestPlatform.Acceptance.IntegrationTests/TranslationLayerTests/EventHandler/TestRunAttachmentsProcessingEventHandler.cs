// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

/// <inheritdoc />
public class TestRunAttachmentsProcessingEventHandler : ITestRunAttachmentsProcessingEventsHandler
{
    public List<AttachmentSet> Attachments { get; private set; }

    public TestRunAttachmentsProcessingCompleteEventArgs? CompleteArgs { get; private set; }

    public List<TestRunAttachmentsProcessingProgressEventArgs> ProgressArgs { get; private set; }

    /// <summary>
    /// Gets the log message.
    /// </summary>
    public string? LogMessage { get; private set; }

    public List<string?> Errors { get; set; }

    /// <summary>
    /// Gets the test message level.
    /// </summary>
    public TestMessageLevel TestMessageLevel { get; private set; }

    public TestRunAttachmentsProcessingEventHandler()
    {
        Errors = new();
        Attachments = new List<AttachmentSet>();
        ProgressArgs = new List<TestRunAttachmentsProcessingProgressEventArgs>();
    }

    public void EnsureSuccess()
    {
        if (Errors.Any())
        {
            throw new InvalidOperationException($"Test run reported errors:{Environment.NewLine}{string.Join(Environment.NewLine + Environment.NewLine, Errors)}");
        }
    }

    public void HandleLogMessage(TestMessageLevel level, string? message)
    {
        LogMessage = message;
        TestMessageLevel = level;
        if (level == TestMessageLevel.Error)
        {
            Errors.Add(message);
        }
    }

    public void HandleRawMessage(string rawMessage)
    {
        // No op
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Usage is unclear so keeping as non-static")]
    public int LaunchProcessWithDebuggerAttached(TestProcessStartInfo _)
    {
        // No op
        return -1;
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Usage is unclear so keeping as non-static")]
    public bool AttachDebuggerToProcess(int _)
    {
        // No op
        return true;
    }

    public void HandleTestRunAttachmentsProcessingComplete(ICollection<AttachmentSet> attachments)
    {
        if (attachments != null)
        {
            Attachments.AddRange(attachments);
        }
    }

    public void HandleTestRunAttachmentsProcessingComplete(TestRunAttachmentsProcessingCompleteEventArgs attachmentsProcessingCompleteEventArgs, IEnumerable<AttachmentSet>? lastChunk)
    {
        if (lastChunk != null)
        {
            Attachments.AddRange(lastChunk);
        }

        if (attachmentsProcessingCompleteEventArgs.Error != null)
        {
            Errors.Add(attachmentsProcessingCompleteEventArgs.Error.Message);
        }

        CompleteArgs = attachmentsProcessingCompleteEventArgs;
    }

    public void HandleProcessedAttachmentsChunk(IEnumerable<AttachmentSet> attachments)
    {
        throw new NotImplementedException();
    }

    public void HandleTestRunAttachmentsProcessingProgress(TestRunAttachmentsProcessingProgressEventArgs attachmentsProcessingProgressEventArgs)
    {
        ProgressArgs.Add(attachmentsProcessingProgressEventArgs);
    }
}
