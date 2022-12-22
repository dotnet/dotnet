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
using System.Runtime.Serialization;
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
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.1.0")]

[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.CollectionDataContractAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.ContractNamespaceAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.DataContractAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.DataMemberAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.EnumMemberAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.IgnoreDataMemberAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.InvalidDataContractException))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.KnownTypeAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.OnDeserializedAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.OnDeserializingAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.OnSerializedAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.OnSerializingAttribute))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.SerializationException))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.StreamingContext))]



namespace System.Runtime.Serialization
{
    public partial interface ISerializationSurrogateProvider
    {
        object GetDeserializedObject(object obj, System.Type targetType);
        object GetObjectToSerialize(object obj, System.Type targetType);
        System.Type GetSurrogateType(System.Type type);
    }
}
