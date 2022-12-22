// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Intent;

public static class Extensions
{
    public static bool IsExcluded(this Assembly asm)
    {
        return asm.CustomAttributes.Any(a => a.AttributeType == typeof(ExcludeAttribute));
    }

    public static List<Type> SkipExcluded(this IEnumerable<Type> e)
    {
        return e.Where(i => i.GetCustomAttribute<ExcludeAttribute>() == null).ToList();
    }

    public static List<Type> SkipNonPublic(this IEnumerable<Type> e)
    {
        return e.Where(i => i.IsPublic || i.IsNestedPublic).ToList();
    }

    public static List<MethodInfo> SkipExcluded(this IEnumerable<MethodInfo> e)
    {
        return e.Where(i =>
            i.Name != nameof(object.ToString)
            && i.Name != nameof(object.GetType)
            && i.Name != nameof(object.GetHashCode)
            && i.Name != nameof(object.Equals)
            && i.GetCustomAttribute<ExcludeAttribute>() == null).ToList();
    }
}
