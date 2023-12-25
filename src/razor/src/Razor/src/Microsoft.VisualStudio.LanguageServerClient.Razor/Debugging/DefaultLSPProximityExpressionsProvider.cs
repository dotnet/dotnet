// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging;

[Shared]
[Export(typeof(LSPProximityExpressionsProvider))]
internal class DefaultLSPProximityExpressionsProvider : LSPProximityExpressionsProvider
{
    private readonly LSPRequestInvoker _requestInvoker;

    private readonly ILogger _logger;

    [ImportingConstructor]
    public DefaultLSPProximityExpressionsProvider(
        LSPRequestInvoker requestInvoker,
        IRazorLoggerFactory loggerFactory)
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
        _logger = loggerFactory.CreateLogger<DefaultLSPProximityExpressionsProvider>();
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
            _logger.LogInformation("The proximity expressions could not be resolved.");
            return null;
        }

        return languageResponse.Expressions;
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
