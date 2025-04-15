// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.AutoInsert;

internal interface IAutoInsertService
{
    ImmutableArray<string> TriggerCharacters { get; }

    bool TryResolveInsertion(
        RazorCodeDocument codeDocument,
        Position position,
        string character,
        bool autoCloseTags,
        [NotNullWhen(true)] out VSInternalDocumentOnAutoInsertResponseItem? insertTextEdit);
}
