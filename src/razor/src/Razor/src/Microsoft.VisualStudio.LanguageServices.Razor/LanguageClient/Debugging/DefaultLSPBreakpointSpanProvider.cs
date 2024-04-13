// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Debugging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

[Export(typeof(LSPBreakpointSpanProvider))]
internal class DefaultLSPBreakpointSpanProvider : LSPBreakpointSpanProvider
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly Lazy<ILogger> _logger;

    [ImportingConstructor]
    public DefaultLSPBreakpointSpanProvider(
        LSPRequestInvoker requestInvoker,
        Lazy<ILoggerFactory> loggerFactory)
    {
        if (requestInvoker is null)
        {
            throw new ArgumentNullException(nameof(requestInvoker));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _requestInvoker = requestInvoker;
        _logger = new Lazy<ILogger>(() => loggerFactory.Value.GetOrCreateLogger<DefaultLSPBreakpointSpanProvider>());
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
            _logger.Value.LogInformation($"The breakpoint position could not be mapped to a valid range.");
            return null;
        }

        return languageResponse.Range;
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
