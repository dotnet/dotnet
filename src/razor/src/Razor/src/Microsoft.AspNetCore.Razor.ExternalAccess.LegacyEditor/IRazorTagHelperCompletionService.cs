// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorTagHelperCompletionService
{
    IRazorElementCompletionContext CreateContext(
        IRazorTagHelperDocumentContext tagHelperContext,
        IEnumerable<string>? existingCompletions,
        string? containingTagName,
        IEnumerable<KeyValuePair<string, string>> attributes,
        string? containingParentTagName,
        bool containingParentIsTagHelper,
        Func<string, bool> inHTMLSchema);

    ImmutableDictionary<string, ImmutableArray<IRazorTagHelperDescriptor>> GetElementCompletions(IRazorElementCompletionContext completionContext);
}
