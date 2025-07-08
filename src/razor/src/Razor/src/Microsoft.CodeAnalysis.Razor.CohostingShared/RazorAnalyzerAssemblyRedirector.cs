// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Reflection;
using IRazorAnalyzerAssemblyRedirector = Microsoft.CodeAnalysis.ExternalAccess.Razor.RazorAnalyzerAssemblyRedirector.IRazorAnalyzerAssemblyRedirector;

namespace Microsoft.VisualStudio.Razor;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[Export(typeof(IRazorAnalyzerAssemblyRedirector))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class RazorAnalyzerAssemblyRedirector() : IRazorAnalyzerAssemblyRedirector
{
    private static readonly ImmutableArray<Type> s_compilerAssemblyTypes = [

        typeof(CodeAnalysis.Razor.CompilerFeatures), // Microsoft.CodeAnalysis.Razor.Compiler.dll
        typeof(AspNetCore.Razor.ArgHelper), // Microsoft.AspNetCore.Razor.Utilities.Shared.dll

        // The following dependencies will be provided by the Compiler ALC so its not strictly required to redirect them, but we do so for completeness. 
        typeof(Microsoft.Extensions.ObjectPool.ObjectPool), // Microsoft.Extensions.ObjectPool.dll
        typeof(ImmutableArray) // System.Collections.Immutable.dll
    ];

    private static readonly FrozenDictionary<string, string> s_compilerAssemblyMap = s_compilerAssemblyTypes.ToFrozenDictionary(t => t.Assembly.GetName().Name!, t => GetAssemblyLocation(t.Assembly));

    public string? RedirectPath(string fullPath)
    {
        var name = Path.GetFileNameWithoutExtension(fullPath);
        return s_compilerAssemblyMap.TryGetValue(name, out var path) ? path : null;
    }

    private static string GetAssemblyLocation(Assembly assembly)
    {
        var location = assembly.Location;
        var name = Path.GetFileName(location);
        var directory = Path.GetDirectoryName(location) ?? "";

        // In VS on windows, depending on who wins the race to load these assemblies, the base directory will either be the tooling root (if Roslyn wins)
        // or the ServiceHubCore subfolder (razor). In the root directory these are netstandard2.0 targeted, in ServiceHubCore they are .NET targeted.
        // We need to always pick the same set of assemblies regardless of who causes us to load. Because this code only runs in a .NET based host,
        // we want to prefer the .NET targeted ServiceHubCore versions if they exist.
        var serviceHubCoreVersion = Path.Combine(directory, "ServiceHubCore", name);
        if (File.Exists(serviceHubCoreVersion))
        {
            return serviceHubCoreVersion;
        }

        return location;
    }
}
