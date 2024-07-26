// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static bool TryGetDocumentChanges(this WorkspaceEdit workspaceEdit, [NotNullWhen(true)] out TextDocumentEdit[]? documentChanges)
    {
        if (workspaceEdit.DocumentChanges?.Value is TextDocumentEdit[] documentEdits)
        {
            documentChanges = documentEdits;
            return true;
        }

        if (workspaceEdit.DocumentChanges?.Value is SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[] sumTypeArray)
        {
            var documentEditList = new List<TextDocumentEdit>();
            foreach (var sumType in sumTypeArray)
            {
                if (sumType.Value is TextDocumentEdit textDocumentEdit)
                {
                    documentEditList.Add(textDocumentEdit);
                }
            }

            if (documentEditList.Count > 0)
            {
                documentChanges = documentEditList.ToArray();
                return true;
            }
        }

        documentChanges = null;
        return false;
    }
}
