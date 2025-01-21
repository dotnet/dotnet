// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using RoslynTextEdit = Roslyn.LanguageServer.Protocol.TextEdit;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(IRoslynCodeActionHelpers)), Shared]
internal sealed class RoslynCodeActionHelpers : IRoslynCodeActionHelpers
{
    public Task<string> GetFormattedNewFileContentsAsync(IProjectSnapshot projectSnapshot, Uri csharpFileUri, string newFileContent, CancellationToken cancellationToken)
    {
        Debug.Assert(projectSnapshot is RemoteProjectSnapshot);
        var project = ((RemoteProjectSnapshot)projectSnapshot).Project;

        var document = project.AddDocument(RazorUri.GetDocumentFilePathFromUri(csharpFileUri), newFileContent);

        return ExternalHandlers.CodeActions.GetFormattedNewFileContentAsync(document, cancellationToken);
    }

    public async Task<TextEdit[]?> GetSimplifiedTextEditsAsync(DocumentContext documentContext, Uri? codeBehindUri, TextEdit edit, CancellationToken cancellationToken)
    {
        Debug.Assert(documentContext is RemoteDocumentContext);
        var context = (RemoteDocumentContext)documentContext;

        Document document;
        if (codeBehindUri is null)
        {
            // Edit is for inserting into the generated document
            document = await context.Snapshot.GetGeneratedDocumentAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            // Edit is for inserting into a C# document
            var solution = context.TextDocument.Project.Solution;
            var documentIds = solution.GetDocumentIdsWithUri(codeBehindUri);
            if (documentIds.Length == 0)
            {
                return null;
            }

            document = solution.GetRequiredDocument(documentIds.First(d => d.ProjectId == context.TextDocument.Project.Id));
        }

        var convertedEdit = JsonHelpers.ToRoslynLSP<RoslynTextEdit, TextEdit>(edit).AssumeNotNull();

        var edits = await ExternalHandlers.CodeActions.GetSimplifiedEditsAsync(document, convertedEdit, cancellationToken).ConfigureAwait(false);

        return JsonHelpers.ToVsLSP<TextEdit[], RoslynTextEdit[]>(edits);
    }
}
