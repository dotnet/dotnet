// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETSTANDARD2_0

using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <inheritdoc/>
public class PlatformAssemblyLoadContext : IAssemblyLoadContext
{
    /// <inheritdoc/>
    public AssemblyName GetAssemblyNameFromPath(string assemblyPath)
    {
        return AssemblyName.GetAssemblyName(assemblyPath);
    }

    public Assembly LoadAssemblyFromPath(string assemblyPath)
    {
        return Assembly.LoadFrom(assemblyPath);
    }
}

#endif
