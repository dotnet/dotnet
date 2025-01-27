// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal class PromoteUsingCodeActionResolver(IFileSystem fileSystem) : IRazorCodeActionResolver
{
    private readonly IFileSystem _fileSystem = fileSystem;

    public string Action => LanguageServerConstants.CodeActions.PromoteUsingDirective;

    public async Task<WorkspaceEdit?> ResolveAsync(DocumentContext documentContext, JsonElement data, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<PromoteToUsingCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);

        var importsFileName = PromoteUsingCodeActionProvider.GetImportsFileName(documentContext.FileKind);

        var file = FilePathNormalizer.Normalize(documentContext.Uri.GetAbsoluteOrUNCPath());
        var folder = Path.GetDirectoryName(file).AssumeNotNull();
        var importsFile = Path.GetFullPath(Path.Combine(folder, "..", importsFileName));
        var importFileUri = VsLspFactory.CreateFilePathUri(importsFile);

        using var edits = new PooledArrayBuilder<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();

        var textToInsert = sourceText.GetSubTextString(TextSpan.FromBounds(actionParams.UsingStart, actionParams.UsingEnd));
        var insertLocation = new LinePosition(0, 0);
        if (!_fileSystem.FileExists(importsFile))
        {
            edits.Add(new CreateFile() { Uri = importFileUri });
        }
        else
        {
            // TODO: Update IFileSystem.ReadFile(...) to return a SourceText without reading a huge string.
            var st = SourceText.From(_fileSystem.ReadFile(importsFile));
            var lastLine = st.Lines[^1];
            insertLocation = new LinePosition(lastLine.LineNumber, 0);
            if (lastLine.GetFirstNonWhitespaceOffset() is { } nonWhiteSpaceOffset)
            {
                // Last line isn't blank, so add a newline, and insert at the end
                textToInsert = Environment.NewLine + textToInsert;
                insertLocation = new LinePosition(insertLocation.Line, lastLine.SpanIncludingLineBreak.Length);
            }
        }

        edits.Add(new TextDocumentEdit
        {
            TextDocument = new OptionalVersionedTextDocumentIdentifier() { Uri = importFileUri },
            Edits = [VsLspFactory.CreateTextEdit(insertLocation, textToInsert)]
        });

        var removeRange = sourceText.GetRange(actionParams.RemoveStart, actionParams.RemoveEnd);

        edits.Add(new TextDocumentEdit
        {
            TextDocument = new OptionalVersionedTextDocumentIdentifier() { Uri = documentContext.Uri },
            Edits = [VsLspFactory.CreateTextEdit(removeRange, string.Empty)]
        });

        return new WorkspaceEdit
        {
            DocumentChanges = edits.ToArray()
        };
    }
}
