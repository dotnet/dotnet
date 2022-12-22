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
using System.Xml;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Runtime.Serialization.Xml")]
[assembly: AssemblyDescription("System.Runtime.Serialization.Xml")]
[assembly: AssemblyDefaultAlias("System.Runtime.Serialization.Xml")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.1.0")]

[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.DataContractResolver))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.DataContractSerializer))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.DataContractSerializerSettings))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.InvalidDataContractException))]
[assembly: TypeForwardedTo(typeof(System.Runtime.Serialization.XmlObjectSerializer))]
[assembly: TypeForwardedTo(typeof(System.Xml.IXmlDictionary))]
[assembly: TypeForwardedTo(typeof(System.Xml.OnXmlDictionaryReaderClose))]
[assembly: TypeForwardedTo(typeof(System.Xml.UniqueId))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlBinaryReaderSession))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlBinaryWriterSession))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlDictionary))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlDictionaryReader))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlDictionaryReaderQuotas))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlDictionaryReaderQuotaTypes))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlDictionaryString))]
[assembly: TypeForwardedTo(typeof(System.Xml.XmlDictionaryWriter))]



namespace System.Runtime.Serialization
{
    public static partial class DataContractSerializerExtensions
    {
        public static System.Runtime.Serialization.ISerializationSurrogateProvider GetSerializationSurrogateProvider(this System.Runtime.Serialization.DataContractSerializer serializer) { throw null; }
        public static void SetSerializationSurrogateProvider(this System.Runtime.Serialization.DataContractSerializer serializer, System.Runtime.Serialization.ISerializationSurrogateProvider provider) { }
    }
}
