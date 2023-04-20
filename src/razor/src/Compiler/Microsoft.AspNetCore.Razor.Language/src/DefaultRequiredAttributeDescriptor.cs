// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRequiredAttributeDescriptor : RequiredAttributeDescriptor
{
    public DefaultRequiredAttributeDescriptor(
        string? name,
        NameComparisonMode nameComparison,
        bool caseSensitive,
        string? value,
        ValueComparisonMode valueComparison,
        string displayName,
        RazorDiagnostic[] diagnostics,
        MetadataCollection metadata)
    {
        Name = name;
        NameComparison = nameComparison;
        CaseSensitive = caseSensitive;
        Value = value;
        ValueComparison = valueComparison;
        DisplayName = displayName;
        Diagnostics = diagnostics;
        Metadata = metadata;
    }
}
