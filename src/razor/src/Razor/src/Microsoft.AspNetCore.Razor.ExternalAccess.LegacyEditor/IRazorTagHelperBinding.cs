// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorTagHelperBinding
{
    ImmutableArray<IRazorTagHelperDescriptor> Descriptors { get; }

    ImmutableArray<IRazorTagMatchingRuleDescriptor> GetBoundRules(IRazorTagHelperDescriptor descriptor);
}
