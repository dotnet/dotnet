// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

internal abstract class TagHelperPooledObjectPolicy<T> : IPooledObjectPolicy<T>
    where T : notnull
{
    private const int MaxSize = 32;

    public abstract T Create();
    public abstract bool Return(T obj);

    protected static void ClearList<TList>(List<TList>? list)
    {
        if (list is null)
        {
            return;
        }

        list.Clear();

        if (list.Capacity > MaxSize)
        {
            list.Capacity = MaxSize;
        }
    }

    protected static void ClearDiagnostics(RazorDiagnosticCollection? collection)
    {
        if (collection is null)
        {
            return;
        }

        collection.Clear();

        if (collection.Capacity > MaxSize)
        {
            collection.Capacity = MaxSize;
        }
    }
}
