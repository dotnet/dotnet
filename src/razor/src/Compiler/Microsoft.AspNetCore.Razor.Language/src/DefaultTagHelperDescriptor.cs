// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultTagHelperDescriptor : TagHelperDescriptor
{
    public DefaultTagHelperDescriptor(
        string kind,
        string name,
        string assemblyName,
        string displayName,
        DocumentationObject documentationObject,
        string? tagOutputHint,
        bool caseSensitive,
        TagMatchingRuleDescriptor[] tagMatchingRules,
        BoundAttributeDescriptor[] attributeDescriptors,
        AllowedChildTagDescriptor[] allowedChildTags,
        MetadataCollection metadata,
        RazorDiagnostic[] diagnostics)
        : base(kind)
    {
        Name = name;
        AssemblyName = assemblyName;
        DisplayName = displayName;
        DocumentationObject = documentationObject;
        TagOutputHint = tagOutputHint;
        CaseSensitive = caseSensitive;
        TagMatchingRules = tagMatchingRules;
        BoundAttributes = attributeDescriptors;
        AllowedChildTags = allowedChildTags;
        Diagnostics = diagnostics;
        Metadata = metadata;
    }
}
