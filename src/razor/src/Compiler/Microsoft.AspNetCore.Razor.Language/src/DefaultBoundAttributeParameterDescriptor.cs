// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultBoundAttributeParameterDescriptor : BoundAttributeParameterDescriptor
{
    public DefaultBoundAttributeParameterDescriptor(
        string kind,
        string? name,
        string? typeName,
        bool isEnum,
        DocumentationObject documentationObject,
        string? displayName,
        bool caseSensitive,
        MetadataCollection metadata,
        RazorDiagnostic[] diagnostics)
        : base(kind)
    {
        Name = name;
        TypeName = typeName;
        IsEnum = isEnum;
        DocumentationObject = documentationObject;
        DisplayName = displayName;
        CaseSensitive = caseSensitive;

        Metadata = metadata;
        Diagnostics = diagnostics;

        IsStringProperty = typeName == typeof(string).FullName || typeName == "string";
        IsBooleanProperty = typeName == typeof(bool).FullName || typeName == "bool";
    }
}
