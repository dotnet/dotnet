// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging;

[Shared]
[Export(typeof(LSPBreakpointSpanProvider))]
internal class DefaultLSPBreakpointSpanProvider : LSPBreakpointSpanProvider
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly HTMLCSharpLanguageServerLogHubLoggerProvider _loggerProvider;

    private ILogger? _logHubLogger = null;

    [ImportingConstructor]
    public DefaultLSPBreakpointSpanProvider(
        LSPRequestInvoker requestInvoker,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _requestInvoker = requestInvoker;
        _loggerProvider = loggerProvider;
    }

    public async override Task<Range?> GetBreakpointSpanAsync(LSPDocumentSnapshot documentSnapshot, Position position, CancellationToken cancellationToken)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }

        // We initialize the logger here instead of the constructor as the breakpoint span provider is constructed
        // *before* the language server. Thus, the log hub has yet to be initialized, thus we would be unable to
        // create the logger at that time.
        await InitializeLogHubAsync(cancellationToken).ConfigureAwait(false);

        var languageQueryParams = new RazorBreakpointSpanParams()
        {
            Position = position,
            Uri = documentSnapshot.Uri
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorBreakpointSpanParams, RazorBreakpointSpanResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorBreakpointSpanEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            CheckRazorBreakpointSpanCapability,
            languageQueryParams,
            cancellationToken).ConfigureAwait(false);

        var languageResponse = response?.Response;
        if (languageResponse is null)
        {
            _logHubLogger?.LogInformation("The breakpoint position could not be mapped to a valid range.");
            return null;
        }

        return languageResponse.Range;
    }

    private async Task InitializeLogHubAsync(CancellationToken cancellationToken)
    {
        if (_logHubLogger is null)
        {
            await _loggerProvider.InitializeLoggerAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            _logHubLogger = _loggerProvider.CreateLogger(nameof(DefaultLSPBreakpointSpanProvider));
        }
    }

    private static bool CheckRazorBreakpointSpanCapability(JToken token)
    {
        if (!RazorLanguageServerCapability.TryGet(token, out var razorCapability))
        {
            return false;
        }

        return razorCapability.BreakpointSpan;
    }
}
