// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.Logging.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETFramework,Version=v4.6.1", FrameworkDisplayName = ".NET Framework 4.6.1")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.Logging.Abstractions")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Logging abstractions for Microsoft.Extensions.Logging.\r\n\r\nCommonly Used Types:\r\nMicrosoft.Extensions.Logging.ILogger\r\nMicrosoft.Extensions.Logging.ILoggerFactory\r\nMicrosoft.Extensions.Logging.ILogger<TCategoryName>\r\nMicrosoft.Extensions.Logging.LogLevel\r\nMicrosoft.Extensions.Logging.Logger<T>\r\nMicrosoft.Extensions.Logging.LoggerMessage\r\nMicrosoft.Extensions.Logging.Abstractions.NullLogger")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.Logging.Abstractions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.Logging
{
    public readonly partial struct EventId
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public EventId(int id, string? name = null) { }

        public int Id { get { throw null; } }

        public string? Name { get { throw null; } }

        public readonly bool Equals(EventId other) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(EventId left, EventId right) { throw null; }

        public static implicit operator EventId(int i) { throw null; }

        public static bool operator !=(EventId left, EventId right) { throw null; }

        public override readonly string ToString() { throw null; }
    }

    public partial interface IExternalScopeProvider
    {
        void ForEachScope<TState>(System.Action<object?, TState> callback, TState state);
        System.IDisposable Push(object? state);
    }

    public partial interface ILogger
    {
        System.IDisposable BeginScope<TState>(TState state);
        bool IsEnabled(LogLevel logLevel);
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception? exception, System.Func<TState, System.Exception?, string> formatter);
    }

    public partial interface ILoggerFactory : System.IDisposable
    {
        void AddProvider(ILoggerProvider provider);
        ILogger CreateLogger(string categoryName);
    }

    public partial interface ILoggerProvider : System.IDisposable
    {
        ILogger CreateLogger(string categoryName);
    }

    public partial interface ILogger<out TCategoryName> : ILogger
    {
    }

    public partial interface ISupportExternalScope
    {
        void SetScopeProvider(IExternalScopeProvider scopeProvider);
    }

    public partial class LogDefineOptions
    {
        public LogDefineOptions() { }

        public bool SkipEnabledCheck { get { throw null; } set { } }
    }

    public static partial class LoggerExtensions
    {
        public static System.IDisposable BeginScope(this ILogger logger, string messageFormat, params object?[] args) { throw null; }

        public static void Log(this ILogger logger, LogLevel logLevel, EventId eventId, System.Exception? exception, string? message, params object?[] args) { }

        public static void Log(this ILogger logger, LogLevel logLevel, EventId eventId, string? message, params object?[] args) { }

        public static void Log(this ILogger logger, LogLevel logLevel, System.Exception? exception, string? message, params object?[] args) { }

        public static void Log(this ILogger logger, LogLevel logLevel, string? message, params object?[] args) { }

        public static void LogCritical(this ILogger logger, EventId eventId, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogCritical(this ILogger logger, EventId eventId, string? message, params object?[] args) { }

        public static void LogCritical(this ILogger logger, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogCritical(this ILogger logger, string? message, params object?[] args) { }

        public static void LogDebug(this ILogger logger, EventId eventId, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogDebug(this ILogger logger, EventId eventId, string? message, params object?[] args) { }

        public static void LogDebug(this ILogger logger, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogDebug(this ILogger logger, string? message, params object?[] args) { }

        public static void LogError(this ILogger logger, EventId eventId, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogError(this ILogger logger, EventId eventId, string? message, params object?[] args) { }

        public static void LogError(this ILogger logger, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogError(this ILogger logger, string? message, params object?[] args) { }

        public static void LogInformation(this ILogger logger, EventId eventId, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogInformation(this ILogger logger, EventId eventId, string? message, params object?[] args) { }

        public static void LogInformation(this ILogger logger, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogInformation(this ILogger logger, string? message, params object?[] args) { }

        public static void LogTrace(this ILogger logger, EventId eventId, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogTrace(this ILogger logger, EventId eventId, string? message, params object?[] args) { }

        public static void LogTrace(this ILogger logger, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogTrace(this ILogger logger, string? message, params object?[] args) { }

        public static void LogWarning(this ILogger logger, EventId eventId, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogWarning(this ILogger logger, EventId eventId, string? message, params object?[] args) { }

        public static void LogWarning(this ILogger logger, System.Exception? exception, string? message, params object?[] args) { }

        public static void LogWarning(this ILogger logger, string? message, params object?[] args) { }
    }

    public partial class LoggerExternalScopeProvider : IExternalScopeProvider
    {
        public LoggerExternalScopeProvider() { }

        public void ForEachScope<TState>(System.Action<object?, TState> callback, TState state) { }

        public System.IDisposable Push(object? state) { throw null; }
    }

    public static partial class LoggerFactoryExtensions
    {
        public static ILogger CreateLogger(this ILoggerFactory factory, System.Type type) { throw null; }

        public static ILogger<T> CreateLogger<T>(this ILoggerFactory factory) { throw null; }
    }

    public static partial class LoggerMessage
    {
        public static System.Action<ILogger, System.Exception?> Define(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options) { throw null; }

        public static System.Action<ILogger, System.Exception?> Define(LogLevel logLevel, EventId eventId, string formatString) { throw null; }

        public static System.Action<ILogger, T1, System.Exception?> Define<T1>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options) { throw null; }

        public static System.Action<ILogger, T1, System.Exception?> Define<T1>(LogLevel logLevel, EventId eventId, string formatString) { throw null; }

        public static System.Action<ILogger, T1, T2, System.Exception?> Define<T1, T2>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options) { throw null; }

        public static System.Action<ILogger, T1, T2, System.Exception?> Define<T1, T2>(LogLevel logLevel, EventId eventId, string formatString) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, System.Exception?> Define<T1, T2, T3>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, System.Exception?> Define<T1, T2, T3>(LogLevel logLevel, EventId eventId, string formatString) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, T4, System.Exception?> Define<T1, T2, T3, T4>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, T4, System.Exception?> Define<T1, T2, T3, T4>(LogLevel logLevel, EventId eventId, string formatString) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, T4, T5, System.Exception?> Define<T1, T2, T3, T4, T5>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, T4, T5, System.Exception?> Define<T1, T2, T3, T4, T5>(LogLevel logLevel, EventId eventId, string formatString) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, T4, T5, T6, System.Exception?> Define<T1, T2, T3, T4, T5, T6>(LogLevel logLevel, EventId eventId, string formatString, LogDefineOptions? options) { throw null; }

        public static System.Action<ILogger, T1, T2, T3, T4, T5, T6, System.Exception?> Define<T1, T2, T3, T4, T5, T6>(LogLevel logLevel, EventId eventId, string formatString) { throw null; }

        public static System.Func<ILogger, System.IDisposable> DefineScope(string formatString) { throw null; }

        public static System.Func<ILogger, T1, System.IDisposable> DefineScope<T1>(string formatString) { throw null; }

        public static System.Func<ILogger, T1, T2, System.IDisposable> DefineScope<T1, T2>(string formatString) { throw null; }

        public static System.Func<ILogger, T1, T2, T3, System.IDisposable> DefineScope<T1, T2, T3>(string formatString) { throw null; }

        public static System.Func<ILogger, T1, T2, T3, T4, System.IDisposable> DefineScope<T1, T2, T3, T4>(string formatString) { throw null; }

        public static System.Func<ILogger, T1, T2, T3, T4, T5, System.IDisposable> DefineScope<T1, T2, T3, T4, T5>(string formatString) { throw null; }

        public static System.Func<ILogger, T1, T2, T3, T4, T5, T6, System.IDisposable> DefineScope<T1, T2, T3, T4, T5, T6>(string formatString) { throw null; }
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public sealed partial class LoggerMessageAttribute : System.Attribute
    {
        public LoggerMessageAttribute() { }

        public LoggerMessageAttribute(int eventId, LogLevel level, string message) { }

        public int EventId { get { throw null; } set { } }

        public string? EventName { get { throw null; } set { } }

        public LogLevel Level { get { throw null; } set { } }

        public string Message { get { throw null; } set { } }

        public bool SkipEnabledCheck { get { throw null; } set { } }
    }

    public partial class Logger<T> : ILogger
    {
        public Logger(ILoggerFactory factory) { }

        System.IDisposable ILogger.BeginScope<TState>(TState state) { throw null; }

        bool ILogger.IsEnabled(LogLevel logLevel) { throw null; }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception exception, System.Func<TState, System.Exception, string> formatter) { }
    }

    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Critical = 5,
        None = 6
    }
}

namespace Microsoft.Extensions.Logging.Abstractions
{
    public readonly partial struct LogEntry<TState>
    {
        private readonly TState _State_k__BackingField;
        private readonly System.Func<TState, System.Exception, string> _Formatter_k__BackingField;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public LogEntry(LogLevel logLevel, string category, EventId eventId, TState state, System.Exception? exception, System.Func<TState, System.Exception?, string> formatter) { }

        public string Category { get { throw null; } }

        public EventId EventId { get { throw null; } }

        public System.Exception? Exception { get { throw null; } }

        public System.Func<TState, System.Exception?, string>? Formatter { get { throw null; } }

        public LogLevel LogLevel { get { throw null; } }

        public TState State { get { throw null; } }
    }

    public partial class NullLogger : ILogger
    {
        public static NullLogger Instance { get { throw null; } }

        public System.IDisposable BeginScope<TState>(TState state) { throw null; }

        public bool IsEnabled(LogLevel logLevel) { throw null; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception? exception, System.Func<TState, System.Exception?, string> formatter) { }
    }

    public partial class NullLoggerFactory : ILoggerFactory, System.IDisposable
    {
        public static readonly NullLoggerFactory Instance;
        public NullLoggerFactory() { }

        public void AddProvider(ILoggerProvider provider) { }

        public ILogger CreateLogger(string name) { throw null; }

        public void Dispose() { }
    }

    public partial class NullLoggerProvider : ILoggerProvider, System.IDisposable
    {
        public static NullLoggerProvider Instance { get { throw null; } }

        public ILogger CreateLogger(string categoryName) { throw null; }

        public void Dispose() { }
    }

    public partial class NullLogger<T> : ILogger
    {
        public static readonly NullLogger<T> Instance;
        public NullLogger() { }

        public System.IDisposable BeginScope<TState>(TState state) { throw null; }

        public bool IsEnabled(LogLevel logLevel) { throw null; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, System.Exception? exception, System.Func<TState, System.Exception?, string> formatter) { }
    }
}