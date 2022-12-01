// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal struct FormattingResult
{
    public FormattingResult(TextEdit[] edits, RazorLanguageKind kind = RazorLanguageKind.Razor)
    {
        if (edits is null)
        {
            throw new ArgumentNullException(nameof(edits));
        }

        Edits = edits;
        Kind = kind;
    }

    public TextEdit[] Edits { get; }

    public RazorLanguageKind Kind { get; }
}
