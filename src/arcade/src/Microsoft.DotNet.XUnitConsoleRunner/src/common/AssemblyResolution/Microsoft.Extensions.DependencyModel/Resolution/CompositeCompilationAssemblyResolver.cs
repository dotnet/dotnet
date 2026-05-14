// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Internal.Microsoft.Extensions.DependencyModel.Resolution
{
    internal class CompositeCompilationAssemblyResolver: ICompilationAssemblyResolver
    {
        private readonly ICompilationAssemblyResolver[] _resolvers;

        public CompositeCompilationAssemblyResolver(ICompilationAssemblyResolver[] resolvers)
        {
            _resolvers = resolvers;
        }

        public bool TryResolveAssemblyPaths(CompilationLibrary library, List<string> assemblies)
        {
            foreach (var resolver in _resolvers)
            {
                if (resolver.TryResolveAssemblyPaths(library, assemblies))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
