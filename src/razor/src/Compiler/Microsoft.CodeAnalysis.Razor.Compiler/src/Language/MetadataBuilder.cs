// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Language;

internal ref struct MetadataBuilder
{
    private readonly ObjectPool<List<KeyValuePair<string, string?>>> _pool;
    private List<KeyValuePair<string, string?>>? _list;

    public MetadataBuilder()
        : this(ListPool<KeyValuePair<string, string?>>.Default)
    {
    }

    public MetadataBuilder(ObjectPool<List<KeyValuePair<string, string?>>> pool)
    {
        _pool = pool;
    }

    public void Dispose()
    {
        ClearAndFree();
    }

    public void Add(string key, string? value)
    {
        _list ??= _pool.Get();
        _list.Add(new(key, value));
    }

    public void Add(KeyValuePair<string, string?> pair)
    {
        _list ??= _pool.Get();
        _list.Add(pair);
    }

    public MetadataCollection Build()
    {
        var result = MetadataCollection.CreateOrEmpty(_list);

        _list = null;

        return result;
    }

    public void ClearAndFree()
    {
        if (_list is { } list)
        {
            _pool.Return(list);
            _list = null;
        }
    }
}
