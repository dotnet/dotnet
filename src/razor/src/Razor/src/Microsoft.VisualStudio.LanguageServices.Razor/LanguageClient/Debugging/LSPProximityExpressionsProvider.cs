// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Debugging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Debugging;

[Export(typeof(ILSPProximityExpressionsProvider))]
[method: ImportingConstructor]
internal class LSPProximityExpressionsProvider(
    LSPRequestInvoker requestInvoker,
    ILoggerFactory loggerFactory) : ILSPProximityExpressionsProvider
{
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<LSPProximityExpressionsProvider>();

    public async Task<IReadOnlyList<string>?> GetProximityExpressionsAsync(LSPDocumentSnapshot documentSnapshot, long hostDocumentSyncVersion, Position position, CancellationToken cancellationToken)
    {
        var proximityExpressionsParams = new RazorProximityExpressionsParams()
        {
            Position = position,
            Uri = documentSnapshot.Uri,
            HostDocumentSyncVersion = hostDocumentSyncVersion
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorProximityExpressionsParams, RazorProximityExpressionsResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorProximityExpressionsEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            proximityExpressionsParams,
            cancellationToken).ConfigureAwait(false);

        var languageResponse = response?.Response;
        if (languageResponse is null)
        {
            _logger.LogInformation($"The proximity expressions could not be resolved.");
            return null;
        }

        return languageResponse.Expressions;
    }
}
