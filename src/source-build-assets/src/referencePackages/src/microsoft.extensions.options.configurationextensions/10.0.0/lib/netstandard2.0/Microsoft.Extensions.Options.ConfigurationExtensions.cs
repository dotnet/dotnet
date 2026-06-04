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
        public static Options.OptionsBuilder<TOptions> Bind<TOptions>(this Options.OptionsBuilder<TOptions> optionsBuilder, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) where TOptions : class { throw null; }
        public static Options.OptionsBuilder<TOptions> Bind<TOptions>(this Options.OptionsBuilder<TOptions> optionsBuilder, Configuration.IConfiguration config) where TOptions : class { throw null; }
        public static Options.OptionsBuilder<TOptions> BindConfiguration<TOptions>(this Options.OptionsBuilder<TOptions> optionsBuilder, string configSectionPath, System.Action<Configuration.BinderOptions>? configureBinder = null) where TOptions : class { throw null; }
    }

    public static partial class OptionsConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) where TOptions : class { throw null; }
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, Configuration.IConfiguration config) where TOptions : class { throw null; }
        public static IServiceCollection Configure<TOptions>(this IServiceCollection services, string? name, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) where TOptions : class { throw null; }
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
        public ConfigureFromConfigurationOptions(Configuration.IConfiguration config) : base(default) { }
    }

    public partial class NamedConfigureFromConfigurationOptions<TOptions> : ConfigureNamedOptions<TOptions> where TOptions : class
    {
        public NamedConfigureFromConfigurationOptions(string? name, Configuration.IConfiguration config, System.Action<Configuration.BinderOptions>? configureBinder) : base(default, default) { }
        public NamedConfigureFromConfigurationOptions(string? name, Configuration.IConfiguration config) : base(default, default) { }
    }
}

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class AllowNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class DisallowNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed partial class DoesNotReturnAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class DoesNotReturnIfAttribute : Attribute
    {
        public DoesNotReturnIfAttribute(bool parameterValue) { }
        public bool ParameterValue { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed partial class MaybeNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class MaybeNullWhenAttribute : Attribute
    {
        public MaybeNullWhenAttribute(bool returnValue) { }
        public bool ReturnValue { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed partial class MemberNotNullAttribute : Attribute
    {
        public MemberNotNullAttribute(string member) { }
        public MemberNotNullAttribute(params string[] members) { }
        public string[] Members { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed partial class MemberNotNullWhenAttribute : Attribute
    {
        public MemberNotNullWhenAttribute(bool returnValue, string member) { }
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members) { }
        public string[] Members { get { throw null; } }
        public bool ReturnValue { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed partial class NotNullAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed partial class NotNullIfNotNullAttribute : Attribute
    {
        public NotNullIfNotNullAttribute(string parameterName) { }
        public string ParameterName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed partial class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) { }
        public bool ReturnValue { get { throw null; } }
    }
}