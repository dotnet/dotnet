// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;

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
