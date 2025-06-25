// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language;

internal readonly struct TypeNameObject
{
    private static readonly ImmutableArray<string> s_knownTypeNames;
    private static readonly FrozenDictionary<string, byte> s_typeNameToIndex;

    private static readonly int s_booleanIndex;
    private static readonly int s_stringIndex;

    static TypeNameObject()
    {
        var knownTypeNames = ImmutableArray.CreateBuilder<string>();
        var typeNameToIndex = new Dictionary<string, byte>(StringComparer.Ordinal);

        Add<object>("object");
        s_booleanIndex = Add<bool>("bool");
        Add<int>("int");
        Add<long>("long");
        Add<short>("short");
        Add<byte>("byte");
        Add<sbyte>("sbyte");
        Add<uint>("uint");
        Add<ulong>("ulong");
        Add<ushort>("ushort");
        Add<float>("float");
        Add<double>("double");
        Add<decimal>("decimal");
        Add<char>("char");
        s_stringIndex = Add<string>("string");
        Add<System.Globalization.CultureInfo>();
        Add<Delegate>();
        Add<Type>();

        // Add any additional types here.

        s_knownTypeNames = knownTypeNames.ToImmutable();
        s_typeNameToIndex = typeNameToIndex.ToFrozenDictionary(StringComparer.Ordinal);

        int Add<T>(string? alias = null)
        {
            var fullName = typeof(T).FullName!;
            Debug.Assert(knownTypeNames.Count < byte.MaxValue, "Too many known type names to fit in a byte index.");
            var index = (byte)knownTypeNames.Count;
            knownTypeNames.Add(fullName);
            typeNameToIndex.Add(fullName, index);

            if (alias is not null)
            {
                typeNameToIndex.Add(alias, index);
            }

            return index;
        }
    }

    private readonly byte? _index;
    private readonly string? _stringValue;

    public TypeNameObject(byte index)
    {
        Debug.Assert(index >= 0 && index < s_knownTypeNames.Length);

        _index = index;
        _stringValue = null;
    }

    public TypeNameObject(string? stringValue)
    {
        Debug.Assert(stringValue is null || !s_typeNameToIndex.ContainsKey(stringValue));

        _index = null;
        _stringValue = stringValue;
    }

    public bool IsNull => _index is null && _stringValue is null;

    public byte? Index => _index;
    public string? StringValue => _stringValue;

    public static TypeNameObject From(string? typeName)
    {
        if (typeName is null)
        {
            return default;
        }

        return s_typeNameToIndex.TryGetValue(typeName, out var index)
            ? new(index)
            : new(typeName);
    }

    public bool IsBoolean => _index == s_booleanIndex;
    public bool IsString => _index == s_stringIndex;

    public readonly string? GetTypeName()
    {
        if (_index is byte index)
        {
            return s_knownTypeNames[index];
        }

        return _stringValue;
    }

    public void AppendToChecksum(in Checksum.Builder builder)
    {
        if (_index is byte index)
        {
            builder.AppendData(index);
        }
        else if (_stringValue is string fullName)
        {
            builder.AppendData(fullName);
        }
        else
        {
            builder.AppendNull();
        }
    }
}
