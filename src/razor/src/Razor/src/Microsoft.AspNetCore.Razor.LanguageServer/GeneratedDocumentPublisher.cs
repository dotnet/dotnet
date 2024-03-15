// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.TextDifferencing;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class GeneratedDocumentPublisher : IGeneratedDocumentPublisher, IRazorStartupService
{
    private readonly Dictionary<DocumentKey, PublishData> _publishedCSharpData;
    private readonly Dictionary<string, PublishData> _publishedHtmlData;
    private readonly ProjectSnapshotManagerBase _projectManager;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private readonly IClientConnection _clientConnection;
    private readonly LanguageServerFeatureOptions _options;
    private readonly ILogger _logger;

    public GeneratedDocumentPublisher(
        ProjectSnapshotManagerBase projectManager,
        ProjectSnapshotManagerDispatcher dispatcher,
        IClientConnection clientConnection,
        LanguageServerFeatureOptions options,
        IRazorLoggerFactory loggerFactory)
    {

        _projectManager = projectManager;
        _dispatcher = dispatcher;
        _clientConnection = clientConnection;
        _options = options;
        _logger = loggerFactory.CreateLogger<GeneratedDocumentPublisher>();
        _publishedCSharpData = new Dictionary<DocumentKey, PublishData>();

        // We don't generate individual Html documents per-project, so in order to ensure diffs are calculated correctly
        // we don't use the project key for the key for this dictionary. This matches when we send edits to the client,
        // as they are only tracking a single Html file for each Razor file path, thus edits need to be correct or we'll
        // get out of sync.
        _publishedHtmlData = new Dictionary<string, PublishData>(FilePathComparer.Instance);

        _projectManager.Changed += ProjectManager_Changed;
    }

    public void PublishCSharp(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        _dispatcher.AssertRunningOnDispatcher();

        // If our generated documents don't have unique file paths, then using project key information is problematic for the client.
        // For example, when a document moves from the Misc Project to a real project, we will update it here, and each version would
        // have a different project key. On the receiving end however, there is only one file path, therefore one version of the contents,
        // so we must ensure we only have a single document to compute diffs from, or things get out of sync.
        if (!_options.IncludeProjectKeyInGeneratedFilePath)
        {
            projectKey = default;
        }

        var key = new DocumentKey(projectKey, filePath);
        if (!_publishedCSharpData.TryGetValue(key, out var previouslyPublishedData))
        {
            _logger.LogDebug("New publish data created for {project} and {filePath}", projectKey, filePath);
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
            PreviousWasEmpty = previouslyPublishedData.SourceText.Length == 0
        };

        _ = _clientConnection.SendNotificationAsync(CustomMessageNames.RazorUpdateCSharpBufferEndpoint, request, CancellationToken.None);
    }

    public void PublishHtml(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        _dispatcher.AssertRunningOnDispatcher();

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
            ProjectKeyId = projectKey.Id,
            Changes = textChanges,
            HostDocumentVersion = hostDocumentVersion,
            PreviousWasEmpty = previouslyPublishedData.SourceText.Length == 0
        };

        _ = _clientConnection.SendNotificationAsync(CustomMessageNames.RazorUpdateHtmlBufferEndpoint, request, CancellationToken.None);
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        _dispatcher.AssertRunningOnDispatcher();

        Assumes.NotNull(_projectManager);

        switch (args.Kind)
        {
            case ProjectChangeKind.DocumentChanged:
                Assumes.NotNull(args.DocumentFilePath);
                if (!_projectManager.IsDocumentOpen(args.DocumentFilePath))
                {
                    // Document closed, evict published source text, unless the server doesn't want us to.
                    if (_options.UpdateBuffersForClosedDocuments)
                    {
                        // Some clients want us to keep generating code even if the document is closed, so if we evict our data,
                        // even though we don't send a didChange for it, the next didChange will be wrong.
                        return;
                    }

                    var projectKey = args.ProjectKey;
                    if (!_options.IncludeProjectKeyInGeneratedFilePath)
                    {
                        projectKey = default;
                    }

                    var key = new DocumentKey(projectKey, args.DocumentFilePath);
                    if (_publishedCSharpData.ContainsKey(key))
                    {
                        var removed = _publishedCSharpData.Remove(key);
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

            case ProjectChangeKind.ProjectRemoved:
                {
                    // When a project is removed, we have to remove all published C# source for files in the project because if it comes back,
                    // or a new one comes back with the same name, we want it to start with a clean slate. We only do this if the project key
                    // is part of the generated file name though, because otherwise a project with the same name is effectively the same project.
                    if (!_options.IncludeProjectKeyInGeneratedFilePath)
                    {
                        break;
                    }

                    using var _ = ListPool<DocumentKey>.GetPooledObject(out var keysToRemove);
                    foreach (var keyValuePair in _publishedCSharpData)
                    {
                        if (keyValuePair.Key.ProjectKey.Equals(args.ProjectKey))
                        {
                            keysToRemove.Add(keyValuePair.Key);
                        }
                    }

                    foreach (var key in keysToRemove)
                    {
                        _publishedCSharpData.Remove(key);
                    }

                    break;
                }
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

    internal TestAccessor GetTestAccessor()
    {
        return new TestAccessor(this);
    }

    internal readonly struct TestAccessor
    {
        private readonly GeneratedDocumentPublisher _instance;

        internal TestAccessor(GeneratedDocumentPublisher instance)
        {
            _instance = instance;
        }

        internal int PublishedCSharpDataCount => _instance._publishedCSharpData.Count;
    }
}
