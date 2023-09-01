// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Folding;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorFoldingRangeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<RazorFoldingRangeResponse?> ProvideFoldingRangesAsync(RazorFoldingRangeRequestParam foldingRangeParams, CancellationToken cancellationToken)
    {
        if (foldingRangeParams is null)
        {
            throw new ArgumentNullException(nameof(foldingRangeParams));
        }

        // Normally we don't like to construct POCOs directly, because it removes potentially unknown data that has been
        // deserialized from the JSON request. To ensure we don't do that we modify the request object (see WithUri call below)
        // but in this case, where we asynchronously fire off two requests, that introduces a problem as we can end up modifying
        // the object before its been used to synchronize one of the virtual documents.
        // We're okay to construct this object _in this specific scenario_ because we know it is only used to synchronize
        // requests inside Razor, and we only use ProjectContext and Uri to do that.
        var hostDocument = new VSTextDocumentIdentifier
        {
            ProjectContext = foldingRangeParams.TextDocument.GetProjectContext(),
            Uri = foldingRangeParams.TextDocument.Uri,
        };

        var csharpRanges = new List<FoldingRange>();
        var csharpTask = Task.Run(async () =>
        {
            var (synchronized, csharpSnapshot) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(foldingRangeParams.HostDocumentVersion, hostDocument, cancellationToken);

            if (synchronized)
            {
                var csharpRequestParams = new FoldingRangeParams()
                {
                    TextDocument = foldingRangeParams.TextDocument.WithUri(csharpSnapshot.Uri),
                };

                var request = await _requestInvoker.ReinvokeRequestOnServerAsync<FoldingRangeParams, IEnumerable<FoldingRange>?>(
                    csharpSnapshot.Snapshot.TextBuffer,
                    Methods.TextDocumentFoldingRange.Name,
                    RazorLSPConstants.RazorCSharpLanguageServerName,
                    SupportsFoldingRange,
                    csharpRequestParams,
                    cancellationToken).ConfigureAwait(false);

                var result = request?.Response;
                if (result is null)
                {
                    csharpRanges = null;
                }
                else
                {
                    csharpRanges.AddRange(result);
                }
            }
        }, cancellationToken);

        var htmlRanges = new List<FoldingRange>();
        var htmlTask = Task.CompletedTask;
        htmlTask = Task.Run(async () =>
        {
            var (synchronized, htmlDocument) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(foldingRangeParams.HostDocumentVersion, hostDocument, cancellationToken);

            if (synchronized)
            {
                var htmlRequestParams = new FoldingRangeParams()
                {
                    TextDocument = new()
                    {
                        Uri = htmlDocument.Uri
                    }
                };

                var request = await _requestInvoker.ReinvokeRequestOnServerAsync<FoldingRangeParams, IEnumerable<FoldingRange>?>(
                    htmlDocument.Snapshot.TextBuffer,
                    Methods.TextDocumentFoldingRange.Name,
                    RazorLSPConstants.HtmlLanguageServerName,
                    SupportsFoldingRange,
                    htmlRequestParams,
                    cancellationToken).ConfigureAwait(false);

                var result = request?.Response;
                if (result is null)
                {
                    htmlRanges = null;
                }
                else
                {
                    htmlRanges.AddRange(result);
                }
            }
        }, cancellationToken);

        try
        {
            await Task.WhenAll(htmlTask, csharpTask).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Return null if any of the tasks getting folding ranges
            // results in an error
            return null;
        }

        // Since VS FoldingRanges doesn't poll once it has a non-null result returning a partial result can lock us
        // into an incomplete view until we edit the document. Better to wait for the other server to be ready.
        if (htmlRanges is null || csharpRanges is null)
        {
            return null;
        }

        return new(htmlRanges.ToImmutableArray(), csharpRanges.ToImmutableArray());
    }

    private static bool SupportsFoldingRange(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        var supportsFoldingRange = serverCapabilities?.FoldingRangeProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;

        return supportsFoldingRange;
    }
}
