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
[assembly: System.Reflection.AssemblyMetadata("NotSupported", "True")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Windows.Extensions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.Versioning.SupportedOSPlatform("windows")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides miscellaneous Windows-specific types\r\n\r\nCommonly Used Types:\r\nSystem.Security.Cryptography.X509Certificates.X509Certificate2UI\r\nSystem.Security.Cryptography.X509Certificates.X509SelectionFlag")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Windows.Extensions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("7.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.FontConverter))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.IconConverter))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.ImageConverter))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.ImageFormatConverter))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Drawing.Printing.MarginsConverter))]
namespace System.Media
{
    [ComponentModel.ToolboxItem(false)]
    public partial class SoundPlayer : ComponentModel.Component, Runtime.Serialization.ISerializable
    {
        public SoundPlayer() { }

        public SoundPlayer(IO.Stream? stream) { }

        protected SoundPlayer(Runtime.Serialization.SerializationInfo serializationInfo, Runtime.Serialization.StreamingContext context) { }

        public SoundPlayer(string soundLocation) { }

        public bool IsLoadCompleted { get { throw null; } }

        public int LoadTimeout { get { throw null; } set { } }

        public string SoundLocation { get { throw null; } set { } }

        public IO.Stream? Stream { get { throw null; } set { } }

        public object? Tag { get { throw null; } set { } }

        public event ComponentModel.AsyncCompletedEventHandler? LoadCompleted { add { } remove { } }

        public event EventHandler? SoundLocationChanged { add { } remove { } }

        public event EventHandler? StreamChanged { add { } remove { } }

        public void Load() { }

        public void LoadAsync() { }

        protected virtual void OnLoadCompleted(ComponentModel.AsyncCompletedEventArgs e) { }

        protected virtual void OnSoundLocationChanged(EventArgs e) { }

        protected virtual void OnStreamChanged(EventArgs e) { }

        public void Play() { }

        public void PlayLooping() { }

        public void PlaySync() { }

        public void Stop() { }

        void Runtime.Serialization.ISerializable.GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public partial class SystemSound
    {
        internal SystemSound() { }

        public void Play() { }
    }

    public static partial class SystemSounds
    {
        public static SystemSound Asterisk { get { throw null; } }

        public static SystemSound Beep { get { throw null; } }

        public static SystemSound Exclamation { get { throw null; } }

        public static SystemSound Hand { get { throw null; } }

        public static SystemSound Question { get { throw null; } }
    }
}

namespace System.Security.Cryptography.X509Certificates
{
    public sealed partial class X509Certificate2UI
    {
        public static void DisplayCertificate(X509Certificate2 certificate, nint hwndParent) { }

        public static void DisplayCertificate(X509Certificate2 certificate) { }

        public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string? title, string? message, X509SelectionFlag selectionFlag, nint hwndParent) { throw null; }

        public static X509Certificate2Collection SelectFromCollection(X509Certificate2Collection certificates, string? title, string? message, X509SelectionFlag selectionFlag) { throw null; }
    }

    public enum X509SelectionFlag
    {
        SingleSelection = 0,
        MultiSelection = 1
    }
}

namespace System.Xaml.Permissions
{
    public partial class XamlAccessLevel
    {
        internal XamlAccessLevel() { }

        public Reflection.AssemblyName AssemblyAccessToAssemblyName { get { throw null; } }

        public string? PrivateAccessToTypeName { get { throw null; } }

        public static XamlAccessLevel AssemblyAccessTo(Reflection.Assembly assembly) { throw null; }

        public static XamlAccessLevel AssemblyAccessTo(Reflection.AssemblyName assemblyName) { throw null; }

        public static XamlAccessLevel PrivateAccessTo(string assemblyQualifiedTypeName) { throw null; }

        public static XamlAccessLevel PrivateAccessTo(Type type) { throw null; }
    }
}