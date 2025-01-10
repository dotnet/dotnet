// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Common utilities and interfaces for all NuGet libraries.")]
[assembly: System.Reflection.AssemblyFileVersion("6.12.1.1")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.12.1+aa7eb9987d28e7169cfabfa484f2fdd22d2b91d2.aa7eb9987d28e7169cfabfa484f2fdd22d2b91d2")]
[assembly: System.Reflection.AssemblyProduct("NuGet")]
[assembly: System.Reflection.AssemblyTitle("NuGet.Common")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/NuGet/NuGet.Client")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NuGet.Common.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100A5276DF8650A58CB43396DC7B3D395F30A82D0D1FA98FBCFE3ABEAD5DE0B1DB6764347A0F6BF0B060A27C202CCD122DB5DED8F596CEBE2ECC3A6629015EEB96C94F6B9E8185D4ACC84C376FF6B1C3147431A4D55CB5736DB97A9E88FCC47D9193F4DB5896DC5817E5D0CBD2641726E7431990BCD2DD7FA1D28493D0CFD9DCFA4")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.12.1.1")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace NuGet.Common
{
    public static partial class ActivityCorrelationId
    {
        public static string Current { get { throw null; } }

        public static void Clear() { }

        public static void StartNew() { }
    }

    public partial class AggregateEnumerableAsync<T> : IEnumerableAsync<T>
    {
        public AggregateEnumerableAsync(System.Collections.Generic.IList<IEnumerableAsync<T>> asyncEnumerables, System.Collections.Generic.IComparer<T>? comparer, System.Collections.Generic.IEqualityComparer<T>? equalityComparer) { }

        public IEnumeratorAsync<T> GetEnumeratorAsync() { throw null; }
    }

    public partial class AggregateEnumeratorAsync<T> : IEnumeratorAsync<T>
    {
        public AggregateEnumeratorAsync(System.Collections.Generic.IList<IEnumerableAsync<T>> asyncEnumerables, System.Collections.Generic.IComparer<T>? orderingComparer, System.Collections.Generic.IEqualityComparer<T>? equalityComparer) { }

        public T Current { get { throw null; } }

        public System.Threading.Tasks.Task<bool> MoveNextAsync() { throw null; }
    }

    public static partial class AsyncLazy
    {
        public static AsyncLazy<T> New<T>(T innerData) { throw null; }

        public static AsyncLazy<T> New<T>(System.Func<T> valueFactory) { throw null; }

        public static AsyncLazy<T> New<T>(System.Func<System.Threading.Tasks.Task<T>> asyncValueFactory) { throw null; }

        public static AsyncLazy<T> New<T>(System.Lazy<System.Threading.Tasks.Task<T>> inner) { throw null; }
    }

    [System.CLSCompliant(true)]
    public partial class AsyncLazy<T>
    {
        public AsyncLazy(System.Func<System.Threading.Tasks.Task<T>> valueFactory) { }

        public AsyncLazy(System.Lazy<System.Threading.Tasks.Task<T>> inner) { }

        public System.Runtime.CompilerServices.TaskAwaiter<T> GetAwaiter() { throw null; }

        public static implicit operator System.Lazy<System.Threading.Tasks.Task<T>>(AsyncLazy<T> outer) { throw null; }
    }

    public partial class AuthTypeFilteredCredentials : System.Net.ICredentials
    {
        public AuthTypeFilteredCredentials(System.Net.NetworkCredential innerCredential, System.Collections.Generic.IEnumerable<string> authTypes) { }

        public System.Collections.Generic.IReadOnlyList<string> AuthTypes { get { throw null; } }

        public System.Net.NetworkCredential InnerCredential { get { throw null; } }

        public System.Net.NetworkCredential? GetCredential(System.Uri uri, string authType) { throw null; }
    }

    public static partial class ClientVersionUtility
    {
        public static string GetNuGetAssemblyVersion() { throw null; }
    }

    public partial class CommandLineArgumentCombinationException : System.Exception, ILogMessageException
    {
        public CommandLineArgumentCombinationException(string message) { }

        public virtual ILogMessage AsLogMessage() { throw null; }
    }

    public static partial class ComparisonUtility
    {
        public static readonly System.StringComparer FrameworkReferenceNameComparer;
    }

    public static partial class ConcurrencyUtilities
    {
        public static void ExecuteWithFileLocked(string filePath, System.Action action) { }

        public static System.Threading.Tasks.Task<T> ExecuteWithFileLockedAsync<T>(string filePath, System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<T>> action, System.Threading.CancellationToken token) { throw null; }
    }

    public partial class CryptoHashProvider
    {
        public CryptoHashProvider() { }

        public CryptoHashProvider(string? hashAlgorithm) { }

        public byte[] CalculateHash(byte[] data) { throw null; }

        public byte[] CalculateHash(System.IO.Stream stream) { throw null; }

        public bool VerifyHash(byte[] data, byte[] hash) { throw null; }
    }

    public static partial class CryptoHashUtility
    {
        public static byte[] ComputeHash(this HashAlgorithmName hashAlgorithmName, byte[] data) { throw null; }

        public static byte[] ComputeHash(this System.Security.Cryptography.HashAlgorithm hashAlgorithm, System.IO.Stream data, bool leaveStreamOpen) { throw null; }

        public static byte[] ComputeHash(this System.Security.Cryptography.HashAlgorithm hashAlgorithm, System.IO.Stream data) { throw null; }

        public static string ComputeHashAsBase64(this System.Security.Cryptography.HashAlgorithm hashAlgorithm, System.IO.Stream data, bool leaveStreamOpen) { throw null; }

        public static string ComputeHashAsBase64(this System.Security.Cryptography.HashAlgorithm hashAlgorithm, System.IO.Stream data) { throw null; }

        public static System.Security.Cryptography.Oid ConvertToOid(this HashAlgorithmName hashAlgorithm) { throw null; }

        public static string ConvertToOidString(this HashAlgorithmName hashAlgorithmName) { throw null; }

        public static string ConvertToOidString(this SignatureAlgorithmName signatureAlgorithmName) { throw null; }

        public static System.Security.Cryptography.HashAlgorithmName ConvertToSystemSecurityHashAlgorithmName(this HashAlgorithmName hashAlgorithmName) { throw null; }

        public static System.Security.Cryptography.HashAlgorithm GetHashAlgorithm(HashAlgorithmName hashAlgorithmName) { throw null; }

        public static System.Security.Cryptography.HashAlgorithm GetHashAlgorithm(string hashAlgorithmName) { throw null; }

        public static HashAlgorithmName GetHashAlgorithmName(string hashAlgorithm) { throw null; }

        public static System.Security.Cryptography.HashAlgorithm GetHashProvider(this HashAlgorithmName hashAlgorithmName) { throw null; }

        public static HashAlgorithmName OidToHashAlgorithmName(string oid) { throw null; }
    }

    public static partial class CultureUtility
    {
        public static void DisableLocalization() { }
    }

    public static partial class DatetimeUtility
    {
        public static string ToReadableTimeFormat(System.TimeSpan time) { throw null; }
    }

    public partial class EnvironmentVariableWrapper : IEnvironmentVariableReader
    {
        public static IEnvironmentVariableReader Instance { get { throw null; } }

        public string? GetEnvironmentVariable(string variable) { throw null; }
    }

    public partial class ExceptionLogger
    {
        public ExceptionLogger(IEnvironmentVariableReader reader) { }

        public static ExceptionLogger Instance { get { throw null; } }

        public bool ShowStack { get { throw null; } }
    }

    public static partial class ExceptionUtilities
    {
        public static string DisplayMessage(System.AggregateException exception) { throw null; }

        public static string DisplayMessage(System.Exception exception, bool indent) { throw null; }

        public static string DisplayMessage(System.Exception exception) { throw null; }

        public static string DisplayMessage(System.Reflection.TargetInvocationException exception) { throw null; }

        public static void LogException(System.Exception ex, ILogger logger, bool logStackAsError) { }

        public static void LogException(System.Exception ex, ILogger logger) { }

        public static System.Exception Unwrap(System.Exception exception) { throw null; }
    }

    public static partial class FileUtility
    {
        public static readonly System.IO.FileShare FileSharePermissions;
        public static readonly int MaxTries;
        public static void Delete(string path) { }

        public static System.Threading.Tasks.Task DeleteWithLock(string filePath) { throw null; }

        public static string GetTempFilePath(string directory) { throw null; }

        public static void Move(string sourceFileName, string destFileName) { }

        public static void Replace(System.Action<string> writeSourceFile, string destFilePath) { }

        public static void Replace(string sourceFileName, string destFileName) { }

        public static System.Threading.Tasks.Task ReplaceAsync(System.Func<string, System.Threading.Tasks.Task> writeSourceFile, string destFilePath) { throw null; }

        public static System.Threading.Tasks.Task ReplaceWithLock(System.Action<string> writeSourceFile, string destFilePath) { throw null; }

        public static T SafeRead<T>(string filePath, System.Func<System.IO.FileStream, string, T> read) { throw null; }

        public static System.Threading.Tasks.Task<T> SafeReadAsync<T>(string filePath, System.Func<System.IO.FileStream, string, System.Threading.Tasks.Task<T>> read) { throw null; }
    }

    public enum HashAlgorithmName
    {
        Unknown = 0,
        SHA256 = 1,
        SHA384 = 2,
        SHA512 = 3,
        SHA1 = 4
    }

    public partial interface ICollectorLogger : ILogger
    {
        System.Collections.Generic.IEnumerable<IRestoreLogMessage> Errors { get; }
    }

    public partial interface IEnumerableAsync<T>
    {
        IEnumeratorAsync<T> GetEnumeratorAsync();
    }

    public partial interface IEnumeratorAsync<T>
    {
        T Current { get; }

        System.Threading.Tasks.Task<bool> MoveNextAsync();
    }

    public partial interface IEnvironmentVariableReader
    {
        string? GetEnvironmentVariable(string variable);
    }

    public partial interface ILogFileContext
    {
        int EndColumnNumber { get; set; }

        int EndLineNumber { get; set; }

        string? FilePath { get; set; }

        int StartColumnNumber { get; set; }

        int StartLineNumber { get; set; }
    }

    public partial interface ILogger
    {
        void Log(ILogMessage message);
        void Log(LogLevel level, string data);
        System.Threading.Tasks.Task LogAsync(ILogMessage message);
        System.Threading.Tasks.Task LogAsync(LogLevel level, string data);
        void LogDebug(string data);
        void LogError(string data);
        void LogInformation(string data);
        void LogInformationSummary(string data);
        void LogMinimal(string data);
        void LogVerbose(string data);
        void LogWarning(string data);
    }

    public partial interface ILogMessage
    {
        NuGetLogCode Code { get; set; }

        LogLevel Level { get; set; }

        string Message { get; set; }

        string? ProjectPath { get; set; }

        System.DateTimeOffset Time { get; set; }

        WarningLevel WarningLevel { get; set; }
    }

    public partial interface ILogMessageException
    {
        ILogMessage AsLogMessage();
    }

    public partial interface INuGetLogMessage : ILogMessage, ILogFileContext
    {
    }

    public partial interface INuGetPathContext
    {
        System.Collections.Generic.IReadOnlyList<string> FallbackPackageFolders { get; }

        string HttpCacheFolder { get; }

        string UserPackageFolder { get; }
    }

    public partial interface INuGetTelemetryService
    {
        void EmitTelemetryEvent(TelemetryEvent telemetryData);
        System.IDisposable StartActivity(string activityName);
    }

    public partial interface IPackLogMessage : INuGetLogMessage, ILogMessage, ILogFileContext
    {
        Frameworks.NuGetFramework? Framework { get; set; }

        string? LibraryId { get; set; }
    }

    public partial interface IRestoreLogMessage : INuGetLogMessage, ILogMessage, ILogFileContext
    {
        string? LibraryId { get; set; }

        bool ShouldDisplay { get; set; }

        System.Collections.Generic.IReadOnlyList<string> TargetGraphs { get; set; }
    }

    public partial interface ITelemetrySession
    {
        void PostEvent(TelemetryEvent telemetryEvent);
    }

    public abstract partial class LegacyLoggerAdapter : ILogger
    {
        public virtual void Log(ILogMessage message) { }

        public void Log(LogLevel level, string data) { }

        public virtual System.Threading.Tasks.Task LogAsync(ILogMessage message) { throw null; }

        public System.Threading.Tasks.Task LogAsync(LogLevel level, string data) { throw null; }

        public abstract void LogDebug(string data);
        public abstract void LogError(string data);
        public abstract void LogInformation(string data);
        public abstract void LogInformationSummary(string data);
        public abstract void LogMinimal(string data);
        public abstract void LogVerbose(string data);
        public abstract void LogWarning(string data);
    }

    public static partial class LocalResourceUtils
    {
        public static void DeleteDirectoryTree(string folderPath, System.Collections.Generic.List<string> failedDeletes) { }
    }

    public abstract partial class LoggerBase : ILogger
    {
        public LoggerBase() { }

        public LoggerBase(LogLevel verbosityLevel) { }

        public LogLevel VerbosityLevel { get { throw null; } set { } }

        protected virtual bool CollectMessage(LogLevel messageLevel) { throw null; }

        protected virtual bool DisplayMessage(LogLevel messageLevel) { throw null; }

        public abstract void Log(ILogMessage message);
        public virtual void Log(LogLevel level, string data) { }

        public abstract System.Threading.Tasks.Task LogAsync(ILogMessage message);
        public virtual System.Threading.Tasks.Task LogAsync(LogLevel level, string data) { throw null; }

        public virtual void LogDebug(string data) { }

        public virtual void LogError(string data) { }

        public virtual void LogInformation(string data) { }

        public virtual void LogInformationSummary(string data) { }

        public virtual void LogMinimal(string data) { }

        public virtual void LogVerbose(string data) { }

        public virtual void LogWarning(string data) { }
    }

    public static partial class LoggingExtensions
    {
        public static string FormatWithCode(this ILogMessage message) { throw null; }

        public static string? GetName(this NuGetLogCode code) { throw null; }

        public static bool TryGetName(this NuGetLogCode code, out string? codeString) { throw null; }
    }

    public enum LogLevel
    {
        Debug = 0,
        Verbose = 1,
        Information = 2,
        Minimal = 3,
        Warning = 4,
        Error = 5
    }

    public partial class LogMessage : ILogMessage
    {
        public LogMessage(LogLevel level, string message, NuGetLogCode code) { }

        public LogMessage(LogLevel level, string message) { }

        public NuGetLogCode Code { get { throw null; } set { } }

        public LogLevel Level { get { throw null; } set { } }

        public string Message { get { throw null; } set { } }

        public string? ProjectPath { get { throw null; } set { } }

        public System.DateTimeOffset Time { get { throw null; } set { } }

        public WarningLevel WarningLevel { get { throw null; } set { } }

        public static LogMessage Create(LogLevel level, string message) { throw null; }

        public static LogMessage CreateError(NuGetLogCode code, string message) { throw null; }

        public static LogMessage CreateWarning(NuGetLogCode code, string message) { throw null; }

        public override string ToString() { throw null; }
    }

    public static partial class LogMessageProperties
    {
        public const string CODE = "code";
        public const string END_COLUMN_NUMBER = "endColumnNumber";
        public const string END_LINE_NUMBER = "endLineNumber";
        public const string FILE_PATH = "filePath";
        public const string LEVEL = "level";
        public const string LIBRARY_ID = "libraryId";
        public const string MESSAGE = "message";
        public const string START_COLUMN_NUMBER = "startColumnNumber";
        public const string START_LINE_NUMBER = "startLineNumber";
        public const string TARGET_GRAPHS = "targetGraphs";
        public const string WARNING_LEVEL = "warningLevel";
    }

    public static partial class MSBuildStringUtility
    {
        public static string? Convert(string? value) { throw null; }

        public static bool? GetBooleanOrNull(string? value) { throw null; }

        public static System.Collections.Generic.IEnumerable<NuGetLogCode> GetDistinctNuGetLogCodesOrDefault(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<NuGetLogCode>?> nugetLogCodeLists) { throw null; }

        public static System.Collections.Generic.IList<NuGetLogCode> GetNuGetLogCodes(string s) { throw null; }

        public static bool IsTrue(string? value) { throw null; }

        public static bool IsTrueOrEmpty(string? value) { throw null; }

        public static string[] Split(string s, params char[] chars) { throw null; }

        public static string[] Split(string s) { throw null; }

        public static string[] TrimAndExcludeNullOrEmpty(string?[]? strings) { throw null; }

        public static string? TrimAndGetNullForEmpty(string? s) { throw null; }
    }

    public static partial class NetworkProtocolUtility
    {
        public static void SetConnectionLimit() { }
    }

    public static partial class NuGetEnvironment
    {
        public static string GetDotNetLocation() { throw null; }

        public static string GetFolderPath(NuGetFolderPath folder) { throw null; }
    }

    public static partial class NuGetEventSource
    {
        public static System.Diagnostics.Tracing.EventSource Instance { get { throw null; } }

        public static bool IsEnabled { get { throw null; } }

        public static partial class Keywords
        {
            public const System.Diagnostics.Tracing.EventKeywords Common = (System.Diagnostics.Tracing.EventKeywords)1L;
            public const System.Diagnostics.Tracing.EventKeywords Configuration = (System.Diagnostics.Tracing.EventKeywords)2L;
            public const System.Diagnostics.Tracing.EventKeywords Logging = (System.Diagnostics.Tracing.EventKeywords)4L;
            public const System.Diagnostics.Tracing.EventKeywords Performance = (System.Diagnostics.Tracing.EventKeywords)8L;
            public const System.Diagnostics.Tracing.EventKeywords Restore = (System.Diagnostics.Tracing.EventKeywords)32L;
            public const System.Diagnostics.Tracing.EventKeywords SdkResolver = (System.Diagnostics.Tracing.EventKeywords)16L;
        }
    }

    public enum NuGetFolderPath
    {
        MachineWideSettingsBaseDirectory = 0,
        MachineWideConfigDirectory = 1,
        UserSettingsDirectory = 2,
        HttpCacheDirectory = 3,
        NuGetHome = 4,
        DefaultMsBuildPath = 5,
        Temp = 6,
        NuGetPluginsCacheDirectory = 7
    }

    public enum NuGetLogCode
    {
        Undefined = 0,
        NU1000 = 1000,
        NU1001 = 1001,
        NU1002 = 1002,
        NU1003 = 1003,
        NU1004 = 1004,
        NU1005 = 1005,
        NU1006 = 1006,
        NU1007 = 1007,
        NU1008 = 1008,
        NU1009 = 1009,
        NU1010 = 1010,
        NU1011 = 1011,
        NU1012 = 1012,
        NU1013 = 1013,
        NU1014 = 1014,
        NU1100 = 1100,
        NU1101 = 1101,
        NU1102 = 1102,
        NU1103 = 1103,
        NU1104 = 1104,
        NU1105 = 1105,
        NU1106 = 1106,
        NU1107 = 1107,
        NU1108 = 1108,
        NU1109 = 1109,
        NU1110 = 1110,
        NU1201 = 1201,
        NU1202 = 1202,
        NU1203 = 1203,
        NU1204 = 1204,
        NU1211 = 1211,
        NU1212 = 1212,
        NU1213 = 1213,
        NU1301 = 1301,
        NU1302 = 1302,
        NU1401 = 1401,
        NU1402 = 1402,
        NU1403 = 1403,
        NU1410 = 1410,
        NU1500 = 1500,
        NU1501 = 1501,
        NU1502 = 1502,
        NU1503 = 1503,
        NU1504 = 1504,
        NU1505 = 1505,
        NU1506 = 1506,
        NU1507 = 1507,
        NU1508 = 1508,
        NU1601 = 1601,
        NU1602 = 1602,
        NU1603 = 1603,
        NU1604 = 1604,
        NU1605 = 1605,
        NU1608 = 1608,
        NU1701 = 1701,
        NU1702 = 1702,
        NU1703 = 1703,
        NU1801 = 1801,
        NU1802 = 1802,
        NU1803 = 1803,
        NU1900 = 1900,
        NU1901 = 1901,
        NU1902 = 1902,
        NU1903 = 1903,
        NU1904 = 1904,
        NU1905 = 1905,
        NU3000 = 3000,
        NU3001 = 3001,
        NU3002 = 3002,
        NU3003 = 3003,
        NU3004 = 3004,
        NU3005 = 3005,
        NU3006 = 3006,
        NU3007 = 3007,
        NU3008 = 3008,
        NU3009 = 3009,
        NU3010 = 3010,
        NU3011 = 3011,
        NU3012 = 3012,
        NU3013 = 3013,
        NU3014 = 3014,
        NU3015 = 3015,
        NU3016 = 3016,
        NU3017 = 3017,
        NU3018 = 3018,
        NU3019 = 3019,
        NU3020 = 3020,
        NU3021 = 3021,
        NU3022 = 3022,
        NU3023 = 3023,
        NU3024 = 3024,
        NU3025 = 3025,
        NU3026 = 3026,
        NU3027 = 3027,
        NU3028 = 3028,
        NU3029 = 3029,
        NU3030 = 3030,
        NU3031 = 3031,
        NU3032 = 3032,
        NU3033 = 3033,
        NU3034 = 3034,
        NU3035 = 3035,
        NU3036 = 3036,
        NU3037 = 3037,
        NU3038 = 3038,
        NU3039 = 3039,
        NU3040 = 3040,
        NU3041 = 3041,
        NU3042 = 3042,
        NU3043 = 3043,
        NU5000 = 5000,
        NU5001 = 5001,
        NU5002 = 5002,
        NU5003 = 5003,
        NU5004 = 5004,
        NU5005 = 5005,
        NU5007 = 5007,
        NU5008 = 5008,
        NU5009 = 5009,
        NU5010 = 5010,
        NU5011 = 5011,
        NU5012 = 5012,
        NU5013 = 5013,
        NU5014 = 5014,
        NU5015 = 5015,
        NU5016 = 5016,
        NU5017 = 5017,
        NU5018 = 5018,
        NU5019 = 5019,
        NU5020 = 5020,
        NU5021 = 5021,
        NU5022 = 5022,
        NU5023 = 5023,
        NU5024 = 5024,
        NU5025 = 5025,
        NU5026 = 5026,
        NU5027 = 5027,
        NU5028 = 5028,
        NU5029 = 5029,
        NU5030 = 5030,
        NU5031 = 5031,
        NU5032 = 5032,
        NU5033 = 5033,
        NU5034 = 5034,
        NU5035 = 5035,
        NU5036 = 5036,
        NU5037 = 5037,
        NU5038 = 5038,
        NU5039 = 5039,
        NU5040 = 5040,
        NU5041 = 5041,
        NU5042 = 5042,
        NU5045 = 5045,
        NU5046 = 5046,
        NU5047 = 5047,
        NU5048 = 5048,
        NU5049 = 5049,
        NU5050 = 5050,
        NU5100 = 5100,
        NU5101 = 5101,
        NU5102 = 5102,
        NU5103 = 5103,
        NU5104 = 5104,
        NU5105 = 5105,
        NU5106 = 5106,
        NU5107 = 5107,
        NU5108 = 5108,
        NU5109 = 5109,
        NU5110 = 5110,
        NU5111 = 5111,
        NU5112 = 5112,
        NU5114 = 5114,
        NU5115 = 5115,
        NU5116 = 5116,
        NU5117 = 5117,
        NU5118 = 5118,
        NU5119 = 5119,
        NU5120 = 5120,
        NU5121 = 5121,
        NU5122 = 5122,
        NU5123 = 5123,
        NU5124 = 5124,
        NU5125 = 5125,
        NU5126 = 5126,
        NU5127 = 5127,
        NU5128 = 5128,
        NU5129 = 5129,
        NU5130 = 5130,
        NU5131 = 5131,
        NU5132 = 5132,
        NU5133 = 5133,
        NU5500 = 5500,
        NU5501 = 5501
    }

    public enum NuGetOperationStatus
    {
        NoOp = 0,
        Succeeded = 1,
        Failed = 2,
        Cancelled = 3
    }

    public partial class NullLogger : LoggerBase
    {
        public static ILogger Instance { get { throw null; } }

        public override void Log(ILogMessage message) { }

        public override void Log(LogLevel level, string data) { }

        public override System.Threading.Tasks.Task LogAsync(ILogMessage message) { throw null; }

        public override System.Threading.Tasks.Task LogAsync(LogLevel level, string data) { throw null; }
    }

    public partial class PackagingLogMessage : IPackLogMessage, INuGetLogMessage, ILogMessage, ILogFileContext
    {
        internal PackagingLogMessage() { }

        public NuGetLogCode Code { get { throw null; } set { } }

        public int EndColumnNumber { get { throw null; } set { } }

        public int EndLineNumber { get { throw null; } set { } }

        public string? FilePath { get { throw null; } set { } }

        public Frameworks.NuGetFramework? Framework { get { throw null; } set { } }

        public LogLevel Level { get { throw null; } set { } }

        public string? LibraryId { get { throw null; } set { } }

        public string Message { get { throw null; } set { } }

        public string? ProjectPath { get { throw null; } set { } }

        public int StartColumnNumber { get { throw null; } set { } }

        public int StartLineNumber { get { throw null; } set { } }

        public System.DateTimeOffset Time { get { throw null; } set { } }

        public WarningLevel WarningLevel { get { throw null; } set { } }

        public static PackagingLogMessage CreateError(string message, NuGetLogCode code) { throw null; }

        public static PackagingLogMessage CreateMessage(string message, LogLevel logLevel) { throw null; }

        public static PackagingLogMessage CreateWarning(string message, NuGetLogCode code, string? libraryId, Frameworks.NuGetFramework? framework) { throw null; }

        public static PackagingLogMessage CreateWarning(string message, NuGetLogCode code) { throw null; }
    }

    public static partial class PathResolver
    {
        public static void FilterPackageFiles<T>(System.Collections.Generic.ICollection<T> source, System.Func<T, string> getPath, System.Collections.Generic.IEnumerable<string> wildcards) { }

        public static System.Collections.Generic.IEnumerable<T> GetFilteredPackageFiles<T>(System.Collections.Generic.ICollection<T> source, System.Func<T, string> getPath, System.Collections.Generic.IEnumerable<string> wildcards) { throw null; }

        public static System.Collections.Generic.IEnumerable<T> GetMatches<T>(System.Collections.Generic.IEnumerable<T> source, System.Func<T, string> getPath, System.Collections.Generic.IEnumerable<string> wildcards) { throw null; }

        public static bool IsDirectoryPath(string? path) { throw null; }

        public static bool IsWildcardSearch(string filter) { throw null; }

        public static string NormalizeWildcardForExcludedFiles(string basePath, string wildcard) { throw null; }

        public static System.Collections.Generic.IEnumerable<SearchPathResult> PerformWildcardSearch(string basePath, string searchPath, bool includeEmptyDirectories, out string normalizedBasePath) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> PerformWildcardSearch(string basePath, string searchPath) { throw null; }

        public partial struct SearchPathResult
        {
            private object _dummy;
            private int _dummyPrimitive;
            public SearchPathResult(string path, bool isFile) { }

            public bool IsFile { get { throw null; } }

            public string Path { get { throw null; } }
        }
    }

    public static partial class PathUtility
    {
        public static bool IsFileSystemCaseInsensitive { get { throw null; } }

        public static void EnsureParentDirectory(string filePath) { }

        public static string EnsureTrailingForwardSlash(string path) { throw null; }

        public static string EnsureTrailingSlash(string path) { throw null; }

        public static string EscapePSPath(string path) { throw null; }

        public static string GetAbsolutePath(string basePath, string relativePath) { throw null; }

        public static string GetDirectoryName(string path) { throw null; }

        public static System.IO.Compression.ZipArchiveEntry? GetEntry(System.IO.Compression.ZipArchive archive, string path) { throw null; }

        public static string GetPath(System.Uri uri) { throw null; }

        public static string GetPathWithBackSlashes(string path) { throw null; }

        public static string GetPathWithDirectorySeparator(string path) { throw null; }

        public static string GetPathWithForwardSlashes(string path) { throw null; }

        public static string GetRelativePath(string path1, string path2, char separator) { throw null; }

        public static string GetRelativePath(string path1, string path2) { throw null; }

        public static System.StringComparer GetStringComparerBasedOnOS() { throw null; }

        public static System.StringComparison GetStringComparisonBasedOnOS() { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetUniquePathsBasedOnOS(System.Collections.Generic.IEnumerable<string> paths) { throw null; }

        public static bool HasTrailingDirectorySeparator(string? path) { throw null; }

        public static bool IsChildOfDirectory(string dir, string candidate) { throw null; }

        public static bool IsDirectorySeparatorChar(char ch) { throw null; }

        public static bool IsSubdirectory(string basePath, string path) { throw null; }

        public static string ReplaceAltDirSeparatorWithDirSeparator(string path) { throw null; }

        public static string ReplaceDirSeparatorWithAltDirSeparator(string path) { throw null; }

        public static string SmartTruncate(string path, int maxWidth) { throw null; }

        public static string StripLeadingDirectorySeparators(string filename) { throw null; }
    }

    public static partial class PathValidator
    {
        public static bool IsValidLocalPath(string path) { throw null; }

        public static bool IsValidRelativePath(string path) { throw null; }

        public static bool IsValidSource(string source) { throw null; }

        public static bool IsValidUncPath(string path) { throw null; }

        public static bool IsValidUrl(string url) { throw null; }
    }

    public static partial class Preprocessor
    {
        public static string Process(System.IO.Stream stream, System.Func<string, string> tokenReplacement) { throw null; }

        public static System.Threading.Tasks.Task<string> ProcessAsync(System.Func<System.Threading.Tasks.Task<System.IO.Stream>> streamTaskFactory, System.Func<string, string> tokenReplacement, System.Threading.CancellationToken cancellationToken) { throw null; }
    }

    public static partial class ProjectJsonPathUtilities
    {
        public static readonly string ProjectConfigFileEnding;
        public static readonly string ProjectConfigFileName;
        public static readonly string ProjectLockFileName;
        public static string GetLockFilePath(string configFilePath) { throw null; }

        public static string GetProjectConfigPath(string directoryPath, string projectName) { throw null; }

        public static string GetProjectConfigWithProjectName(string projectName) { throw null; }

        public static string GetProjectLockFileNameWithProjectName(string projectName) { throw null; }

        public static string? GetProjectNameFromConfigFileName(string configPath) { throw null; }

        public static bool IsProjectConfig(string configPath) { throw null; }
    }

    public partial class RestoreLogMessage : IRestoreLogMessage, INuGetLogMessage, ILogMessage, ILogFileContext
    {
        public RestoreLogMessage(LogLevel logLevel, NuGetLogCode errorCode, string errorString, string? targetGraph, bool logToInnerLogger) { }

        public RestoreLogMessage(LogLevel logLevel, NuGetLogCode errorCode, string errorString, string? targetGraph) { }

        public RestoreLogMessage(LogLevel logLevel, NuGetLogCode errorCode, string errorString) { }

        public RestoreLogMessage(LogLevel logLevel, string errorString) { }

        public NuGetLogCode Code { get { throw null; } set { } }

        public int EndColumnNumber { get { throw null; } set { } }

        public int EndLineNumber { get { throw null; } set { } }

        public string? FilePath { get { throw null; } set { } }

        public LogLevel Level { get { throw null; } set { } }

        public string? LibraryId { get { throw null; } set { } }

        public string Message { get { throw null; } set { } }

        public string? ProjectPath { get { throw null; } set { } }

        public bool ShouldDisplay { get { throw null; } set { } }

        public int StartColumnNumber { get { throw null; } set { } }

        public int StartLineNumber { get { throw null; } set { } }

        public System.Collections.Generic.IReadOnlyList<string> TargetGraphs { get { throw null; } set { } }

        public System.DateTimeOffset Time { get { throw null; } set { } }

        public WarningLevel WarningLevel { get { throw null; } set { } }

        public static RestoreLogMessage CreateError(NuGetLogCode code, string message, string? libraryId, params string[] targetGraphs) { throw null; }

        public static RestoreLogMessage CreateError(NuGetLogCode code, string message) { throw null; }

        public static RestoreLogMessage CreateWarning(NuGetLogCode code, string message, string? libraryId, params string[] targetGraphs) { throw null; }

        public static RestoreLogMessage CreateWarning(NuGetLogCode code, string message) { throw null; }
    }

    public enum RevocationMode
    {
        Online = 0,
        Offline = 1
    }

    public static partial class RuntimeEnvironmentHelper
    {
        public static bool IsLinux { get { throw null; } }

        public static bool IsMacOSX { get { throw null; } }

        public static bool IsMono { get { throw null; } }

        public static bool IsRunningInVisualStudio { get { throw null; } }

        public static bool IsWindows { get { throw null; } }
    }

    public enum SignatureAlgorithmName
    {
        Unknown = 0,
        SHA256RSA = 1,
        SHA384RSA = 2,
        SHA512RSA = 3
    }

    public enum SignatureValidationMode
    {
        Accept = 0,
        Require = 1
    }

    public partial class TelemetryActivity : System.IDisposable
    {
        internal TelemetryActivity() { }

        public static INuGetTelemetryService? NuGetTelemetryService { get { throw null; } set { } }

        public System.Guid OperationId { get { throw null; } }

        public System.Guid ParentId { get { throw null; } }

        public TelemetryEvent TelemetryEvent { get { throw null; } set { } }

        public static TelemetryActivity Create(TelemetryEvent telemetryEvent) { throw null; }

        public static TelemetryActivity Create(System.Guid parentId, TelemetryEvent telemetryEvent) { throw null; }

        public static TelemetryActivity Create(System.Guid parentId, string eventName) { throw null; }

        public static TelemetryActivity Create(string eventName) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public static void EmitTelemetryEvent(TelemetryEvent TelemetryEvent) { }

        public void EndIntervalMeasure(string propertyName) { }

        public System.IDisposable StartIndependentInterval(string propertyName) { throw null; }

        public void StartIntervalMeasure() { }
    }

    public partial class TelemetryEvent
    {
        public TelemetryEvent(string eventName, System.Collections.Generic.Dictionary<string, object?> properties) { }

        public TelemetryEvent(string eventName) { }

        public System.Collections.Generic.IDictionary<string, object?> ComplexData { get { throw null; } }

        public int Count { get { throw null; } }

        public object? this[string key] { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public void AddPiiData(string key, object? value) { }

        public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, object?>> GetEnumerator() { throw null; }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object?>> GetPiiData() { throw null; }
    }

    public static partial class TelemetryServiceUtility
    {
        public static System.TimeSpan GetTimerElapsedTime() { throw null; }

        public static double GetTimerElapsedTimeInSeconds() { throw null; }

        public static void StartOrResumeTimer() { }

        public static void StopTimer() { }
    }

    public partial class Token
    {
        public Token(TokenCategory category, string value) { }

        public TokenCategory Category { get { throw null; } }

        public string Value { get { throw null; } }
    }

    public enum TokenCategory
    {
        Text = 0,
        Variable = 1
    }

    public partial class Tokenizer
    {
        public Tokenizer(string text) { }

        public Token? Read() { throw null; }
    }

    public static partial class UriUtility
    {
        public static System.Uri CreateSourceUri(string source, System.UriKind kind = System.UriKind.Absolute) { throw null; }

        public static string? GetAbsolutePath(string? rootDirectory, string? path) { throw null; }

        public static string GetAbsolutePathFromFile(string? sourceFile, string path) { throw null; }

        public static string GetLocalPath(string localOrUriPath) { throw null; }

        public static bool IsNuGetOrg(string? source) { throw null; }

        public static System.Uri? TryCreateSourceUri(string source, System.UriKind kind) { throw null; }

        public static string UrlEncodeOdataParameter(string value) { throw null; }
    }

    public enum WarningLevel
    {
        Severe = 1,
        Important = 2,
        Minimal = 3,
        Default = 4
    }

    [System.Obsolete("This class is obsolete and will be removed in a future release.")]
    public static partial class XmlUtility
    {
        public static System.Xml.Linq.XDocument Load(string filePath) { throw null; }
    }
}

namespace NuGet.Common.Migrations
{
    public static partial class MigrationRunner
    {
        public static void Run() { }
    }
}