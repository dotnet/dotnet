// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

internal partial class RazorDiagnosticsPublisher : IDocumentProcessedListener, IDisposable
{
    private readonly record struct PublishedDiagnostics(IReadOnlyList<RazorDiagnostic> Razor, Diagnostic[]? CSharp)
    {
        public int Count => Razor.Count + (CSharp?.Length ?? 0);
    }

    private static readonly TimeSpan s_publishDelay = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan s_clearClosedDocumentsDelay = TimeSpan.FromSeconds(5);

    private readonly IProjectSnapshotManager _projectManager;
    private readonly IClientConnection _clientConnection;
    private readonly ILogger _logger;
    private readonly LanguageServerFeatureOptions _options;
    private readonly Lazy<RazorTranslateDiagnosticsService> _translateDiagnosticsService;
    private readonly Lazy<IDocumentContextFactory> _documentContextFactory;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<IDocumentSnapshot> _workQueue;
    private readonly Dictionary<string, PublishedDiagnostics> _publishedDiagnostics;

    private Task _clearClosedDocumentsTask = Task.CompletedTask;

    public RazorDiagnosticsPublisher(
        IProjectSnapshotManager projectManager,
        IClientConnection clientConnection,
        LanguageServerFeatureOptions options,
        Lazy<RazorTranslateDiagnosticsService> translateDiagnosticsService,
        Lazy<IDocumentContextFactory> documentContextFactory,
        ILoggerFactory loggerFactory)
        : this(projectManager, clientConnection, options,
              translateDiagnosticsService, documentContextFactory,
               loggerFactory, s_publishDelay)
    {
    }

    // Present for test to specify publish delay
    protected RazorDiagnosticsPublisher(
        IProjectSnapshotManager projectManager,
        IClientConnection clientConnection,
        LanguageServerFeatureOptions options,
        Lazy<RazorTranslateDiagnosticsService> translateDiagnosticsService,
        Lazy<IDocumentContextFactory> documentContextFactory,
        ILoggerFactory loggerFactory,
        TimeSpan publishDelay)
    {
        _projectManager = projectManager;
        _clientConnection = clientConnection;
        _options = options;
        _translateDiagnosticsService = translateDiagnosticsService;
        _documentContextFactory = documentContextFactory;

        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<IDocumentSnapshot>(publishDelay, ProcessBatchAsync, _disposeTokenSource.Token);

        _publishedDiagnostics = new Dictionary<string, PublishedDiagnostics>(FilePathComparer.Instance);
        _logger = loggerFactory.GetOrCreateLogger<RazorDiagnosticsPublisher>();
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    public void DocumentProcessed(RazorCodeDocument codeDocument, IDocumentSnapshot document)
    {
        _workQueue.AddWork(document);

        StartDelayToClearDocuments();
    }

    private async ValueTask ProcessBatchAsync(ImmutableArray<IDocumentSnapshot> items, CancellationToken token)
    {
        foreach (var document in items.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            await PublishDiagnosticsAsync(document, token).ConfigureAwait(false);
        }
    }

    private async Task PublishDiagnosticsAsync(IDocumentSnapshot document, CancellationToken cancellationToken)
    {
        var result = await document.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var csharpDiagnostics = await GetCSharpDiagnosticsAsync(document, cancellationToken).ConfigureAwait(false);
        var razorDiagnostics = result.GetCSharpDocument().Diagnostics;

        lock (_publishedDiagnostics)
        {
            var filePath = document.FilePath;

            // See if these are the same diagnostics as last time. If so, we don't need to publish.
            if (_publishedDiagnostics.TryGetValue(filePath, out var previousDiagnostics))
            {
                var sameRazorDiagnostics = razorDiagnostics.SequenceEqual(previousDiagnostics.Razor);
                var sameCSharpDiagnostics = (csharpDiagnostics, previousDiagnostics.CSharp) switch
                {
                    (not null, not null) => csharpDiagnostics.SequenceEqual(previousDiagnostics.CSharp),
                    (null, null) => true,
                    _ => false,
                };

                if (sameRazorDiagnostics && sameCSharpDiagnostics)
                {
                    return;
                }
            }

            _publishedDiagnostics[filePath] = new(razorDiagnostics, csharpDiagnostics);
        }

        if (!document.TryGetText(out var sourceText))
        {
            Debug.Fail("Document source text should already be available.");
            return;
        }

        Diagnostic[] combinedDiagnostics = [
            .. razorDiagnostics.Select(d => RazorDiagnosticConverter.Convert(d, sourceText, document)),
            .. csharpDiagnostics ?? []
        ];

        PublishDiagnosticsForFilePath(document.FilePath, combinedDiagnostics);

        _logger.LogTrace($"Publishing diagnostics for document '{document.FilePath}': {string.Join(", ", razorDiagnostics.Select(diagnostic => diagnostic.Id))}");

        async Task<Diagnostic[]?> GetCSharpDiagnosticsAsync(IDocumentSnapshot document, CancellationToken token)
        {
            if (_options.DelegateToCSharpOnDiagnosticPublish)
            {
                var uriBuilder = new UriBuilder()
                {
                    Scheme = Uri.UriSchemeFile,
                    Path = document.FilePath,
                    Host = string.Empty,
                };

                var delegatedParams = new DocumentDiagnosticParams
                {
                    TextDocument = new TextDocumentIdentifier { Uri = uriBuilder.Uri },
                };

                var delegatedResponse = await _clientConnection
                    .SendRequestAsync<DocumentDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                        CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                        delegatedParams,
                        token)
                    .ConfigureAwait(false);

                if (delegatedResponse.HasValue &&
                    delegatedResponse.Value.TryGetFirst(out var fullDiagnostics) &&
                    fullDiagnostics.Items is not null)
                {
                    if (_documentContextFactory.Value.TryCreate(delegatedParams.TextDocument.Uri, projectContext: null, out var documentContext))
                    {
                        return await _translateDiagnosticsService.Value
                            .TranslateAsync(RazorLanguageKind.CSharp, fullDiagnostics.Items, documentContext.Snapshot, cancellationToken)
                            .ConfigureAwait(false);
                    }
                }
            }

            return null;
        }
    }

