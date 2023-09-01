// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.VisualStudio.Editor.Razor;

internal sealed class ElementCompletionResult
{
    public IReadOnlyDictionary<string, IEnumerable<TagHelperDescriptor>> Completions { get; }

    private ElementCompletionResult(IReadOnlyDictionary<string, IEnumerable<TagHelperDescriptor>> completions)
    {
        Completions = completions;
    }

    internal static ElementCompletionResult Create(Dictionary<string, HashSet<TagHelperDescriptor>> completions)
    {
        if (completions is null)
        {
            throw new ArgumentNullException(nameof(completions));
        }

        var readonlyCompletions = new Dictionary<string, IEnumerable<TagHelperDescriptor>>(
            capacity: completions.Count,
            comparer: completions.Comparer);

        foreach (var (key, value) in completions)
        {
            readonlyCompletions.Add(key, value);
        }

        return new ElementCompletionResult(readonlyCompletions);
    }
}
