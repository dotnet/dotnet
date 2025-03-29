// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;

internal static class Extensions
{
    public static void Add<T>(this Dictionary<Type, object> map, NonCachingFormatter<T> formatter)
    {
        map.Add(TopLevelFormatter<T>.TargetType, formatter);
    }

    public static void Add<T>(this Dictionary<Type, object> map, TopLevelFormatter<T> formatter)
    {
        map.Add(TopLevelFormatter<T>.TargetType, formatter);
    }

    public static void Add<T>(this Dictionary<Type, object> map, ValueFormatter<T> formatter)
    {
        map.Add(TopLevelFormatter<T>.TargetType, formatter);
    }
}
