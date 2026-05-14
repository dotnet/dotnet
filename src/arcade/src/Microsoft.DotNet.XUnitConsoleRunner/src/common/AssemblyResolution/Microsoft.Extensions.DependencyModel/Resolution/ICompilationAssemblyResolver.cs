// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Internal.Microsoft.Extensions.DependencyModel.Resolution
{
    internal interface ICompilationAssemblyResolver
    {
        bool TryResolveAssemblyPaths(CompilationLibrary library, List<string> assemblies);
    }
}
