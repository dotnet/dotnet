// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
