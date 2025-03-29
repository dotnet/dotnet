// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.TextDifferencing;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class GeneratedDocumentPublisher : IGeneratedDocumentPublisher, IRazorStartupService
{
    private readonly Dictionary<DocumentKey, PublishData> _publishedCSharpData;
    private readonly Dictionary<string, PublishData> _publishedHtmlData;
    private readonly ProjectSnapshotManager _projectManager;
    private readonly IClientConnection _clientConnection;
    private readonly LanguageServerFeatureOptions _options;
    private readonly ILogger _logger;

    public GeneratedDocumentPublisher(
        ProjectSnapshotManager projectManager,
        IClientConnection clientConnection,
        LanguageServerFeatureOptions options,
        ILoggerFactory loggerFactory)
    {
        _projectManager = projectManager;
        _clientConnection = clientConnection;
        _options = options;
        _logger = loggerFactory.GetOrCreateLogger<GeneratedDocumentPublisher>();
        _publishedCSharpData = [];

        // We don't generate individual Html documents per-project, so in order to ensure diffs are calculated correctly
        // we don't use the project key for the key for this dictionary. This matches when we send edits to the client,
        // as they are only tracking a single Html file for each Razor file path, thus edits need to be correct or we'll
        // get out of sync.
        _publishedHtmlData = new Dictionary<string, PublishData>(FilePathComparer.Instance);

        _projectManager.Changed += ProjectManager_Changed;
    }

    public void PublishCSharp(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
    {
        // If our generated documents don't have unique file paths, then using project key information is problematic for the client.
        // For example, when a document moves from the Misc Project to a real project, we will update it here, and each version would
        // have a different project key. On the receiving end however, there is only one file path, therefore one version of the contents,
        // so we must ensure we only have a single document to compute diffs from, or things get out of sync.
        var documentKey = _options.IncludeProjectKeyInGeneratedFilePath
            ? new DocumentKey(projectKey, filePath)
            : new DocumentKey(ProjectKey.Unknown, filePath);

        PublishData? previouslyPublishedData;
        ImmutableArray<TextChange> textChanges;

        lock (_publishedCSharpData)
        {
            if (!_publishedCSharpData.TryGetValue(documentKey, out previouslyPublishedData))
            {
                _logger.LogDebug($"New publish data created for {documentKey.ProjectKey} and {filePath}");
                previouslyPublishedData = PublishData.Default;
            }

            if (previouslyPublishedData.HostDocumentVersion > hostDocumentVersion)
            {
                // We've already published a newer version of this document. No-op.
                _logger.LogWarning($"Skipping publish of C# for {documentKey.ProjectKey}/{filePath} because we've already published version {previouslyPublishedData.HostDocumentVersion}, and this request is for {hostDocumentVersion} (and {projectKey}).");
                return;
            }

            textChanges = SourceTextDiffer.GetMinimalTextChanges(previouslyPublishedData.SourceText, sourceText);
            if (textChanges.Length == 0 && hostDocumentVersion == previouslyPublishedData.HostDocumentVersion)
            {
                // Source texts match along with host document versions. We've already published something that looks like this. No-op.
                return;
            }

            _logger.LogDebug(
                $"Updating C# buffer of {filePath} for project {documentKey.ProjectKey} to correspond with host document " +
                $"version {hostDocumentVersion}. {previouslyPublishedData.SourceText.Length} -> {sourceText.Length} = Change delta of " +
                $"{sourceText.Length - previouslyPublishedData.SourceText.Length} via {textChanges.Length} text changes.");

            _publishedCSharpData[documentKey] = new PublishData(sourceText, hostDocumentVersion);
        }

        var request = new UpdateBufferRequest()
        {
            HostDocumentFilePath = filePath,
            ProjectKeyId = projectKey.Id,
            Changes = [.. textChanges.Select(static t => t.ToRazorTextChange())],
            HostDocumentVersion = hostDocumentVersion,
            PreviousHostDocumentVersion = previouslyPublishedData.HostDocumentVersion,
            PreviousWasEmpty = previouslyPublishedData.SourceText.Length == 0,
            Checksum = Convert.ToBase64String([.. sourceText.GetChecksum()]),
            ChecksumAlgorithm = sourceText.ChecksumAlgorithm,
            SourceEncodingCodePage = sourceText.Encoding?.CodePage
        };

        _clientConnection.SendNotificationAsync(CustomMessageNames.RazorUpdateCSharpBufferEndpoint, request, CancellationToken.None).Forget();
    }

    public void PublishHtml(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
    {
        PublishData? previouslyPublishedData;
        ImmutableArray<TextChange> textChanges;

        lock (_publishedHtmlData)
        {
            if (!_publishedHtmlData.TryGetValue(filePath, out previouslyPublishedData))
            {
                previouslyPublishedData = PublishData.Default;
            }

            if (previouslyPublishedData.HostDocumentVersion > hostDocumentVersion)
            {
                // We've already published a newer version of this document. No-op.
                _logger.LogWarning($"Skipping publish of Html for {filePath} because we've already published version {previouslyPublishedData.HostDocumentVersion}, and this request is for {hostDocumentVersion}.");
                return;
            }

            textChanges = SourceTextDiffer.GetMinimalTextChanges(previouslyPublishedData.SourceText, sourceText);
            if (textChanges.Length == 0 && hostDocumentVersion == previouslyPublishedData.HostDocumentVersion)
            {
                // Source texts match along with host document versions. We've already published something that looks like this. No-op.
                return;
            }

            _logger.LogDebug(
                $"Updating HTML buffer of {filePath} to correspond with host document version {hostDocumentVersion}. {previouslyPublishedData.SourceText.Length} -> {sourceText.Length} = Change delta of {sourceText.Length - previouslyPublishedData.SourceText.Length} via {textChanges.Length} text changes.");

            _publishedHtmlData[filePath] = new PublishData(sourceText, hostDocumentVersion);
        }

        var request = new UpdateBufferRequest()
        {
            HostDocumentFilePath = filePath,
            ProjectKeyId = projectKey.Id,
            Changes = [.. textChanges.Select(static t => t.ToRazorTextChange())],
            HostDocumentVersion = hostDocumentVersion,
            PreviousWasEmpty = previouslyPublishedData.SourceText.Length == 0,
            Checksum = Convert.ToBase64String([.. sourceText.GetChecksum()]),
            ChecksumAlgorithm = sourceText.ChecksumAlgorithm,
            SourceEncodingCodePage = sourceText.Encoding?.CodePage
        };

        _clientConnection.SendNotificationAsync(CustomMessageNames.RazorUpdateHtmlBufferEndpoint, request, CancellationToken.None).Forget();
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.IsSolutionClosing)
        {
            return;
        }

        switch (args.Kind)
        {
            case ProjectChangeKind.DocumentRemoved:
                {
                    if (!_options.IncludeProjectKeyInGeneratedFilePath)
                    {
                        break;
                    }

                    // When a C# document is removed we remove it from the publishing, because it could come back with the same name
                    var key = new DocumentKey(args.ProjectKey, args.DocumentFilePath.AssumeNotNull());

                    lock (_publishedCSharpData)
                    {
                        if (_publishedCSharpData.Remove(key))
                        {
                            _logger.LogDebug($"Removing previous C# publish data for {key.ProjectKey}/{key.FilePath}");
                        }
                    }

                    break;
                }

            case ProjectChangeKind.DocumentChanged:
                var documentFilePath = args.DocumentFilePath.AssumeNotNull();

                if (!_projectManager.IsDocumentOpen(documentFilePath))
                {
                    // Document closed, evict published source text, unless the server doesn't want us to.
                    if (_options.UpdateBuffersForClosedDocuments)
                    {
                        // Some clients want us to keep generating code even if the document is closed, so if we evict our data,
                        // even though we don't send a didChange for it, the next didChange will be wrong.
                        return;
                    }

                    var documentKey = _options.IncludeProjectKeyInGeneratedFilePath
                        ? new DocumentKey(args.ProjectKey, documentFilePath)
                        : new DocumentKey(ProjectKey.Unknown, documentFilePath);

                    lock (_publishedCSharpData)
                    {
                        if (_publishedCSharpData.Remove(documentKey))
                        {
                            _logger.LogDebug($"Removing previous C# publish data for {documentKey.ProjectKey}/{documentKey.FilePath}");
                        }
                    }

                    lock (_publishedHtmlData)
                    {
                        if (_publishedHtmlData.Remove(documentFilePath))
                        {
                            _logger.LogDebug($"Removing previous Html publish data for {documentKey.ProjectKey}/{documentKey.FilePath}");
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

                    lock (_publishedCSharpData)
                    {
                        using var keysToRemove = new PooledArrayBuilder<DocumentKey>();

                        foreach (var (documentKey, _) in _publishedCSharpData)
                        {
                            if (documentKey.ProjectKey == args.ProjectKey)
                            {
                                keysToRemove.Add(documentKey);
                            }
                        }

                        foreach (var key in keysToRemove)
                        {
                            if (_publishedCSharpData.Remove(key))
                            {
                                _logger.LogDebug($"Removing previous C# publish data for {key.ProjectKey}/{key.FilePath}");
                            }
                        }
                    }

                    break;
                }
        }
    }

    private sealed record PublishData(SourceText SourceText, int? HostDocumentVersion)
    {
        public static readonly PublishData Default = new(SourceText.From(string.Empty), null);
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
