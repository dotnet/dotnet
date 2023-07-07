// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.Configuration.Binder.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("CommitHash", "2679cfd94694a29230726708717ab9c4579a5d80")]
[assembly: System.Reflection.AssemblyMetadata("BuildNumber", "30846")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation.")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Functionality to bind an object to data in configuration providers for Microsoft.Extensions.Configuration.")]
[assembly: System.Reflection.AssemblyFileVersion("2.1.1.18157")]
[assembly: System.Reflection.AssemblyInformationalVersion("2.1.1-rtm-30846")]
[assembly: System.Reflection.AssemblyProduct("Microsoft .NET Extensions")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Configuration.Binder")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("2.1.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Configuration
{
    public partial class BinderOptions
    {
        public bool BindNonPublicProperties { get { throw null; } set { } }
    }

    public static partial class ConfigurationBinder
    {
        public static void Bind(this IConfiguration configuration, object instance, System.Action<BinderOptions> configureOptions) { }

        public static void Bind(this IConfiguration configuration, object instance) { }

        public static void Bind(this IConfiguration configuration, string key, object instance) { }

        public static object Get(this IConfiguration configuration, System.Type type, System.Action<BinderOptions> configureOptions) { throw null; }

        public static object Get(this IConfiguration configuration, System.Type type) { throw null; }

        public static T Get<T>(this IConfiguration configuration, System.Action<BinderOptions> configureOptions) { throw null; }

        public static T Get<T>(this IConfiguration configuration) { throw null; }

        public static object GetValue(this IConfiguration configuration, System.Type type, string key, object defaultValue) { throw null; }

        public static object GetValue(this IConfiguration configuration, System.Type type, string key) { throw null; }

        public static T GetValue<T>(this IConfiguration configuration, string key, T defaultValue) { throw null; }

        public static T GetValue<T>(this IConfiguration configuration, string key) { throw null; }
    }
}