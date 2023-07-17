// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorBoundAttributeDescriptor
{
    string Name { get; }
    string DisplayName { get; }
    string? Documentation { get; }
    bool CaseSensitive { get; }
    string? IndexerNamePrefix { get; }

    ImmutableArray<IRazorBoundAttributeParameterDescriptor> BoundAttributeParameters { get; }
}
