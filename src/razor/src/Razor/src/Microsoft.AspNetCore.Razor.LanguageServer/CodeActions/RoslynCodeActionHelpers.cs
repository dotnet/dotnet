// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class RoslynCodeActionHelpers(IClientConnection clientConnection) : IRoslynCodeActionHelpers
{
    private readonly IClientConnection _clientConnection = clientConnection;

    public Task<string?> GetFormattedNewFileContentsAsync(string projectFilePath, Uri csharpFileUri, string newFileContent, CancellationToken cancellationToken)
    {
        var parameters = new FormatNewFileParams()
        {
            Project = new TextDocumentIdentifier
            {
                Uri = new Uri(projectFilePath, UriKind.Absolute)
            },
            Document = new TextDocumentIdentifier
            {
                Uri = csharpFileUri
            },
            Contents = newFileContent
        };
        return _clientConnection.SendRequestAsync<FormatNewFileParams, string?>(CustomMessageNames.RazorFormatNewFileEndpointName, parameters, cancellationToken);
    }

    public Task<TextEdit[]?> GetSimplifiedTextEditsAsync(Uri codeBehindUri, TextEdit edit, bool requiresVirtualDocument, CancellationToken cancellationToken)
    {
        var delegatedParams = new DelegatedSimplifyMethodParams(
            new TextDocumentIdentifierAndVersion(new TextDocumentIdentifier() { Uri = codeBehindUri }, 1),
            requiresVirtualDocument,
            edit);

        return _clientConnection.SendRequestAsync<DelegatedSimplifyMethodParams, TextEdit[]?>(
            CustomMessageNames.RazorSimplifyMethodEndpointName,
            delegatedParams,
            cancellationToken);
    }
}
