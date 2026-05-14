// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;

/// <summary>
/// Caches filename and line number for symbols in assembly.
/// </summary>
internal interface ISymbolReader : IDisposable
{
    /// <summary>
    /// Cache symbols from binary path
    /// </summary>
    /// <param name="binaryPath">
    /// The binary path is assembly path Ex: \path\to\bin\Debug\simpleproject.dll
    /// </param>
    /// <param name="searchPath">
    /// search path.
    /// </param>
    void CacheSymbols(string binaryPath, string? searchPath);

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
    /// Returns INavigationData which contains file name and line number.
    /// </returns>
    INavigationData? GetNavigationData(string declaringTypeName, string methodName);
}
