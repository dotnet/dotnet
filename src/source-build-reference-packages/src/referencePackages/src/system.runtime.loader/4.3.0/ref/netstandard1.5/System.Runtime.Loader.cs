// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Runtime.Loader")]
[assembly: System.Reflection.AssemblyDescription("System.Runtime.Loader")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Runtime.Loader")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection.Metadata
{
    public static partial class AssemblyExtensions
    {
        [CLSCompliant(false)]
        public static unsafe bool TryGetRawMetadata(this Assembly assembly, out byte* blob, out int length) { throw null; }
    }
}

namespace System.Runtime.Loader
{
    public abstract partial class AssemblyLoadContext
    {
        public static AssemblyLoadContext Default { get { throw null; } }

        public event Func<AssemblyLoadContext, Reflection.AssemblyName, Reflection.Assembly> Resolving { add { } remove { } }

        public event Action<AssemblyLoadContext> Unloading { add { } remove { } }

        public static Reflection.AssemblyName GetAssemblyName(string assemblyPath) { throw null; }

        public static AssemblyLoadContext GetLoadContext(Reflection.Assembly assembly) { throw null; }

        protected abstract Reflection.Assembly Load(Reflection.AssemblyName assemblyName);
        public Reflection.Assembly LoadFromAssemblyName(Reflection.AssemblyName assemblyName) { throw null; }

        public Reflection.Assembly LoadFromAssemblyPath(string assemblyPath) { throw null; }

        public Reflection.Assembly LoadFromNativeImagePath(string nativeImagePath, string assemblyPath) { throw null; }

        public Reflection.Assembly LoadFromStream(IO.Stream assembly, IO.Stream assemblySymbols) { throw null; }

        public Reflection.Assembly LoadFromStream(IO.Stream assembly) { throw null; }

        protected virtual IntPtr LoadUnmanagedDll(string unmanagedDllName) { throw null; }

        protected IntPtr LoadUnmanagedDllFromPath(string unmanagedDllPath) { throw null; }

        public void SetProfileOptimizationRoot(string directoryPath) { }

        public void StartProfileOptimization(string profile) { }
    }
}