// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal static class AddUsingsCodeActionProviderHelper
{
    public static async Task<TextEdit[]> GetUsingStatementEditsAsync(RazorCodeDocument codeDocument, SourceText originalCSharpText, SourceText changedCSharpText, CancellationToken cancellationToken)
    {
        // Now that we're done with everything, lets see if there are any using statements to fix up
        // We do this by comparing the original generated C# code, and the changed C# code, and look for a difference
        // in using statements. We can't use edits for this for two main reasons:
        //
        // 1. Using statements in the generated code might come from _Imports.razor, or from this file, and C# will shove them anywhere
        // 2. The edit might not be clean. eg given:
        //      using System;
        //      using System.Text;
        //    Adding "using System.Linq;" could result in an insert of "Linq;\r\nusing System." on line 2
        //
        // So because of the above, we look for a difference in C# using directive nodes directly from the C# syntax tree, and apply them manually
        // to the Razor document.

        var oldUsings = await FindUsingDirectiveStringsAsync(originalCSharpText, cancellationToken).ConfigureAwait(false);
        var newUsings = await FindUsingDirectiveStringsAsync(changedCSharpText, cancellationToken).ConfigureAwait(false);

        var edits = new List<TextEdit>();
        foreach (var usingStatement in newUsings.Except(oldUsings))
        {
            // This identifier will be eventually thrown away.
            Debug.Assert(codeDocument.Source.FilePath != null);
            var identifier = new OptionalVersionedTextDocumentIdentifier { Uri = new Uri(codeDocument.Source.FilePath, UriKind.Relative) };
            var workspaceEdit = AddUsingsCodeActionResolver.CreateAddUsingWorkspaceEdit(usingStatement, additionalEdit: null, codeDocument, codeDocumentIdentifier: identifier);
            edits.AddRange(workspaceEdit.DocumentChanges!.Value.First.First().Edits);
        }

        return edits.ToArray();
    }

    private static async Task<IEnumerable<string>> FindUsingDirectiveStringsAsync(SourceText originalCSharpText, CancellationToken cancellationToken)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(originalCSharpText, cancellationToken: cancellationToken);
        var syntaxRoot = await syntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);

        // We descend any compilation unit (ie, the file) or and namespaces because the compiler puts all usings inside
        // the namespace node.
        var usings = syntaxRoot.DescendantNodes(n => n is BaseNamespaceDeclarationSyntax or CompilationUnitSyntax)
            // Filter to using directives
            .OfType<UsingDirectiveSyntax>()
            // Select everything after the initial "using " part of the statement, and excluding the ending semi-colon. The
            // semi-colon is valid in Razor, but users find it surprising. This is slightly lazy, for sure, but has
            // the advantage of us not caring about changes to C# syntax, we just grab whatever Roslyn wanted to put in, so
            // we should still work in C# v26
            .Select(u => u.ToString()["using ".Length..^1]);

        return usings;
    }

    internal static readonly Regex AddUsingVSCodeAction = new Regex("@?using ([^;]+);?$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    // Internal for testing
    internal static string GetNamespaceFromFQN(string fullyQualifiedName)
    {
        if (!TrySplitNamespaceAndType(fullyQualifiedName.AsSpan(), out var namespaceName, out _))
        {
            return string.Empty;
        }

        return namespaceName.ToString();
    }

    internal static bool TryCreateAddUsingResolutionParams(string fullyQualifiedName, Uri uri, TextDocumentEdit? additionalEdit, [NotNullWhen(true)] out string? @namespace, [NotNullWhen(true)] out RazorCodeActionResolutionParams? resolutionParams)
    {
        @namespace = GetNamespaceFromFQN(fullyQualifiedName);
        if (string.IsNullOrEmpty(@namespace))
        {
            @namespace = null;
            resolutionParams = null;
            return false;
        }

        var actionParams = new AddUsingsCodeActionParams
        {
            Uri = uri,
            Namespace = @namespace,
            AdditionalEdit = additionalEdit
        };

        resolutionParams = new RazorCodeActionResolutionParams
        {
            Action = LanguageServerConstants.CodeActions.AddUsing,
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = actionParams,
        };

        return true;
    }

    /// <summary>
    /// Extracts the namespace from a C# add using statement provided by Visual Studio
    /// </summary>
    /// <param name="csharpAddUsing">Add using statement of the form `using System.X;`</param>
    /// <param name="namespace">Extract namespace `System.X`</param>
    /// <param name="prefix">The prefix to show, before the namespace, if any</param>
    /// <returns></returns>
    internal static bool TryExtractNamespace(string csharpAddUsing, out string @namespace, out string prefix)
    {
        // We must remove any leading/trailing new lines from the add using edit
        csharpAddUsing = csharpAddUsing.Trim();
        var regexMatchedTextEdit = AddUsingVSCodeAction.Match(csharpAddUsing);
        if (!regexMatchedTextEdit.Success ||

            // Two Regex matching groups are expected
            // 1. `using namespace;`
            // 2. `namespace`
            regexMatchedTextEdit.Groups.Count != 2)
        {
            // Text edit in an unexpected format
            @namespace = string.Empty;
            prefix = string.Empty;
            return false;
        }

        @namespace = regexMatchedTextEdit.Groups[1].Value;
        prefix = csharpAddUsing[..regexMatchedTextEdit.Index];
        return true;
    }

    internal static bool TrySplitNamespaceAndType(ReadOnlySpan<char> fullTypeName, out ReadOnlySpan<char> @namespace, out ReadOnlySpan<char> typeName)
    {
        @namespace = default;
        typeName = default;

        if (fullTypeName.IsEmpty)
        {
            return false;
        }

        var nestingLevel = 0;
        var splitLocation = -1;
        for (var i = fullTypeName.Length - 1; i >= 0; i--)
        {
            var c = fullTypeName[i];
            if (c == Type.Delimiter && nestingLevel == 0)
            {
                splitLocation = i;
                break;
            }
            else if (c == '>')
            {
                nestingLevel++;
            }
            else if (c == '<')
            {
                nestingLevel--;
            }
        }

        if (splitLocation == -1)
        {
            typeName = fullTypeName;
            return true;
        }

        @namespace = fullTypeName[..splitLocation];

        var typeNameStartLocation = splitLocation + 1;
        if (typeNameStartLocation < fullTypeName.Length)
        {
            typeName = fullTypeName[typeNameStartLocation..];
        }

        return true;
    }
}
