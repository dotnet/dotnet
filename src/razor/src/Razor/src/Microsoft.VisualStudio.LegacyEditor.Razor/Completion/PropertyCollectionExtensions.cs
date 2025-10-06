// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

internal static class PropertyCollectionExtensions
{
    public static object CompletionItemKindsKey = new();

    public static void SetCompletionItemKinds(this PropertyCollection properties, ICollection<RazorCompletionItemKind> completionItemKinds)
    {
        if (properties is null)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        if (completionItemKinds is null)
        {
            throw new ArgumentNullException(nameof(completionItemKinds));
        }

        properties[CompletionItemKindsKey] = completionItemKinds;
    }

    public static bool TryGetCompletionItemKinds(this PropertyCollection properties, [NotNullWhen(returnValue: true)] out ICollection<RazorCompletionItemKind>? completionItemKinds)
    {
        if (properties is null)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        return properties.TryGetProperty(CompletionItemKindsKey, out completionItemKinds);
    }
}
