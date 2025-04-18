﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.LanguageService;

namespace Microsoft.CodeAnalysis.AddOrRemoveAccessibilityModifiers;

internal static partial class AddOrRemoveAccessibilityModifiersConstants
{
    public const string ModifiersAdded = nameof(ModifiersAdded);
}

internal interface IAddOrRemoveAccessibilityModifiers
{
    bool ShouldUpdateAccessibilityModifier(
        IAccessibilityFacts accessibilityFacts,
        SyntaxNode member,
        AccessibilityModifiersRequired option,
        out SyntaxToken name,
        out bool modifiersAdded);
}
