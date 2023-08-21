// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(RazorCustomMessageTarget))]
internal partial class RazorCustomMessageTarget
{
    private readonly TrackingLSPDocumentManager _documentManager;
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly ITelemetryReporter _telemetryReporter;
    private readonly FormattingOptionsProvider _formattingOptionsProvider;
    private readonly IClientSettingsManager _editorSettingsManager;
    private readonly LSPDocumentSynchronizer _documentSynchronizer;
    private readonly IOutputWindowLogger? _outputWindowLogger;

    [ImportingConstructor]
    public RazorCustomMessageTarget(
        LSPDocumentManager documentManager,
        JoinableTaskContext joinableTaskContext,
        LSPRequestInvoker requestInvoker,
        FormattingOptionsProvider formattingOptionsProvider,
        IClientSettingsManager editorSettingsManager,
        LSPDocumentSynchronizer documentSynchronizer,
        ITelemetryReporter telemetryReporter,
        [Import(AllowDefault = true)] IOutputWindowLogger? outputWindowLogger)
    {
        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (formattingOptionsProvider is null)
        {
            throw new ArgumentNullException(nameof(formattingOptionsProvider));
        }

        if (editorSettingsManager is null)
        {
            throw new ArgumentNullException(nameof(editorSettingsManager));
        }

        if (documentSynchronizer is null)
        {
            throw new ArgumentNullException(nameof(documentSynchronizer));
        }

        _documentManager = (TrackingLSPDocumentManager)documentManager;

        if (_documentManager is null)
        {
            throw new ArgumentException("The LSP document manager should be of type " + typeof(TrackingLSPDocumentManager).FullName, nameof(_documentManager));
        }

        if (telemetryReporter is null)
        {
            throw new ArgumentNullException(nameof(telemetryReporter));
        }

        _joinableTaskFactory = joinableTaskContext.Factory;

        _requestInvoker = requestInvoker;
        _formattingOptionsProvider = formattingOptionsProvider;
        _editorSettingsManager = editorSettingsManager;
        _documentSynchronizer = documentSynchronizer;
        _telemetryReporter = telemetryReporter;
        _outputWindowLogger = outputWindowLogger;
    }

    // Testing constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal RazorCustomMessageTarget(TrackingLSPDocumentManager documentManager,
        LSPDocumentSynchronizer documentSynchronizer)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _documentManager = documentManager;
        _documentSynchronizer = documentSynchronizer;
    }

    private async Task<DelegationRequestDetails?> GetProjectedRequestDetailsAsync(IDelegatedParams request, CancellationToken cancellationToken)
    {
        string languageServerName;

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (request.ProjectedKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                _documentManager,
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken,
                rejectOnNewerParallelRequest: false);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (request.ProjectedKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                _documentManager,
                request.Identifier.Version,
                request.Identifier.TextDocumentIdentifier,
                cancellationToken,
                rejectOnNewerParallelRequest: false);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized)
        {
            return null;
        }

        return new DelegationRequestDetails(languageServerName, virtualDocumentSnapshot.Uri, virtualDocumentSnapshot.Snapshot.TextBuffer);
    }

    private record struct DelegationRequestDetails(string LanguageServerName, Uri ProjectedUri, ITextBuffer TextBuffer);
}
