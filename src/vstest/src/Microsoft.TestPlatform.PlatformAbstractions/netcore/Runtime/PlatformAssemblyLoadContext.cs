// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System.Reflection;
using System.Runtime.Loader;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <inheritdoc/>
public class PlatformAssemblyLoadContext : IAssemblyLoadContext
{
    /// <inheritdoc/>
    public AssemblyName GetAssemblyNameFromPath(string assemblyPath)
    {
        return AssemblyLoadContext.GetAssemblyName(assemblyPath);
    }

    public Assembly LoadAssemblyFromPath(string assemblyPath)
    {
        return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
    }
}

#endif
