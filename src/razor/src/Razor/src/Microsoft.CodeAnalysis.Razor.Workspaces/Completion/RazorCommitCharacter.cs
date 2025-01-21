// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal readonly record struct RazorCommitCharacter(string Character, bool Insert = true)
{
    public static ImmutableArray<RazorCommitCharacter> CreateArray(ReadOnlySpan<string> characters, bool insert = true)
    {
        using var converted = new PooledArrayBuilder<RazorCommitCharacter>(capacity: characters.Length);

        foreach (var ch in characters)
        {
            converted.Add(new(ch, insert));
        }

        return converted.DrainToImmutable();
    }
}
