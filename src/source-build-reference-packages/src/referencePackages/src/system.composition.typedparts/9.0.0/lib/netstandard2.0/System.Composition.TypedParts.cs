// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Composition.TypedParts")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides container configuration and some extension methods for the Managed Extensibility Framework (MEF).")]
[assembly: System.Reflection.AssemblyFileVersion("9.0.24.52809")]
[assembly: System.Reflection.AssemblyInformationalVersion("9.0.0+9d5a6a9aa463d6d10b0b0ba6d5982cc82f363dc3")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Composition.TypedParts")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("9.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Composition
{
    public static partial class CompositionContextExtensions
    {
        public static void SatisfyImports(this CompositionContext compositionContext, object objectWithLooseImports, Convention.AttributedModelProvider conventions) { }
        public static void SatisfyImports(this CompositionContext compositionContext, object objectWithLooseImports) { }
    }
}

namespace System.Composition.Hosting
{
    public partial class ContainerConfiguration
    {
        public CompositionHost CreateContainer() { throw null; }
        public ContainerConfiguration WithAssemblies(Collections.Generic.IEnumerable<Reflection.Assembly> assemblies, Convention.AttributedModelProvider conventions) { throw null; }
        public ContainerConfiguration WithAssemblies(Collections.Generic.IEnumerable<Reflection.Assembly> assemblies) { throw null; }
        public ContainerConfiguration WithAssembly(Reflection.Assembly assembly, Convention.AttributedModelProvider conventions) { throw null; }
        public ContainerConfiguration WithAssembly(Reflection.Assembly assembly) { throw null; }
        public ContainerConfiguration WithDefaultConventions(Convention.AttributedModelProvider conventions) { throw null; }
        public ContainerConfiguration WithExport(Type contractType, object exportedInstance, string contractName = null, Collections.Generic.IDictionary<string, object> metadata = null) { throw null; }
        public ContainerConfiguration WithExport(Type contractType, object exportedInstance) { throw null; }
        public ContainerConfiguration WithExport<TExport>(TExport exportedInstance, string contractName = null, Collections.Generic.IDictionary<string, object> metadata = null) { throw null; }
        public ContainerConfiguration WithExport<TExport>(TExport exportedInstance) { throw null; }
        public ContainerConfiguration WithPart(Type partType, Convention.AttributedModelProvider conventions) { throw null; }
        public ContainerConfiguration WithPart(Type partType) { throw null; }
        public ContainerConfiguration WithPart<TPart>() { throw null; }
        public ContainerConfiguration WithPart<TPart>(Convention.AttributedModelProvider conventions) { throw null; }
        public ContainerConfiguration WithParts(Collections.Generic.IEnumerable<Type> partTypes, Convention.AttributedModelProvider conventions) { throw null; }
        public ContainerConfiguration WithParts(Collections.Generic.IEnumerable<Type> partTypes) { throw null; }
        public ContainerConfiguration WithParts(params Type[] partTypes) { throw null; }
        public ContainerConfiguration WithProvider(Core.ExportDescriptorProvider exportDescriptorProvider) { throw null; }
    }
}