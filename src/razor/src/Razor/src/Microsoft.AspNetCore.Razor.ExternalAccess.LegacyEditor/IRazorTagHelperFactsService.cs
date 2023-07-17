// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorTagHelperFactsService
{
    IRazorTagHelperBinding? GetTagHelperBinding(
        IRazorTagHelperDocumentContext documentContext,
        string? tagName,
        IEnumerable<KeyValuePair<string, string>> attributes,
        string? parentTag,
        bool parentIsTagHelper);

    ImmutableArray<IRazorBoundAttributeDescriptor> GetBoundTagHelperAttributes(
        IRazorTagHelperDocumentContext documentContext,
        string attributeName,
        IRazorTagHelperBinding binding);

    ImmutableArray<IRazorTagHelperDescriptor> GetTagHelpersGivenTag(
        IRazorTagHelperDocumentContext documentContext,
        string tagName,
        string? parentTag);
}
