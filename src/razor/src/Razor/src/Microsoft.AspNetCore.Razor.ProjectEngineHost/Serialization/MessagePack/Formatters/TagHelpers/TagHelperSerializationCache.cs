// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed partial class TagHelperSerializationCache
{
    public static readonly ObjectPool<TagHelperSerializationCache> Pool = DefaultPool.Create(Policy.Instance);

    private static readonly ObjectPool<Dictionary<MetadataCollection, int>> s_metadataPool
        = DictionaryPool<MetadataCollection, int>.Default;

    private static readonly ObjectPool<Dictionary<string, int>> s_stringPool
        = StringDictionaryPool<int>.Ordinal;

    private ReferenceMap<MetadataCollection>? _metadataMap;
    private ReferenceMap<string>? _stringMap;

    private TagHelperSerializationCache()
    {
    }

    public ReferenceMap<MetadataCollection> Metadata
        => _metadataMap ??= new ReferenceMap<MetadataCollection>(s_metadataPool);

    public ReferenceMap<string> Strings
        => _stringMap ??= new ReferenceMap<string>(s_stringPool);
}
