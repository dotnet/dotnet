// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.Logging.Configuration, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("CommitHash", "a0398f0baf9ecc8e5fb6afc3a9b89f0d49cf041e")]
[assembly: System.Reflection.AssemblyMetadata("BuildNumber", "30846")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation.")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Logging infrastructure default implementation for Microsoft.Extensions.Logging.")]
[assembly: System.Reflection.AssemblyFileVersion("2.1.1.18157")]
[assembly: System.Reflection.AssemblyInformationalVersion("2.1.1-rtm-30846")]
[assembly: System.Reflection.AssemblyProduct("Microsoft .NET Extensions")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Logging")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("2.1.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddLogging(this IServiceCollection services, System.Action<Logging.ILoggingBuilder> configure) { throw null; }

        public static IServiceCollection AddLogging(this IServiceCollection services) { throw null; }
    }
}

namespace Microsoft.Extensions.Logging
{
    public static partial class FilterLoggingBuilderExtensions
    {
        public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, System.Func<LogLevel, bool> levelFilter) { throw null; }

        public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, System.Func<string, LogLevel, bool> categoryLevelFilter) { throw null; }

        public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, System.Func<string, string, LogLevel, bool> filter) { throw null; }

        public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, string category, LogLevel level) { throw null; }

        public static ILoggingBuilder AddFilter(this ILoggingBuilder builder, string category, System.Func<LogLevel, bool> levelFilter) { throw null; }

        public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, System.Func<LogLevel, bool> levelFilter) { throw null; }

        public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, System.Func<string, LogLevel, bool> categoryLevelFilter) { throw null; }

        public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, System.Func<string, string, LogLevel, bool> filter) { throw null; }

        public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, string category, LogLevel level) { throw null; }

        public static LoggerFilterOptions AddFilter(this LoggerFilterOptions builder, string category, System.Func<LogLevel, bool> levelFilter) { throw null; }

        public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, System.Func<LogLevel, bool> levelFilter)
            where T : ILoggerProvider { throw null; }

        public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, System.Func<string, LogLevel, bool> categoryLevelFilter)
            where T : ILoggerProvider { throw null; }

        public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, string category, LogLevel level)
            where T : ILoggerProvider { throw null; }

        public static ILoggingBuilder AddFilter<T>(this ILoggingBuilder builder, string category, System.Func<LogLevel, bool> levelFilter)
            where T : ILoggerProvider { throw null; }

        public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, System.Func<LogLevel, bool> levelFilter)
            where T : ILoggerProvider { throw null; }

        public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, System.Func<string, LogLevel, bool> categoryLevelFilter)
            where T : ILoggerProvider { throw null; }

        public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, string category, LogLevel level)
            where T : ILoggerProvider { throw null; }

        public static LoggerFilterOptions AddFilter<T>(this LoggerFilterOptions builder, string category, System.Func<LogLevel, bool> levelFilter)
            where T : ILoggerProvider { throw null; }
    }

    public partial interface ILoggingBuilder
    {
        DependencyInjection.IServiceCollection Services { get; }
    }

    public partial class LoggerFactory : ILoggerFactory, System.IDisposable
    {
        public LoggerFactory() { }

        public LoggerFactory(System.Collections.Generic.IEnumerable<ILoggerProvider> providers, LoggerFilterOptions filterOptions) { }

        public LoggerFactory(System.Collections.Generic.IEnumerable<ILoggerProvider> providers, Options.IOptionsMonitor<LoggerFilterOptions> filterOption) { }

        public LoggerFactory(System.Collections.Generic.IEnumerable<ILoggerProvider> providers) { }

        public void AddProvider(ILoggerProvider provider) { }

        protected virtual bool CheckDisposed() { throw null; }

        public ILogger CreateLogger(string categoryName) { throw null; }

        public void Dispose() { }
    }

    public partial class LoggerFilterOptions
    {
        public LogLevel MinLevel { get { throw null; } set { } }

        public System.Collections.Generic.IList<LoggerFilterRule> Rules { get { throw null; } }
    }

    public partial class LoggerFilterRule
    {
        public LoggerFilterRule(string providerName, string categoryName, LogLevel? logLevel, System.Func<string, string, LogLevel, bool> filter) { }

        public string CategoryName { get { throw null; } }

        public System.Func<string, string, LogLevel, bool> Filter { get { throw null; } }

        public LogLevel? LogLevel { get { throw null; } }

        public string ProviderName { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public static partial class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddProvider(this ILoggingBuilder builder, ILoggerProvider provider) { throw null; }

        public static ILoggingBuilder ClearProviders(this ILoggingBuilder builder) { throw null; }

        public static ILoggingBuilder SetMinimumLevel(this ILoggingBuilder builder, LogLevel level) { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public partial class ProviderAliasAttribute : System.Attribute
    {
        public ProviderAliasAttribute(string alias) { }

        public string Alias { get { throw null; } }
    }
}