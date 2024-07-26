// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Debugging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

[Export(typeof(LSPBreakpointSpanProvider))]
internal class DefaultLSPBreakpointSpanProvider : LSPBreakpointSpanProvider
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public DefaultLSPBreakpointSpanProvider(
        LSPRequestInvoker requestInvoker,
        ILoggerFactory loggerFactory)
    {
        _requestInvoker = requestInvoker;
        _logger = loggerFactory.GetOrCreateLogger<DefaultLSPBreakpointSpanProvider>();
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
            languageQueryParams,
            cancellationToken).ConfigureAwait(false);

        var languageResponse = response?.Response;
        if (languageResponse is null)
        {
            _logger.LogInformation($"The breakpoint position could not be mapped to a valid range.");
            return null;
        }

        return languageResponse.Range;
    }
}
