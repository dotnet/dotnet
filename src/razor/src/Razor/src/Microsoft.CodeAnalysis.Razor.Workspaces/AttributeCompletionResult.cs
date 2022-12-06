// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.VisualStudio.Editor.Razor;

public abstract class AttributeCompletionResult
{
    private AttributeCompletionResult()
    {
    }

    public abstract IReadOnlyDictionary<string, IEnumerable<BoundAttributeDescriptor>> Completions { get; }

    internal static AttributeCompletionResult Create(Dictionary<string, HashSet<BoundAttributeDescriptor>> completions)
    {
        if (completions is null)
        {
            throw new ArgumentNullException(nameof(completions));
        }

        var readonlyCompletions = completions.ToDictionary(
            key => key.Key,
            value => (IEnumerable<BoundAttributeDescriptor>)value.Value,
            completions.Comparer);
        var result = new DefaultAttributeCompletionResult(readonlyCompletions);

        return result;
    }

    private class DefaultAttributeCompletionResult : AttributeCompletionResult
    {
        private readonly IReadOnlyDictionary<string, IEnumerable<BoundAttributeDescriptor>> _completions;

        public DefaultAttributeCompletionResult(IReadOnlyDictionary<string, IEnumerable<BoundAttributeDescriptor>> completions)
        {
            _completions = completions;
        }

        public override IReadOnlyDictionary<string, IEnumerable<BoundAttributeDescriptor>> Completions => _completions;
    }
}
