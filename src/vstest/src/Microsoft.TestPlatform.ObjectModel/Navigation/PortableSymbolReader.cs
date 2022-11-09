// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;

/// <summary>
///     The portable symbol reader.
/// </summary>
internal class PortableSymbolReader : ISymbolReader
{
    /// <summary>
    ///     Key in first dict is Type FullName
    ///     Key in second dict is method name
    /// </summary>
    private readonly Dictionary<string, Dictionary<string, DiaNavigationData>> _methodsNavigationDataForType = new();

    /// <summary>
    /// The cache symbols.
    /// </summary>
    /// <param name="binaryPath">
    /// The binary path.
    /// </param>
    /// <param name="searchPath">
    /// The search path.
    /// </param>
    public void CacheSymbols(string binaryPath, string? searchPath)
    {
        PopulateCacheForTypeAndMethodSymbols(binaryPath);
    }

    /// <summary>
    /// The dispose.
    /// </summary>
    public void Dispose()
    {
        foreach (var methodsNavigationData in _methodsNavigationDataForType.Values)
        {
            methodsNavigationData.Clear();
        }

        _methodsNavigationDataForType.Clear();
    }

    /// <summary>
    /// The get navigation data.
    /// </summary>
    /// <param name="declaringTypeName">
    /// The declaring type name.
    /// </param>
    /// <param name="methodName">
    /// The method name.
    /// </param>
    /// <returns>
    /// The <see cref="INavigationData"/>.
    /// </returns>
    public INavigationData? GetNavigationData(string declaringTypeName, string methodName)
    {
        INavigationData? navigationData = null;
        if (_methodsNavigationDataForType.ContainsKey(declaringTypeName))
        {
            var methodDict = _methodsNavigationDataForType[declaringTypeName];
            if (methodDict.ContainsKey(methodName))
            {
                navigationData = methodDict[methodName];
            }
        }

        return navigationData;
    }

    /// <summary>
    /// The populate cache for type and method symbols.
    /// </summary>
    /// <param name="binaryPath">
    /// The binary path.
    /// </param>
    private void PopulateCacheForTypeAndMethodSymbols(string binaryPath)
    {
        try
        {
            var pdbFilePath = Path.ChangeExtension(binaryPath, ".pdb");
            using PortablePdbReader pdbReader = File.Exists(pdbFilePath)
                ? CreatePortablePdbReaderFromExistingPdbFile(pdbFilePath)
                : CreatePortablePdbReaderFromPEData(binaryPath);

            // At this point, the assembly should be already loaded into the load context. We query for a reference to
            // find the types and cache the symbol information. Let the loader follow default lookup order instead of
            // forcing load from a specific path.
            Assembly asm;
            try
            {
                asm = Assembly.Load(new PlatformAssemblyLoadContext().GetAssemblyNameFromPath(binaryPath));
            }
            catch (FileNotFoundException)
            {
                // fallback when the assembly is not loaded
                asm = Assembly.LoadFile(binaryPath);
            }

            foreach (var type in asm.GetTypes())
            {
                // Get declared method infos
                var methodInfoList = type.GetTypeInfo().DeclaredMethods;
                var methodsNavigationData = new Dictionary<string, DiaNavigationData>();

                foreach (var methodInfo in methodInfoList)
                {
                    var diaNavigationData = pdbReader.GetDiaNavigationData(methodInfo);
                    if (diaNavigationData != null)
                    {
                        methodsNavigationData[methodInfo.Name] = diaNavigationData;
                    }
                    else
                    {
                        EqtTrace.Error($"Unable to find source information for method: {methodInfo.Name} type: {type.FullName}");
                    }
                }

                if (methodsNavigationData.Count != 0)
                {
                    _methodsNavigationDataForType[type.FullName!] = methodsNavigationData;
                }
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("PortableSymbolReader: Failed to load symbols for binary: {0}", binaryPath);
            EqtTrace.Error(ex);
            Dispose();
            throw;
        }
    }

    /// <summary>
    /// Reads the pdb data from the dlls itself, either by loading the referenced pdb file, or by reading
    /// embedded pdb from the dll itself.
    /// </summary>
    /// <param name="binaryPath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static PortablePdbReader CreatePortablePdbReaderFromPEData(string binaryPath)
    {
        using var dllStream = new FileStream(binaryPath, FileMode.Open, FileAccess.Read);
        using var peReader = new PEReader(dllStream);

        var hasPdb = peReader.TryOpenAssociatedPortablePdb(binaryPath, pdbPath => new FileStream(pdbPath, FileMode.Open, FileAccess.Read), out var mp, pdbPath: out _);

        // The out parameters don't give additional info about the pdbFile in case it is not found. So we have few reasons to fail:
        if (!hasPdb)
        {
            throw new InvalidOperationException($"Cannot find portable .PDB file for {binaryPath}. This can have multiple reasons:"
                + "\n- The dll was built with <DebugType>portable</DebugType> and the pdb file is missing (it was deleted, or not moved together with the dll)."
                + "\n- The dll was built with <DebugType>embedded</DebugType> and there is some unknown error reading the metadata from the dll."
                + "\n- The sll was built with <DebugType>none</DebugType> and the pdb file was never even emitted during build."
                + "\n- Additionally if your dll is built with <DebugType>full</DebugType>, see FullPdbReader instead.");
        }

        TPDebug.Assert(mp is not null, "mp is null");
        return new PortablePdbReader(mp);
    }

    private static PortablePdbReader CreatePortablePdbReaderFromExistingPdbFile(string pdbFilePath)
    {
        return new PortablePdbReader(new FileHelper().GetStream(pdbFilePath, FileMode.Open, FileAccess.Read));
    }
}
