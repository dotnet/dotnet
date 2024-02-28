// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal sealed partial class RazorSemanticTokensLegendService
{
    public class Modifiers
    {
        private static readonly string s_razorCode = "razorCode";

        public int RazorCodeModifier => _modifierMap[s_razorCode];

        private readonly Dictionary<string, int> _modifierMap;

        public string[] TokenModifiers { get; }

        public Modifiers()
        {
            using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

            builder.AddRange(RazorSemanticTokensAccessor.GetTokenModifiers());

            var modifierMap = new Dictionary<string, int>();
            foreach (var razorModifier in GetStaticFieldValues(typeof(Modifiers)))
            {
                // Modifiers is a flags enum, so numeric values are powers of 2
                modifierMap.Add(razorModifier, (int)Math.Pow(builder.Count, 2));
                builder.Add(razorModifier);
            }

            _modifierMap = modifierMap;

            TokenModifiers = builder.ToArray();
        }
    }
}
