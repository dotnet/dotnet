// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.TextDifferencing;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultGeneratedDocumentPublisher : GeneratedDocumentPublisher
{
    private readonly Dictionary<DocumentKey, PublishData> _publishedCSharpData;
    private readonly Dictionary<DocumentKey, PublishData> _publishedHtmlData;
    private readonly ClientNotifierServiceBase _server;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ILogger _logger;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private ProjectSnapshotManagerBase? _projectSnapshotManager;

    public DefaultGeneratedDocumentPublisher(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ClientNotifierServiceBase server,
        LanguageServerFeatureOptions languageServerFeatureOptions,
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

        if (languageServerFeatureOptions is null)
        {
            throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _server = server;
        _languageServerFeatureOptions = languageServerFeatureOptions;
        _logger = loggerFactory.CreateLogger<DefaultGeneratedDocumentPublisher>();
        _publishedCSharpData = new Dictionary<DocumentKey, PublishData>();
        _publishedHtmlData = new Dictionary<DocumentKey, PublishData>();
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

    public override void PublishCSharp(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
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

        var key = new DocumentKey(projectKey, filePath);
        if (!_publishedCSharpData.TryGetValue(key, out var previouslyPublishedData))
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
                "Updating C# buffer of {0} for project {1} to correspond with host document version {2}. {3} -> {4} = Change delta of {5} via {6} text changes.",
                filePath,
                projectKey,
                hostDocumentVersion,
                previousDocumentLength,
                currentDocumentLength,
                documentLengthDelta,
                textChanges.Count);
        }

        _publishedCSharpData[key] = new PublishData(sourceText, hostDocumentVersion);

        var request = new UpdateBufferRequest()
        {
            HostDocumentFilePath = filePath,
            ProjectKeyId = projectKey.Id,
            Changes = textChanges,
            HostDocumentVersion = hostDocumentVersion,
        };

        // HACK: We know about a document being in multiple projects, but despite having ProjectKeyId in the request, currently the other end
        // of this LSP message only knows about a single document file path. To prevent confusing them, we just send an update for the first project
        // in the list.
        if (_projectSnapshotManager is { } projectSnapshotManager &&
            projectSnapshotManager.GetLoadedProject(projectKey) is { } projectSnapshot &&
            projectSnapshotManager.GetAllProjectKeys(projectSnapshot.FilePath).First() != projectKey)
        {
            return;
        }

        _ = _server.SendNotificationAsync(CustomMessageNames.RazorUpdateCSharpBufferEndpoint, request, CancellationToken.None);
    }

    public override void PublishHtml(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
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

        var key = new DocumentKey(projectKey, filePath);
        if (!_publishedHtmlData.TryGetValue(key, out var previouslyPublishedData))
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

        _publishedHtmlData[key] = new PublishData(sourceText, hostDocumentVersion);

        var request = new UpdateBufferRequest()
        {
            HostDocumentFilePath = filePath,
            ProjectKeyId = projectKey.Id,
            Changes = textChanges,
            HostDocumentVersion = hostDocumentVersion,
        };

        _ = _server.SendNotificationAsync(CustomMessageNames.RazorUpdateHtmlBufferEndpoint, request, CancellationToken.None);
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
                    var key = new DocumentKey(args.ProjectKey, args.DocumentFilePath);
                    // Document closed, evict published source text, unless the server doesn't want us to.
                    if (_languageServerFeatureOptions.UpdateBuffersForClosedDocuments)
                    {
                        // Some clients want us to keep generating code even if the document is closed, so if we evict our data,
                        // even though we don't send a didChange for it, the next didChange will be wrong.
                        return;
                    }

                    if (_publishedCSharpData.ContainsKey(key))
                    {
                        var removed = _publishedCSharpData.Remove(key);
                        if (!removed)
                        {
                            _logger.LogError("Published data should be protected by the project snapshot manager's thread and should never fail to remove.");
                            Debug.Fail("Published data should be protected by the project snapshot manager's thread and should never fail to remove.");
                        }
                    }

                    if (_publishedHtmlData.ContainsKey(key))
                    {
                        var removed = _publishedHtmlData.Remove(key);
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
