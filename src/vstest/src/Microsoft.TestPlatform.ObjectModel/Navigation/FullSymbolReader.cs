// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;

/// <summary>
/// To get method's file name, startline and endline from desktop assembly file.
/// </summary>
internal class FullSymbolReader : ISymbolReader
{
    /// <summary>
    /// To check isDisposed
    /// </summary>
    private bool _isDisposed;

    private IDiaDataSource? _source;
    private IDiaSession? _session;

    /// <summary>
    /// Holds type symbols available in the source.
    /// </summary>
    private readonly Dictionary<string, IDiaSymbol> _typeSymbols = new();

    /// <summary>
    /// Holds method symbols for all types in the source.
    /// Methods in different types can have same name, hence separated dictionary is created for each type.
    /// Bug: Method overrides in same type are not handled (not a regression)
    /// ToDo(Solution): Use method token along with as identifier, this will always give unique method.The adapters would need to send this token to us to correct the behavior.
    /// </summary>
    private readonly Dictionary<string, Dictionary<string, IDiaSymbol>> _methodSymbols = new();

    /// <summary>
    /// dispose caches
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        // Use SupressFinalize in case a subclass
        // of this type implements a finalizer.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Cache symbols from binary path
    /// </summary>
    /// <param name="binaryPath">
    /// The binary path is assembly path Ex: \path\to\bin\Debug\simpleproject.dll
    /// </param>
    /// <param name="searchPath">
    /// search path.
    /// </param>
    public void CacheSymbols(string binaryPath, string? searchPath)
    {
        try
        {
            if (OpenSession(binaryPath, searchPath))
            {
                PopulateCacheForTypeAndMethodSymbols();
            }
        }
        catch (COMException)
        {
            Dispose();
            throw;
        }
    }

    /// <summary>
    /// Gets Navigation data from caches
    /// </summary>
    /// <param name="declaringTypeName">
    /// Type name Ex: MyNameSpace.MyType
    /// </param>
    /// <param name="methodName">
    /// Method name in declaringTypeName Ex: Method1
    /// </param>
    /// <returns>
    /// <see cref="INavigationData"/>.
    /// Returns INavigationData which contains filename and line number.
    /// </returns>
    public INavigationData? GetNavigationData(string declaringTypeName, string methodName)
    {
        INavigationData? navigationData = null;
        IDiaSymbol? typeSymbol = GetTypeSymbol(declaringTypeName, SymTagEnum.SymTagCompiland);
        IDiaSymbol? methodSymbol;
        if (typeSymbol != null)
        {
            methodSymbol = GetMethodSymbol(typeSymbol, methodName);
        }
        else
        {
            // May be a managed C++ test assembly...
            string fullMethodName = declaringTypeName.Replace(".", "::");
            fullMethodName = fullMethodName + "::" + methodName;

            methodSymbol = GetTypeSymbol(fullMethodName, SymTagEnum.SymTagFunction);
        }

        if (methodSymbol != null)
        {
            navigationData = GetSymbolNavigationData(methodSymbol);
        }

        return navigationData;
    }

