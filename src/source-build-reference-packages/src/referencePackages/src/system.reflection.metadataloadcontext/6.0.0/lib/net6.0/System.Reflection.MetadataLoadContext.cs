// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.MetadataLoadContext")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides read-only reflection on assemblies in an isolated context with support for assemblies that have different architectures and runtimes.\r\n\r\nCommonly Used Types:\r\nSystem.Reflection.MetadataLoadContext\r\nSystem.Reflection.MetadataAssemblyResolver")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Reflection.MetadataLoadContext")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection
{
    public abstract partial class MetadataAssemblyResolver
    {
        protected MetadataAssemblyResolver() { }

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