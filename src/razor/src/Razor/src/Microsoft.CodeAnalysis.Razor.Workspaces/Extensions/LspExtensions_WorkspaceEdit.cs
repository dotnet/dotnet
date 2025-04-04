// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static bool TryGetTextDocumentEdits(this WorkspaceEdit workspaceEdit, [NotNullWhen(true)] out TextDocumentEdit[]? textDocumentEdits)
    {
        if (workspaceEdit.DocumentChanges?.Value is TextDocumentEdit[] documentEdits)
        {
            textDocumentEdits = documentEdits;
            return true;
        }

        if (workspaceEdit.DocumentChanges?.Value is SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[] sumTypeArray)
        {
            using var builder = new PooledArrayBuilder<TextDocumentEdit>();
            foreach (var sumType in sumTypeArray)
            {
                if (sumType.Value is TextDocumentEdit textDocumentEdit)
                {
                    builder.Add(textDocumentEdit);
                }
            }

            if (builder.Count > 0)
            {
                textDocumentEdits = builder.ToArray();
                return true;
            }
        }

        textDocumentEdits = null;
        return false;
    }
}
