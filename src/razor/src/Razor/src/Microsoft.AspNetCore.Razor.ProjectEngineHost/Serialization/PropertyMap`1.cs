// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed class PropertyMap<TData>
    where TData : struct
{
    private readonly Dictionary<string, ReadPropertyValue<TData>> _map;

    public PropertyMap(params (string, ReadPropertyValue<TData>)[] pairs)
    {
        var map = new Dictionary<string, ReadPropertyValue<TData>>(capacity: pairs.Length);

        foreach (var (key, value) in pairs)
        {
            map.Add(key, value);
        }

        _map = map;
    }

    public bool TryGetPropertyReader(string key, [MaybeNullWhen(false)] out ReadPropertyValue<TData> value)
        => _map.TryGetValue(key, out value);
}
