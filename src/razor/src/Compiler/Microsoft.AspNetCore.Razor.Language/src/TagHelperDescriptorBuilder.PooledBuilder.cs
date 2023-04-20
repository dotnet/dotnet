// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract partial class TagHelperDescriptorBuilder
{
    public struct PooledBuilder : IDisposable
    {
        private readonly DefaultTagHelperDescriptorBuilder _builder;
        private bool _disposed;

        internal PooledBuilder(DefaultTagHelperDescriptorBuilder builder)
        {
            _builder = builder;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                DefaultTagHelperDescriptorBuilder.ReturnInstance(_builder);
                _disposed = true;
            }
        }
    }
}
