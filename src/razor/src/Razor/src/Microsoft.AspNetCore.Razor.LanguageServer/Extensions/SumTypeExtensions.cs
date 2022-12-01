// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class SumTypeExtensions
{
    internal static int Count(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdit))
        {
            return textDocumentEdit.Length;
        }
        else if (sumType.TryGetSecond(out var edits))
        {
            return edits.Length;
        }
        else
        {
            throw new NotImplementedException("Shouldn't be possible");
        }
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile> ElementAt(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType, int elementIndex)
    {
        if (sumType.TryGetFirst(out var textDocumentEdits))
        {
            return textDocumentEdits[elementIndex];
        }
        else if (sumType.TryGetSecond(out var edits))
        {
            return edits[elementIndex];
        }
        else
        {
            throw new NotImplementedException("Shouldn't be possible");
        }
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[] ToArray(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdit))
        {
            return textDocumentEdit.Select(s => (SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>)s).ToArray();
        }
        else if (sumType.TryGetSecond(out var edits))
        {
            return edits.ToArray();
        }
        else
        {
            throw new NotImplementedException("Shouldn't be possible");
        }
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile> First(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdits))
        {
            return textDocumentEdits.First();
        }
        else if (sumType.TryGetSecond(out var edits))
        {
            return edits.First();
        }
        else
        {
            throw new NotImplementedException("Shouldn't be possible");
        }
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile> Last(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdits))
        {
            return textDocumentEdits.Last();
        }
        else if (sumType.TryGetSecond(out var edits))
        {
            return edits.Last();
        }
        else
        {
            throw new NotImplementedException("Shouldn't be possible");
        }
    }
}
