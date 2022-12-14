// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed class TypeForwardedFromAttribute : Attribute
    {
        public TypeForwardedFromAttribute(string assemblyFullName)
        {
            ArgumentException.ThrowIfNullOrEmpty(assemblyFullName);
            AssemblyFullName = assemblyFullName;
        }

        public string AssemblyFullName { get; }
    }
}
