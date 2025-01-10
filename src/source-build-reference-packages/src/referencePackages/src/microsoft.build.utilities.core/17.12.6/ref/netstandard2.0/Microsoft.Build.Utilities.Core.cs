// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Build.Utilities.UnitTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010015c01ae1f50e8cc09ba9eac9147cf8fd9fce2cfe9f8dce4f7301c4132ca9fb50ce8cbf1df4dc18dd4d210e4345c744ecb3365ed327efdbc52603faa5e21daa11234c8c4a73e51f03bf192544581ebe107adee3a34928e39d04e524a9ce729d5090bfd7dad9d10c722c0def9ccc08ff0a03790e48bcd1f9b6c476063e1966a1c4")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.SafeDirectories)]
[assembly: System.Resources.NeutralResourcesLanguage("en")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.Build.Utilities.Core.dll")]
[assembly: System.Reflection.AssemblyFileVersion("17.12.6.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("17.12.6+db5f6012cb7f6e2dd7066c50c573c0d352713407")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® Build Tools®")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Build.Utilities.Core.dll")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/msbuild")]
[assembly: System.Reflection.AssemblyVersionAttribute("15.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Build.Utilities
{
    [Framework.LoadInSeparateAppDomain]
    [System.Obsolete("AppDomains are no longer supported in .NET Core or .NET 5.0 or higher.")]
    public abstract partial class AppDomainIsolatedTask : System.MarshalByRefObject, Framework.ITask
    {
        protected AppDomainIsolatedTask() { }

        protected AppDomainIsolatedTask(System.Resources.ResourceManager taskResources, string helpKeywordPrefix) { }

        protected AppDomainIsolatedTask(System.Resources.ResourceManager taskResources) { }

        public Framework.IBuildEngine BuildEngine { get { throw null; } set { } }

        protected string HelpKeywordPrefix { get { throw null; } set { } }

        public Framework.ITaskHost HostObject { get { throw null; } set { } }

        public TaskLoggingHelper Log { get { throw null; } }

        protected System.Resources.ResourceManager TaskResources { get { throw null; } set { } }

        public abstract bool Execute();
        [System.Obsolete("AppDomains are no longer supported in .NET Core or .NET 5.0 or higher.")]
        public override object InitializeLifetimeService() { throw null; }
    }

    public partial class AssemblyFoldersExInfo
    {
        public AssemblyFoldersExInfo(Win32.RegistryHive hive, Win32.RegistryView view, string registryKey, string directoryPath, System.Version targetFrameworkVersion) { }

        public string DirectoryPath { get { throw null; } }

        public Win32.RegistryHive Hive { get { throw null; } }

        public string Key { get { throw null; } }

        public System.Version TargetFrameworkVersion { get { throw null; } }

        public Win32.RegistryView View { get { throw null; } }
    }

    public partial class AssemblyFoldersFromConfigInfo
    {
        public AssemblyFoldersFromConfigInfo(string directoryPath, System.Version targetFrameworkVersion) { }

        public string DirectoryPath { get { throw null; } }

        public System.Version TargetFrameworkVersion { get { throw null; } }
    }

    public partial class CommandLineBuilder
    {
        public CommandLineBuilder() { }

        public CommandLineBuilder(bool quoteHyphensOnCommandLine, bool useNewLineSeparator) { }

        public CommandLineBuilder(bool quoteHyphensOnCommandLine) { }

        protected System.Text.StringBuilder CommandLine { get { throw null; } }

        public int Length { get { throw null; } }

        public void AppendFileNameIfNotNull(Framework.ITaskItem fileItem) { }

        public void AppendFileNameIfNotNull(string fileName) { }

        public void AppendFileNamesIfNotNull(Framework.ITaskItem[] fileItems, string delimiter) { }

        public void AppendFileNamesIfNotNull(string[] fileNames, string delimiter) { }

        protected void AppendFileNameWithQuoting(string fileName) { }

        protected void AppendQuotedTextToBuffer(System.Text.StringBuilder buffer, string unquotedTextToAppend) { }

        protected void AppendSpaceIfNotEmpty() { }

        public void AppendSwitch(string switchName) { }

        public void AppendSwitchIfNotNull(string switchName, Framework.ITaskItem parameter) { }

        public void AppendSwitchIfNotNull(string switchName, Framework.ITaskItem[] parameters, string delimiter) { }

        public void AppendSwitchIfNotNull(string switchName, string parameter) { }

        public void AppendSwitchIfNotNull(string switchName, string[] parameters, string delimiter) { }

        public void AppendSwitchUnquotedIfNotNull(string switchName, Framework.ITaskItem parameter) { }

        public void AppendSwitchUnquotedIfNotNull(string switchName, Framework.ITaskItem[] parameters, string delimiter) { }

        public void AppendSwitchUnquotedIfNotNull(string switchName, string parameter) { }

        public void AppendSwitchUnquotedIfNotNull(string switchName, string[] parameters, string delimiter) { }

        public void AppendTextUnquoted(string textToAppend) { }

        protected void AppendTextWithQuoting(string textToAppend) { }

        protected virtual bool IsQuotingRequired(string parameter) { throw null; }

        public override string ToString() { throw null; }

        protected virtual void VerifyThrowNoEmbeddedDoubleQuotes(string switchName, string parameter) { }
    }

    public enum DotNetFrameworkArchitecture
    {
        Current = 0,
        Bitness32 = 1,
        Bitness64 = 2
    }

    public enum HostObjectInitializationStatus
    {
        UseHostObjectToExecute = 0,
        UseAlternateToolToExecute = 1,
        NoActionReturnSuccess = 2,
        NoActionReturnFailure = 3
    }

    public static partial class LockCheck
    {
        public static string GetLockedFileMessage(string filePath) { throw null; }
    }

    public abstract partial class Logger : Framework.ILogger
    {
        public virtual string Parameters { get { throw null; } set { } }

        public virtual Framework.LoggerVerbosity Verbosity { get { throw null; } set { } }

        public virtual string FormatErrorEvent(Framework.BuildErrorEventArgs args) { throw null; }

        public virtual string FormatWarningEvent(Framework.BuildWarningEventArgs args) { throw null; }

        public abstract void Initialize(Framework.IEventSource eventSource);
        public bool IsVerbosityAtLeast(Framework.LoggerVerbosity checkVerbosity) { throw null; }

        public virtual void Shutdown() { }
    }

    public enum MultipleVersionSupport
    {
        Allow = 0,
        Warning = 1,
        Error = 2
    }

    public partial class MuxLogger : Framework.INodeLogger, Framework.ILogger
    {
        public bool IncludeEvaluationMetaprojects { get { throw null; } set { } }

        public bool IncludeEvaluationProfiles { get { throw null; } set { } }

        public bool IncludeEvaluationPropertiesAndItems { get { throw null; } set { } }

        public bool IncludeTaskInputs { get { throw null; } set { } }

        public string Parameters { get { throw null; } set { } }

        public Framework.LoggerVerbosity Verbosity { get { throw null; } set { } }

        public void Initialize(Framework.IEventSource eventSource, int maxNodeCount) { }

        public void Initialize(Framework.IEventSource eventSource) { }

        public void RegisterLogger(int submissionId, Framework.ILogger logger) { }

        public void Shutdown() { }

        public bool UnregisterLoggers(int submissionId) { throw null; }
    }

    public static partial class ProcessorArchitecture
    {
        public const string AMD64 = "AMD64";
        public const string ARM = "ARM";
        public const string ARM64 = "ARM64";
        public const string ARMV6 = "ARMV6";
        public const string IA64 = "IA64";
        public const string LOONGARCH64 = "LOONGARCH64";
        public const string MSIL = "MSIL";
        public const string PPC64LE = "PPC64LE";
        public const string S390X = "S390X";
        public const string WASM = "WASM";
        public const string X86 = "x86";
        public static string CurrentProcessArchitecture { get { throw null; } }
    }

    public partial class SDKManifest
    {
        public SDKManifest(string pathToSdk) { }

        public System.Collections.Generic.IDictionary<string, string> AppxLocations { get { throw null; } }

        public string CopyRedistToSubDirectory { get { throw null; } }

        public string DependsOnSDK { get { throw null; } }

        public string DisplayName { get { throw null; } }

        public System.Collections.Generic.IDictionary<string, string> FrameworkIdentities { get { throw null; } }

        public string FrameworkIdentity { get { throw null; } }

        public string MaxOSVersionTested { get { throw null; } }

        public string MaxPlatformVersion { get { throw null; } }

        public string MinOSVersion { get { throw null; } }

        public string MinVSVersion { get { throw null; } }

        public string MoreInfo { get { throw null; } }

        public string PlatformIdentity { get { throw null; } }

        public string ProductFamilyName { get { throw null; } }

        public bool ReadError { get { throw null; } }

        public string ReadErrorMessage { get { throw null; } }

        public SDKType SDKType { get { throw null; } }

        public string SupportedArchitectures { get { throw null; } }

        public string SupportPrefer32Bit { get { throw null; } }

        public MultipleVersionSupport SupportsMultipleVersions { get { throw null; } }

        public string TargetPlatform { get { throw null; } }

        public string TargetPlatformMinVersion { get { throw null; } }

        public string TargetPlatformVersion { get { throw null; } }

        public static partial class Attributes
        {
            public const string APPX = "APPX";
            public const string AppxLocation = "AppxLocation";
            public const string CopyLocalExpandedReferenceAssemblies = "CopyLocalExpandedReferenceAssemblies";
            public const string CopyRedist = "CopyRedist";
            public const string CopyRedistToSubDirectory = "CopyRedistToSubDirectory";
            public const string DependsOnSDK = "DependsOn";
            public const string DisplayName = "DisplayName";
            public const string ExpandReferenceAssemblies = "ExpandReferenceAssemblies";
            public const string FrameworkIdentity = "FrameworkIdentity";
            public const string MaxOSVersionTested = "MaxOSVersionTested";
            public const string MaxPlatformVersion = "MaxPlatformVersion";
            public const string MinOSVersion = "MinOSVersion";
            public const string MinVSVersion = "MinVSVersion";
            public const string MoreInfo = "MoreInfo";
            public const string PlatformIdentity = "PlatformIdentity";
            public const string ProductFamilyName = "ProductFamilyName";
            public const string SDKType = "SDKType";
            public const string SupportedArchitectures = "SupportedArchitectures";
            public const string SupportPrefer32Bit = "SupportPrefer32Bit";
            public const string SupportsMultipleVersions = "SupportsMultipleVersions";
            public const string TargetedSDK = "TargetedSDKArchitecture";
            public const string TargetedSDKConfiguration = "TargetedSDKConfiguration";
            public const string TargetPlatform = "TargetPlatform";
            public const string TargetPlatformMinVersion = "TargetPlatformMinVersion";
            public const string TargetPlatformVersion = "TargetPlatformVersion";
        }
    }

    public enum SDKType
    {
        Unspecified = 0,
        External = 1,
        Platform = 2,
        Framework = 3
    }

    public enum TargetDotNetFrameworkVersion
    {
        Version11 = 0,
        Version20 = 1,
        Version30 = 2,
        Version35 = 3,
        Version40 = 4,
        Version45 = 5,
        Version451 = 6,
        Version46 = 7,
        Version461 = 8,
        Version452 = 9,
        Version462 = 10,
        Version47 = 11,
        Version471 = 12,
        Version472 = 13,
        Version48 = 14,
        VersionLatest = 14,
        Version481 = 15,
        Latest = 9999
    }

    public partial class TargetPlatformSDK : System.IEquatable<TargetPlatformSDK>
    {
        public TargetPlatformSDK(string targetPlatformIdentifier, System.Version targetPlatformVersion, string path) { }

        public string DisplayName { get { throw null; } }

        public System.Version MinOSVersion { get { throw null; } }

        public System.Version MinVSVersion { get { throw null; } }

        public string Path { get { throw null; } set { } }

        public string TargetPlatformIdentifier { get { throw null; } }

        public System.Version TargetPlatformVersion { get { throw null; } }

        public bool ContainsPlatform(string targetPlatformIdentifier, string targetPlatformVersion) { throw null; }

        public bool Equals(TargetPlatformSDK other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public abstract partial class Task : Framework.ITask
    {
        protected Task() { }

        protected Task(System.Resources.ResourceManager taskResources, string helpKeywordPrefix) { }

        protected Task(System.Resources.ResourceManager taskResources) { }

        public Framework.IBuildEngine BuildEngine { get { throw null; } set { } }

        public Framework.IBuildEngine2 BuildEngine2 { get { throw null; } }

        public Framework.IBuildEngine3 BuildEngine3 { get { throw null; } }

        public Framework.IBuildEngine4 BuildEngine4 { get { throw null; } }

        public Framework.IBuildEngine5 BuildEngine5 { get { throw null; } }

        public Framework.IBuildEngine6 BuildEngine6 { get { throw null; } }

        public Framework.IBuildEngine7 BuildEngine7 { get { throw null; } }

        public Framework.IBuildEngine8 BuildEngine8 { get { throw null; } }

        public Framework.IBuildEngine9 BuildEngine9 { get { throw null; } }

        protected string HelpKeywordPrefix { get { throw null; } set { } }

        public Framework.ITaskHost HostObject { get { throw null; } set { } }

        public TaskLoggingHelper Log { get { throw null; } }

        protected System.Resources.ResourceManager TaskResources { get { throw null; } set { } }

        public abstract bool Execute();
    }

    public sealed partial class TaskItem : Framework.ITaskItem2, Framework.ITaskItem
    {
        public TaskItem() { }

        public TaskItem(Framework.ITaskItem sourceItem) { }

        public TaskItem(string itemSpec, System.Collections.IDictionary itemMetadata) { }

        public TaskItem(string itemSpec) { }

        public string ItemSpec { get { throw null; } set { } }

        public int MetadataCount { get { throw null; } }

        public System.Collections.ICollection MetadataNames { get { throw null; } }

        string Framework.ITaskItem2.EvaluatedIncludeEscaped { get { throw null; } set { } }

        public System.Collections.IDictionary CloneCustomMetadata() { throw null; }

        public void CopyMetadataTo(Framework.ITaskItem destinationItem) { }

        public string GetMetadata(string metadataName) { throw null; }

        System.Collections.IDictionary Framework.ITaskItem2.CloneCustomMetadataEscaped() { throw null; }

        string Framework.ITaskItem2.GetMetadataValueEscaped(string metadataName) { throw null; }

        void Framework.ITaskItem2.SetMetadataValueLiteral(string metadataName, string metadataValue) { }

        public static explicit operator string(TaskItem taskItemToCast) { throw null; }

        public void RemoveMetadata(string metadataName) { }

        public void SetMetadata(string metadataName, string metadataValue) { }

        public override string ToString() { throw null; }
    }

    public partial class TaskLoggingHelper
    {
        public TaskLoggingHelper(Framework.IBuildEngine buildEngine, string taskName) { }

        public TaskLoggingHelper(Framework.ITask taskInstance) { }

        protected Framework.IBuildEngine BuildEngine { get { throw null; } }

        public bool HasLoggedErrors { get { throw null; } }

        public string HelpKeywordPrefix { get { throw null; } set { } }

        public bool IsTaskInputLoggingEnabled { get { throw null; } }

        protected string TaskName { get { throw null; } }

        public System.Resources.ResourceManager TaskResources { get { throw null; } set { } }

        public string ExtractMessageCode(string message, out string messageWithoutCodePrefix) { throw null; }

        public virtual string FormatResourceString(string resourceName, params object[] args) { throw null; }

        public virtual string FormatString(string unformatted, params object[] args) { throw null; }

        public static string GetInnerExceptionMessageString(System.Exception e) { throw null; }

        public virtual string GetResourceMessage(string resourceName) { throw null; }

        public void LogCommandLine(Framework.MessageImportance importance, string commandLine) { }

        public void LogCommandLine(string commandLine) { }

        public void LogCriticalMessage(string subcategory, string code, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs) { }

        public void LogError(string message, params object[] messageArgs) { }

        public void LogError(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs) { }

        public void LogError(string subcategory, string errorCode, string helpKeyword, string helpLink, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs) { }

        public void LogErrorFromException(System.Exception exception, bool showStackTrace, bool showDetail, string file) { }

        public void LogErrorFromException(System.Exception exception, bool showStackTrace) { }

        public void LogErrorFromException(System.Exception exception) { }

        public void LogErrorFromResources(string messageResourceName, params object[] messageArgs) { }

        public void LogErrorFromResources(string subcategoryResourceName, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string messageResourceName, params object[] messageArgs) { }

        public void LogErrorWithCodeFromResources(string messageResourceName, params object[] messageArgs) { }

        public void LogErrorWithCodeFromResources(string subcategoryResourceName, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string messageResourceName, params object[] messageArgs) { }

        public void LogExternalProjectFinished(string message, string helpKeyword, string projectFile, bool succeeded) { }

        public void LogExternalProjectStarted(string message, string helpKeyword, string projectFile, string targetNames) { }

        public void LogIncludeGeneratedFile(string filePath, string content) { }

        public void LogMessage(Framework.MessageImportance importance, string message, params object[] messageArgs) { }

        public void LogMessage(string message, params object[] messageArgs) { }

        public void LogMessage(string subcategory, string code, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, Framework.MessageImportance importance, string message, params object[] messageArgs) { }

        public void LogMessageFromResources(Framework.MessageImportance importance, string messageResourceName, params object[] messageArgs) { }

        public void LogMessageFromResources(string messageResourceName, params object[] messageArgs) { }

        public bool LogMessageFromText(string lineOfText, Framework.MessageImportance messageImportance) { throw null; }

        public bool LogMessagesFromFile(string fileName, Framework.MessageImportance messageImportance) { throw null; }

        public bool LogMessagesFromFile(string fileName) { throw null; }

        public bool LogMessagesFromStream(System.IO.TextReader stream, Framework.MessageImportance messageImportance) { throw null; }

        public bool LogsMessagesOfImportance(Framework.MessageImportance importance) { throw null; }

        public void LogTelemetry(string eventName, System.Collections.Generic.IDictionary<string, string> properties) { }

        public void LogWarning(string message, params object[] messageArgs) { }

        public void LogWarning(string subcategory, string warningCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs) { }

        public void LogWarning(string subcategory, string warningCode, string helpKeyword, string helpLink, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs) { }

        public void LogWarningFromException(System.Exception exception, bool showStackTrace) { }

        public void LogWarningFromException(System.Exception exception) { }

        public void LogWarningFromResources(string messageResourceName, params object[] messageArgs) { }

        public void LogWarningFromResources(string subcategoryResourceName, string warningCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string messageResourceName, params object[] messageArgs) { }

        public void LogWarningWithCodeFromResources(string messageResourceName, params object[] messageArgs) { }

        public void LogWarningWithCodeFromResources(string subcategoryResourceName, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string messageResourceName, params object[] messageArgs) { }
    }

    public static partial class ToolLocationHelper
    {
        public static string CurrentToolsVersion { get { throw null; } }

        public static string PathToSystem { get { throw null; } }

        public static void ClearSDKStaticCache() { }

        public static System.Collections.Generic.IDictionary<string, string> FilterPlatformExtensionSDKs(System.Version targetPlatformVersion, System.Collections.Generic.IDictionary<string, string> extensionSdks) { throw null; }

        public static System.Collections.Generic.IList<TargetPlatformSDK> FilterTargetPlatformSdks(System.Collections.Generic.IList<TargetPlatformSDK> targetPlatformSdkList, System.Version osVersion, System.Version vsVersion) { throw null; }

        public static string FindRootFolderWhereAllFilesExist(string possibleRoots, string relativeFilePaths) { throw null; }

        public static System.Collections.Generic.IList<AssemblyFoldersExInfo> GetAssemblyFoldersExInfo(string registryRoot, string targetFrameworkVersion, string registryKeySuffix, string osVersion, string platform, System.Reflection.ProcessorArchitecture targetProcessorArchitecture) { throw null; }

        public static System.Collections.Generic.IList<AssemblyFoldersFromConfigInfo> GetAssemblyFoldersFromConfigInfo(string configFile, string targetFrameworkVersion, System.Reflection.ProcessorArchitecture targetProcessorArchitecture) { throw null; }

        public static string GetDisplayNameForTargetFrameworkDirectory(string targetFrameworkDirectory, System.Runtime.Versioning.FrameworkName frameworkName) { throw null; }

        public static string GetDotNetFrameworkRootRegistryKey(TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetDotNetFrameworkSdkInstallKeyValue(TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion) { throw null; }

        public static string GetDotNetFrameworkSdkInstallKeyValue(TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetDotNetFrameworkSdkRootRegistryKey(TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion) { throw null; }

        public static string GetDotNetFrameworkSdkRootRegistryKey(TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetDotNetFrameworkVersionFolderPrefix(TargetDotNetFrameworkVersion version) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetFoldersInVSInstalls(System.Version minVersion = null, System.Version maxVersion = null, string subFolder = null) { throw null; }

        public static string GetFoldersInVSInstallsAsString(string minVersionString = null, string maxVersionString = null, string subFolder = null) { throw null; }

        public static string GetLatestSDKTargetPlatformVersion(string sdkIdentifier, string sdkVersion, string[] sdkRoots) { throw null; }

        public static string GetLatestSDKTargetPlatformVersion(string sdkIdentifier, string sdkVersion) { throw null; }

        public static string GetPathToBuildTools(string toolsVersion, DotNetFrameworkArchitecture architecture) { throw null; }

        public static string GetPathToBuildTools(string toolsVersion) { throw null; }

        public static string GetPathToBuildToolsFile(string fileName, string toolsVersion, DotNetFrameworkArchitecture architecture) { throw null; }

        public static string GetPathToBuildToolsFile(string fileName, string toolsVersion) { throw null; }

        public static string GetPathToDotNetFramework(TargetDotNetFrameworkVersion version, DotNetFrameworkArchitecture architecture) { throw null; }

        public static string GetPathToDotNetFramework(TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetPathToDotNetFrameworkFile(string fileName, TargetDotNetFrameworkVersion version, DotNetFrameworkArchitecture architecture) { throw null; }

        public static string GetPathToDotNetFrameworkFile(string fileName, TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetPathToDotNetFrameworkReferenceAssemblies(TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetPathToDotNetFrameworkSdk() { throw null; }

        public static string GetPathToDotNetFrameworkSdk(TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion) { throw null; }

        public static string GetPathToDotNetFrameworkSdk(TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetPathToDotNetFrameworkSdkFile(string fileName, TargetDotNetFrameworkVersion version, DotNetFrameworkArchitecture architecture) { throw null; }

        public static string GetPathToDotNetFrameworkSdkFile(string fileName, TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion, DotNetFrameworkArchitecture architecture) { throw null; }

        public static string GetPathToDotNetFrameworkSdkFile(string fileName, TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion) { throw null; }

        public static string GetPathToDotNetFrameworkSdkFile(string fileName, TargetDotNetFrameworkVersion version) { throw null; }

        public static string GetPathToDotNetFrameworkSdkFile(string fileName) { throw null; }

        public static System.Collections.Generic.IList<string> GetPathToReferenceAssemblies(System.Runtime.Versioning.FrameworkName frameworkName) { throw null; }

        public static System.Collections.Generic.IList<string> GetPathToReferenceAssemblies(string targetFrameworkRootPath, System.Runtime.Versioning.FrameworkName frameworkName) { throw null; }

        public static System.Collections.Generic.IList<string> GetPathToReferenceAssemblies(string targetFrameworkRootPath, string targetFrameworkFallbackSearchPaths, System.Runtime.Versioning.FrameworkName frameworkName) { throw null; }

        public static System.Collections.Generic.IList<string> GetPathToReferenceAssemblies(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetFrameworkProfile, string targetFrameworkRootPath, string targetFrameworkFallbackSearchPaths) { throw null; }

        public static System.Collections.Generic.IList<string> GetPathToReferenceAssemblies(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetFrameworkProfile, string targetFrameworkRootPath) { throw null; }

        public static System.Collections.Generic.IList<string> GetPathToReferenceAssemblies(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetFrameworkProfile) { throw null; }

        public static string GetPathToStandardLibraries(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetFrameworkProfile, string platformTarget, string targetFrameworkRootPath, string targetFrameworkFallbackSearchPaths) { throw null; }

        public static string GetPathToStandardLibraries(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetFrameworkProfile, string platformTarget, string targetFrameworkRootPath) { throw null; }

        public static string GetPathToStandardLibraries(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetFrameworkProfile, string platformTarget) { throw null; }

        public static string GetPathToStandardLibraries(string targetFrameworkIdentifier, string targetFrameworkVersion, string targetFrameworkProfile) { throw null; }

        public static string GetPathToSystemFile(string fileName) { throw null; }

        [System.Obsolete("Consider using GetPlatformSDKLocation instead")]
        public static string GetPathToWindowsSdk(TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion) { throw null; }

        [System.Obsolete("Consider using GetPlatformSDKLocationFile instead")]
        public static string GetPathToWindowsSdkFile(string fileName, TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion, DotNetFrameworkArchitecture architecture) { throw null; }

        [System.Obsolete("Consider using GetPlatformSDKLocationFile instead")]
        public static string GetPathToWindowsSdkFile(string fileName, TargetDotNetFrameworkVersion version, VisualStudioVersion visualStudioVersion) { throw null; }

        public static string GetPlatformExtensionSDKLocation(string sdkMoniker, string targetPlatformIdentifier, string targetPlatformVersion, string diskRoots, string extensionDiskRoots, string registryRoot) { throw null; }

        public static string GetPlatformExtensionSDKLocation(string sdkMoniker, string targetPlatformIdentifier, string targetPlatformVersion, string diskRoots, string registryRoot) { throw null; }

        public static string GetPlatformExtensionSDKLocation(string sdkMoniker, string targetPlatformIdentifier, string targetPlatformVersion) { throw null; }

        public static string GetPlatformExtensionSDKLocation(string sdkMoniker, string targetPlatformIdentifier, System.Version targetPlatformVersion, string[] diskRoots, string registryRoot) { throw null; }

        public static string GetPlatformExtensionSDKLocation(string sdkMoniker, string targetPlatformIdentifier, System.Version targetPlatformVersion, string[] diskRoots, string[] extensionDiskRoots, string registryRoot) { throw null; }

        public static string GetPlatformExtensionSDKLocation(string sdkMoniker, string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IDictionary<string, string> GetPlatformExtensionSDKLocations(string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IDictionary<string, string> GetPlatformExtensionSDKLocations(string[] diskRoots, string registryRoot, string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IDictionary<string, string> GetPlatformExtensionSDKLocations(string[] diskRoots, string[] extensionDiskRoots, string registryRoot, string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IDictionary<string, System.Tuple<string, string>> GetPlatformExtensionSDKLocationsAndVersions(string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IDictionary<string, System.Tuple<string, string>> GetPlatformExtensionSDKLocationsAndVersions(string[] diskRoots, string registryRoot, string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IDictionary<string, System.Tuple<string, string>> GetPlatformExtensionSDKLocationsAndVersions(string[] diskRoots, string[] multiPlatformDiskRoots, string registryRoot, string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static string[] GetPlatformOrFrameworkExtensionSdkReferences(string extensionSdkMoniker, string targetSdkIdentifier, string targetSdkVersion, string diskRoots, string extensionDiskRoots, string registryRoot, string targetPlatformIdentifier, string targetPlatformVersion) { throw null; }

        public static string[] GetPlatformOrFrameworkExtensionSdkReferences(string extensionSdkMoniker, string targetSdkIdentifier, string targetSdkVersion, string diskRoots, string extensionDiskRoots, string registryRoot) { throw null; }

        public static string GetPlatformSDKDisplayName(string targetPlatformIdentifier, string targetPlatformVersion, string diskRoots, string registryRoot) { throw null; }

        public static string GetPlatformSDKDisplayName(string targetPlatformIdentifier, string targetPlatformVersion) { throw null; }

        public static string GetPlatformSDKLocation(string targetPlatformIdentifier, string targetPlatformVersion, string diskRoots, string registryRoot) { throw null; }

        public static string GetPlatformSDKLocation(string targetPlatformIdentifier, string targetPlatformVersion) { throw null; }

        public static string GetPlatformSDKLocation(string targetPlatformIdentifier, System.Version targetPlatformVersion, string[] diskRoots, string registryRoot) { throw null; }

        public static string GetPlatformSDKLocation(string targetPlatformIdentifier, System.Version targetPlatformVersion) { throw null; }

        public static string GetPlatformSDKPropsFileLocation(string sdkIdentifier, string sdkVersion, string targetPlatformIdentifier, string targetPlatformMinVersion, string targetPlatformVersion, string diskRoots, string registryRoot) { throw null; }

        public static string GetPlatformSDKPropsFileLocation(string sdkIdentifier, string sdkVersion, string targetPlatformIdentifier, string targetPlatformMinVersion, string targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetPlatformsForSDK(string sdkIdentifier, System.Version sdkVersion, string[] diskRoots, string registryRoot) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> GetPlatformsForSDK(string sdkIdentifier, System.Version sdkVersion) { throw null; }

        public static string GetProgramFilesReferenceAssemblyRoot() { throw null; }

        public static string GetSDKContentFolderPath(string sdkIdentifier, string sdkVersion, string targetPlatformIdentifier, string targetPlatformMinVersion, string targetPlatformVersion, string folderName, string diskRoot = null) { throw null; }

        public static System.Collections.Generic.IList<string> GetSDKDesignTimeFolders(string sdkRoot, string targetConfiguration, string targetArchitecture) { throw null; }

        public static System.Collections.Generic.IList<string> GetSDKDesignTimeFolders(string sdkRoot) { throw null; }

        public static System.Collections.Generic.IList<string> GetSDKRedistFolders(string sdkRoot, string targetConfiguration, string targetArchitecture) { throw null; }

        public static System.Collections.Generic.IList<string> GetSDKRedistFolders(string sdkRoot) { throw null; }

        public static System.Collections.Generic.IList<string> GetSDKReferenceFolders(string sdkRoot, string targetConfiguration, string targetArchitecture) { throw null; }

        public static System.Collections.Generic.IList<string> GetSDKReferenceFolders(string sdkRoot) { throw null; }

        public static System.Collections.Generic.IList<string> GetSupportedTargetFrameworks() { throw null; }

        public static string[] GetTargetPlatformReferences(string sdkIdentifier, string sdkVersion, string targetPlatformIdentifier, string targetPlatformMinVersion, string targetPlatformVersion, string diskRoots, string registryRoot) { throw null; }

        public static string[] GetTargetPlatformReferences(string sdkIdentifier, string sdkVersion, string targetPlatformIdentifier, string targetPlatformMinVersion, string targetPlatformVersion) { throw null; }

        public static System.Collections.Generic.IList<TargetPlatformSDK> GetTargetPlatformSdks() { throw null; }

        public static System.Collections.Generic.IList<TargetPlatformSDK> GetTargetPlatformSdks(string[] diskRoots, string registryRoot) { throw null; }

        public static System.Runtime.Versioning.FrameworkName HighestVersionOfTargetFrameworkIdentifier(string targetFrameworkRootDirectory, string frameworkIdentifier) { throw null; }
    }

    public abstract partial class ToolTask : Task, Framework.IIncrementalTask, Framework.ICancelableTask, Framework.ITask
    {
        protected ToolTask() { }

        protected ToolTask(System.Resources.ResourceManager taskResources, string helpKeywordPrefix) { }

        protected ToolTask(System.Resources.ResourceManager taskResources) { }

        protected bool canBeIncremental { get { throw null; } set { } }

        public bool EchoOff { get { throw null; } set { } }

        [System.Obsolete("Use EnvironmentVariables property")]
        protected virtual System.Collections.Generic.Dictionary<string, string> EnvironmentOverride { get { throw null; } }

        public string[] EnvironmentVariables { get { throw null; } set { } }

        [Framework.Output]
        public int ExitCode { get { throw null; } }

        public bool FailIfNotIncremental { get { throw null; } set { } }

        protected virtual bool HasLoggedErrors { get { throw null; } }

        public bool LogStandardErrorAsError { get { throw null; } set { } }

        protected virtual System.Text.Encoding ResponseFileEncoding { get { throw null; } }

        protected virtual System.Text.Encoding StandardErrorEncoding { get { throw null; } }

        public string StandardErrorImportance { get { throw null; } set { } }

        protected Framework.MessageImportance StandardErrorImportanceToUse { get { throw null; } }

        protected virtual Framework.MessageImportance StandardErrorLoggingImportance { get { throw null; } }

        protected virtual System.Text.Encoding StandardOutputEncoding { get { throw null; } }

        public string StandardOutputImportance { get { throw null; } set { } }

        protected Framework.MessageImportance StandardOutputImportanceToUse { get { throw null; } }

        protected virtual Framework.MessageImportance StandardOutputLoggingImportance { get { throw null; } }

        protected int TaskProcessTerminationTimeout { get { throw null; } set { } }

        public virtual int Timeout { get { throw null; } set { } }

        protected System.Threading.ManualResetEvent ToolCanceled { get { throw null; } }

        public virtual string ToolExe { get { throw null; } set { } }

        protected abstract string ToolName { get; }

        public string ToolPath { get { throw null; } set { } }

        public bool UseCommandProcessor { get { throw null; } set { } }

        public string UseUtf8Encoding { get { throw null; } set { } }

        public bool YieldDuringToolExecution { get { throw null; } set { } }

        protected virtual string AdjustCommandsForOperatingSystem(string input) { throw null; }

        protected virtual bool CallHostObjectToExecute() { throw null; }

        public virtual void Cancel() { }

        protected void DeleteTempFile(string fileName) { }

        public override bool Execute() { throw null; }

        protected virtual int ExecuteTool(string pathToTool, string responseFileCommands, string commandLineCommands) { throw null; }

        protected virtual string GenerateCommandLineCommands() { throw null; }

        protected abstract string GenerateFullPathToTool();
        protected virtual string GenerateResponseFileCommands() { throw null; }

        protected virtual System.Diagnostics.ProcessStartInfo GetProcessStartInfo(string pathToTool, string commandLineCommands, string responseFileSwitch) { throw null; }

        protected virtual string GetResponseFileSwitch(string responseFilePath) { throw null; }

        protected virtual string GetWorkingDirectory() { throw null; }

        protected virtual bool HandleTaskExecutionErrors() { throw null; }

        protected virtual HostObjectInitializationStatus InitializeHostObject() { throw null; }

        protected virtual void LogEventsFromTextOutput(string singleLine, Framework.MessageImportance messageImportance) { }

        protected virtual void LogPathToTool(string toolName, string pathToTool) { }

        protected virtual void LogToolCommand(string message) { }

        protected virtual void ProcessStarted() { }

        protected void ReceiveExitNotification(object sender, System.EventArgs e) { }

        protected void ReceiveStandardErrorData(object sender, System.Diagnostics.DataReceivedEventArgs e) { }

        protected void ReceiveStandardOutputData(object sender, System.Diagnostics.DataReceivedEventArgs e) { }

        protected virtual string ResponseFileEscape(string responseString) { throw null; }

        protected virtual bool SkipTaskExecution() { throw null; }

        protected virtual System.Diagnostics.Process StartToolProcess(System.Diagnostics.Process proc) { throw null; }

        protected internal virtual bool ValidateParameters() { throw null; }
    }

    public static partial class TrackedDependencies
    {
        public static Framework.ITaskItem[] ExpandWildcards(Framework.ITaskItem[] expand) { throw null; }
    }

    public enum VisualStudioVersion
    {
        Version100 = 0,
        Version110 = 1,
        Version120 = 2,
        Version140 = 3,
        Version150 = 4,
        Version160 = 5,
        Version170 = 6,
        VersionLatest = 6
    }
}