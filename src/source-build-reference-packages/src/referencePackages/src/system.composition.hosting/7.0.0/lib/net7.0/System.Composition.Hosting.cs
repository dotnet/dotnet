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
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Composition.Hosting")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides Managed Extensibility Framework types that are useful to developers of extensible applications, or hosts.\r\n\r\nCommonly Used Types:\r\nSystem.Composition.Hosting.CompositionHost")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Composition.Hosting")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("7.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Composition.Hosting
{
    public sealed partial class CompositionHost : CompositionContext, IDisposable
    {
        public static CompositionHost CreateCompositionHost(Collections.Generic.IEnumerable<Core.ExportDescriptorProvider> providers) { throw null; }

        public static CompositionHost CreateCompositionHost(params Core.ExportDescriptorProvider[] providers) { throw null; }

        public void Dispose() { }

        public override bool TryGetExport(Core.CompositionContract contract, out object export) { throw null; }
    }
}

namespace System.Composition.Hosting.Core
{
    public delegate object CompositeActivator(LifetimeContext context, CompositionOperation operation);
    public partial class CompositionDependency
    {
        public CompositionContract Contract { get { throw null; } }

        public bool IsPrerequisite { get { throw null; } }

        public object Site { get { throw null; } }

        public ExportDescriptorPromise Target { get { throw null; } }

        public static CompositionDependency Missing(CompositionContract contract, object site) { throw null; }

        public static CompositionDependency Oversupplied(CompositionContract contract, Collections.Generic.IEnumerable<ExportDescriptorPromise> targets, object site) { throw null; }

        public static CompositionDependency Satisfied(CompositionContract contract, ExportDescriptorPromise target, bool isPrerequisite, object site) { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class CompositionOperation : IDisposable
    {
        public void AddNonPrerequisiteAction(Action action) { }

        public void AddPostCompositionAction(Action action) { }

        public void Dispose() { }

        public static object Run(LifetimeContext outermostLifetimeContext, CompositeActivator compositionRootActivator) { throw null; }
    }

    public abstract partial class DependencyAccessor
    {
        protected DependencyAccessor() { }

        protected abstract Collections.Generic.IEnumerable<ExportDescriptorPromise> GetPromises(CompositionContract exportKey);
        public Collections.Generic.IEnumerable<CompositionDependency> ResolveDependencies(object site, CompositionContract contract, bool isPrerequisite) { throw null; }

        public CompositionDependency ResolveRequiredDependency(object site, CompositionContract contract, bool isPrerequisite) { throw null; }

        public bool TryResolveOptionalDependency(object site, CompositionContract contract, bool isPrerequisite, out CompositionDependency dependency) { throw null; }
    }

    public abstract partial class ExportDescriptor
    {
        protected ExportDescriptor() { }

        public abstract CompositeActivator Activator { get; }
        public abstract Collections.Generic.IDictionary<string, object> Metadata { get; }

        public static ExportDescriptor Create(CompositeActivator activator, Collections.Generic.IDictionary<string, object> metadata) { throw null; }
    }

    public partial class ExportDescriptorPromise
    {
        public ExportDescriptorPromise(CompositionContract contract, string origin, bool isShared, Func<Collections.Generic.IEnumerable<CompositionDependency>> dependencies, Func<Collections.Generic.IEnumerable<CompositionDependency>, ExportDescriptor> getDescriptor) { }

        public CompositionContract Contract { get { throw null; } }

        public Collections.ObjectModel.ReadOnlyCollection<CompositionDependency> Dependencies { get { throw null; } }

        public bool IsShared { get { throw null; } }

        public string Origin { get { throw null; } }

        public ExportDescriptor GetDescriptor() { throw null; }

        public override string ToString() { throw null; }
    }

    public abstract partial class ExportDescriptorProvider
    {
        protected static readonly Func<Collections.Generic.IEnumerable<CompositionDependency>> NoDependencies;
        protected static readonly Collections.Generic.IEnumerable<ExportDescriptorPromise> NoExportDescriptors;
        protected static readonly Collections.Generic.IDictionary<string, object> NoMetadata;
        protected ExportDescriptorProvider() { }

        public abstract Collections.Generic.IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor);
    }

    public sealed partial class LifetimeContext : CompositionContext, IDisposable
    {
        public void AddBoundInstance(IDisposable instance) { }

        public static int AllocateSharingId() { throw null; }

        public void Dispose() { }

        public LifetimeContext FindContextWithin(string sharingBoundary) { throw null; }

        public object GetOrCreate(int sharingId, CompositionOperation operation, CompositeActivator creator) { throw null; }

        public override string ToString() { throw null; }

        public override bool TryGetExport(CompositionContract contract, out object export) { throw null; }
    }
}