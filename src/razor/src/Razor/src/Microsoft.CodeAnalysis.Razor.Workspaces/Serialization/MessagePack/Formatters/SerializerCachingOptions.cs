// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal partial class SerializerCachingOptions(MessagePackSerializerOptions copyFrom) : MessagePackSerializerOptions(copyFrom), IDisposable
{
    private static readonly ObjectPool<Dictionary<MetadataCollection, int>> s_metadataPool
        = DictionaryPool<MetadataCollection, int>.Default;

    private static readonly ObjectPool<Dictionary<string, int>> s_stringPool
        = StringDictionaryPool<int>.Ordinal;

    private ReferenceMap<MetadataCollection>? _metadataMap;
    private ReferenceMap<string>? _stringMap;

    public ReferenceMap<MetadataCollection> Metadata
        => _metadataMap ??= new ReferenceMap<MetadataCollection>(s_metadataPool);

    public ReferenceMap<string> Strings
        => _stringMap ??= new ReferenceMap<string>(s_stringPool);

    public void Dispose()
    {
        if (_metadataMap is { } metadataMap)
        {
            metadataMap.Dispose();
            _metadataMap = null;
        }

        if (_stringMap is { } stringMap)
        {
            stringMap.Dispose();
            _stringMap = null;
        }
    }
}
