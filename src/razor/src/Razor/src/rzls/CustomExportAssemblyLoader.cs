// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class CustomExportAssemblyLoader(string baseDirectory) : IAssemblyLoader
{
    /// <summary>
    /// Cache assemblies that are already loaded by AssemblyName comparison
    /// </summary>
    private readonly Dictionary<AssemblyName, Assembly> _loadedAssemblies = new(AssemblyNameComparer.Instance);

    /// <summary>
    /// Base directory to search for <see cref="Assembly.LoadFrom(string)"/> if initial load fails
    /// </summary>
    private readonly string _baseDirectory = baseDirectory;

    public Assembly LoadAssembly(AssemblyName assemblyName)
    {
        Assembly? assembly;

        lock (_loadedAssemblies)
        {
            if (_loadedAssemblies.TryGetValue(assemblyName, out assembly))
            {
                return assembly;
            }
        }

        assembly = LoadAssemblyCore(assemblyName);

        lock (_loadedAssemblies)
        {
            _loadedAssemblies[assemblyName] = assembly;
        }

        return assembly;
    }

    public Assembly LoadAssembly(string assemblyFullName, string? codeBasePath)
    {
        var assemblyName = new AssemblyName(assemblyFullName);

        if (codeBasePath is not null)
        {
#pragma warning disable SYSLIB0044 // Type or member is obsolete
            assemblyName.CodeBase = codeBasePath;
#pragma warning restore SYSLIB0044 // Type or member is obsolete
        }

        return LoadAssembly(assemblyName);
    }

    private Assembly LoadAssemblyCore(AssemblyName assemblyName)
    {
        // Attempt to load the assembly normally, but fall back to Assembly.LoadFrom in the base
        // directory if the assembly load fails

        try
        {
            return Assembly.Load(assemblyName);
        }
        catch (FileNotFoundException)
        {
            // Carry on trying to load by path below.
        }

        var simpleName = assemblyName.Name!;
        var assemblyPath = Path.Combine(_baseDirectory, simpleName + ".dll");
        if (File.Exists(assemblyPath))
        {
            return Assembly.LoadFrom(assemblyPath);
        }

        throw new FileNotFoundException($"Could not find assembly {assemblyName} at {assemblyPath}");
    }

    private sealed class AssemblyNameComparer : IEqualityComparer<AssemblyName>
    {
        public static readonly AssemblyNameComparer Instance = new();

        private AssemblyNameComparer()
        {
        }

        public bool Equals(AssemblyName? x, AssemblyName? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Name == y.Name;
        }

        public int GetHashCode(AssemblyName obj)
        {
            return obj.Name?.GetHashCode() ?? 0;
        }
    }
}
