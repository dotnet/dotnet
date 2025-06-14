// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Razor.CodeActions;

internal class AddUsingsCodeActionResolver : IRazorCodeActionResolver
{
    public string Action => LanguageServerConstants.CodeActions.AddUsing;

    public async Task<WorkspaceEdit?> ResolveAsync(DocumentContext documentContext, JsonElement data, RazorFormattingOptions options, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<AddUsingsCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var documentSnapshot = documentContext.Snapshot;

        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier() { Uri = documentContext.Uri };

        using var documentChanges = new PooledArrayBuilder<TextDocumentEdit>();

        // Need to add the additional edit first, as the actual usings go at the top of the file, and would
        // change the ranges needed in the additional edit if they went in first
        if (actionParams.AdditionalEdit is not null)
        {
            documentChanges.Add(actionParams.AdditionalEdit);
        }

        documentChanges.Add(new TextDocumentEdit()
        {
            TextDocument = codeDocumentIdentifier,
            Edits = [AddUsingsHelper.CreateAddUsingTextEdit(actionParams.Namespace, codeDocument)]
        });

        return new WorkspaceEdit()
        {
            DocumentChanges = documentChanges.ToArray(),
        };
    }

    internal static bool TryCreateAddUsingResolutionParams(string fullyQualifiedName, VSTextDocumentIdentifier textDocument, TextDocumentEdit? additionalEdit, Uri? delegatedDocumentUri, [NotNullWhen(true)] out string? @namespace, [NotNullWhen(true)] out RazorCodeActionResolutionParams? resolutionParams)
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
            Namespace = @namespace,
            AdditionalEdit = additionalEdit
        };

        resolutionParams = new RazorCodeActionResolutionParams
        {
            TextDocument = textDocument,
            Action = LanguageServerConstants.CodeActions.AddUsing,
            Language = RazorLanguageKind.Razor,
            DelegatedDocumentUri = delegatedDocumentUri,
            Data = actionParams,
        };

        return true;
    }

    // Internal for testing
    internal static string GetNamespaceFromFQN(string fullyQualifiedName)
    {
        if (!TrySplitNamespaceAndType(fullyQualifiedName.AsSpan(), out var namespaceName, out _))
        {
            return string.Empty;
        }

        return namespaceName.ToString();
    }

    private static bool TrySplitNamespaceAndType(ReadOnlySpan<char> fullTypeName, out ReadOnlySpan<char> @namespace, out ReadOnlySpan<char> typeName)
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
