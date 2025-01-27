// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions.Razor;

internal class GenerateMethodCodeActionResolver(
    IRoslynCodeActionHelpers roslynCodeActionHelpers,
    IDocumentMappingService documentMappingService,
    IRazorFormattingService razorFormattingService,
    IFileSystem fileSystem) : IRazorCodeActionResolver
{
    private readonly IRoslynCodeActionHelpers _roslynCodeActionHelpers = roslynCodeActionHelpers;
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService;
    private readonly IFileSystem _fileSystem = fileSystem;

    private const string BeginningIndents = $"{FormattingUtilities.InitialIndent}{FormattingUtilities.Indent}";

    public string Action => LanguageServerConstants.CodeActions.GenerateEventHandler;

    public async Task<WorkspaceEdit?> ResolveAsync(DocumentContext documentContext, JsonElement data, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<GenerateMethodCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var code = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var uriPath = FilePathNormalizer.Normalize(documentContext.Uri.GetAbsoluteOrUNCPath());
        var razorClassName = Path.GetFileNameWithoutExtension(uriPath);
        var codeBehindPath = $"{uriPath}.cs";

        if (!_fileSystem.FileExists(codeBehindPath) ||
            razorClassName is null ||
            !code.TryComputeNamespace(fallbackToRootNamespace: true, out var razorNamespace))
        {
            return await GenerateMethodInCodeBlockAsync(
                code,
                actionParams,
                documentContext,
                razorNamespace: null,
                razorClassName,
                options,
                cancellationToken).ConfigureAwait(false);
        }

        // TODO: Update IFileSystem.ReadFile(...) to return a SourceText without reading a huge string.
        var content = _fileSystem.ReadFile(codeBehindPath);
        var text = SourceText.From(content, Encoding.UTF8);
        if (GetCSharpClassDeclarationSyntax(text, razorNamespace, razorClassName) is not { } @class)
        {
            // The code behind file is malformed, generate the code in the razor file instead.
            return await GenerateMethodInCodeBlockAsync(
                code,
                actionParams,
                documentContext,
                razorNamespace,
                razorClassName,
                options,
                cancellationToken).ConfigureAwait(false);
        }

        var codeBehindUri = VsLspFactory.CreateFilePathUri(codeBehindPath);

        var codeBehindTextDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier() { Uri = codeBehindUri };

        var templateWithMethodSignature = PopulateMethodSignature(actionParams);
        var classLocationLineSpan = @class.GetLocation().GetLineSpan();
        var formattedMethod = FormattingUtilities.AddIndentationToMethod(
            templateWithMethodSignature,
            options.TabSize,
            options.InsertSpaces,
            @class.SpanStart,
            classLocationLineSpan.StartLinePosition.Character,
            text);

        var edit = VsLspFactory.CreateTextEdit(
            line: classLocationLineSpan.EndLinePosition.Line,
            character: 0,
            $"{formattedMethod}{Environment.NewLine}");

        var result = await _roslynCodeActionHelpers.GetSimplifiedTextEditsAsync(documentContext, codeBehindUri, edit, cancellationToken).ConfigureAwait(false);

        var codeBehindTextDocEdit = new TextDocumentEdit()
        {
            TextDocument = codeBehindTextDocumentIdentifier,
            Edits = result ?? [edit]
        };

        return new WorkspaceEdit() { DocumentChanges = new[] { codeBehindTextDocEdit } };
    }

    private async Task<WorkspaceEdit> GenerateMethodInCodeBlockAsync(
        RazorCodeDocument code,
        GenerateMethodCodeActionParams actionParams,
        DocumentContext documentContext,
        string? razorNamespace,
        string? razorClassName,
        RazorFormattingOptions options,
        CancellationToken cancellationToken)
    {
        var templateWithMethodSignature = PopulateMethodSignature(actionParams);
        var edits = CodeBlockService.CreateFormattedTextEdit(code, templateWithMethodSignature, options);

        // If there are 3 edits, this means that there is no existing @code block, so we have an edit for '@code {', the method stub, and '}'.
        // Otherwise, a singular edit means that an @code block does exist and the only edit is adding the method stub.
        var editToSendToRoslyn = edits.Length == 3 ? edits[1] : edits[0];
        if (edits.Length == 3
            && razorClassName is not null
            && (razorNamespace is not null || code.TryComputeNamespace(fallbackToRootNamespace: true, out razorNamespace))
            && GetCSharpClassDeclarationSyntax(code.GetOrParseCSharpSyntaxTree(cancellationToken), razorNamespace, razorClassName) is { } @class)
        {
            // There is no existing @code block. This means that there is no code block source mapping in the generated C# document
            // to place the code, so we cannot utilize the document mapping service and the formatting service.
            // We are going to arbitrarily place the method at the end of the class in the generated C# file to
            // just get the simplified text that comes back from Roslyn.

            var classLocationLineSpan = @class.GetLocation().GetLineSpan();
            var tempTextEdit = VsLspFactory.CreateTextEdit(
                line: classLocationLineSpan.EndLinePosition.Line,
                character: 0,
                editToSendToRoslyn.NewText);

            var result = await _roslynCodeActionHelpers.GetSimplifiedTextEditsAsync(documentContext, codeBehindUri: null, tempTextEdit, cancellationToken).ConfigureAwait(false);

            // Roslyn should have passed back 2 edits. One that contains the simplified method stub and the other that contains the new
            // location for the class end brace since we had asked to insert the method stub at the original class end brace location.
            // We will only use the edit that contains the method stub.
            Debug.Assert(result is null || result.Length == 2, $"Unexpected response to {CustomMessageNames.RazorSimplifyMethodEndpointName} from Roslyn");
            var simplificationEdit = result?.FirstOrDefault(edit => edit.NewText.Contains("private"));
            if (simplificationEdit is not null)
            {
                // Roslyn will have removed the beginning formatting, put it back.
                var formatting = editToSendToRoslyn.NewText[0..editToSendToRoslyn.NewText.IndexOf("private")];
                editToSendToRoslyn.NewText = $"{formatting}{simplificationEdit.NewText.TrimEnd()}";
            }
        }
        else if (_documentMappingService.TryMapToGeneratedDocumentRange(code.GetCSharpDocument(), editToSendToRoslyn.Range, out var remappedRange))
        {
            // If the call to Roslyn is successful, the razor formatting service will format incorrectly if our manual formatting is present,
            // strip our manual formatting from the method so we just have a valid method signature.
            var unformattedMethodSignature = templateWithMethodSignature
                .Replace(FormattingUtilities.InitialIndent, string.Empty)
                .Replace(FormattingUtilities.Indent, string.Empty);

            var remappedEdit = VsLspFactory.CreateTextEdit(remappedRange, unformattedMethodSignature);
            var result = await _roslynCodeActionHelpers.GetSimplifiedTextEditsAsync(documentContext, codeBehindUri: null, remappedEdit, cancellationToken).ConfigureAwait(false);

            if (result is not null)
            {
                var formattingOptions = new RazorFormattingOptions()
                {
                    TabSize = options.TabSize,
                    InsertSpaces = options.InsertSpaces,
                    CodeBlockBraceOnNextLine = options.CodeBlockBraceOnNextLine
                };

                var formattedChange = await _razorFormattingService.TryGetCSharpCodeActionEditAsync(
                    documentContext,
                    result.SelectAsArray(code.GetCSharpSourceText().GetTextChange),
                    formattingOptions,
                    cancellationToken).ConfigureAwait(false);

                edits = formattedChange is { } change ? [code.Source.Text.GetTextEdit(change)] : [];
            }
        }

        var razorTextDocEdit = new TextDocumentEdit()
        {
            TextDocument = new OptionalVersionedTextDocumentIdentifier() { Uri = documentContext.Uri },
            Edits = edits,
        };

        return new WorkspaceEdit() { DocumentChanges = new[] { razorTextDocEdit } };
    }

    private static string PopulateMethodSignature(GenerateMethodCodeActionParams actionParams)
    {
        var returnType = actionParams.IsAsync
            ? "global::System.Threading.Tasks.Task"
            : "void";

        var parameters = actionParams.EventParameterType is null
            ? string.Empty // Couldn't find the params, generate no params instead.
            : $"global::{actionParams.EventParameterType} args";

        return $$"""
            {{BeginningIndents}}private {{returnType}} {{actionParams.MethodName}}({{parameters}})
            {{BeginningIndents}}{
            {{BeginningIndents}}{{FormattingUtilities.Indent}}throw new global::System.NotImplementedException();
            {{BeginningIndents}}}
            """;
    }

    private static ClassDeclarationSyntax? GetCSharpClassDeclarationSyntax(SourceText csharpContent, string razorNamespace, string razorClassName)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(csharpContent);
        return GetCSharpClassDeclarationSyntax(syntaxTree, razorNamespace, razorClassName);
    }

    private static ClassDeclarationSyntax? GetCSharpClassDeclarationSyntax(SyntaxTree csharpSyntaxTree, string razorNamespace, string razorClassName)
    {
        var compilationUnit = csharpSyntaxTree.GetCompilationUnitRoot();
        var @namespace = compilationUnit.Members
            .FirstOrDefault(m => m is BaseNamespaceDeclarationSyntax { } @namespace && @namespace.Name.ToString() == razorNamespace);
        if (@namespace is null)
        {
            return null;
        }

        var @class = ((BaseNamespaceDeclarationSyntax)@namespace).Members
            .FirstOrDefault(m => m is ClassDeclarationSyntax { } @class && razorClassName == @class.Identifier.Text);
        if (@class is null)
        {
            return null;
        }

        return (ClassDeclarationSyntax)@class;
    }
}
