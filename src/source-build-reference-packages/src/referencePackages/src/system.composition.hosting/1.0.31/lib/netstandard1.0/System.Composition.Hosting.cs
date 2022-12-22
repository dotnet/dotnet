// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Composition.Hosting")]
[assembly: AssemblyDescription("System.Composition.Hosting")]
[assembly: AssemblyDefaultAlias("System.Composition.Hosting")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("1.0.31.0")]




namespace System.Composition.Hosting
{
    public sealed partial class CompositionHost : System.Composition.CompositionContext, System.IDisposable
    {
        internal CompositionHost() { }
        public static System.Composition.Hosting.CompositionHost CreateCompositionHost(System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.ExportDescriptorProvider> providers) { throw null; }
        public static System.Composition.Hosting.CompositionHost CreateCompositionHost(params System.Composition.Hosting.Core.ExportDescriptorProvider[] providers) { throw null; }
        public void Dispose() { }
        public override bool TryGetExport(System.Composition.Hosting.Core.CompositionContract contract, out object export) { throw null; }
    }
}
namespace System.Composition.Hosting.Core
{
    public delegate object CompositeActivator(System.Composition.Hosting.Core.LifetimeContext context, System.Composition.Hosting.Core.CompositionOperation operation);
    public partial class CompositionDependency
    {
        internal CompositionDependency() { }
        public System.Composition.Hosting.Core.CompositionContract Contract { get { throw null; } }
        public bool IsPrerequisite { get { throw null; } }
        public object Site { get { throw null; } }
        public System.Composition.Hosting.Core.ExportDescriptorPromise Target { get { throw null; } }
        public static System.Composition.Hosting.Core.CompositionDependency Missing(System.Composition.Hosting.Core.CompositionContract contract, object site) { throw null; }
        public static System.Composition.Hosting.Core.CompositionDependency Oversupplied(System.Composition.Hosting.Core.CompositionContract contract, System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.ExportDescriptorPromise> targets, object site) { throw null; }
        public static System.Composition.Hosting.Core.CompositionDependency Satisfied(System.Composition.Hosting.Core.CompositionContract contract, System.Composition.Hosting.Core.ExportDescriptorPromise target, bool isPrerequisite, object site) { throw null; }
        public override string ToString() { throw null; }
    }
    public sealed partial class CompositionOperation : System.IDisposable
    {
        internal CompositionOperation() { }
        public void AddNonPrerequisiteAction(System.Action action) { }
        public void AddPostCompositionAction(System.Action action) { }
        public void Dispose() { }
        public static object Run(System.Composition.Hosting.Core.LifetimeContext outermostLifetimeContext, System.Composition.Hosting.Core.CompositeActivator compositionRootActivator) { throw null; }
    }
    public abstract partial class DependencyAccessor
    {
        protected DependencyAccessor() { }
        protected abstract System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.ExportDescriptorPromise> GetPromises(System.Composition.Hosting.Core.CompositionContract exportKey);
        public System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.CompositionDependency> ResolveDependencies(object site, System.Composition.Hosting.Core.CompositionContract contract, bool isPrerequisite) { throw null; }
        public System.Composition.Hosting.Core.CompositionDependency ResolveRequiredDependency(object site, System.Composition.Hosting.Core.CompositionContract contract, bool isPrerequisite) { throw null; }
        public bool TryResolveOptionalDependency(object site, System.Composition.Hosting.Core.CompositionContract contract, bool isPrerequisite, out System.Composition.Hosting.Core.CompositionDependency dependency) { throw null; }
    }
    public abstract partial class ExportDescriptor
    {
        protected ExportDescriptor() { }
        public abstract System.Composition.Hosting.Core.CompositeActivator Activator { get; }
        public abstract System.Collections.Generic.IDictionary<string, object> Metadata { get; }
        public static System.Composition.Hosting.Core.ExportDescriptor Create(System.Composition.Hosting.Core.CompositeActivator activator, System.Collections.Generic.IDictionary<string, object> metadata) { throw null; }
    }
    public partial class ExportDescriptorPromise
    {
        public ExportDescriptorPromise(System.Composition.Hosting.Core.CompositionContract contract, string origin, bool isShared, System.Func<System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.CompositionDependency>> dependencies, System.Func<System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.CompositionDependency>, System.Composition.Hosting.Core.ExportDescriptor> getDescriptor) { }
        public System.Composition.Hosting.Core.CompositionContract Contract { get { throw null; } }
        public System.Collections.ObjectModel.ReadOnlyCollection<System.Composition.Hosting.Core.CompositionDependency> Dependencies { get { throw null; } }
        public bool IsShared { get { throw null; } }
        public string Origin { get { throw null; } }
        public System.Composition.Hosting.Core.ExportDescriptor GetDescriptor() { throw null; }
        public override string ToString() { throw null; }
    }
    public abstract partial class ExportDescriptorProvider
    {
        protected static readonly System.Func<System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.CompositionDependency>> NoDependencies;
        protected static readonly System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.ExportDescriptorPromise> NoExportDescriptors;
        protected static readonly System.Collections.Generic.IDictionary<string, object> NoMetadata;
        protected ExportDescriptorProvider() { }
        public abstract System.Collections.Generic.IEnumerable<System.Composition.Hosting.Core.ExportDescriptorPromise> GetExportDescriptors(System.Composition.Hosting.Core.CompositionContract contract, System.Composition.Hosting.Core.DependencyAccessor descriptorAccessor);
    }
    public sealed partial class LifetimeContext : System.Composition.CompositionContext, System.IDisposable
    {
        internal LifetimeContext() { }
        public void AddBoundInstance(System.IDisposable instance) { }
        public static int AllocateSharingId() { throw null; }
        public void Dispose() { }
        public System.Composition.Hosting.Core.LifetimeContext FindContextWithin(string sharingBoundary) { throw null; }
        public object GetOrCreate(int sharingId, System.Composition.Hosting.Core.CompositionOperation operation, System.Composition.Hosting.Core.CompositeActivator creator) { throw null; }
        public override string ToString() { throw null; }
        public override bool TryGetExport(System.Composition.Hosting.Core.CompositionContract contract, out object export) { throw null; }
    }
}
