// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if NETFRAMEWORK
using System;

namespace Microsoft.Build.Framework
{
    /// <summary>
    /// Compatibility shim for <c>Microsoft.Build.Framework.MSBuildMultiThreadableTaskAttribute</c>, which only exists
    /// in MSBuild 18.6 and later. The .NET Framework build of NuGet's MSBuild tasks compiles against the in-box MSBuild
    /// reference assemblies, which do not contain this type, so this polyfill lets the shared task source apply the
    /// marker without conditional compilation at each task. MSBuild detects the attribute by namespace and name only
    /// (ignoring the defining assembly), so a self-defined copy is recognized by 18.6+ hosts and ignored by older ones.
    /// On .NET (non-NETFRAMEWORK) targets the real attribute from the Microsoft.Build.Framework package is used instead.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class MSBuildMultiThreadableTaskAttribute : Attribute
    {
    }
}
#endif
