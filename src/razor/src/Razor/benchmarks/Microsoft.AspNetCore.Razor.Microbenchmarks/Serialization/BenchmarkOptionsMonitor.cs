// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

internal class BenchmarkOptionsMonitor<T> : IOptionsMonitor<T>
{
    public BenchmarkOptionsMonitor(T value)
    {
        CurrentValue = value;
    }

    public T CurrentValue { get; }

    public T Get(string name) => CurrentValue;

    public IDisposable OnChange(Action<T, string> listener) => new Disposable();

    private class Disposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
