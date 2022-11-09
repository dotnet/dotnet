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
[assembly: AssemblyTitle("System.Composition.TypedParts")]
[assembly: AssemblyDescription("System.Composition.TypedParts")]
[assembly: AssemblyDefaultAlias("System.Composition.TypedParts")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("1.0.31.0")]




namespace System.Composition
{
    public static partial class CompositionContextExtensions
    {
        public static void SatisfyImports(this System.Composition.CompositionContext compositionContext, object objectWithLooseImports) { }
        public static void SatisfyImports(this System.Composition.CompositionContext compositionContext, object objectWithLooseImports, System.Composition.Convention.AttributedModelProvider conventions) { }
    }
}
namespace System.Composition.Hosting
{
    public partial class ContainerConfiguration
    {
        public ContainerConfiguration() { }
        public System.Composition.Hosting.CompositionHost CreateContainer() { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithAssemblies(System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithAssemblies(System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies, System.Composition.Convention.AttributedModelProvider conventions) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithAssembly(System.Reflection.Assembly assembly) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithAssembly(System.Reflection.Assembly assembly, System.Composition.Convention.AttributedModelProvider conventions) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithDefaultConventions(System.Composition.Convention.AttributedModelProvider conventions) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithPart(System.Type partType) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithPart(System.Type partType, System.Composition.Convention.AttributedModelProvider conventions) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithParts(System.Collections.Generic.IEnumerable<System.Type> partTypes) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithParts(System.Collections.Generic.IEnumerable<System.Type> partTypes, System.Composition.Convention.AttributedModelProvider conventions) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithParts(params System.Type[] partTypes) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithPart<TPart>() { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithPart<TPart>(System.Composition.Convention.AttributedModelProvider conventions) { throw null; }
        public System.Composition.Hosting.ContainerConfiguration WithProvider(System.Composition.Hosting.Core.ExportDescriptorProvider exportDescriptorProvider) { throw null; }
    }
}
