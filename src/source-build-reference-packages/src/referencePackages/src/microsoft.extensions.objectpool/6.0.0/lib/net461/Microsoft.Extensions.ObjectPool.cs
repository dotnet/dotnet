// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("Microsoft.Extensions.ObjectPool")]
[assembly: AssemblyDescription("Microsoft.Extensions.ObjectPool")]
[assembly: AssemblyDefaultAlias("Microsoft.Extensions.ObjectPool")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("6.0.21.52608")]
[assembly: AssemblyInformationalVersion("6.0.21.52608 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("6.0.0.0")]




namespace Microsoft.Extensions.ObjectPool
{
    public partial class DefaultObjectPoolProvider : Microsoft.Extensions.ObjectPool.ObjectPoolProvider
    {
        public DefaultObjectPoolProvider() { }
        public int MaximumRetained { get { throw null; } set { } }
        public override Microsoft.Extensions.ObjectPool.ObjectPool<T> Create<T>(Microsoft.Extensions.ObjectPool.IPooledObjectPolicy<T> policy) { throw null; }
    }
    public partial class DefaultObjectPool<T> : Microsoft.Extensions.ObjectPool.ObjectPool<T> where T : class
    {
        public DefaultObjectPool(Microsoft.Extensions.ObjectPool.IPooledObjectPolicy<T> policy) { }
        public DefaultObjectPool(Microsoft.Extensions.ObjectPool.IPooledObjectPolicy<T> policy, int maximumRetained) { }
        public override T Get() { throw null; }
        public override void Return(T obj) { }
    }
    public partial class DefaultPooledObjectPolicy<T> : Microsoft.Extensions.ObjectPool.PooledObjectPolicy<T> where T : class, new()
    {
        public DefaultPooledObjectPolicy() { }
        public override T Create() { throw null; }
        public override bool Return(T obj) { throw null; }
    }
    public partial interface IPooledObjectPolicy<T> where T : notnull
    {
        T Create();
        bool Return(T obj);
    }
    public partial class LeakTrackingObjectPoolProvider : Microsoft.Extensions.ObjectPool.ObjectPoolProvider
    {
        public LeakTrackingObjectPoolProvider(Microsoft.Extensions.ObjectPool.ObjectPoolProvider inner) { }
        public override Microsoft.Extensions.ObjectPool.ObjectPool<T> Create<T>(Microsoft.Extensions.ObjectPool.IPooledObjectPolicy<T> policy) { throw null; }
    }
    public partial class LeakTrackingObjectPool<T> : Microsoft.Extensions.ObjectPool.ObjectPool<T> where T : class
    {
        public LeakTrackingObjectPool(Microsoft.Extensions.ObjectPool.ObjectPool<T> inner) { }
        public override T Get() { throw null; }
        public override void Return(T obj) { }
    }
    public static partial class ObjectPool
    {
        public static Microsoft.Extensions.ObjectPool.ObjectPool<T> Create<T>(Microsoft.Extensions.ObjectPool.IPooledObjectPolicy<T>? policy = null) where T : class, new() { throw null; }
    }
    public abstract partial class ObjectPoolProvider
    {
        protected ObjectPoolProvider() { }
        public Microsoft.Extensions.ObjectPool.ObjectPool<T> Create<T>() where T : class, new() { throw null; }
        public abstract Microsoft.Extensions.ObjectPool.ObjectPool<T> Create<T>(Microsoft.Extensions.ObjectPool.IPooledObjectPolicy<T> policy) where T : class;
    }
    public static partial class ObjectPoolProviderExtensions
    {
        public static Microsoft.Extensions.ObjectPool.ObjectPool<System.Text.StringBuilder> CreateStringBuilderPool(this Microsoft.Extensions.ObjectPool.ObjectPoolProvider provider) { throw null; }
        public static Microsoft.Extensions.ObjectPool.ObjectPool<System.Text.StringBuilder> CreateStringBuilderPool(this Microsoft.Extensions.ObjectPool.ObjectPoolProvider provider, int initialCapacity, int maximumRetainedCapacity) { throw null; }
    }
    public abstract partial class ObjectPool<T> where T : class
    {
        protected ObjectPool() { }
        public abstract T Get();
        public abstract void Return(T obj);
    }
    public abstract partial class PooledObjectPolicy<T> : Microsoft.Extensions.ObjectPool.IPooledObjectPolicy<T> where T : notnull
    {
        protected PooledObjectPolicy() { }
        public abstract T Create();
        public abstract bool Return(T obj);
    }
    public partial class StringBuilderPooledObjectPolicy : Microsoft.Extensions.ObjectPool.PooledObjectPolicy<System.Text.StringBuilder>
    {
        public StringBuilderPooledObjectPolicy() { }
        public int InitialCapacity { get { throw null; } set { } }
        public int MaximumRetainedCapacity { get { throw null; } set { } }
        public override System.Text.StringBuilder Create() { throw null; }
        public override bool Return(System.Text.StringBuilder obj) { throw null; }
    }
}
