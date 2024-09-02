// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class AddUsingsCodeActionResolver(IDocumentContextFactory documentContextFactory) : IRazorCodeActionResolver
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;

    public string Action => LanguageServerConstants.CodeActions.AddUsing;

    public async Task<WorkspaceEdit?> ResolveAsync(JsonElement data, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<AddUsingsCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        if (!_documentContextFactory.TryCreate(actionParams.Uri, out var documentContext))
        {
            return null;
        }

        var documentSnapshot = documentContext.Snapshot;

        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier() { Uri = actionParams.Uri };
        return AddUsingsHelper.CreateAddUsingWorkspaceEdit(actionParams.Namespace, actionParams.AdditionalEdit, codeDocument, codeDocumentIdentifier);
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
