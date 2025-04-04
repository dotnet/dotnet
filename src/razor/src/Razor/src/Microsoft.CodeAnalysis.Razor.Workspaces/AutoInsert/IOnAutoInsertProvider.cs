// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.AutoInsert;

internal interface IOnAutoInsertProvider : IOnAutoInsertTriggerCharacterProvider
{
    bool TryResolveInsertion(
        Position position,
        RazorCodeDocument codeDocument,
        bool enableAutoClosingTags,
        [NotNullWhen(true)] out VSInternalDocumentOnAutoInsertResponseItem? autoInsertEdit);
}