    private bool OpenSession(string filename, string? searchPath)
    {
        try
        {
            // Activate the DIA data source COM object
            _source = DiaSourceObject.GetDiaSourceObject();

            if (_source == null)
            {
                return false;
            }

            // UWP(App model) scenario
            if (!Path.IsPathRooted(filename))
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), filename);
                if (StringUtils.IsNullOrEmpty(searchPath))
                {
                    searchPath = Directory.GetCurrentDirectory();
                }
            }

            // Load the data for the executable
            int hResult = _source.LoadDataForExe(filename, searchPath, IntPtr.Zero);
            if (HResult.Failed(hResult))
            {
                throw new COMException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.FailedToCreateDiaSession, hResult));
            }

            // Open the session and return it
            if (HResult.Failed(_source.OpenSession(out _session)))
            {
                return false;
            }
        }
        catch (COMException)
        {
            throw;
        }
        finally
        {
            if (_source != null)
            {
                Marshal.FinalReleaseComObject(_source);
            }
        }

        return true;
    }

    private DiaNavigationData GetSymbolNavigationData(IDiaSymbol symbol)
    {
        ValidateArg.NotNull(symbol, nameof(symbol));

        DiaNavigationData navigationData = new(null, int.MaxValue, int.MinValue);

        IDiaEnumLineNumbers? lines = null;

        try
        {
            // Get the address section
            if (HResult.Failed(symbol.GetAddressSection(out uint section)))
            {
                return navigationData;
            }

            // Get the address offset
            if (HResult.Failed(symbol.GetAddressOffset(out uint offset)))
            {
                return navigationData;
            }

            // Get the length of the symbol
            if (HResult.Failed(symbol.GetLength(out long length)))
            {
                return navigationData;
            }

            TPDebug.Assert(_session is not null, "_session is null");
            _session.FindLinesByAddress(section, offset, (uint)length, out lines);

            while (true)
            {
                lines.GetNext(1, out IDiaLineNumber? lineNumber, out uint celt);

                if (celt != 1)
                {
                    break;
                }

                IDiaSourceFile? sourceFile = null;
                try
                {
                    lineNumber.GetSourceFile(out sourceFile);

                    // Get startline
                    lineNumber.GetLineNumber(out uint startLine);

                    // Get endline
                    lineNumber.GetLineNumberEnd(out uint endLine);

                    // The magic hex constant below works around weird data reported from GetSequencePoints.
                    // The constant comes from ILDASM's source code, which performs essentially the same test.
                    const uint magic = 0xFEEFEE;
                    if (startLine >= magic || endLine >= magic)
                    {
                        continue;
                    }

                    sourceFile.GetFilename(out var srcFileName);

                    navigationData.FileName = srcFileName;
                    navigationData.MinLineNumber = Math.Min(navigationData.MinLineNumber, (int)startLine);
                    navigationData.MaxLineNumber = Math.Max(navigationData.MaxLineNumber, (int)endLine);
                }
                finally
                {
                    ReleaseComObject(ref sourceFile);
                    ReleaseComObject(ref lineNumber);
                }
            }
        }
        finally
        {
            ReleaseComObject(ref lines);
        }

        return navigationData;
    }

    private void PopulateCacheForTypeAndMethodSymbols()
    {
        IDiaEnumSymbols? enumTypeSymbols = null;
        IDiaSymbol? global = null;
        try
        {
            TPDebug.Assert(_session is not null, "_session is null");
            _session.GetGlobalScope(out global);
            global.FindChildren(SymTagEnum.SymTagCompiland, null, 0, out enumTypeSymbols);

            // NOTE::
            // If foreach loop is used instead of Enumerator iterator, for some reason it leaves
            // the reference to pdb active, which prevents pdb from being rebuilt (in VS IDE scenario).
            enumTypeSymbols.GetNext(1, out IDiaSymbol typeSymbol, out uint celtTypeSymbol);
            while (celtTypeSymbol == 1 && typeSymbol != null)
            {
                typeSymbol.GetName(out var name);
                _typeSymbols[name] = typeSymbol;

                IDiaEnumSymbols? enumMethodSymbols = null;
                try
                {
                    Dictionary<string, IDiaSymbol> methodSymbolsForType = new();
                    typeSymbol.FindChildren(SymTagEnum.SymTagFunction, null, 0, out enumMethodSymbols);

                    enumMethodSymbols.GetNext(1, out IDiaSymbol methodSymbol, out uint celtMethodSymbol);
                    while (celtMethodSymbol == 1 && methodSymbol != null)
                    {
                        methodSymbol.GetName(out var methodName);
                        UpdateMethodSymbolCache(methodName, methodSymbol, methodSymbolsForType);
                        enumMethodSymbols.GetNext(1, out methodSymbol, out celtMethodSymbol);
                    }

                    _methodSymbols[name] = methodSymbolsForType;
                }
                catch (Exception ex)
                {
                    EqtTrace.Error(
                        "Ignoring the exception while iterating method symbols:{0} for type:{1}",
                        ex,
                        name);
                }
                finally
                {
                    ReleaseComObject(ref enumMethodSymbols);
                }

                enumTypeSymbols.GetNext(1, out typeSymbol, out celtTypeSymbol);
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error("Ignoring the exception while iterating type symbols:{0}", ex);
        }
        finally
        {
            ReleaseComObject(ref enumTypeSymbols);
            ReleaseComObject(ref global);
        }
    }

    private IDiaSymbol? GetTypeSymbol(string typeName, SymTagEnum symTag)
    {
        ValidateArg.NotNullOrEmpty(typeName, nameof(typeName));

        IDiaEnumSymbols? enumSymbols = null;
        IDiaSymbol? typeSymbol = null;
        IDiaSymbol? global = null;

        try
        {
            typeName = typeName.Replace('+', '.');
            if (_typeSymbols.ContainsKey(typeName))
            {
                return _typeSymbols[typeName];
            }

            TPDebug.Assert(_session is not null, "_session is null");
            _session.GetGlobalScope(out global);
            global.FindChildren(symTag, typeName, 0, out enumSymbols);

            enumSymbols.GetNext(1, out typeSymbol, out uint celt);

#if DEBUG
            if (typeSymbol == null)
            {
                IDiaEnumSymbols? enumAllSymbols = null;
                try
                {
                    global.FindChildren(symTag, null, 0, out enumAllSymbols);
                    List<string> children = new();

                    while (true)
                    {
                        enumAllSymbols.GetNext(1, out IDiaSymbol? childSymbol, out uint fetchedCount);
                        if (fetchedCount == 0 || childSymbol == null)
                        {
                            break;
                        }

                        childSymbol.GetName(out var childSymbolName);
                        children.Add(childSymbolName);
                        ReleaseComObject(ref childSymbol);
                    }

                    TPDebug.Assert(children.Count > 0);
                }
                finally
                {
                    ReleaseComObject(ref enumAllSymbols);
                }
            }

#endif
        }
        finally
        {
            ReleaseComObject(ref enumSymbols);
            ReleaseComObject(ref global);
        }

        if (typeSymbol != null)
        {
            _typeSymbols[typeName] = typeSymbol;
        }

        return typeSymbol;
    }

    private IDiaSymbol? GetMethodSymbol(IDiaSymbol typeSymbol, string methodName)
    {
        ValidateArg.NotNull(typeSymbol, nameof(typeSymbol));
        ValidateArg.NotNullOrEmpty(methodName, nameof(methodName));

        IDiaEnumSymbols? enumSymbols = null;
        IDiaSymbol? methodSymbol = null;
        Dictionary<string, IDiaSymbol> methodSymbolsForType;

        try
        {
            typeSymbol.GetName(out string symbolName);
            if (_methodSymbols.ContainsKey(symbolName))
            {
                methodSymbolsForType = _methodSymbols[symbolName];
                if (methodSymbolsForType.ContainsKey(methodName))
                {
                    return methodSymbolsForType[methodName];
                }
            }
            else
            {
                methodSymbolsForType = new Dictionary<string, IDiaSymbol>();
                _methodSymbols[symbolName] = methodSymbolsForType;
            }

            typeSymbol.FindChildren(SymTagEnum.SymTagFunction, methodName, 0, out enumSymbols);

            enumSymbols.GetNext(1, out methodSymbol, out uint celtFetched);

#if DEBUG
            if (methodSymbol == null)
            {
                IDiaEnumSymbols? enumAllSymbols = null;
                try
                {
                    typeSymbol.FindChildren(SymTagEnum.SymTagFunction, null, 0, out enumAllSymbols);
                    List<string> children = new();

                    while (true)
                    {
                        enumAllSymbols.GetNext(1, out IDiaSymbol? childSymbol, out uint fetchedCount);
                        if (fetchedCount == 0 || childSymbol == null)
                        {
                            break;
                        }

                        childSymbol.GetName(out string childSymbolName);
                        children.Add(childSymbolName);
                        ReleaseComObject(ref childSymbol);
                    }

                    TPDebug.Assert(children.Count > 0);
                }
                finally
                {
                    ReleaseComObject(ref enumAllSymbols);
                }
            }

#endif
        }
        finally
        {
            ReleaseComObject(ref enumSymbols);
        }

        if (methodSymbol != null)
        {
            methodSymbolsForType[methodName] = methodSymbol;
        }

        return methodSymbol;
    }

    /// <summary>
    /// Update the method symbol cache.
    /// </summary>
    private static void UpdateMethodSymbolCache(string methodName, IDiaSymbol methodSymbol, Dictionary<string, IDiaSymbol> methodSymbolCache)
    {
        TPDebug.Assert(!StringUtils.IsNullOrEmpty(methodName), "MethodName cannot be empty.");
        TPDebug.Assert(methodSymbol != null, "Method symbol cannot be null.");
        TPDebug.Assert(methodSymbolCache != null, "Method symbol cache cannot be null.");

        // #827589, In case a type has overloaded methods, then there could be a method already in the
        // cache which should be disposed.
        if (methodSymbolCache.TryGetValue(methodName, out IDiaSymbol? oldSymbol))
        {
            ReleaseComObject(ref oldSymbol);
        }

        methodSymbolCache[methodName] = methodSymbol;
    }

    private static void ReleaseComObject<T>(ref T? obj)
        where T : class
    {
        if (obj != null)
        {
            Marshal.FinalReleaseComObject(obj);
            obj = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            foreach (Dictionary<string, IDiaSymbol> methodSymbolsForType in _methodSymbols.Values)
            {
                foreach (IDiaSymbol methodSymbol in methodSymbolsForType.Values)
                {
                    IDiaSymbol? symToRelease = methodSymbol;
                    ReleaseComObject(ref symToRelease);
                }

                methodSymbolsForType.Clear();
            }

            _methodSymbols.Clear();
            foreach (IDiaSymbol typeSymbol in _typeSymbols.Values)
            {
                IDiaSymbol? symToRelease = typeSymbol;
                ReleaseComObject(ref symToRelease);
            }

            _typeSymbols.Clear();
            ReleaseComObject(ref _session);
            ReleaseComObject(ref _source);
        }

        _isDisposed = true;
    }
}
