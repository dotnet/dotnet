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
[assembly: AssemblyTitle("System.Runtime.Serialization.Primitives")]
[assembly: AssemblyDescription("System.Runtime.Serialization.Primitives")]
[assembly: AssemblyDefaultAlias("System.Runtime.Serialization.Primitives")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.0.30319.17929")]
[assembly: AssemblyInformationalVersion("4.0.30319.17929 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.Runtime.Serialization
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Struct, Inherited=false, AllowMultiple=false)]
    public sealed partial class CollectionDataContractAttribute : System.Attribute
    {
        public CollectionDataContractAttribute() { }
        public bool IsReference { get { throw null; } set { } }
        public string ItemName { get { throw null; } set { } }
        public string KeyName { get { throw null; } set { } }
        public string Name { get { throw null; } set { } }
        public string Namespace { get { throw null; } set { } }
        public string ValueName { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Module, Inherited=false, AllowMultiple=true)]
    public sealed partial class ContractNamespaceAttribute : System.Attribute
    {
        public ContractNamespaceAttribute(string contractNamespace) { }
        public string ClrNamespace { get { throw null; } set { } }
        public string ContractNamespace { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Enum | System.AttributeTargets.Struct, Inherited=false, AllowMultiple=false)]
    public sealed partial class DataContractAttribute : System.Attribute
    {
        public DataContractAttribute() { }
        public bool IsReference { get { throw null; } set { } }
        public string Name { get { throw null; } set { } }
        public string Namespace { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Property, Inherited=false, AllowMultiple=false)]
    public sealed partial class DataMemberAttribute : System.Attribute
    {
        public DataMemberAttribute() { }
        public bool EmitDefaultValue { get { throw null; } set { } }
        public bool IsRequired { get { throw null; } set { } }
        public string Name { get { throw null; } set { } }
        public int Order { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field, Inherited=false, AllowMultiple=false)]
    public sealed partial class EnumMemberAttribute : System.Attribute
    {
        public EnumMemberAttribute() { }
        public string Value { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Property, Inherited=false, AllowMultiple=false)]
    public sealed partial class IgnoreDataMemberAttribute : System.Attribute
    {
        public IgnoreDataMemberAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Struct, Inherited=true, AllowMultiple=true)]
    public sealed partial class KnownTypeAttribute : System.Attribute
    {
        public KnownTypeAttribute(string methodName) { }
        public KnownTypeAttribute(System.Type type) { }
        public string MethodName { get { throw null; } }
        public System.Type Type { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false)]
    public sealed partial class OnDeserializedAttribute : System.Attribute
    {
        public OnDeserializedAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false)]
    public sealed partial class OnDeserializingAttribute : System.Attribute
    {
        public OnDeserializingAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false)]
    public sealed partial class OnSerializedAttribute : System.Attribute
    {
        public OnSerializedAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false)]
    public sealed partial class OnSerializingAttribute : System.Attribute
    {
        public OnSerializingAttribute() { }
    }
    public partial class SerializationException : System.Exception
    {
        public SerializationException() { }
        public SerializationException(string message) { }
        public SerializationException(string message, System.Exception innerException) { }
    }
    public partial struct StreamingContext
    {
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
}
