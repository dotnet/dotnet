// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using MessagePack;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

internal partial class SerializerCachingOptions(MessagePackSerializerOptions copyFrom) : MessagePackSerializerOptions(copyFrom), IDisposable
{
    private static readonly DictionaryPool<string, int> s_stringPool = SpecializedPools.StringDictionary<int>.Ordinal;

    private ReferenceMap<string>? _stringMap;

    public ReferenceMap<string> Strings
        => _stringMap ??= new ReferenceMap<string>(s_stringPool);

    public void Dispose()
    {
        if (_stringMap is { } stringMap)
        {
            stringMap.Dispose();
            _stringMap = null;
        }
    }
}
