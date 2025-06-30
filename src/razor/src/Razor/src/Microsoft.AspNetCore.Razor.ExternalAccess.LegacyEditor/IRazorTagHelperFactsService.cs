// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
