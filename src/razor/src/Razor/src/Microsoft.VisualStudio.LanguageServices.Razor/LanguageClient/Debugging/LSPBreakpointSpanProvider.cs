// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

[Export(typeof(ILSPBreakpointSpanProvider))]
[method: ImportingConstructor]
internal class LSPBreakpointSpanProvider(
    LSPRequestInvoker requestInvoker,
    ILoggerFactory loggerFactory) : ILSPBreakpointSpanProvider
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<LSPBreakpointSpanProvider>();

    public async Task<Range?> GetBreakpointSpanAsync(LSPDocumentSnapshot documentSnapshot, long hostDocumentSyncVersion, Position position, CancellationToken cancellationToken)
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
