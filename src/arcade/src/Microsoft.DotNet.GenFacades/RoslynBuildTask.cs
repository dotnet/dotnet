// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Reflection;
#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace Microsoft.DotNet.Build.Tasks
{
    public abstract partial class RoslynBuildTask : BuildTask
    {
        [Required]
        public string RoslynAssembliesPath { get; set; }

        public override bool Execute()
        {
#if NETCOREAPP
            AssemblyLoadContext currentContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly())!;
            currentContext.Resolving += ResolverForRoslyn;
#else
            AppDomain.CurrentDomain.AssemblyResolve += ResolverForRoslyn;
#endif
            try
            {
                return ExecuteCore();
            }
            finally
            {
#if NETCOREAPP
                currentContext.Resolving -= ResolverForRoslyn;
#else
                AppDomain.CurrentDomain.AssemblyResolve -= ResolverForRoslyn;
#endif
            }
        }

        public abstract bool ExecuteCore();
        
#if NETCOREAPP
        private Assembly ResolverForRoslyn(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            return LoadRoslyn(assemblyName, path => context.LoadFromAssemblyPath(path));
        }
#else
        private Assembly ResolverForRoslyn(object sender, ResolveEventArgs args)
        {
            AssemblyName name = new(args.Name);
            return LoadRoslyn(name, path => Assembly.LoadFrom(path));
        }
#endif

        private Assembly LoadRoslyn(AssemblyName name, Func<string, Assembly> loadFromPath)
        {
            const string codeAnalysisName = "Microsoft.CodeAnalysis";
            const string codeAnalysisCsharpName = "Microsoft.CodeAnalysis.CSharp";
            if (name.Name == codeAnalysisName || name.Name == codeAnalysisCsharpName)
            {
                Assembly asm = loadFromPath(Path.Combine(RoslynAssembliesPath!, $"{name.Name}.dll"));
                Version resolvedVersion = asm.GetName().Version;
                if (resolvedVersion < name.Version)
                {
                    throw new Exception($"The minimum version required of Roslyn is '{name.Version}' and you are using '{resolvedVersion}' version of the Roslyn. You can update the sdk to get the latest version.");
                }

                // Being extra defensive but we want to avoid that we accidentally load two different versions of either
                // of the roslyn assemblies from a different location, so let's load them both on the first request.
                Assembly _ = name.Name == codeAnalysisName ?
                    loadFromPath(Path.Combine(RoslynAssembliesPath!, $"{codeAnalysisCsharpName}.dll")) :
                    loadFromPath(Path.Combine(RoslynAssembliesPath!, $"{codeAnalysisName}.dll"));

                return asm;
            }

            return null;
        }
    }
}
