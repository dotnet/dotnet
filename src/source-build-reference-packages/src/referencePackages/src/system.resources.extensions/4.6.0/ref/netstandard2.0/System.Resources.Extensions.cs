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
[assembly: AssemblyTitle("System.Resources.Extensions")]
[assembly: AssemblyDescription("System.Resources.Extensions")]
[assembly: AssemblyDefaultAlias("System.Resources.Extensions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.700.19.46214")]
[assembly: AssemblyInformationalVersion("4.700.19.46214 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.Resources.Extensions
{
    public sealed partial class DeserializingResourceReader : System.Collections.IEnumerable, System.IDisposable, System.Resources.IResourceReader
    {
        public DeserializingResourceReader(System.IO.Stream stream) { }
        public DeserializingResourceReader(string fileName) { }
        public void Close() { }
        public void Dispose() { }
        public System.Collections.IDictionaryEnumerator GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public sealed partial class PreserializedResourceWriter : System.IDisposable, System.Resources.IResourceWriter
    {
        public PreserializedResourceWriter(System.IO.Stream stream) { }
        public PreserializedResourceWriter(string fileName) { }
        public void AddActivatorResource(string name, System.IO.Stream value, string typeName, bool closeAfterWrite = false) { }
        public void AddBinaryFormattedResource(string name, byte[] value, string typeName = null) { }
        public void AddResource(string name, byte[] value) { }
        public void AddResource(string name, System.IO.Stream value, bool closeAfterWrite = false) { }
        public void AddResource(string name, object value) { }
        public void AddResource(string name, string value) { }
        public void AddResource(string name, string value, string typeName) { }
        public void AddTypeConverterResource(string name, byte[] value, string typeName) { }
        public void Close() { }
        public void Dispose() { }
        public void Generate() { }
    }
}
