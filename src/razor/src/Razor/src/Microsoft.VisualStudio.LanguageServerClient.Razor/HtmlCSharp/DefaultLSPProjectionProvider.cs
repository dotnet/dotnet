// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[Export(typeof(LSPProjectionProvider))]
internal class DefaultLSPProjectionProvider : LSPProjectionProvider
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly LSPDocumentSynchronizer _documentSynchronizer;
    private readonly RazorLogger _activityLogger;
    private readonly HTMLCSharpLanguageServerLogHubLoggerProvider _loggerProvider;

    private ILogger? _logHubLogger = null;

    [ImportingConstructor]
    public DefaultLSPProjectionProvider(
        LSPRequestInvoker requestInvoker,
        LSPDocumentSynchronizer documentSynchronizer,
        RazorLogger razorLogger,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (documentSynchronizer is null)
        {
            throw new ArgumentNullException(nameof(documentSynchronizer));
        }

        if (razorLogger is null)
        {
            throw new ArgumentNullException(nameof(razorLogger));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _requestInvoker = requestInvoker;
        _documentSynchronizer = documentSynchronizer;
        _activityLogger = razorLogger;
        _loggerProvider = loggerProvider;
    }

    public override Task<ProjectionResult?> GetProjectionAsync(LSPDocumentSnapshot documentSnapshot, Position position, CancellationToken cancellationToken)
        => GetProjectionCoreAsync(documentSnapshot, position, rejectOnNewerParallelRequest: true, cancellationToken);

    public override Task<ProjectionResult?> GetProjectionForCompletionAsync(LSPDocumentSnapshot documentSnapshot, Position position, CancellationToken cancellationToken)
        => GetProjectionCoreAsync(documentSnapshot, position, rejectOnNewerParallelRequest: false, cancellationToken);

    private async Task<ProjectionResult?> GetProjectionCoreAsync(LSPDocumentSnapshot documentSnapshot, Position position, bool rejectOnNewerParallelRequest, CancellationToken cancellationToken)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }

        // We initialize the logger here instead of the constructor as the projection provider is constructed
        // *before* the language server. Thus, the log hub has yet to be initialized, thus we would be unable to
        // create the logger at that time.
        await InitializeLogHubAsync(cancellationToken).ConfigureAwait(false);

        var languageQueryParams = new RazorLanguageQueryParams()
        {
            Position = position,
            Uri = documentSnapshot.Uri
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorLanguageQueryParams, RazorLanguageQueryResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorLanguageQueryEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            CheckRazorLanguageQueryCapability,
            languageQueryParams,
            cancellationToken).ConfigureAwait(false);

        var languageResponse = response?.Response;
        if (languageResponse is null)
        {
            _logHubLogger?.LogInformation("The language server is still being spun up. Could not resolve the projection.");
            return null;
        }

        VirtualDocumentSnapshot virtualDocument;
        if (languageResponse.HostDocumentVersion is null)
        {
            // There should always be a document version attached to an open document.
            // Log it and move on as if it was synchronized.
            _activityLogger.LogVerbose($"Could not find a document version associated with the document '{documentSnapshot.Uri}'");
            _logHubLogger?.LogWarning("Could not find a document version associated with the document '{documentSnapshotUri}'", documentSnapshot.Uri);
            if (languageResponse.Kind == RazorLanguageKind.CSharp)
            {
                if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var cSharpDocument))
                {
                    _logHubLogger?.LogInformation("Could not find projection for {languageResponseKind:G}.", languageResponse.Kind);
                    return null;
                }
                else
                {
                    virtualDocument = cSharpDocument;
                }
            }
            else if (languageResponse.Kind == RazorLanguageKind.Html)
            {
                if (!documentSnapshot.TryGetVirtualDocument<HtmlVirtualDocumentSnapshot>(out var htmlDocument))
                {
                    _logHubLogger?.LogInformation("Could not find projection for {languageResponseKind:G}.", languageResponse.Kind);
                    return null;
                }
                else
                {
                    virtualDocument = htmlDocument;
                }
            }
            else
            {
                _logHubLogger?.LogError("Projections not supported for LanguageKind {languageResponseKind}", languageResponse.Kind);
                return null;
            }
        }
        else
        {
            bool synchronized;
            if (languageResponse.Kind == RazorLanguageKind.CSharp)
            {
                (synchronized, virtualDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(documentSnapshot.Version, documentSnapshot.Uri, rejectOnNewerParallelRequest, cancellationToken).ConfigureAwait(false);
            }
            else if (languageResponse.Kind == RazorLanguageKind.Html)
            {
                (synchronized, virtualDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(documentSnapshot.Version, documentSnapshot.Uri, rejectOnNewerParallelRequest, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                _logHubLogger?.LogInformation("Could not find projection for {languageResponseKind:G}.", languageResponse.Kind);
                return null;
            }

            if (!synchronized)
            {
                _logHubLogger?.LogInformation("Could not synchronize.");
                return null;
            }
        }

        var result = new ProjectionResult()
        {
            Uri = virtualDocument.Uri,
            Position = languageResponse.Position,
            PositionIndex = languageResponse.PositionIndex,
            LanguageKind = languageResponse.Kind,
            HostDocumentVersion = languageResponse.HostDocumentVersion
        };

        return result;
    }

    private async Task InitializeLogHubAsync(CancellationToken cancellationToken)
    {
        if (_logHubLogger is null)
        {
            await _loggerProvider.InitializeLoggerAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            _logHubLogger = _loggerProvider.CreateLogger(nameof(DefaultLSPProjectionProvider));
        }
    }

    private static bool CheckRazorLanguageQueryCapability(JToken token)
    {
        if (!RazorLanguageServerCapability.TryGet(token, out var razorCapability))
        {
            return false;
        }

        return razorCapability.LanguageQuery;
    }
}
