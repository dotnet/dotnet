// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class RoslynCodeActionHelpers(IClientConnection clientConnection) : IRoslynCodeActionHelpers
{
    private static readonly Lazy<Workspace> s_workspace = new Lazy<Workspace>(() => new AdhocWorkspace());

    private readonly IClientConnection _clientConnection = clientConnection;

    public async Task<string> GetFormattedNewFileContentsAsync(IProjectSnapshot projectSnapshot, Uri csharpFileUri, string newFileContent, CancellationToken cancellationToken)
    {
        var parameters = new FormatNewFileParams()
        {
            Project = new TextDocumentIdentifier
            {
                Uri = new Uri(projectSnapshot.FilePath, UriKind.Absolute)
            },
            Document = new TextDocumentIdentifier
            {
                Uri = csharpFileUri
            },
            Contents = newFileContent
        };

        var fixedContent = await _clientConnection.SendRequestAsync<FormatNewFileParams, string?>(CustomMessageNames.RazorFormatNewFileEndpointName, parameters, cancellationToken).ConfigureAwait(false);

        if (fixedContent is not null)
        {
            return fixedContent;
        }

        // Sadly we can't use a "real" workspace here, because we don't have access. If we use our workspace, it wouldn't have the right settings
        // for C# formatting, only Razor formatting, and we have no access to Roslyn's real workspace, since it could be in another process.
        var node = await CSharpSyntaxTree.ParseText(newFileContent, cancellationToken: cancellationToken).GetRootAsync(cancellationToken).ConfigureAwait(false);
        node = Formatter.Format(node, s_workspace.Value, cancellationToken: cancellationToken);

        return node.ToFullString();
    }

    public Task<TextEdit[]?> GetSimplifiedTextEditsAsync(DocumentContext documentContext, Uri? codeBehindUri, TextEdit edit, CancellationToken cancellationToken)
    {
        var tdi = codeBehindUri is null
            ? documentContext.GetTextDocumentIdentifierAndVersion()
            : new TextDocumentIdentifierAndVersion(new TextDocumentIdentifier() { Uri = codeBehindUri }, 1);
        var delegatedParams = new DelegatedSimplifyMethodParams(
            tdi,
            RequiresVirtualDocument: codeBehindUri == null,
            edit);

        return _clientConnection.SendRequestAsync<DelegatedSimplifyMethodParams, TextEdit[]?>(
            CustomMessageNames.RazorSimplifyMethodEndpointName,
            delegatedParams,
            cancellationToken);
    }
}
