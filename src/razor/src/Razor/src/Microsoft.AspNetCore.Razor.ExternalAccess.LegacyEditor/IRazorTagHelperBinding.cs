// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorTagHelperBinding
{
    ImmutableArray<IRazorTagHelperDescriptor> Descriptors { get; }

    ImmutableArray<IRazorTagMatchingRuleDescriptor> GetBoundRules(IRazorTagHelperDescriptor descriptor);
}
