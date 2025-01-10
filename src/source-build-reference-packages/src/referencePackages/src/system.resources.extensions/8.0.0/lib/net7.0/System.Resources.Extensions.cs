// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Resources.Extensions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides classes which read and write resources in a format that supports non-primitive objects.\r\n\r\nCommonly Used Types:\r\nSystem.Resources.Extensions.DeserializingResourceReader\r\nSystem.Resources.Extensions.PreserializedResourceWriter")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Resources.Extensions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Resources.Extensions
{
    public sealed partial class DeserializingResourceReader : IResourceReader, Collections.IEnumerable, IDisposable
    {
        public DeserializingResourceReader(IO.Stream stream) { }

        public DeserializingResourceReader(string fileName) { }

        public void Close() { }

        public void Dispose() { }

        public Collections.IDictionaryEnumerator GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class PreserializedResourceWriter : IResourceWriter, IDisposable
    {
        public PreserializedResourceWriter(IO.Stream stream) { }

        public PreserializedResourceWriter(string fileName) { }

        public void AddActivatorResource(string name, IO.Stream value, string typeName, bool closeAfterWrite = false) { }

        [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public void AddBinaryFormattedResource(string name, byte[] value, string? typeName = null) { }

        public void AddResource(string name, byte[]? value) { }

        public void AddResource(string name, IO.Stream? value, bool closeAfterWrite = false) { }

        public void AddResource(string name, object? value) { }

        public void AddResource(string name, string value, string typeName) { }

        public void AddResource(string name, string? value) { }

        public void AddTypeConverterResource(string name, byte[] value, string typeName) { }

        public void Close() { }

        public void Dispose() { }

        public void Generate() { }
    }
}