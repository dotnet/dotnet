// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using CSharpSyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using CSharpSyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal class ExtractToCodeBehindCodeActionResolver : RazorCodeActionResolver
{
    private readonly DocumentContextFactory _documentContextFactory;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    public ExtractToCodeBehindCodeActionResolver(
        DocumentContextFactory documentContextFactory,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    public override string Action => LanguageServerConstants.CodeActions.ExtractToCodeBehindAction;

    public override async Task<WorkspaceEdit?> ResolveAsync(JObject data, CancellationToken cancellationToken)
    {
        if (data is null)
        {
            return null;
        }

        var actionParams = data.ToObject<ExtractToCodeBehindCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var path = FilePathNormalizer.Normalize(actionParams.Uri.GetAbsoluteOrUNCPath());

        var documentContext = await _documentContextFactory.TryCreateAsync(actionParams.Uri, cancellationToken).ConfigureAwait(false);
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        if (!FileKinds.IsComponent(codeDocument.GetFileKind()))
        {
            return null;
        }

        var codeBehindPath = GenerateCodeBehindPath(path);

        // VS Code in Windows expects path to start with '/'
        var updatedCodeBehindPath = _languageServerFeatureOptions.ReturnCodeActionAndRenamePathsWithPrefixedSlash && !codeBehindPath.StartsWith("/")
            ? '/' + codeBehindPath
            : codeBehindPath;

        var codeBehindUri = new UriBuilder
        {
            Scheme = Uri.UriSchemeFile,
            Path = updatedCodeBehindPath,
            Host = string.Empty,
        }.Uri;

        var text = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (text is null)
        {
            return null;
        }

        var className = Path.GetFileNameWithoutExtension(path);
        var codeBlockContent = text.GetSubTextString(new CodeAnalysis.Text.TextSpan(actionParams.ExtractStart, actionParams.ExtractEnd - actionParams.ExtractStart));
        var codeBehindContent = GenerateCodeBehindClass(className, actionParams.Namespace, codeBlockContent, codeDocument);

        var start = codeDocument.Source.Lines.GetLocation(actionParams.RemoveStart);
        var end = codeDocument.Source.Lines.GetLocation(actionParams.RemoveEnd);
        var removeRange = new Range
        {
            Start = new Position(start.LineIndex, start.CharacterIndex),
            End = new Position(end.LineIndex, end.CharacterIndex)
        };

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = actionParams.Uri };
        var codeBehindDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier { Uri = codeBehindUri };

        var documentChanges = new SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]
        {
            new CreateFile { Uri = codeBehindUri },
            new TextDocumentEdit
            {
                TextDocument = codeDocumentIdentifier,
                Edits = new[]
                {
                    new TextEdit
                    {
                        NewText = string.Empty,
                        Range = removeRange,
                    }
                },
            },
            new TextDocumentEdit
            {
                TextDocument = codeBehindDocumentIdentifier,
                Edits  = new[]
                {
                    new TextEdit
                    {
                        NewText = codeBehindContent,
                        Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) },
                    }
                },
            }
        };

        return new WorkspaceEdit
        {
            DocumentChanges = documentChanges,
        };
    }

    /// <summary>
    /// Generate a file path with adjacent to our input path that has the
    /// correct codebehind extension, using numbers to differentiate from
    /// any collisions.
    /// </summary>
    /// <param name="path">The origin file path.</param>
    /// <returns>A non-existent file path with the same base name and a codebehind extension.</returns>
    private string GenerateCodeBehindPath(string path)
    {
        var n = 0;
        string codeBehindPath;
        do
        {
            var identifier = n > 0 ? n.ToString(CultureInfo.InvariantCulture) : string.Empty;  // Make it look nice
            var directoryName = Path.GetDirectoryName(path);
            Assumes.NotNull(directoryName);

            codeBehindPath = Path.Combine(
                directoryName,
                $"{Path.GetFileNameWithoutExtension(path)}{identifier}{Path.GetExtension(path)}.cs");
            n++;
        }
        while (File.Exists(codeBehindPath));

        return codeBehindPath;
    }

    /// <summary>
    /// Determine all explicit and implicit using statements in the code
    /// document using the intermediate node.
    /// </summary>
    /// <param name="razorCodeDocument">The code document to analyze.</param>
    /// <returns>An enumerable of the qualified namespaces.</returns>
    private static IEnumerable<string> FindUsings(RazorCodeDocument razorCodeDocument)
    {
        return razorCodeDocument
            .GetDocumentIntermediateNode()
            .FindDescendantNodes<UsingDirectiveIntermediateNode>()
            .Select(n => n.Content);
    }

    /// <summary>
    /// Generate a complete C# compilation unit containing a partial class
    /// with the given name, body contents, and the namespace and all
    /// usings from the existing code document.
    /// </summary>
    /// <param name="className">Name of the resultant partial class.</param>
    /// <param name="namespaceName">Name of the namespace to put the reusltant class in.</param>
    /// <param name="contents">Class body contents.</param>
    /// <param name="razorCodeDocument">Existing code document we're extracting from.</param>
    /// <returns></returns>
    private static string GenerateCodeBehindClass(string className, string namespaceName, string contents, RazorCodeDocument razorCodeDocument)
    {
        var mock = (ClassDeclarationSyntax)CSharpSyntaxFactory.ParseMemberDeclaration($"class Class {contents}")!;
        var @class = CSharpSyntaxFactory
            .ClassDeclaration(className)
            .AddModifiers(CSharpSyntaxFactory.Token(CSharpSyntaxKind.PublicKeyword), CSharpSyntaxFactory.Token(CSharpSyntaxKind.PartialKeyword))
            .AddMembers(mock.Members.ToArray());

        var @namespace = CSharpSyntaxFactory
            .NamespaceDeclaration(CSharpSyntaxFactory.ParseName(namespaceName))
            .AddMembers(@class);

        var usings = FindUsings(razorCodeDocument)
            .Select(u => CSharpSyntaxFactory.UsingDirective(CSharpSyntaxFactory.ParseName(u)))
            .ToArray();
        var compilationUnit = CSharpSyntaxFactory
            .CompilationUnit()
            .AddUsings(usings)
            .AddMembers(@namespace);

        return compilationUnit.NormalizeWhitespace().ToFullString();
    }
}
