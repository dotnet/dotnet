// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultGeneratedDocumentPublisher : GeneratedDocumentPublisher
{
    private readonly Dictionary<string, PublishData> _publishedCSharpData;
    private readonly Dictionary<string, PublishData> _publishedHtmlData;
    private readonly ClientNotifierServiceBase _server;
    private readonly ILogger _logger;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private ProjectSnapshotManagerBase? _projectSnapshotManager;

    public DefaultGeneratedDocumentPublisher(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ClientNotifierServiceBase server,
        ILoggerFactory loggerFactory)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (server is null)
        {
            throw new ArgumentNullException(nameof(server));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _server = server;
        _logger = loggerFactory.CreateLogger<DefaultGeneratedDocumentPublisher>();
        _publishedCSharpData = new Dictionary<string, PublishData>(FilePathComparer.Instance);
        _publishedHtmlData = new Dictionary<string, PublishData>(FilePathComparer.Instance);
    }

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectSnapshotManager = projectManager;
        _projectSnapshotManager.Changed += ProjectSnapshotManager_Changed;
    }

    public override void PublishCSharp(string filePath, SourceText sourceText, int hostDocumentVersion)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (!_publishedCSharpData.TryGetValue(filePath, out var previouslyPublishedData))
        {
            previouslyPublishedData = PublishData.Default;
        }

        var textChanges = SourceTextDiffer.GetMinimalTextChanges(previouslyPublishedData.SourceText, sourceText);
        if (textChanges.Count == 0 && hostDocumentVersion == previouslyPublishedData.HostDocumentVersion)
        {
            // Source texts match along with host document versions. We've already published something that looks like this. No-op.
            return;
        }

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            var previousDocumentLength = previouslyPublishedData.SourceText.Length;
            var currentDocumentLength = sourceText.Length;
            var documentLengthDelta = sourceText.Length - previousDocumentLength;
            _logger.LogTrace(
                "Updating C# buffer of {0} to correspond with host document version {1}. {2} -> {3} = Change delta of {4} via {5} text changes.",
                filePath,
                hostDocumentVersion,
                previousDocumentLength,
                currentDocumentLength,
                documentLengthDelta,
                textChanges.Count);
        }

        _publishedCSharpData[filePath] = new PublishData(sourceText, hostDocumentVersion);

        var request = new UpdateBufferRequest()
        {
            HostDocumentFilePath = filePath,
            Changes = textChanges,
            HostDocumentVersion = hostDocumentVersion,
        };

        _ = _server.SendNotificationAsync(RazorLanguageServerCustomMessageTargets.RazorUpdateCSharpBufferEndpoint, request, CancellationToken.None);
    }

    public override void PublishHtml(string filePath, SourceText sourceText, int hostDocumentVersion)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (!_publishedHtmlData.TryGetValue(filePath, out var previouslyPublishedData))
        {
            previouslyPublishedData = PublishData.Default;
        }

        var textChanges = SourceTextDiffer.GetMinimalTextChanges(previouslyPublishedData.SourceText, sourceText);
        if (textChanges.Count == 0 && hostDocumentVersion == previouslyPublishedData.HostDocumentVersion)
        {
            // Source texts match along with host document versions. We've already published something that looks like this. No-op.
            return;
        }

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            var previousDocumentLength = previouslyPublishedData.SourceText.Length;
            var currentDocumentLength = sourceText.Length;
            var documentLengthDelta = sourceText.Length - previousDocumentLength;
            _logger.LogTrace(
                "Updating HTML buffer of {0} to correspond with host document version {1}. {2} -> {3} = Change delta of {4} via {5} text changes.",
                filePath,
                hostDocumentVersion,
                previousDocumentLength,
                currentDocumentLength,
                documentLengthDelta,
                textChanges.Count);
        }

        _publishedHtmlData[filePath] = new PublishData(sourceText, hostDocumentVersion);

        var request = new UpdateBufferRequest()
        {
            HostDocumentFilePath = filePath,
            Changes = textChanges,
            HostDocumentVersion = hostDocumentVersion,
        };

        _ = _server.SendNotificationAsync(RazorLanguageServerCustomMessageTargets.RazorUpdateHtmlBufferEndpoint, request, CancellationToken.None);
    }

    private void ProjectSnapshotManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        Assumes.NotNull(_projectSnapshotManager);

        switch (args.Kind)
        {
            case ProjectChangeKind.DocumentChanged:
                Assumes.NotNull(args.DocumentFilePath);
                if (!_projectSnapshotManager.IsDocumentOpen(args.DocumentFilePath))
                {
                    // Document closed, evict published source text.
                    if (_publishedCSharpData.ContainsKey(args.DocumentFilePath))
                    {
                        var removed = _publishedCSharpData.Remove(args.DocumentFilePath);
                        if (!removed)
                        {
                            _logger.LogError("Published data should be protected by the project snapshot manager's thread and should never fail to remove.");
                            Debug.Fail("Published data should be protected by the project snapshot manager's thread and should never fail to remove.");
                        }
                    }

                    if (_publishedHtmlData.ContainsKey(args.DocumentFilePath))
                    {
                        var removed = _publishedHtmlData.Remove(args.DocumentFilePath);
                        if (!removed)
                        {
                            _logger.LogError("Published data should be protected by the project snapshot manager's thread and should never fail to remove.");
                            Debug.Fail("Published data should be protected by the project snapshot manager's thread and should never fail to remove.");
                        }
                    }
                }

                break;
        }
    }

    private sealed class PublishData
    {
        public static readonly PublishData Default = new PublishData(SourceText.From(string.Empty), null);

        public PublishData(SourceText sourceText, int? hostDocumentVersion)
        {
            SourceText = sourceText;
            HostDocumentVersion = hostDocumentVersion;
        }

        public SourceText SourceText { get; }

        public int? HostDocumentVersion { get; }
    }
}
