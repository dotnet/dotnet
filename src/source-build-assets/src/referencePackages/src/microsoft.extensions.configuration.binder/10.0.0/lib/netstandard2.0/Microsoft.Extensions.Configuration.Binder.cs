// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Configuration.Binder")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides the functionality to bind an object to data in configuration providers for Microsoft.Extensions.Configuration. This package enables you to represent the configuration data as strongly-typed classes defined in the application code. To bind a configuration, use the Microsoft.Extensions.Configuration.ConfigurationBinder.Get extension method on the IConfiguration object. To use this package, you also need to install a package for the configuration provider, for example, Microsoft.Extensions.Configuration.Json for the JSON provider.")]
[assembly: System.Reflection.AssemblyFileVersion("10.0.25.52411")]
[assembly: System.Reflection.AssemblyInformationalVersion("10.0.0+b0f34d51fccc69fd334253924abd8d6853fad7aa")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Configuration.Binder")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/dotnet")]
[assembly: System.Reflection.AssemblyVersionAttribute("10.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Configuration
{
    public partial class BinderOptions
    {
        public bool BindNonPublicProperties { get { throw null; } set { } }
        public bool ErrorOnUnknownConfiguration { get { throw null; } set { } }
    }
    public static partial class ConfigurationBinder
    {
        public static void Bind(this IConfiguration configuration, object? instance, System.Action<BinderOptions>? configureOptions) { }
        public static void Bind(this IConfiguration configuration, object? instance) { }
        public static void Bind(this IConfiguration configuration, string key, object? instance) { }
        public static object? Get(this IConfiguration configuration, System.Type type, System.Action<BinderOptions>? configureOptions) { throw null; }
        public static object? Get(this IConfiguration configuration, System.Type type) { throw null; }
        public static T? Get<T>(this IConfiguration configuration, System.Action<BinderOptions>? configureOptions) { throw null; }
        public static T? Get<T>(this IConfiguration configuration) { throw null; }
        public static object? GetValue(this IConfiguration configuration, System.Type type, string key, object? defaultValue) { throw null; }
        public static object? GetValue(this IConfiguration configuration, System.Type type, string key) { throw null; }
        public static T? GetValue<T>(this IConfiguration configuration, string key, T defaultValue) { throw null; }
        public static T? GetValue<T>(this IConfiguration configuration, string key) { throw null; }
    }
}