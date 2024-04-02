// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
internal sealed class RestrictedInternalsVisibleToAttribute(string assemblyName, params string[] allowedNamespaces) : Attribute
{
    public string AssemblyName { get; } = assemblyName;
    public ImmutableArray<string> AllowedNamespaces { get; } = allowedNamespaces.ToImmutableArray();
}
