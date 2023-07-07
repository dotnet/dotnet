// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Resources.Extensions")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Resources.Extensions")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.46214")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.0.0+4ac4c0367003fe3973a3648eb0715ddb0e3bbcea")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("System.Resources.Extensions")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Resources.Extensions
{
    public sealed partial class DeserializingResourceReader : Collections.IEnumerable, IDisposable, IResourceReader
    {
        public DeserializingResourceReader(IO.Stream stream) { }

        public DeserializingResourceReader(string fileName) { }

        public void Close() { }

        public void Dispose() { }

        public Collections.IDictionaryEnumerator GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class PreserializedResourceWriter : IDisposable, IResourceWriter
    {
        public PreserializedResourceWriter(IO.Stream stream) { }

        public PreserializedResourceWriter(string fileName) { }

        public void AddActivatorResource(string name, IO.Stream value, string typeName, bool closeAfterWrite = false) { }

        public void AddBinaryFormattedResource(string name, byte[] value, string typeName = null) { }

        public void AddResource(string name, byte[] value) { }

        public void AddResource(string name, IO.Stream value, bool closeAfterWrite = false) { }

        public void AddResource(string name, object value) { }

        public void AddResource(string name, string value, string typeName) { }

        public void AddResource(string name, string value) { }

        public void AddTypeConverterResource(string name, byte[] value, string typeName) { }

        public void Close() { }

        public void Dispose() { }

        public void Generate() { }
    }
}