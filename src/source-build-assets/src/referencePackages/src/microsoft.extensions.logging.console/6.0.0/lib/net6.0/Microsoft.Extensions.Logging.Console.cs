// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.Logging.Console.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Logging.Console")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Console logger provider implementation for Microsoft.Extensions.Logging.")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Logging.Console")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Logging
{
    [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
    public static partial class ConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddConsole(this ILoggingBuilder builder, System.Action<Console.ConsoleLoggerOptions> configure) { throw null; }

        [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "AddConsoleFormatter and RegisterProviderOptions are only dangerous when the Options type cannot be statically analyzed, but that is not the case here. The DynamicallyAccessedMembers annotations on them will make sure to preserve the right members from the different options objects.")]
        public static ILoggingBuilder AddConsole(this ILoggingBuilder builder) { throw null; }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static ILoggingBuilder AddConsoleFormatter<TFormatter, TOptions>(this ILoggingBuilder builder, System.Action<TOptions> configure)
            where TFormatter : Console.ConsoleFormatter where TOptions : Console.ConsoleFormatterOptions { throw null; }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
        public static ILoggingBuilder AddConsoleFormatter<TFormatter, TOptions>(this ILoggingBuilder builder)
            where TFormatter : Console.ConsoleFormatter where TOptions : Console.ConsoleFormatterOptions { throw null; }

        public static ILoggingBuilder AddJsonConsole(this ILoggingBuilder builder, System.Action<Console.JsonConsoleFormatterOptions> configure) { throw null; }

        public static ILoggingBuilder AddJsonConsole(this ILoggingBuilder builder) { throw null; }

        public static ILoggingBuilder AddSimpleConsole(this ILoggingBuilder builder, System.Action<Console.SimpleConsoleFormatterOptions> configure) { throw null; }

        public static ILoggingBuilder AddSimpleConsole(this ILoggingBuilder builder) { throw null; }

        public static ILoggingBuilder AddSystemdConsole(this ILoggingBuilder builder, System.Action<Console.ConsoleFormatterOptions> configure) { throw null; }

        public static ILoggingBuilder AddSystemdConsole(this ILoggingBuilder builder) { throw null; }
    }
}

namespace Microsoft.Extensions.Logging.Console
{
    public abstract partial class ConsoleFormatter
    {
        protected ConsoleFormatter(string name) { }

        public string Name { get { throw null; } }

        public abstract void Write<TState>(in Abstractions.LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, System.IO.TextWriter textWriter);
    }

    public static partial class ConsoleFormatterNames
    {
        public const string Json = "json";
        public const string Simple = "simple";
        public const string Systemd = "systemd";
    }

    public partial class ConsoleFormatterOptions
    {
        public bool IncludeScopes { get { throw null; } set { } }

        public string TimestampFormat { get { throw null; } set { } }

        public bool UseUtcTimestamp { get { throw null; } set { } }
    }

    [System.Obsolete("ConsoleLoggerFormat has been deprecated.")]
    public enum ConsoleLoggerFormat
    {
        Default = 0,
        Systemd = 1
    }

    public partial class ConsoleLoggerOptions
    {
        [System.Obsolete("ConsoleLoggerOptions.DisableColors has been deprecated. Use SimpleConsoleFormatterOptions.ColorBehavior instead.")]
        public bool DisableColors { get { throw null; } set { } }

        [System.Obsolete("ConsoleLoggerOptions.Format has been deprecated. Use ConsoleLoggerOptions.FormatterName instead.")]
        public ConsoleLoggerFormat Format { get { throw null; } set { } }

        public string FormatterName { get { throw null; } set { } }

        [System.Obsolete("ConsoleLoggerOptions.IncludeScopes has been deprecated. Use ConsoleFormatterOptions.IncludeScopes instead.")]
        public bool IncludeScopes { get { throw null; } set { } }

        public LogLevel LogToStandardErrorThreshold { get { throw null; } set { } }

        [System.Obsolete("ConsoleLoggerOptions.TimestampFormat has been deprecated. Use ConsoleFormatterOptions.TimestampFormat instead.")]
        public string TimestampFormat { get { throw null; } set { } }

        [System.Obsolete("ConsoleLoggerOptions.UseUtcTimestamp has been deprecated. Use ConsoleFormatterOptions.UseUtcTimestamp instead.")]
        public bool UseUtcTimestamp { get { throw null; } set { } }
    }

    [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
    [ProviderAlias("Console")]
    public partial class ConsoleLoggerProvider : ILoggerProvider, System.IDisposable, ISupportExternalScope
    {
        public ConsoleLoggerProvider(Options.IOptionsMonitor<ConsoleLoggerOptions> options, System.Collections.Generic.IEnumerable<ConsoleFormatter> formatters) { }

        public ConsoleLoggerProvider(Options.IOptionsMonitor<ConsoleLoggerOptions> options) { }

        public ILogger CreateLogger(string name) { throw null; }

        public void Dispose() { }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider) { }
    }

    public partial class JsonConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public System.Text.Json.JsonWriterOptions JsonWriterOptions { get { throw null; } set { } }
    }

    public enum LoggerColorBehavior
    {
        Default = 0,
        Enabled = 1,
        Disabled = 2
    }

    public partial class SimpleConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public LoggerColorBehavior ColorBehavior { get { throw null; } set { } }

        public bool SingleLine { get { throw null; } set { } }
    }
}