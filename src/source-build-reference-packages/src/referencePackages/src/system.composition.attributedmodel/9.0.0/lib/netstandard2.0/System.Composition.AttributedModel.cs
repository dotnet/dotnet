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
[assembly: System.Reflection.AssemblyDefaultAlias("System.Composition.AttributedModel")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides the foundational attributes that allow you to declare parts for composition, such as imports, exports, and metadata with the Managed Extensibility Framework (MEF).")]
[assembly: System.Reflection.AssemblyFileVersion("9.0.24.52809")]
[assembly: System.Reflection.AssemblyInformationalVersion("9.0.0+9d5a6a9aa463d6d10b0b0ba6d5982cc82f363dc3")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Composition.AttributedModel")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("9.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Composition
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public partial class ExportAttribute : Attribute
    {
        public ExportAttribute() { }
        public ExportAttribute(string contractName, Type contractType) { }
        public ExportAttribute(string contractName) { }
        public ExportAttribute(Type contractType) { }
        public string ContractName { get { throw null; } }
        public Type ContractType { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public sealed partial class ExportMetadataAttribute : Attribute
    {
        public ExportMetadataAttribute(string name, object value) { }
        public string Name { get { throw null; } }
        public object Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public partial class ImportAttribute : Attribute
    {
        public ImportAttribute() { }
        public ImportAttribute(string contractName) { }
        public bool AllowDefault { get { throw null; } set { } }
        public string ContractName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed partial class ImportingConstructorAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public partial class ImportManyAttribute : Attribute
    {
        public ImportManyAttribute() { }
        public ImportManyAttribute(string contractName) { }
        public string ContractName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed partial class ImportMetadataConstraintAttribute : Attribute
    {
        public ImportMetadataConstraintAttribute(string name, object value) { }
        public string Name { get { throw null; } }
        public object Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed partial class MetadataAttributeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed partial class OnImportsSatisfiedAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public partial class PartMetadataAttribute : Attribute
    {
        public PartMetadataAttribute(string name, object value) { }
        public string Name { get { throw null; } }
        public object Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed partial class PartNotDiscoverableAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public partial class SharedAttribute : PartMetadataAttribute
    {
        public SharedAttribute() : base(default!, default!) { }
        public SharedAttribute(string sharingBoundaryName) : base(default!, default!) { }
        public string SharingBoundary { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, Inherited = false)]
    [MetadataAttribute]
    [CLSCompliant(false)]
    public sealed partial class SharingBoundaryAttribute : Attribute
    {
        public SharingBoundaryAttribute(params string[] sharingBoundaryNames) { }
        public Collections.ObjectModel.ReadOnlyCollection<string> SharingBoundaryNames { get { throw null; } }
    }
}

namespace System.Composition.Convention
{
    public abstract partial class AttributedModelProvider
    {
        public abstract Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(Type reflectedType, Reflection.MemberInfo member);
        public abstract Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(Type reflectedType, Reflection.ParameterInfo parameter);
    }
}