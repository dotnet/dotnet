// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Reflection.AssemblyDescription("System.Runtime.Serialization.Primitives.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Runtime.Serialization.Primitives.dll")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyTitle("System.Runtime.Serialization.Primitives.dll")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Runtime.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed partial class CollectionDataContractAttribute : Attribute
    {
        public bool IsReference { get { throw null; } set { } }

        public string ItemName { get { throw null; } set { } }

        public string KeyName { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public string Namespace { get { throw null; } set { } }

        public string ValueName { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, Inherited = false, AllowMultiple = true)]
    public sealed partial class ContractNamespaceAttribute : Attribute
    {
        public ContractNamespaceAttribute(string contractNamespace) { }

        public string ClrNamespace { get { throw null; } set { } }

        public string ContractNamespace { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public sealed partial class DataContractAttribute : Attribute
    {
        public bool IsReference { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public string Namespace { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed partial class DataMemberAttribute : Attribute
    {
        public bool EmitDefaultValue { get { throw null; } set { } }

        public bool IsRequired { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public int Order { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed partial class EnumMemberAttribute : Attribute
    {
        public string Value { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed partial class IgnoreDataMemberAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed partial class OnDeserializedAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed partial class OnDeserializingAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed partial class OnSerializedAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed partial class OnSerializingAttribute : Attribute
    {
    }

    public partial class SerializationException : Exception
    {
        public SerializationException() { }

        public SerializationException(string message, Exception innerException) { }

        public SerializationException(string message) { }
    }

    public partial struct StreamingContext
    {
        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }
}