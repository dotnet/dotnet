// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.DependencyModel.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v1.3", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("Windows_NT_Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Abstractions for reading `.deps` files.")]
[assembly: System.Reflection.AssemblyFileVersion("2.1.0")]
[assembly: System.Reflection.AssemblyInformationalVersion("2.1.0 @BuiltBy: dlab-DDVSOWINAGE024 @Branch: release/2.1 @SrcCode: https://github.com/dotnet/core-setup/tree/caa7b7e2bad98e56a687fb5cbaf60825500800f7")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.DependencyModel")]
[assembly: System.Reflection.AssemblyVersionAttribute("2.1.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.DependencyModel
{
    public partial class CompilationLibrary : Library
    {
        public CompilationLibrary(string type, string name, string version, string hash, System.Collections.Generic.IEnumerable<string> assemblies, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable, string path, string hashPath) : base(default!, default!, default!, default!, default!, default) { }

        public CompilationLibrary(string type, string name, string version, string hash, System.Collections.Generic.IEnumerable<string> assemblies, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable) : base(default!, default!, default!, default!, default!, default) { }

        public System.Collections.Generic.IReadOnlyList<string> Assemblies { get { throw null; } }
    }

    public partial class CompilationOptions
    {
        public CompilationOptions(System.Collections.Generic.IEnumerable<string> defines, string languageVersion, string platform, bool? allowUnsafe, bool? warningsAsErrors, bool? optimize, string keyFile, bool? delaySign, bool? publicSign, string debugType, bool? emitEntryPoint, bool? generateXmlDocumentation) { }

        public bool? AllowUnsafe { get { throw null; } }

        public string DebugType { get { throw null; } }

        public static CompilationOptions Default { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<string> Defines { get { throw null; } }

        public bool? DelaySign { get { throw null; } }

        public bool? EmitEntryPoint { get { throw null; } }

        public bool? GenerateXmlDocumentation { get { throw null; } }

        public string KeyFile { get { throw null; } }

        public string LanguageVersion { get { throw null; } }

        public bool? Optimize { get { throw null; } }

        public string Platform { get { throw null; } }

        public bool? PublicSign { get { throw null; } }

        public bool? WarningsAsErrors { get { throw null; } }
    }

    public partial struct Dependency
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Dependency(string name, string version) { }

        public string Name { get { throw null; } }

        public string Version { get { throw null; } }

        public bool Equals(Dependency other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class DependencyContext
    {
        public DependencyContext(TargetInfo target, CompilationOptions compilationOptions, System.Collections.Generic.IEnumerable<CompilationLibrary> compileLibraries, System.Collections.Generic.IEnumerable<RuntimeLibrary> runtimeLibraries, System.Collections.Generic.IEnumerable<RuntimeFallbacks> runtimeGraph) { }

        public CompilationOptions CompilationOptions { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<CompilationLibrary> CompileLibraries { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<RuntimeFallbacks> RuntimeGraph { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<RuntimeLibrary> RuntimeLibraries { get { throw null; } }

        public TargetInfo Target { get { throw null; } }

        public DependencyContext Merge(DependencyContext other) { throw null; }
    }

    public static partial class DependencyContextExtensions
    {
        public static System.Collections.Generic.IEnumerable<System.Reflection.AssemblyName> GetDefaultAssemblyNames(this DependencyContext self) { throw null; }

        public static System.Collections.Generic.IEnumerable<System.Reflection.AssemblyName> GetDefaultAssemblyNames(this RuntimeLibrary self, DependencyContext context) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetDefaultNativeAssets(this DependencyContext self) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetDefaultNativeAssets(this RuntimeLibrary self, DependencyContext context) { throw null; }

        public static System.Collections.Generic.IEnumerable<RuntimeFile> GetDefaultNativeRuntimeFileAssets(this DependencyContext self) { throw null; }

        public static System.Collections.Generic.IEnumerable<RuntimeFile> GetDefaultNativeRuntimeFileAssets(this RuntimeLibrary self, DependencyContext context) { throw null; }

        public static System.Collections.Generic.IEnumerable<System.Reflection.AssemblyName> GetRuntimeAssemblyNames(this DependencyContext self, string runtimeIdentifier) { throw null; }

        public static System.Collections.Generic.IEnumerable<System.Reflection.AssemblyName> GetRuntimeAssemblyNames(this RuntimeLibrary self, DependencyContext context, string runtimeIdentifier) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetRuntimeNativeAssets(this DependencyContext self, string runtimeIdentifier) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetRuntimeNativeAssets(this RuntimeLibrary self, DependencyContext context, string runtimeIdentifier) { throw null; }

        public static System.Collections.Generic.IEnumerable<RuntimeFile> GetRuntimeNativeRuntimeFileAssets(this DependencyContext self, string runtimeIdentifier) { throw null; }

        public static System.Collections.Generic.IEnumerable<RuntimeFile> GetRuntimeNativeRuntimeFileAssets(this RuntimeLibrary self, DependencyContext context, string runtimeIdentifier) { throw null; }
    }

    public partial class DependencyContextJsonReader : IDependencyContextReader, System.IDisposable
    {
        public DependencyContextJsonReader() { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public DependencyContext Read(System.IO.Stream stream) { throw null; }

        public System.Collections.Generic.IEnumerable<Dependency> ReadTargetLibraryDependencies(Newtonsoft.Json.JsonTextReader reader) { throw null; }
    }

    public partial class DependencyContextWriter
    {
        public DependencyContextWriter() { }

        public void Write(DependencyContext context, System.IO.Stream stream) { }
    }

    public partial interface IDependencyContextReader : System.IDisposable
    {
        DependencyContext Read(System.IO.Stream stream);
    }

    public partial class Library
    {
        public Library(string type, string name, string version, string hash, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable, string path, string hashPath, string runtimeStoreManifestName = null) { }

        public Library(string type, string name, string version, string hash, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable, string path, string hashPath) { }

        public Library(string type, string name, string version, string hash, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable) { }

        public System.Collections.Generic.IReadOnlyList<Dependency> Dependencies { get { throw null; } }

        public string Hash { get { throw null; } }

        public string HashPath { get { throw null; } }

        public string Name { get { throw null; } }

        public string Path { get { throw null; } }

        public string RuntimeStoreManifestName { get { throw null; } }

        public bool Serviceable { get { throw null; } }

        public string Type { get { throw null; } }

        public string Version { get { throw null; } }
    }

    public partial class ResourceAssembly
    {
        public ResourceAssembly(string path, string locale) { }

        public string Locale { get { throw null; } set { } }

        public string Path { get { throw null; } set { } }
    }

    public partial class RuntimeAssembly
    {
        public RuntimeAssembly(string assemblyName, string path) { }

        public System.Reflection.AssemblyName Name { get { throw null; } }

        public string Path { get { throw null; } }

        public static RuntimeAssembly Create(string path) { throw null; }
    }

    public partial class RuntimeAssetGroup
    {
        public RuntimeAssetGroup(string runtime, System.Collections.Generic.IEnumerable<RuntimeFile> runtimeFiles) { }

        public RuntimeAssetGroup(string runtime, System.Collections.Generic.IEnumerable<string> assetPaths) { }

        public RuntimeAssetGroup(string runtime, params string[] assetPaths) { }

        public System.Collections.Generic.IReadOnlyList<string> AssetPaths { get { throw null; } }

        public string Runtime { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<RuntimeFile> RuntimeFiles { get { throw null; } }
    }

    public partial class RuntimeFallbacks
    {
        public RuntimeFallbacks(string runtime, System.Collections.Generic.IEnumerable<string> fallbacks) { }

        public RuntimeFallbacks(string runtime, params string[] fallbacks) { }

        public System.Collections.Generic.IReadOnlyList<string> Fallbacks { get { throw null; } set { } }

        public string Runtime { get { throw null; } set { } }
    }

    public partial class RuntimeFile
    {
        public RuntimeFile(string path, string assemblyVersion, string fileVersion) { }

        public string AssemblyVersion { get { throw null; } }

        public string FileVersion { get { throw null; } }

        public string Path { get { throw null; } }
    }

    public partial class RuntimeLibrary : Library
    {
        public RuntimeLibrary(string type, string name, string version, string hash, System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> runtimeAssemblyGroups, System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> nativeLibraryGroups, System.Collections.Generic.IEnumerable<ResourceAssembly> resourceAssemblies, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable, string path, string hashPath, string runtimeStoreManifestName) : base(default!, default!, default!, default!, default!, default) { }

        public RuntimeLibrary(string type, string name, string version, string hash, System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> runtimeAssemblyGroups, System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> nativeLibraryGroups, System.Collections.Generic.IEnumerable<ResourceAssembly> resourceAssemblies, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable, string path, string hashPath) : base(default!, default!, default!, default!, default!, default) { }

        public RuntimeLibrary(string type, string name, string version, string hash, System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> runtimeAssemblyGroups, System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> nativeLibraryGroups, System.Collections.Generic.IEnumerable<ResourceAssembly> resourceAssemblies, System.Collections.Generic.IEnumerable<Dependency> dependencies, bool serviceable) : base(default!, default!, default!, default!, default!, default) { }

        public System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> NativeLibraryGroups { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<ResourceAssembly> ResourceAssemblies { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<RuntimeAssetGroup> RuntimeAssemblyGroups { get { throw null; } }
    }

    public partial class TargetInfo
    {
        public TargetInfo(string framework, string runtime, string runtimeSignature, bool isPortable) { }

        public string Framework { get { throw null; } }

        public bool IsPortable { get { throw null; } }

        public string Runtime { get { throw null; } }

        public string RuntimeSignature { get { throw null; } }
    }
}

namespace Microsoft.Extensions.DependencyModel.Resolution
{
    public partial class CompositeCompilationAssemblyResolver : ICompilationAssemblyResolver
    {
        public CompositeCompilationAssemblyResolver(ICompilationAssemblyResolver[] resolvers) { }

        public bool TryResolveAssemblyPaths(CompilationLibrary library, System.Collections.Generic.List<string> assemblies) { throw null; }
    }

    public partial class DotNetReferenceAssembliesPathResolver
    {
        public static readonly string DotNetReferenceAssembliesPathEnv;
        public DotNetReferenceAssembliesPathResolver() { }

        public static string Resolve() { throw null; }
    }

    public partial interface ICompilationAssemblyResolver
    {
        bool TryResolveAssemblyPaths(CompilationLibrary library, System.Collections.Generic.List<string> assemblies);
    }

    public partial class PackageCompilationAssemblyResolver : ICompilationAssemblyResolver
    {
        public PackageCompilationAssemblyResolver() { }

        public PackageCompilationAssemblyResolver(string nugetPackageDirectory) { }

        public bool TryResolveAssemblyPaths(CompilationLibrary library, System.Collections.Generic.List<string> assemblies) { throw null; }
    }

    public partial class ReferenceAssemblyPathResolver : ICompilationAssemblyResolver
    {
        public ReferenceAssemblyPathResolver() { }

        public ReferenceAssemblyPathResolver(string defaultReferenceAssembliesPath, string[] fallbackSearchPaths) { }

        public bool TryResolveAssemblyPaths(CompilationLibrary library, System.Collections.Generic.List<string> assemblies) { throw null; }
    }
}

namespace System.Collections.Generic
{
    public static partial class CollectionExtensions
    {
        public static IEnumerable<string> GetDefaultAssets(this IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeAssetGroup> self) { throw null; }

        public static Microsoft.Extensions.DependencyModel.RuntimeAssetGroup GetDefaultGroup(this IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeAssetGroup> self) { throw null; }

        public static IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeFile> GetDefaultRuntimeFileAssets(this IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeAssetGroup> self) { throw null; }

        public static IEnumerable<string> GetRuntimeAssets(this IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeAssetGroup> self, string runtime) { throw null; }

        public static IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeFile> GetRuntimeFileAssets(this IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeAssetGroup> self, string runtime) { throw null; }

        public static Microsoft.Extensions.DependencyModel.RuntimeAssetGroup GetRuntimeGroup(this IEnumerable<Microsoft.Extensions.DependencyModel.RuntimeAssetGroup> self, string runtime) { throw null; }
    }
}