// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

internal interface IOnAutoInsertProvider
{
    string TriggerCharacter { get; }

    bool TryResolveInsertion(Position position, FormattingContext context, [NotNullWhen(true)] out TextEdit? edit, out InsertTextFormat format);
}
