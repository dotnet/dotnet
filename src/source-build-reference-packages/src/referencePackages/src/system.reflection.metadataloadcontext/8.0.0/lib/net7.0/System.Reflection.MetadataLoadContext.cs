// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.MetadataLoadContext")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides read-only reflection on assemblies in an isolated context with support for assemblies that target different processor architectures and runtimes. Using MetadataLoadContext enables you to inspect assemblies without loading them into the main execution context. Assemblies in MetadataLoadContext are treated only as metadata, that is, you can read information about their members, but cannot execute any code contained in them.")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Reflection.MetadataLoadContext")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection
{
    public abstract partial class MetadataAssemblyResolver
    {
        public abstract Assembly? Resolve(MetadataLoadContext context, AssemblyName assemblyName);
    }

    public sealed partial class MetadataLoadContext : IDisposable
    {
        public MetadataLoadContext(MetadataAssemblyResolver resolver, string? coreAssemblyName = null) { }

        public Assembly? CoreAssembly { get { throw null; } }

        public void Dispose() { }

        public Collections.Generic.IEnumerable<Assembly> GetAssemblies() { throw null; }

        public Assembly LoadFromAssemblyName(AssemblyName assemblyName) { throw null; }

        public Assembly LoadFromAssemblyName(string assemblyName) { throw null; }

        public Assembly LoadFromAssemblyPath(string assemblyPath) { throw null; }

        public Assembly LoadFromByteArray(byte[] assembly) { throw null; }

        public Assembly LoadFromStream(IO.Stream assembly) { throw null; }
    }

    public partial class PathAssemblyResolver : MetadataAssemblyResolver
    {
        public PathAssemblyResolver(Collections.Generic.IEnumerable<string> assemblyPaths) { }

        public override Assembly? Resolve(MetadataLoadContext context, AssemblyName assemblyName) { throw null; }
    }
}