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
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Logging.Configuration")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Configuration support for Microsoft.Extensions.Logging.")]
[assembly: System.Reflection.AssemblyFileVersion("10.0.25.52411")]
[assembly: System.Reflection.AssemblyInformationalVersion("10.0.0+b0f34d51fccc69fd334253924abd8d6853fad7aa")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Logging.Configuration")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/dotnet")]
[assembly: System.Reflection.AssemblyVersionAttribute("10.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Logging
{
    public static partial class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddConfiguration(this ILoggingBuilder builder, Extensions.Configuration.IConfiguration configuration) { throw null; }
    }
}

namespace Microsoft.Extensions.Logging.Configuration
{
    public partial interface ILoggerProviderConfigurationFactory
    {
        Extensions.Configuration.IConfiguration GetConfiguration(System.Type providerType);
    }

    public partial interface ILoggerProviderConfiguration<T>
    {
        Extensions.Configuration.IConfiguration Configuration { get; }
    }

    public static partial class LoggerProviderOptions
    {
        public static void RegisterProviderOptions<TOptions, TProvider>(DependencyInjection.IServiceCollection services) where TOptions : class { }
    }
    public partial class LoggerProviderOptionsChangeTokenSource<TOptions, TProvider> : Options.ConfigurationChangeTokenSource<TOptions>
    {
        public LoggerProviderOptionsChangeTokenSource(ILoggerProviderConfiguration<TProvider> providerConfiguration) : base(default!) { }
    }

    public static partial class LoggingBuilderConfigurationExtensions
    {
        public static void AddConfiguration(this ILoggingBuilder builder) { }
    }
}