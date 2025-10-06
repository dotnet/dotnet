// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

/// <summary>
///  Compares <see cref="CompletionItem"/>s by display text using the current culture.
/// </summary>
internal sealed class CompletionItemDisplayTextComparer : IComparer<CompletionItem>
{
    public static readonly CompletionItemDisplayTextComparer Instance = new();

    private CompletionItemDisplayTextComparer()
    {
    }

    public int Compare(CompletionItem x, CompletionItem y)
    {
        var displayText1 = x?.DisplayText;
        var displayText2 = y?.DisplayText;

        if (displayText1 is null)
        {
            if (displayText2 is not null)
            {
                return -1;
            }

            return 0;
        }
        else if (displayText2 is null)
        {
            return 1;
        }

        return StringComparer.CurrentCulture.Compare(displayText1, displayText2);
    }
}