    private void StartDelayToClearDocuments()
    {
        if (!_clearClosedDocumentsTask.IsCompleted)
        {
            return;
        }

        _clearClosedDocumentsTask = ClearClosedDocumentsAfterDelayAsync();

        async Task ClearClosedDocumentsAfterDelayAsync()
        {
            await Task.Delay(s_clearClosedDocumentsDelay, _disposeTokenSource.Token).ConfigureAwait(false);

            ClearClosedDocuments();
        }
    }

    private void ClearClosedDocuments()
    {
        lock (_publishedDiagnostics)
        {
            using var documentsToRemove = new PooledArrayBuilder<(string filePath, bool publishToClear)>(capacity: _publishedDiagnostics.Count);

            foreach (var (filePath, diagnostics) in _publishedDiagnostics)
            {
                if (!_projectManager.IsDocumentOpen(filePath))
                {
                    // If there were previously published diagnostics for this document, take note so
                    // we can publish an empty set of diagnostics.
                    documentsToRemove.Add((filePath, publishToClear: diagnostics.Count > 0));
                }
            }

            if (documentsToRemove.Count == 0)
            {
                return;
            }

            foreach (var (filePath, publishToClear) in documentsToRemove)
            {
                _publishedDiagnostics.Remove(filePath);

                if (publishToClear)
                {
                    PublishDiagnosticsForFilePath(filePath, []);
                }
            }

            if (_publishedDiagnostics.Count > 0)
            {
                // There's no way for us to know when a document is closed at this layer. Therefore, we need to poll every X seconds
                // and check if the currently tracked documents are closed. In practice this work is super minimal.
                StartDelayToClearDocuments();
            }
        }
    }

    private void PublishDiagnosticsForFilePath(string filePath, Diagnostic[] diagnostics)
    {
        var uriBuilder = new UriBuilder()
        {
            Scheme = Uri.UriSchemeFile,
            Path = filePath,
            Host = string.Empty,
        };

        _clientConnection
            .SendNotificationAsync(
                Methods.TextDocumentPublishDiagnosticsName,
                new PublishDiagnosticParams()
                {
                    Uri = uriBuilder.Uri,
                    Diagnostics = diagnostics,
                },
                _disposeTokenSource.Token)
            .Forget();
    }
}
