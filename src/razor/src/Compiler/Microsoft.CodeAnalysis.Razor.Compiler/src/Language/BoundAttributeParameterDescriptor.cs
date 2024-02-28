// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class BoundAttributeParameterDescriptor : TagHelperObject<BoundAttributeParameterDescriptor>
{
    [Flags]
    private enum BoundAttributeParameterFlags
    {
        CaseSensitive = 1 << 0,
        IsEnum = 1 << 1,
        IsStringProperty = 1 << 2,
        IsBooleanProperty = 1 << 3
    }

    private readonly BoundAttributeParameterFlags _flags;
    private readonly DocumentationObject _documentationObject;

    public string Kind { get; }
    public string Name { get; }
    public string TypeName { get; }
    public string DisplayName { get; }

    public bool CaseSensitive => (_flags & BoundAttributeParameterFlags.CaseSensitive) != 0;
    public bool IsEnum => (_flags & BoundAttributeParameterFlags.IsEnum) != 0;
    public bool IsStringProperty => (_flags & BoundAttributeParameterFlags.IsStringProperty) != 0;
    public bool IsBooleanProperty => (_flags & BoundAttributeParameterFlags.IsBooleanProperty) != 0;

    public MetadataCollection Metadata { get; }

    internal BoundAttributeParameterDescriptor(
        string kind,
        string name,
        string typeName,
        bool isEnum,
        DocumentationObject documentationObject,
        string displayName,
        bool caseSensitive,
        MetadataCollection metadata,
        ImmutableArray<RazorDiagnostic> diagnostics)
        : base(diagnostics)
    {
        Kind = kind;
        Name = name;
        TypeName = typeName;
        _documentationObject = documentationObject;
        DisplayName = displayName;
        Metadata = metadata ?? MetadataCollection.Empty;

        BoundAttributeParameterFlags flags = 0;

        if (isEnum)
        {
            flags |= BoundAttributeParameterFlags.IsEnum;
        }

        if (caseSensitive)
        {
            flags |= BoundAttributeParameterFlags.CaseSensitive;
        }

        if (typeName == typeof(string).FullName || typeName == "string")
        {
            flags |= BoundAttributeParameterFlags.IsStringProperty;
        }

        if (typeName == typeof(bool).FullName || typeName == "bool")
        {
            flags |= BoundAttributeParameterFlags.IsBooleanProperty;
        }

        _flags = flags;
    }

    private protected override void BuildChecksum(in Checksum.Builder builder)
    {
        builder.AppendData(Kind);
        builder.AppendData(Name);
        builder.AppendData(TypeName);
        builder.AppendData(DisplayName);

        DocumentationObject.AppendToChecksum(in builder);

        builder.AppendData(CaseSensitive);
        builder.AppendData(IsEnum);
        builder.AppendData(IsBooleanProperty);
        builder.AppendData(IsStringProperty);
        builder.AppendData(Metadata.Checksum);
    }

    public string? Documentation => _documentationObject.GetText();

    internal DocumentationObject DocumentationObject => _documentationObject;

    public override string ToString()
        => DisplayName ?? base.ToString()!;
}
