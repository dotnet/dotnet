// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>
    /// Wrapper class representing shorter syntax of Lazy&lt;Task&lt;T&gt;&gt;"/>.
    /// Useful when declaring a lazy async factory of value T.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    [CLSCompliant(true)]
    public class AsyncLazy<T>
    {
        private readonly Lazy<Task<T>> _inner;

        public AsyncLazy(Func<Task<T>> valueFactory)
        {
            _inner = new Lazy<Task<T>>(valueFactory);
        }

        public AsyncLazy(Lazy<Task<T>> inner)
        {
            _inner = inner;
        }

        public TaskAwaiter<T> GetAwaiter() => _inner.Value.GetAwaiter();

        public static implicit operator Lazy<Task<T>>(AsyncLazy<T> outer) => outer._inner;  // implicit conversion
    }

    /// <summary>
    /// Shortcuts to common Lazy&lt;Task&lt;T&gt;&gt; constructor calls
    /// </summary>
    public static class AsyncLazy
    {
        public static AsyncLazy<T> New<T>(Func<Task<T>> asyncValueFactory) => new AsyncLazy<T>(asyncValueFactory);

        public static AsyncLazy<T> New<T>(Func<T> valueFactory) => new AsyncLazy<T>(() => Task.Run(valueFactory));

        public static AsyncLazy<T> New<T>(Lazy<Task<T>> inner) => new AsyncLazy<T>(inner);

        public static AsyncLazy<T> New<T>(T innerData) => new AsyncLazy<T>(() => Task.FromResult(innerData));
    }
}
