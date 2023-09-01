// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.WrapWithTag;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by Visual Studio to wrap the current selection with a tag
    [JsonRpcMethod(LanguageServerConstants.RazorWrapWithTagEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalWrapWithTagResponse> RazorWrapWithTagAsync(VSInternalWrapWithTagParams wrapWithParams, CancellationToken cancellationToken)
    {
        // Same as in LanguageServerConstants, and in Web Tools
        const string HtmlWrapWithTagEndpoint = "textDocument/_vsweb_wrapWithTag";

        var response = new VSInternalWrapWithTagResponse(wrapWithParams.Range, Array.Empty<TextEdit>());

        var (synchronized, htmlDocument) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
            wrapWithParams.TextDocument.Version,
            wrapWithParams.TextDocument,
            cancellationToken);

        if (!synchronized)
        {
            Debug.Fail("Document was not synchronized");
            return response;
        }

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        var projectedUri = htmlDocument.Uri;

        // We call the Html language server to do the actual work here, now that we have the virtual document that they know about
        var request = new VSInternalWrapWithTagParams(
            wrapWithParams.Range,
            wrapWithParams.TagName,
            wrapWithParams.Options,
            new VersionedTextDocumentIdentifier() { Uri = projectedUri, });

        var textBuffer = htmlDocument.Snapshot.TextBuffer;
        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalWrapWithTagParams, VSInternalWrapWithTagResponse>(
            textBuffer,
            HtmlWrapWithTagEndpoint,
            languageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        if (result?.Response is not null)
        {
            response = result.Response;
        }

        return response;
    }
}
