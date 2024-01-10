// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;

[Export(typeof(IDocumentColorService)), Shared]
[method: ImportingConstructor]
internal sealed class DocumentColorService() : IDocumentColorService
{
    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.DocumentColorProvider = new DocumentColorOptions();
    }

    public async Task<ColorInformation[]> GetColorInformationAsync(IClientConnection clientConnection, DocumentColorParams request, VersionedDocumentContext? documentContext, CancellationToken cancellationToken)
    {
        // Workaround for Web Tools bug https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1743689 where they sometimes
        // send requests for filenames that are stale, possibly due to adornment taggers being cached incorrectly (or caching
        // filenames incorrectly)
        if (documentContext is null)
        {
            return [];
        }

        var delegatedRequest = new DelegatedDocumentColorParams()
        {
            HostDocumentVersion = documentContext.Version,
            TextDocument = request.TextDocument
        };

        var documentColors = await clientConnection.SendRequestAsync<DelegatedDocumentColorParams, ColorInformation[]>(
            CustomMessageNames.RazorProvideHtmlDocumentColorEndpoint,
            delegatedRequest,
            cancellationToken).ConfigureAwait(false);

        if (documentColors is null)
        {
            return [];
        }

        // HTML and Razor documents have identical mapping locations. Because of this we can return the result as-is.
        return documentColors;
    }
}
