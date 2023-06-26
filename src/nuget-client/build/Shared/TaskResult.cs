// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuGet;

#nullable enable

internal static class TaskResult
{
    /// <summary>
    /// Gets a <see cref="Task{TResult}"/> that's completed successfully with the result of <see langword="true"/>.
    /// </summary>
    public static Task<bool> True { get; } = Task.FromResult(true);

    /// <summary>
    /// Gets a <see cref="Task{TResult}"/> that's completed successfully with the result of <see langword="false"/>.
    /// </summary>
    public static Task<bool> False { get; } = Task.FromResult(false);

    /// <summary>
    /// Returns a <see cref="Task{TResult}"/> that's completed successfully with the result of <paramref name="b"/>.
    /// </summary>
    public static Task<bool> Boolean(bool b)
    {
        return b ? True : False;
    }

    /// <summary>
    /// Gets a <see cref="Task{TResult}"/> that's completed successfully with the result of <see langword="true"/>.
    /// </summary>
    public static Task<int> Zero { get; } = Task.FromResult(0);

    /// <summary>
    /// Gets a <see cref="Task{TResult}"/> that's completed successfully with the result of <see langword="false"/>.
    /// </summary>
    public static Task<int> One { get; } = Task.FromResult(1);

    /// <summary>
    /// Returns a <see cref="Task{TResult}"/> that's completed successfully with the result of <paramref name="i"/>.
    /// </summary>
    public static Task<int> Integer(int i)
    {
        return i switch
        {
            0 => Zero,
            1 => One,
            _ => Task.FromResult(i)
        };
    }

    /// <summary>
    /// Returns a <see cref="Task{TResult}"/> of type <typeparamref name="T" /> that's completed successfully with the result of <see langword="null"/>.
    /// </summary>
    public static Task<T?> Null<T>() where T : class => NullTaskResult<T>.Instance;

    private static class NullTaskResult<T> where T : class
    {
        public static readonly Task<T?> Instance = Task.FromResult<T?>(null);
    }

    /// <summary>
    /// Returns a <see cref="Task{TResult}"/> whose value is an empty enumerable of type <typeparamref name="T" />.
    /// </summary>
    public static Task<IEnumerable<T>> EmptyEnumerable<T>() => EmptyEnumerableTaskResult<T>.Instance;

    private static class EmptyEnumerableTaskResult<T>
    {
        public static readonly Task<IEnumerable<T>> Instance = Task.FromResult(Enumerable.Empty<T>());
    }

    /// <summary>
    /// Returns a <see cref="Task{TResult}"/> whose value is an empty array with element type <typeparamref name="T" />.
    /// </summary>
    public static Task<T[]> EmptyArray<T>() => EmptyArrayTaskResult<T>.Instance;

    private static class EmptyArrayTaskResult<T>
    {
        public static readonly Task<T[]> Instance = Task.FromResult(Array.Empty<T>());
    }
}
