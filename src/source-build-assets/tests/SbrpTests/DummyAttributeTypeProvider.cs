// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace SbrpTests;

internal class DummyAttributeTypeProvider : ICustomAttributeTypeProvider<Type?>
{
    public static readonly DummyAttributeTypeProvider Instance = new();

    public Type? GetPrimitiveType(PrimitiveTypeCode typeCode) => default(Type);

    public Type? GetSystemType() => default(Type);

    public Type? GetSZArrayType(Type? elementType) => default(Type);

    public Type? GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind) => default(Type);

    public Type? GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind) => default(Type);

    public Type? GetTypeFromSerializedName(string name) => default(Type);

    public PrimitiveTypeCode GetUnderlyingEnumType(Type? type) => default(PrimitiveTypeCode);

    public bool IsSystemType(Type? type) => default(bool);
}
