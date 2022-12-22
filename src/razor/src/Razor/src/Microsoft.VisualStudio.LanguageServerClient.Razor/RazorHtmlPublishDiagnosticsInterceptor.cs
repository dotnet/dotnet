// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Microsoft.VisualStudio.Utilities;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(MessageInterceptor))]
[InterceptMethod(Methods.TextDocumentPublishDiagnosticsName)]
[ContentType(RazorLSPConstants.HtmlLSPContentTypeName)]
[ContentType(RazorLSPConstants.CssLSPContentTypeName)]
[ContentType(RazorLSPConstants.TypeScriptLSPContentTypeName)]
internal class RazorHtmlPublishDiagnosticsInterceptor : MessageInterceptor
{
    private readonly LSPDocumentManager _documentManager;
    private readonly LSPDiagnosticsTranslator _diagnosticsProvider;
    private readonly RazorLSPConventions _razorConventions;
    private readonly HTMLCSharpLanguageServerLogHubLoggerProvider _loggerProvider;

    private ILogger? _logger;

    [ImportingConstructor]
    public RazorHtmlPublishDiagnosticsInterceptor(
        LSPDocumentManager documentManager,
        LSPDiagnosticsTranslator diagnosticsProvider,
        RazorLSPConventions razorConventions,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (diagnosticsProvider is null)
        {
            throw new ArgumentNullException(nameof(diagnosticsProvider));
        }

        if (razorConventions is null)
        {
            throw new ArgumentNullException(nameof(razorConventions));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _documentManager = documentManager;
        _diagnosticsProvider = diagnosticsProvider;
        _razorConventions = razorConventions;
        _loggerProvider = loggerProvider;
    }

    public override async Task<InterceptionResult> ApplyChangesAsync(JToken token, string containedLanguageName, CancellationToken cancellationToken)
    {
        if (token is null)
        {
            throw new ArgumentNullException(nameof(token));
        }

        cancellationToken.ThrowIfCancellationRequested();

        // The diagnostics interceptor isn't a part of the HTMLCSharpLanguageServer stack as it's lifecycle is a bit different.
        // It initializes before the actual language server, as we export it to be used directly with WTE.
        // Consequently, if we don't initialize the logger here, then the logger will be unavailable for logging.
        await InitializeLogHubLoggerAsync(cancellationToken).ConfigureAwait(false);

        var diagnosticParams = token.ToObject<PublishDiagnosticParams>();

        if (diagnosticParams?.Uri is null)
        {
            var exception = new ArgumentException("Conversion of token failed.");

            _logger?.LogError(exception, $"Not a {nameof(PublishDiagnosticParams)}");

            throw exception;
        }

        // We only support interception of Virtual HTML Files
        if (!_razorConventions.IsVirtualHtmlFile(diagnosticParams.Uri))
        {
            return CreateDefaultResponse(token);
        }

        _logger?.LogInformation("Received HTML Publish diagnostic request for {diagnosticParamsUri} with {diagnosticsLength} diagnostics.",
            diagnosticParams.Uri, diagnosticParams.Diagnostics.Length);

        var htmlDocumentUri = diagnosticParams.Uri;
        var razorDocumentUri = _razorConventions.GetRazorDocumentUri(htmlDocumentUri);

        // Note; this is an `interceptor` & not a handler, hence
        // it's possible another interceptor mutates this request
        // later in the toolchain. Such an interceptor would likely
        // expect a `__virtual.html` suffix instead of `.razor`.
        diagnosticParams.Uri = razorDocumentUri;

        if (!_documentManager.TryGetDocument(razorDocumentUri, out var razorDocumentSnapshot))
        {
            _logger?.LogInformation("Failed to find document {razorDocumentUri}.", razorDocumentUri);
            return CreateEmptyDiagnosticsResponse(diagnosticParams);
        }

        if (!razorDocumentSnapshot.TryGetVirtualDocument<HtmlVirtualDocumentSnapshot>(out var htmlDocumentSnapshot) ||
            !htmlDocumentSnapshot.Uri.Equals(htmlDocumentUri))
        {
            _logger?.LogInformation("Failed to find virtual HTML document {htmlDocumentUri}.", htmlDocumentUri);
            return CreateEmptyDiagnosticsResponse(diagnosticParams);
        }

        // Return early if there aren't any diagnostics to process
        if (diagnosticParams.Diagnostics?.Any() != true)
        {
            _logger?.LogInformation("No diagnostics to process.");
            return CreateResponse(diagnosticParams);
        }

        var processedDiagnostics = await _diagnosticsProvider.TranslateAsync(
            RazorLanguageKind.Html,
            razorDocumentUri,
            diagnosticParams.Diagnostics,
            cancellationToken
        ).ConfigureAwait(false);

        if (processedDiagnostics is null)
        {
            _logger?.LogWarning("Failed to send request to diagnostic translation server for {htmlDocumentUri}.", htmlDocumentUri);
            return CreateEmptyDiagnosticsResponse(diagnosticParams);
        }

        // Note it's possible the document version changed between when the diagnostics were created
        // and when we finished remapping the diagnostics. This could result in lingering / misaligned diagnostics.
        // We're choosing to do this over clearing out the diagnostics as that would lead to flickering.
        //
        // This'll need to be revisited based on preferences with flickering vs lingering.

        _logger?.LogInformation("Returning {diagnosticsLength} diagnostics.", processedDiagnostics.Diagnostics?.Length ?? 0);
        diagnosticParams.Diagnostics = processedDiagnostics.Diagnostics!;

        return CreateResponse(diagnosticParams);

        static InterceptionResult CreateDefaultResponse(JToken token)
        {
            return new(token, changedDocumentUri: false);
        }

        static InterceptionResult CreateEmptyDiagnosticsResponse(PublishDiagnosticParams diagnosticParams)
        {
            diagnosticParams.Diagnostics = Array.Empty<Diagnostic>();
            return CreateResponse(diagnosticParams);
        }

        static InterceptionResult CreateResponse(PublishDiagnosticParams diagnosticParams)
        {
            var newToken = JToken.FromObject(diagnosticParams);
            var interceptionResult = new InterceptionResult(newToken, changedDocumentUri: true);
            return interceptionResult;
        }
    }

    private async Task InitializeLogHubLoggerAsync(CancellationToken cancellationToken)
    {
        if (_logger is null)
        {
            await _loggerProvider.InitializeLoggerAsync(cancellationToken).ConfigureAwait(false);
            _logger = _loggerProvider.CreateLogger(nameof(RazorHtmlPublishDiagnosticsInterceptor));
        }
    }
}
