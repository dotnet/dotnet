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
[assembly: AssemblyTitle("System.Composition.AttributedModel")]
[assembly: AssemblyDescription("System.Composition.AttributedModel")]
[assembly: AssemblyDefaultAlias("System.Composition.AttributedModel")]
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
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Property, AllowMultiple=true, Inherited=false)]
    public partial class ExportAttribute : System.Attribute
    {
        public ExportAttribute() { }
        public ExportAttribute(string contractName) { }
        public ExportAttribute(string contractName, System.Type contractType) { }
        public ExportAttribute(System.Type contractType) { }
        public string ContractName { get { throw null; } }
        public System.Type ContractType { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Interface | System.AttributeTargets.Property, AllowMultiple=true, Inherited=false)]
    public sealed partial class ExportMetadataAttribute : System.Attribute
    {
        public ExportMetadataAttribute(string name, object value) { }
        public string Name { get { throw null; } }
        public object Value { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter | System.AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
    public partial class ImportAttribute : System.Attribute
    {
        public ImportAttribute() { }
        public ImportAttribute(string contractName) { }
        public bool AllowDefault { get { throw null; } set { } }
        public string ContractName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Constructor, AllowMultiple=false, Inherited=false)]
    public sealed partial class ImportingConstructorAttribute : System.Attribute
    {
        public ImportingConstructorAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter | System.AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
    public partial class ImportManyAttribute : System.Attribute
    {
        public ImportManyAttribute() { }
        public ImportManyAttribute(string contractName) { }
        public string ContractName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Property, Inherited=false)]
    public sealed partial class ImportMetadataConstraintAttribute : System.Attribute
    {
        public ImportMetadataConstraintAttribute(string name, object value) { }
        public string Name { get { throw null; } }
        public object Value { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed partial class MetadataAttributeAttribute : System.Attribute
    {
        public MetadataAttributeAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false)]
    public sealed partial class OnImportsSatisfiedAttribute : System.Attribute
    {
        public OnImportsSatisfiedAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, AllowMultiple=true, Inherited=false)]
    public partial class PartMetadataAttribute : System.Attribute
    {
        public PartMetadataAttribute(string name, object value) { }
        public string Name { get { throw null; } }
        public object Value { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public sealed partial class PartNotDiscoverableAttribute : System.Attribute
    {
        public PartNotDiscoverableAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, Inherited=false)]
    public partial class SharedAttribute : System.Composition.PartMetadataAttribute
    {
        public SharedAttribute() : base (default(string), default(object)) { }
        public SharedAttribute(string sharingBoundaryName) : base (default(string), default(object)) { }
        public string SharingBoundary { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter | System.AttributeTargets.Property, Inherited=false)]
    [System.CLSCompliantAttribute(false)]
    [System.Composition.MetadataAttributeAttribute]
    public sealed partial class SharingBoundaryAttribute : System.Attribute
    {
        public SharingBoundaryAttribute(params string[] sharingBoundaryNames) { }
        public System.Collections.ObjectModel.ReadOnlyCollection<string> SharingBoundaryNames { get { throw null; } }
    }
}
namespace System.Composition.Convention
{
    public abstract partial class AttributedModelProvider
    {
        protected AttributedModelProvider() { }
        public abstract System.Collections.Generic.IEnumerable<System.Attribute> GetCustomAttributes(System.Type reflectedType, System.Reflection.MemberInfo member);
        public abstract System.Collections.Generic.IEnumerable<System.Attribute> GetCustomAttributes(System.Type reflectedType, System.Reflection.ParameterInfo parameter);
    }
}
