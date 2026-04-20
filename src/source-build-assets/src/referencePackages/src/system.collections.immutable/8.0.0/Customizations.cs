// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

// See https://github.com/dotnet/sdk/issues/33693 for why this is necessary.

#if !NET8_0

namespace System.Collections.Immutable
{
    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableList), "Create")]
    public partial interface IImmutableList<T>
    {}

    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableQueue), "Create")]
    public partial interface IImmutableQueue<T>
    {}

    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableHashSet), "Create")]
    public partial interface IImmutableSet<T> 
    {}
    
    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableStack), "Create")]
    public partial interface IImmutableStack<T>
    {}

    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableArray), "Create")]
    public readonly partial struct ImmutableArray<T> 
    {}
    
    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableHashSet), "Create")]
    public sealed partial class ImmutableHashSet<T>
    {}
    
    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableList), "Create")]
    public sealed partial class ImmutableList<T>
    {}

    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableQueue), "Create")]
    public sealed partial class ImmutableQueue<T>
    {}

    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableSortedSet), "Create")]
    public sealed partial class ImmutableSortedSet<T>
    {}

    [Runtime.CompilerServices.CollectionBuilder(typeof(ImmutableStack), "Create")]
    public sealed partial class ImmutableStack<T>
    {}
}

namespace System.Runtime.CompilerServices
{
    internal sealed class CollectionBuilderAttribute(Type builderType, string methodName) : Attribute
    {
        public Type BuilderType { get; } = builderType;

        public string MethodName { get; } = methodName;
    }
}

#endif
