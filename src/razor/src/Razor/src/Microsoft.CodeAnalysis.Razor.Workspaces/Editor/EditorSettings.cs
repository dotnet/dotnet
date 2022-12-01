// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Internal;

namespace Microsoft.CodeAnalysis.Razor.Editor;

internal sealed class EditorSettings : IEquatable<EditorSettings>
{
    public static readonly EditorSettings Default = new(indentWithTabs: false, indentSize: 4);

    public EditorSettings(bool indentWithTabs, int indentSize)
    {
        if (indentSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(indentSize));
        }

        IndentWithTabs = indentWithTabs;
        IndentSize = indentSize;
    }

    public bool IndentWithTabs { get; }

    public int IndentSize { get; }

    public bool Equals(EditorSettings? other)
        => other is not null &&
           IndentWithTabs == other.IndentWithTabs &&
           IndentSize == other.IndentSize;

    public override bool Equals(object? other)
    {
        return Equals(other as EditorSettings);
    }

    public override int GetHashCode()
    {
        var combiner = HashCodeCombiner.Start();
        combiner.Add(IndentWithTabs);
        combiner.Add(IndentSize);

        return combiner.CombinedHash;
    }
}
