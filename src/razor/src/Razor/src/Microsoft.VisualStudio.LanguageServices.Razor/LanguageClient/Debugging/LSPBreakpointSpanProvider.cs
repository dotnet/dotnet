// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Debugging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

[Export(typeof(ILSPBreakpointSpanProvider))]
[method: ImportingConstructor]
internal class LSPBreakpointSpanProvider(
    LSPRequestInvoker requestInvoker,
    ILoggerFactory loggerFactory) : ILSPBreakpointSpanProvider
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<LSPBreakpointSpanProvider>();

    public async Task<LspRange?> GetBreakpointSpanAsync(LSPDocumentSnapshot documentSnapshot, long hostDocumentSyncVersion, Position position, CancellationToken cancellationToken)
    {
        var languageQueryParams = new RazorBreakpointSpanParams()
        {
            Position = position,
            Uri = documentSnapshot.Uri,
            HostDocumentSyncVersion = hostDocumentSyncVersion
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
