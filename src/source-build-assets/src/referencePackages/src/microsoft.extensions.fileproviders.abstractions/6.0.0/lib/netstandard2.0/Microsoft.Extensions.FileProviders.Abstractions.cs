// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.FileProviders.Abstractions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Abstractions of files and directories.\r\n\r\nCommonly Used Types:\r\nMicrosoft.Extensions.FileProviders.IDirectoryContents\r\nMicrosoft.Extensions.FileProviders.IFileInfo\r\nMicrosoft.Extensions.FileProviders.IFileProvider")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.FileProviders.Abstractions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.FileProviders
{
    public partial interface IDirectoryContents : System.Collections.Generic.IEnumerable<IFileInfo>, System.Collections.IEnumerable
    {
        bool Exists { get; }
    }

    public partial interface IFileInfo
    {
        bool Exists { get; }

        bool IsDirectory { get; }

        System.DateTimeOffset LastModified { get; }

        long Length { get; }

        string Name { get; }

        string PhysicalPath { get; }

        System.IO.Stream CreateReadStream();
    }

    public partial interface IFileProvider
    {
        IDirectoryContents GetDirectoryContents(string subpath);
        IFileInfo GetFileInfo(string subpath);
        Primitives.IChangeToken Watch(string filter);
    }

    public partial class NotFoundDirectoryContents : IDirectoryContents, System.Collections.Generic.IEnumerable<IFileInfo>, System.Collections.IEnumerable
    {
        public bool Exists { get { throw null; } }

        public static NotFoundDirectoryContents Singleton { get { throw null; } }

        public System.Collections.Generic.IEnumerator<IFileInfo> GetEnumerator() { throw null; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public partial class NotFoundFileInfo : IFileInfo
    {
        public NotFoundFileInfo(string name) { }

        public bool Exists { get { throw null; } }

        public bool IsDirectory { get { throw null; } }

        public System.DateTimeOffset LastModified { get { throw null; } }

        public long Length { get { throw null; } }

        public string Name { get { throw null; } }

        public string PhysicalPath { get { throw null; } }

        public System.IO.Stream CreateReadStream() { throw null; }
    }

    public partial class NullChangeToken : Primitives.IChangeToken
    {
        internal NullChangeToken() { }

        public bool ActiveChangeCallbacks { get { throw null; } }

        public bool HasChanged { get { throw null; } }

        public static NullChangeToken Singleton { get { throw null; } }

        public System.IDisposable RegisterChangeCallback(System.Action<object> callback, object state) { throw null; }
    }

    public partial class NullFileProvider : IFileProvider
    {
        public IDirectoryContents GetDirectoryContents(string subpath) { throw null; }

        public IFileInfo GetFileInfo(string subpath) { throw null; }

        public Primitives.IChangeToken Watch(string filter) { throw null; }
    }
}