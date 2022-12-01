// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
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
[Export(typeof(LSPProximityExpressionsProvider))]
internal class DefaultLSPProximityExpressionsProvider : LSPProximityExpressionsProvider
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly HTMLCSharpLanguageServerLogHubLoggerProvider _loggerProvider;

    private ILogger? _logHubLogger = null;

    [ImportingConstructor]
    public DefaultLSPProximityExpressionsProvider(
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

    public async override Task<IReadOnlyList<string>?> GetProximityExpressionsAsync(LSPDocumentSnapshot documentSnapshot, Position position, CancellationToken cancellationToken)
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

        var proximityExpressionsParams = new RazorProximityExpressionsParams()
        {
            Position = position,
            Uri = documentSnapshot.Uri
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<RazorProximityExpressionsParams, RazorProximityExpressionsResponse>(
            documentSnapshot.Snapshot.TextBuffer,
            LanguageServerConstants.RazorProximityExpressionsEndpoint,
            RazorLSPConstants.RazorLanguageServerName,
            CheckRazorProximityExpressionsCapability,
            proximityExpressionsParams,
            cancellationToken).ConfigureAwait(false);

        var languageResponse = response?.Response;
        if (languageResponse is null)
        {
            _logHubLogger?.LogInformation("The proximity expressions could not be resolved.");
            return null;
        }

        return languageResponse.Expressions;
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

    private static bool CheckRazorProximityExpressionsCapability(JToken token)
    {
        if (!RazorLanguageServerCapability.TryGet(token, out var razorCapability))
        {
            return false;
        }

        return razorCapability.ProximityExpressions;
    }
}
