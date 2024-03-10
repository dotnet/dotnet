// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

// Copied from https://github.com/dotnet/runtime

#if !NET8_0_OR_GREATER

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
internal sealed class CollectionBuilderAttribute : Attribute
{
    public CollectionBuilderAttribute(Type builderType, string methodName)
    {
        BuilderType = builderType;
        MethodName = methodName;
    }

    public Type BuilderType { get; }

    public string MethodName { get; }
}

#else

using System.Runtime.CompilerServices;

#pragma warning disable RS0016 // Add public types and members to the declared API (this is a supporting forwarder for an internal polyfill API)
[assembly: TypeForwardedTo(typeof(CollectionBuilderAttribute))]
#pragma warning restore RS0016 // Add public types and members to the declared API

#endif
