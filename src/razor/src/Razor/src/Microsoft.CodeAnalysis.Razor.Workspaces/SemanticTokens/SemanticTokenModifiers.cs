// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Razor.SemanticTokens;

internal sealed class SemanticTokenModifiers
{
    private static readonly string s_razorCode = "razorCode";

    public int RazorCodeModifier => _modifierMap[s_razorCode];

    public string[] All { get; }

    private readonly Dictionary<string, int> _modifierMap;

    public SemanticTokenModifiers(string[] tokenModifiers)
    {
        var modifierMap = new Dictionary<string, int>();
        foreach (var modifier in tokenModifiers)
        {
            // Modifiers is a flags enum, so numeric values are powers of 2, and we skip 0
            modifierMap.Add(modifier, (int)Math.Pow(2, modifierMap.Count));
        }

        _modifierMap = modifierMap;

        All = tokenModifiers;
    }
}
