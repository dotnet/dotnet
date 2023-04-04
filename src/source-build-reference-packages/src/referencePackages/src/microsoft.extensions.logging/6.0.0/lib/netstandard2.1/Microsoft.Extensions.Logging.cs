// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Logging")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Logging infrastructure default implementation for Microsoft.Extensions.Logging.")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Logging")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
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
    [System.Flags]
    public enum ActivityTrackingOptions
    {
        None = 0,
        SpanId = 1,
        TraceId = 2,
        ParentId = 4,
        TraceState = 8,
        TraceFlags = 16,
        Tags = 32,
        Baggage = 64
    }

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

        public LoggerFactory(System.Collections.Generic.IEnumerable<ILoggerProvider> providers, Options.IOptionsMonitor<LoggerFilterOptions> filterOption, Options.IOptions<LoggerFactoryOptions> options = null) { }

        public LoggerFactory(System.Collections.Generic.IEnumerable<ILoggerProvider> providers, Options.IOptionsMonitor<LoggerFilterOptions> filterOption) { }

        public LoggerFactory(System.Collections.Generic.IEnumerable<ILoggerProvider> providers) { }

        public void AddProvider(ILoggerProvider provider) { }

        protected virtual bool CheckDisposed() { throw null; }

        public static ILoggerFactory Create(System.Action<ILoggingBuilder> configure) { throw null; }

        public ILogger CreateLogger(string categoryName) { throw null; }

        public void Dispose() { }
    }

    public partial class LoggerFactoryOptions
    {
        public LoggerFactoryOptions() { }

        public ActivityTrackingOptions ActivityTrackingOptions { get { throw null; } set { } }
    }

    public partial class LoggerFilterOptions
    {
        public LoggerFilterOptions() { }

        public bool CaptureScopes { get { throw null; } set { } }

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

        public static ILoggingBuilder Configure(this ILoggingBuilder builder, System.Action<LoggerFactoryOptions> action) { throw null; }

        public static ILoggingBuilder SetMinimumLevel(this ILoggingBuilder builder, LogLevel level) { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public partial class ProviderAliasAttribute : System.Attribute
    {
        public ProviderAliasAttribute(string alias) { }

        public string Alias { get { throw null; } }
    }
}