// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.VisualStudio.Editor.Razor;

internal sealed class ElementCompletionContext
{
    public TagHelperDocumentContext DocumentContext { get; }
    public IEnumerable<string> ExistingCompletions { get; }
    public string? ContainingTagName { get; }
    public ImmutableArray<KeyValuePair<string, string>> Attributes { get; }
    public string? ContainingParentTagName { get; }
    public bool ContainingParentIsTagHelper { get; }
    public Func<string, bool> InHTMLSchema { get; }

    public ElementCompletionContext(
        TagHelperDocumentContext documentContext,
        IEnumerable<string>? existingCompletions,
        string? containingTagName,
        ImmutableArray<KeyValuePair<string, string>> attributes,
        string? containingParentTagName,
        bool containingParentIsTagHelper,
        Func<string, bool> inHTMLSchema)
    {
        DocumentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
        ExistingCompletions = existingCompletions ?? Array.Empty<string>();
        ContainingTagName = containingTagName;
        Attributes = attributes;
        ContainingParentTagName = containingParentTagName;
        ContainingParentIsTagHelper = containingParentIsTagHelper;
        InHTMLSchema = inHTMLSchema ?? throw new ArgumentNullException(nameof(inHTMLSchema));
    }
}
