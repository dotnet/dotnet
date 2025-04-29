// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    internal static int Count(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdit))
        {
            return textDocumentEdit.Length;
        }

        if (sumType.TryGetSecond(out var edits))
        {
            return edits.Length;
        }

        return Assumed.Unreachable<int>();
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile> ElementAt(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType, int elementIndex)
    {
        if (sumType.TryGetFirst(out var textDocumentEdits))
        {
            return textDocumentEdits[elementIndex];
        }

        if (sumType.TryGetSecond(out var edits))
        {
            return edits[elementIndex];
        }

        return Assumed.Unreachable<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[] ToArray(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdit))
        {
            return textDocumentEdit.Select(s => (SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>)s).ToArray();
        }

        if (sumType.TryGetSecond(out var edits))
        {
            return edits.ToArray();
        }

        return Assumed.Unreachable<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]>();
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile> First(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdits))
        {
            return textDocumentEdits.First();
        }

        if (sumType.TryGetSecond(out var edits))
        {
            return edits.First();
        }

        return Assumed.Unreachable<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();
    }

    internal static SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile> Last(this SumType<TextDocumentEdit[], SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>[]> sumType)
    {
        if (sumType.TryGetFirst(out var textDocumentEdits))
        {
            return textDocumentEdits.Last();
        }

        if (sumType.TryGetSecond(out var edits))
        {
            return edits.Last();
        }

        return Assumed.Unreachable<SumType<TextDocumentEdit, CreateFile, RenameFile, DeleteFile>>();
    }
}
