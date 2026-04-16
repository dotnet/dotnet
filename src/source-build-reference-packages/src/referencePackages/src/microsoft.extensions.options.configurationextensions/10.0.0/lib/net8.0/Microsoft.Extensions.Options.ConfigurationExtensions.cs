// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Options.ConfigurationExtensions")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides additional configuration specific functionality related to Options.")]
[assembly: System.Reflection.AssemblyFileVersion("10.0.25.52411")]
[assembly: System.Reflection.AssemblyInformationalVersion("10.0.0+b0f34d51fccc69fd334253924abd8d6853fad7aa")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Options.ConfigurationExtensions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/dotnet")]
[assembly: System.Reflection.AssemblyVersionAttribute("10.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class OptionsBuilderConfigurationExtensions
    {
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static Options.OptionsBuilder<TOptions> Bind<TOptions>(this Options.OptionsBuilder<TOptions> optionsBuilder, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) where TOptions : class { throw null; }
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static Options.OptionsBuilder<TOptions> Bind<TOptions>(this Options.OptionsBuilder<TOptions> optionsBuilder, Configuration.IConfiguration config) where TOptions : class { throw null; }
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static Options.OptionsBuilder<TOptions> BindConfiguration<TOptions>(this Options.OptionsBuilder<TOptions> optionsBuilder, string configSectionPath, System.Action<Configuration.BinderOptions>? configureBinder = null) where TOptions : class { throw null; }
    }
    public static partial class OptionsConfigurationServiceCollectionExtensions
    {
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) where TOptions : class { throw null; }
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Configuration.IConfiguration config) where TOptions : class { throw null; }
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string? name, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) where TOptions : class { throw null; }
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string? name, Configuration.IConfiguration config) where TOptions : class { throw null; }
    }
}

namespace Microsoft.Extensions.Options
{
    public partial class ConfigurationChangeTokenSource<TOptions> : IOptionsChangeTokenSource<TOptions>
    {
        public ConfigurationChangeTokenSource(Configuration.IConfiguration config) { }
        public ConfigurationChangeTokenSource(string? name, Configuration.IConfiguration config) { }
        public string Name { get { throw null; } }

        public Primitives.IChangeToken GetChangeToken() { throw null; }
    }

    public partial class ConfigureFromConfigurationOptions<TOptions> : ConfigureOptions<TOptions> where TOptions : class
    {
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public ConfigureFromConfigurationOptions(Configuration.IConfiguration config) : base(default) { }
    }

    public partial class NamedConfigureFromConfigurationOptions<TOptions> : ConfigureNamedOptions<TOptions> where TOptions : class
    {
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public NamedConfigureFromConfigurationOptions(string? name, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) : base(default, default) { }
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Binding strongly typed objects to configuration values may require generating dynamic code at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public NamedConfigureFromConfigurationOptions(string? name, Configuration.IConfiguration config) : base(default, default) { }
    }
}