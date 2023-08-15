// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorProjectContextsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSProjectContextList?> ProjectContextsAsync(DelegatedProjectContextsParams request, CancellationToken cancellationToken)
    {
        var hostDocument = request.Identifier.TextDocumentIdentifier;
        var (synchronized, virtualDocument) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
            _documentManager,
            request.Identifier.Version,
            hostDocument,
            cancellationToken).ConfigureAwait(false);

        if (!synchronized)
        {
            return null;
        }

        var projectContextParams = new VSGetProjectContextsParams()
        {
            TextDocument = new TextDocumentItem()
            {
                LanguageId = CodeAnalysis.LanguageNames.CSharp,
                Uri = virtualDocument.Uri,
                Version = virtualDocument.Snapshot.Version.VersionNumber,
                Text = virtualDocument.Snapshot.GetText(),
            }
        };

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSGetProjectContextsParams, VSProjectContextList?>(
            virtualDocument.Snapshot.TextBuffer,
            VSMethods.GetProjectContextsName,
            RazorLSPConstants.RazorCSharpLanguageServerName,
            projectContextParams,
            cancellationToken).ConfigureAwait(false);

        return response?.Response;
    }
}
