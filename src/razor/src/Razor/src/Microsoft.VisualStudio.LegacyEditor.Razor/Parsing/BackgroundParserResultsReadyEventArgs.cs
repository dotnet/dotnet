// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language;
using static Microsoft.VisualStudio.LegacyEditor.Razor.Parsing.BackgroundParser;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

internal class BackgroundParserResultsReadyEventArgs : EventArgs
{
    public BackgroundParserResultsReadyEventArgs(ChangeReference edit, RazorCodeDocument codeDocument)
    {
        if (edit is null)
        {
            throw new ArgumentNullException(nameof(edit));
        }

        if (codeDocument is null)
        {
            throw new ArgumentNullException(nameof(codeDocument));
        }

        ChangeReference = edit;
        CodeDocument = codeDocument;
    }

    public ChangeReference ChangeReference { get; }

    public RazorCodeDocument CodeDocument { get; }
}
