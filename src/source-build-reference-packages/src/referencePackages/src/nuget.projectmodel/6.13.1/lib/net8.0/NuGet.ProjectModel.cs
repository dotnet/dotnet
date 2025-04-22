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
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("NuGet's core types and interfaces for PackageReference-based restore, such as lock files, assets file and internal restore models.")]
[assembly: System.Reflection.AssemblyFileVersion("6.13.1.3")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.13.1+3fd0e588e53525f0cd037d7b91174c0ca78ac65c.3fd0e588e53525f0cd037d7b91174c0ca78ac65c")]
[assembly: System.Reflection.AssemblyProduct("NuGet")]
[assembly: System.Reflection.AssemblyTitle("NuGet.ProjectModel")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/NuGet/NuGet.Client")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NuGet.ProjectModel.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100A5276DF8650A58CB43396DC7B3D395F30A82D0D1FA98FBCFE3ABEAD5DE0B1DB6764347A0F6BF0B060A27C202CCD122DB5DED8F596CEBE2ECC3A6629015EEB96C94F6B9E8185D4ACC84C376FF6B1C3147431A4D55CB5736DB97A9E88FCC47D9193F4DB5896DC5817E5D0CBD2641726E7431990BCD2DD7FA1D28493D0CFD9DCFA4")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.13.1.3")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace NuGet.ProjectModel
{
    public partial class AssetsLogMessage : IAssetsLogMessage, System.IEquatable<IAssetsLogMessage>
    {
        public AssetsLogMessage(Common.LogLevel logLevel, Common.NuGetLogCode errorCode, string errorString, string targetGraph) { }

        public AssetsLogMessage(Common.LogLevel logLevel, Common.NuGetLogCode errorCode, string errorString) { }

        public Common.NuGetLogCode Code { get { throw null; } }

        public int EndColumnNumber { get { throw null; } set { } }

        public int EndLineNumber { get { throw null; } set { } }

        public string FilePath { get { throw null; } set { } }

        public Common.LogLevel Level { get { throw null; } }

        public string LibraryId { get { throw null; } set { } }

        public string Message { get { throw null; } }

        public string ProjectPath { get { throw null; } set { } }

        public int StartColumnNumber { get { throw null; } set { } }

        public int StartLineNumber { get { throw null; } set { } }

        public System.Collections.Generic.IReadOnlyList<string> TargetGraphs { get { throw null; } set { } }

        public Common.WarningLevel WarningLevel { get { throw null; } set { } }

        public static IAssetsLogMessage Create(Common.IRestoreLogMessage logMessage) { throw null; }

        public bool Equals(IAssetsLogMessage other) { throw null; }

        public override bool Equals(object other) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial struct BuildAction : System.IEquatable<BuildAction>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public static readonly BuildAction AndroidAsset;
        public static readonly BuildAction AndroidResource;
        public static readonly BuildAction ApplicationDefinition;
        public static readonly BuildAction BundleResource;
        public static readonly BuildAction CodeAnalysisDictionary;
        public static readonly BuildAction Compile;
        public static readonly BuildAction Content;
        public static readonly BuildAction DesignData;
        public static readonly BuildAction DesignDataWithDesignTimeCreatableTypes;
        public static readonly BuildAction EmbeddedResource;
        public static readonly BuildAction None;
        public static readonly BuildAction Page;
        public static readonly BuildAction Resource;
        public static readonly BuildAction SplashScreen;
        public bool IsKnown { get { throw null; } }

        public string Value { get { throw null; } }

        public bool Equals(BuildAction other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(BuildAction left, BuildAction right) { throw null; }

        public static bool operator !=(BuildAction left, BuildAction right) { throw null; }

        public static BuildAction Parse(string value) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class BuildOptions : System.IEquatable<BuildOptions>
    {
        public string OutputName { get { throw null; } set { } }

        public BuildOptions Clone() { throw null; }

        public bool Equals(BuildOptions other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class CacheFile : System.IEquatable<CacheFile>
    {
        public CacheFile(string dgSpecHash) { }

        public string DgSpecHash { get { throw null; } }

        public System.Collections.Generic.IList<string> ExpectedPackageFilePaths { get { throw null; } set { } }

        [System.Obsolete("File existence checks are a function of time not the cache file content.")]
        public bool HasAnyMissingPackageFiles { get { throw null; } set { } }

        public bool IsValid { get { throw null; } }

        public System.Collections.Generic.IList<IAssetsLogMessage> LogMessages { get { throw null; } set { } }

        public string ProjectFilePath { get { throw null; } set { } }

        public bool Success { get { throw null; } set { } }

        public int Version { get { throw null; } set { } }

        public bool Equals(CacheFile other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public static partial class CacheFileFormat
    {
        public static CacheFile Read(System.IO.Stream stream, Common.ILogger log, string path) { throw null; }

        public static void Write(System.IO.Stream stream, CacheFile cacheFile) { }

        public static void Write(string filePath, CacheFile lockFile) { }
    }

    public partial class CentralTransitiveDependencyGroup : System.IEquatable<CentralTransitiveDependencyGroup>
    {
        public CentralTransitiveDependencyGroup(Frameworks.NuGetFramework framework, System.Collections.Generic.IEnumerable<LibraryModel.LibraryDependency> transitiveDependencies) { }

        public string FrameworkName { get { throw null; } }

        public System.Collections.Generic.IEnumerable<LibraryModel.LibraryDependency> TransitiveDependencies { get { throw null; } }

        public bool Equals(CentralTransitiveDependencyGroup other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class DependencyGraphSpec
    {
        public DependencyGraphSpec() { }

        public DependencyGraphSpec(bool isReadOnly) { }

        public System.Collections.Generic.IReadOnlyList<PackageSpec> Projects { get { throw null; } }

        public System.Collections.Generic.IReadOnlyList<string> Restore { get { throw null; } }

        public void AddProject(PackageSpec projectSpec) { }

        public void AddRestore(string projectUniqueName) { }

        public DependencyGraphSpec CreateFromClosure(string projectUniqueName, System.Collections.Generic.IReadOnlyList<PackageSpec> closure) { throw null; }

        public System.Collections.Generic.IReadOnlyList<PackageSpec> GetClosure(string rootUniqueName) { throw null; }

        public static string GetDGSpecFileName(string projectName) { throw null; }

        public string GetHash() { throw null; }

        public System.Collections.Generic.IReadOnlyList<string> GetParents(string rootUniqueName) { throw null; }

        public PackageSpec GetProjectSpec(string projectUniqueName) { throw null; }

        public static DependencyGraphSpec Load(string path) { throw null; }

        public void Save(System.IO.Stream stream) { }

        public void Save(string path) { }

        public static System.Collections.Generic.IReadOnlyList<PackageSpec> SortPackagesByDependencyOrder(System.Collections.Generic.IEnumerable<PackageSpec> packages) { throw null; }

        public static DependencyGraphSpec Union(System.Collections.Generic.IEnumerable<DependencyGraphSpec> dgSpecs) { throw null; }

        public DependencyGraphSpec WithoutRestores() { throw null; }

        public DependencyGraphSpec WithoutTools() { throw null; }

        public DependencyGraphSpec WithPackageSpecs(System.Collections.Generic.IEnumerable<PackageSpec> packageSpecs) { throw null; }

        public DependencyGraphSpec WithProjectClosure(string projectUniqueName) { throw null; }

        public DependencyGraphSpec WithReplacedSpec(PackageSpec project) { throw null; }
    }

    public partial class ExternalProjectReference : System.IEquatable<ExternalProjectReference>, System.IComparable<ExternalProjectReference>
    {
        public ExternalProjectReference(string uniqueName, PackageSpec packageSpec, string msbuildProjectPath, System.Collections.Generic.IEnumerable<string> projectReferences) { }

        public ExternalProjectReference(string uniqueName, string packageSpecProjectName, string packageSpecPath, string msbuildProjectPath, System.Collections.Generic.IEnumerable<string> projectReferences) { }

        public System.Collections.Generic.IReadOnlyList<string> ExternalProjectReferences { get { throw null; } }

        public string MSBuildProjectPath { get { throw null; } }

        public PackageSpec PackageSpec { get { throw null; } }

        public string PackageSpecProjectName { get { throw null; } }

        public string ProjectJsonPath { get { throw null; } }

        public string ProjectName { get { throw null; } }

        public string UniqueName { get { throw null; } }

        public int CompareTo(ExternalProjectReference other) { throw null; }

        public bool Equals(ExternalProjectReference other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class FileFormatException : System.Exception
    {
        public FileFormatException(string message, System.Exception innerException) { }

        public FileFormatException(string message) { }

        public int Column { get { throw null; } }

        public int Line { get { throw null; } }

        public string Path { get { throw null; } }

        public static FileFormatException Create(System.Exception exception, Newtonsoft.Json.Linq.JToken value, string path) { throw null; }

        public static FileFormatException Create(string message, Newtonsoft.Json.Linq.JToken value, string path) { throw null; }
    }

    public sealed partial class HashObjectWriter : RuntimeModel.IObjectWriter, System.IDisposable
    {
        public HashObjectWriter(Packaging.IHashFunction hashFunc) { }

        public void Dispose() { }

        public string GetHash() { throw null; }

        public void WriteArrayEnd() { }

        public void WriteArrayStart(string name) { }

        public void WriteNameArray(string name, System.Collections.Generic.IEnumerable<string> values) { }

        public void WriteNameValue(string name, bool value) { }

        public void WriteNameValue(string name, int value) { }

        public void WriteNameValue(string name, string value) { }

        public void WriteNonEmptyNameArray(string name, System.Collections.Generic.IEnumerable<string> values) { }

        public void WriteObjectEnd() { }

        public void WriteObjectStart() { }

        public void WriteObjectStart(string name) { }
    }

    public partial interface IAssetsLogMessage
    {
        Common.NuGetLogCode Code { get; }

        int EndColumnNumber { get; }

        int EndLineNumber { get; }

        string FilePath { get; }

        Common.LogLevel Level { get; }

        string LibraryId { get; }

        string Message { get; }

        string ProjectPath { get; }

        int StartColumnNumber { get; }

        int StartLineNumber { get; }

        System.Collections.Generic.IReadOnlyList<string> TargetGraphs { get; }

        Common.WarningLevel WarningLevel { get; }
    }

    public partial interface IExternalProjectReferenceProvider
    {
        System.Collections.Generic.IReadOnlyList<ExternalProjectReference> GetEntryPoints();
        System.Collections.Generic.IReadOnlyList<ExternalProjectReference> GetReferences(string entryPointPath);
    }

    public partial class IncludeExcludeFiles : System.IEquatable<IncludeExcludeFiles>
    {
        public System.Collections.Generic.IReadOnlyList<string> Exclude { get { throw null; } set { } }

        public System.Collections.Generic.IReadOnlyList<string> ExcludeFiles { get { throw null; } set { } }

        public System.Collections.Generic.IReadOnlyList<string> Include { get { throw null; } set { } }

        public System.Collections.Generic.IReadOnlyList<string> IncludeFiles { get { throw null; } set { } }

        public IncludeExcludeFiles Clone() { throw null; }

        public bool Equals(IncludeExcludeFiles other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public bool HandleIncludeExcludeFiles(Newtonsoft.Json.Linq.JObject jsonObject) { throw null; }
    }

    public static partial class JsonPackageSpecReader
    {
        public static readonly string Files;
        public static readonly string HideWarningsAndErrors;
        public static readonly string PackageType;
        public static readonly string PackOptions;
        public static readonly string RestoreOptions;
        public static readonly string RestoreSettings;
        [System.Obsolete("This method is obsolete and will be removed in a future release.")]
        public static PackageSpec GetPackageSpec(Newtonsoft.Json.Linq.JObject rawPackageSpec, string name, string packageSpecPath, string snapshotValue) { throw null; }

        [System.Obsolete("This method is obsolete and will be removed in a future release.")]
        public static PackageSpec GetPackageSpec(Newtonsoft.Json.Linq.JObject json) { throw null; }

        public static PackageSpec GetPackageSpec(System.IO.Stream stream, string name, string packageSpecPath, string snapshotValue) { throw null; }

        public static PackageSpec GetPackageSpec(string json, string name, string packageSpecPath) { throw null; }

        public static PackageSpec GetPackageSpec(string name, string packageSpecPath) { throw null; }
    }

    public static partial class JTokenExtensions
    {
        public static T GetValue<T>(this Newtonsoft.Json.Linq.JToken token, string name) { throw null; }

        public static T[] ValueAsArray<T>(this Newtonsoft.Json.Linq.JToken jToken, string name) { throw null; }

        public static T[] ValueAsArray<T>(this Newtonsoft.Json.Linq.JToken jToken) { throw null; }
    }

    public partial class LockFile : System.IEquatable<LockFile>
    {
        public static readonly char DirectorySeparatorChar;
        public static readonly Frameworks.NuGetFramework ToolFramework;
        public System.Collections.Generic.IList<CentralTransitiveDependencyGroup> CentralTransitiveDependencyGroups { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileLibrary> Libraries { get { throw null; } set { } }

        public System.Collections.Generic.IList<IAssetsLogMessage> LogMessages { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> PackageFolders { get { throw null; } set { } }

        public PackageSpec PackageSpec { get { throw null; } set { } }

        public string Path { get { throw null; } set { } }

        public System.Collections.Generic.IList<ProjectFileDependencyGroup> ProjectFileDependencyGroups { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileTarget> Targets { get { throw null; } set { } }

        public int Version { get { throw null; } set { } }

        public bool Equals(LockFile other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public LockFileLibrary GetLibrary(string name, Versioning.NuGetVersion version) { throw null; }

        public LockFileTarget GetTarget(Frameworks.NuGetFramework framework, string runtimeIdentifier) { throw null; }

        public LockFileTarget GetTarget(string frameworkAlias, string runtimeIdentifier) { throw null; }

        public bool IsValidForPackageSpec(PackageSpec spec, int requestLockFileVersion) { throw null; }

        public bool IsValidForPackageSpec(PackageSpec spec) { throw null; }
    }

    public partial class LockFileContentFile : LockFileItem
    {
        public static readonly string BuildActionProperty;
        public static readonly string CodeLanguageProperty;
        public static readonly string CopyToOutputProperty;
        public static readonly string OutputPathProperty;
        public static readonly string PPOutputPathProperty;
        public LockFileContentFile(string path) : base(default!) { }

        public BuildAction BuildAction { get { throw null; } set { } }

        public string CodeLanguage { get { throw null; } set { } }

        public bool CopyToOutput { get { throw null; } set { } }

        public string OutputPath { get { throw null; } set { } }

        public string PPOutputPath { get { throw null; } set { } }
    }

    public partial class LockFileDependency : System.IEquatable<LockFileDependency>
    {
        public string ContentHash { get { throw null; } set { } }

        public System.Collections.Generic.IList<Packaging.Core.PackageDependency> Dependencies { get { throw null; } set { } }

        public string Id { get { throw null; } set { } }

        public Versioning.VersionRange RequestedVersion { get { throw null; } set { } }

        public Versioning.NuGetVersion ResolvedVersion { get { throw null; } set { } }

        public PackageDependencyType Type { get { throw null; } set { } }

        public bool Equals(LockFileDependency other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class LockFileDependencyIdVersionComparer : System.Collections.Generic.IEqualityComparer<LockFileDependency>
    {
        public static LockFileDependencyIdVersionComparer Default { get { throw null; } }

        public bool Equals(LockFileDependency x, LockFileDependency y) { throw null; }

        public int GetHashCode(LockFileDependency obj) { throw null; }
    }

    [System.Obsolete("This is an unused class and will be removed in a future version.")]
    public partial class LockFileDependencyProvider : DependencyResolver.IDependencyProvider
    {
        public LockFileDependencyProvider(LockFile lockFile) { }

        public LibraryModel.Library GetLibrary(LibraryModel.LibraryRange libraryRange, Frameworks.NuGetFramework targetFramework) { throw null; }

        public bool SupportsType(LibraryModel.LibraryDependencyTarget libraryType) { throw null; }
    }

    public static partial class LockFileExtensions
    {
        public static System.Collections.Generic.IEnumerable<LockFileTarget> GetTargetGraphs(this IAssetsLogMessage message, LockFile assetsFile) { throw null; }

        public static System.Collections.Generic.IEnumerable<LockFileTargetLibrary> GetTargetLibraries(this IAssetsLogMessage message, LockFile assetsFile) { throw null; }

        public static LockFileTargetLibrary GetTargetLibrary(this LockFileTarget target, string libraryId) { throw null; }
    }

    public partial class LockFileFormat
    {
        public static readonly string AssetsFileName;
        public static readonly string LockFileName;
        public static readonly int Version;
        public LockFile Parse(string lockFileContent, Common.ILogger log, string path) { throw null; }

        public LockFile Parse(string lockFileContent, string path) { throw null; }

        public LockFile Read(System.IO.Stream stream, Common.ILogger log, string path) { throw null; }

        public LockFile Read(System.IO.Stream stream, string path) { throw null; }

        [System.Obsolete("This method is deprecated. Use Read(Stream, ILogger, string) instead.")]
        public LockFile Read(System.IO.TextReader reader, Common.ILogger log, string path) { throw null; }

        [System.Obsolete("This method is deprecated. Use Read(Stream, string) instead.")]
        public LockFile Read(System.IO.TextReader reader, string path) { throw null; }

        public LockFile Read(string filePath, Common.ILogger log) { throw null; }

        public LockFile Read(string filePath) { throw null; }

        public string Render(LockFile lockFile) { throw null; }

        public void Write(System.IO.Stream stream, LockFile lockFile) { }

        public void Write(System.IO.TextWriter textWriter, LockFile lockFile) { }

        public void Write(string filePath, LockFile lockFile) { }
    }

    public partial class LockFileItem : System.IEquatable<LockFileItem>
    {
        public static readonly string AliasesProperty;
        public LockFileItem(string path) { }

        public string Path { get { throw null; } }

        public System.Collections.Generic.IDictionary<string, string> Properties { get { throw null; } }

        public bool Equals(LockFileItem other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        protected string GetProperty(string name) { throw null; }

        public static implicit operator LockFileItem(string path) { throw null; }

        protected void SetProperty(string name, string value) { }

        public override string ToString() { throw null; }
    }

    public partial class LockFileLibrary : System.IEquatable<LockFileLibrary>
    {
        public System.Collections.Generic.IList<string> Files { get { throw null; } set { } }

        public bool HasTools { get { throw null; } set { } }

        public bool IsServiceable { get { throw null; } set { } }

        public string MSBuildProject { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public string Path { get { throw null; } set { } }

        public string Sha512 { get { throw null; } set { } }

        public string Type { get { throw null; } set { } }

        public Versioning.NuGetVersion Version { get { throw null; } set { } }

        public LockFileLibrary Clone() { throw null; }

        public bool Equals(LockFileLibrary other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    [System.Flags]
    public enum LockFileReadFlags
    {
        Libraries = 1,
        Targets = 2,
        ProjectFileDependencyGroups = 4,
        PackageFolders = 8,
        PackageSpec = 16,
        CentralTransitiveDependencyGroups = 32,
        LogMessages = 64,
        All = 127
    }

    public partial class LockFileRuntimeTarget : LockFileItem
    {
        public static readonly string AssetTypeProperty;
        public static readonly string RidProperty;
        public LockFileRuntimeTarget(string path, string runtime, string assetType) : base(default!) { }

        public LockFileRuntimeTarget(string path) : base(default!) { }

        public string AssetType { get { throw null; } set { } }

        public string Runtime { get { throw null; } set { } }
    }

    public partial class LockFileTarget : System.IEquatable<LockFileTarget>
    {
        public System.Collections.Generic.IList<LockFileTargetLibrary> Libraries { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public string RuntimeIdentifier { get { throw null; } set { } }

        public Frameworks.NuGetFramework TargetFramework { get { throw null; } set { } }

        public bool Equals(LockFileTarget other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class LockFileTargetLibrary : System.IEquatable<LockFileTargetLibrary>
    {
        public System.Collections.Generic.IList<LockFileItem> Build { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> BuildMultiTargeting { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> CompileTimeAssemblies { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileContentFile> ContentFiles { get { throw null; } set { } }

        public System.Collections.Generic.IList<Packaging.Core.PackageDependency> Dependencies { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> EmbedAssemblies { get { throw null; } set { } }

        public string? Framework { get { throw null; } set { } }

        public System.Collections.Generic.IList<string> FrameworkAssemblies { get { throw null; } set { } }

        public System.Collections.Generic.IList<string> FrameworkReferences { get { throw null; } set { } }

        public string? Name { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> NativeLibraries { get { throw null; } set { } }

        public System.Collections.Generic.IList<Packaging.Core.PackageType> PackageType { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> ResourceAssemblies { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> RuntimeAssemblies { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileRuntimeTarget> RuntimeTargets { get { throw null; } set { } }

        public System.Collections.Generic.IList<LockFileItem> ToolsAssemblies { get { throw null; } set { } }

        public string? Type { get { throw null; } set { } }

        public Versioning.NuGetVersion? Version { get { throw null; } set { } }

        public bool Equals(LockFileTargetLibrary? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public void Freeze() { }

        public override int GetHashCode() { throw null; }
    }

    public static partial class LockFileUtilities
    {
        public static LockFile GetLockFile(string lockFilePath, Common.ILogger logger, LockFileReadFlags flags) { throw null; }

        public static LockFile GetLockFile(string lockFilePath, Common.ILogger logger) { throw null; }
    }

    public partial class LockFileValidationResult
    {
        public LockFileValidationResult(bool isValid, System.Collections.Generic.IReadOnlyList<string> invalidReasons) { }

        public System.Collections.Generic.IReadOnlyList<string> InvalidReasons { get { throw null; } }

        public bool IsValid { get { throw null; } }
    }

    public enum PackageDependencyType
    {
        Direct = 0,
        Transitive = 1,
        Project = 2,
        CentralTransitive = 3
    }

    public partial class PackagesConfigProjectRestoreMetadata : ProjectRestoreMetadata
    {
        public string PackagesConfigPath { get { throw null; } set { } }

        public string RepositoryPath { get { throw null; } set { } }

        public override ProjectRestoreMetadata Clone() { throw null; }

        public bool Equals(PackagesConfigProjectRestoreMetadata obj) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class PackagesLockFile : System.IEquatable<PackagesLockFile>
    {
        public PackagesLockFile() { }

        public PackagesLockFile(int version) { }

        public string Path { get { throw null; } set { } }

        public System.Collections.Generic.IList<PackagesLockFileTarget> Targets { get { throw null; } set { } }

        public int Version { get { throw null; } set { } }

        public bool Equals(PackagesLockFile other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public static partial class PackagesLockFileFormat
    {
        public static readonly string LockFileName;
        public static readonly int PackagesLockFileVersion;
        public static readonly int Version;
        public static PackagesLockFile Parse(string lockFileContent, Common.ILogger log, string path) { throw null; }

        public static PackagesLockFile Parse(string lockFileContent, string path) { throw null; }

        public static PackagesLockFile Read(System.IO.Stream stream, Common.ILogger log, string path) { throw null; }

        public static PackagesLockFile Read(System.IO.TextReader reader, Common.ILogger log, string path) { throw null; }

        public static PackagesLockFile Read(string filePath, Common.ILogger log) { throw null; }

        public static PackagesLockFile Read(string filePath) { throw null; }

        public static string Render(PackagesLockFile lockFile) { throw null; }

        public static void Write(System.IO.Stream stream, PackagesLockFile lockFile) { }

        public static void Write(System.IO.TextWriter textWriter, PackagesLockFile lockFile) { }

        public static void Write(string filePath, PackagesLockFile lockFile) { }
    }

    public partial class PackagesLockFileTarget : System.IEquatable<PackagesLockFileTarget>
    {
        public System.Collections.Generic.IList<LockFileDependency> Dependencies { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public string RuntimeIdentifier { get { throw null; } set { } }

        public Frameworks.NuGetFramework TargetFramework { get { throw null; } set { } }

        public bool Equals(PackagesLockFileTarget other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public static partial class PackagesLockFileUtilities
    {
        public static string GetNuGetLockFilePath(PackageSpec project) { throw null; }

        public static string GetNuGetLockFilePath(string baseDirectory, string projectName) { throw null; }

        [System.Obsolete("This method is obsolete. Call IsLockFileValid instead.")]
        public static bool IsLockFileStillValid(DependencyGraphSpec dgSpec, PackagesLockFile nuGetLockFile) { throw null; }

        public static LockFileValidityWithMatchedResults IsLockFileStillValid(PackagesLockFile expected, PackagesLockFile actual) { throw null; }

        public static LockFileValidationResult IsLockFileValid(DependencyGraphSpec dgSpec, PackagesLockFile nuGetLockFile) { throw null; }

        public static bool IsNuGetLockFileEnabled(PackageSpec project) { throw null; }

        public partial class LockFileValidityWithMatchedResults
        {
            public static readonly LockFileValidityWithMatchedResults Invalid;
            public LockFileValidityWithMatchedResults(bool isValid, System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<LockFileDependency, LockFileDependency>> matchedDependencies) { }

            public bool IsValid { get { throw null; } }

            public System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<LockFileDependency, LockFileDependency>> MatchedDependencies { get { throw null; } }
        }
    }

    public partial class PackageSpec
    {
        public static readonly Versioning.NuGetVersion DefaultVersion;
        public static readonly string PackageSpecFileName;
        public PackageSpec() { }

        public PackageSpec(System.Collections.Generic.IList<TargetFrameworkInformation> frameworks) { }

        [System.Obsolete]
        public string[] Authors { get { throw null; } set { } }

        public string BaseDirectory { get { throw null; } }

        [System.Obsolete]
        public BuildOptions BuildOptions { get { throw null; } set { } }

        [System.Obsolete]
        public System.Collections.Generic.IList<string> ContentFiles { get { throw null; } set { } }

        [System.Obsolete]
        public string Copyright { get { throw null; } set { } }

        public System.Collections.Generic.IList<LibraryModel.LibraryDependency> Dependencies { get { throw null; } set { } }

        [System.Obsolete]
        public string Description { get { throw null; } set { } }

        public string FilePath { get { throw null; } set { } }

        [System.Obsolete]
        public bool HasVersionSnapshot { get { throw null; } set { } }

        [System.Obsolete]
        public string IconUrl { get { throw null; } set { } }

        [System.Obsolete]
        public bool IsDefaultVersion { get { throw null; } set { } }

        [System.Obsolete]
        public string Language { get { throw null; } set { } }

        [System.Obsolete]
        public string LicenseUrl { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        [System.Obsolete]
        public string[] Owners { get { throw null; } set { } }

        [System.Obsolete]
        public System.Collections.Generic.IDictionary<string, string> PackInclude { get { throw null; } }

        [System.Obsolete]
        public PackOptions PackOptions { get { throw null; } set { } }

        [System.Obsolete]
        public string ProjectUrl { get { throw null; } set { } }

        [System.Obsolete]
        public string ReleaseNotes { get { throw null; } set { } }

        [System.Obsolete]
        public bool RequireLicenseAcceptance { get { throw null; } set { } }

        public ProjectRestoreMetadata RestoreMetadata { get { throw null; } set { } }

        public ProjectRestoreSettings RestoreSettings { get { throw null; } set { } }

        public RuntimeModel.RuntimeGraph RuntimeGraph { get { throw null; } set { } }

        [System.Obsolete]
        public System.Collections.Generic.IDictionary<string, System.Collections.Generic.IEnumerable<string>> Scripts { get { throw null; } }

        [System.Obsolete]
        public string Summary { get { throw null; } set { } }

        [System.Obsolete]
        public string[] Tags { get { throw null; } set { } }

        public System.Collections.Generic.IList<TargetFrameworkInformation> TargetFrameworks { get { throw null; } }

        public string Title { get { throw null; } set { } }

        public Versioning.NuGetVersion Version { get { throw null; } set { } }

        public PackageSpec Clone() { throw null; }

        public bool Equals(PackageSpec other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public static partial class PackageSpecExtensions
    {
        public static ProjectRestoreMetadataFrameworkInfo GetRestoreMetadataFramework(this PackageSpec project, Frameworks.NuGetFramework targetFramework) { throw null; }

        public static TargetFrameworkInformation GetTargetFramework(this PackageSpec project, Frameworks.NuGetFramework targetFramework) { throw null; }
    }

    public static partial class PackageSpecOperations
    {
        public static void AddOrUpdateDependency(PackageSpec spec, Packaging.Core.PackageDependency dependency, System.Collections.Generic.IEnumerable<Frameworks.NuGetFramework> frameworksToAdd) { }

        public static void AddOrUpdateDependency(PackageSpec spec, Packaging.Core.PackageDependency dependency) { }

        public static void AddOrUpdateDependency(PackageSpec spec, Packaging.Core.PackageIdentity identity, System.Collections.Generic.IEnumerable<Frameworks.NuGetFramework> frameworksToAdd) { }

        public static void AddOrUpdateDependency(PackageSpec spec, Packaging.Core.PackageIdentity identity) { }

        public static bool HasPackage(PackageSpec spec, string packageId) { throw null; }

        public static void RemoveDependency(PackageSpec spec, string packageId) { }
    }

    public partial class PackageSpecReferenceDependencyProvider : DependencyResolver.IDependencyProvider
    {
        public PackageSpecReferenceDependencyProvider(System.Collections.Generic.IEnumerable<ExternalProjectReference> externalProjects, Common.ILogger logger, bool useLegacyDependencyGraphResolution) { }

        public PackageSpecReferenceDependencyProvider(System.Collections.Generic.IEnumerable<ExternalProjectReference> externalProjects, Common.ILogger logger) { }

        public LibraryModel.Library GetLibrary(LibraryModel.LibraryRange libraryRange, Frameworks.NuGetFramework targetFramework) { throw null; }

        public bool SupportsType(LibraryModel.LibraryDependencyTarget libraryType) { throw null; }
    }

    public static partial class PackageSpecUtility
    {
        public static bool IsSnapshotVersion(string version) { throw null; }

        public static Versioning.NuGetVersion SpecifySnapshot(string version, string snapshotValue) { throw null; }
    }

    public sealed partial class PackageSpecWriter
    {
        public static void Write(PackageSpec packageSpec, RuntimeModel.IObjectWriter writer) { }

        public static void WriteToFile(PackageSpec packageSpec, string filePath) { }
    }

    public partial class PackOptions : System.IEquatable<PackOptions>
    {
        public IncludeExcludeFiles IncludeExcludeFiles { get { throw null; } set { } }

        public System.Collections.Generic.IDictionary<string, IncludeExcludeFiles> Mappings { get { throw null; } set { } }

        public System.Collections.Generic.IReadOnlyList<Packaging.Core.PackageType> PackageType { get { throw null; } set { } }

        public PackOptions Clone() { throw null; }

        public bool Equals(PackOptions other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class ProjectFileDependencyGroup : System.IEquatable<ProjectFileDependencyGroup>
    {
        public ProjectFileDependencyGroup(string frameworkName, System.Collections.Generic.IEnumerable<string> dependencies) { }

        public System.Collections.Generic.IEnumerable<string> Dependencies { get { throw null; } }

        public string FrameworkName { get { throw null; } }

        public bool Equals(ProjectFileDependencyGroup other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class ProjectRestoreMetadata : System.IEquatable<ProjectRestoreMetadata>
    {
        public string CacheFilePath { get { throw null; } set { } }

        public bool CentralPackageFloatingVersionsEnabled { get { throw null; } set { } }

        public bool CentralPackageTransitivePinningEnabled { get { throw null; } set { } }

        public bool CentralPackageVersionOverrideDisabled { get { throw null; } set { } }

        public bool CentralPackageVersionsEnabled { get { throw null; } set { } }

        public System.Collections.Generic.IList<string> ConfigFilePaths { get { throw null; } set { } }

        public bool CrossTargeting { get { throw null; } set { } }

        public System.Collections.Generic.IList<string> FallbackFolders { get { throw null; } set { } }

        public System.Collections.Generic.IList<ProjectRestoreMetadataFile> Files { get { throw null; } set { } }

        public bool LegacyPackagesDirectory { get { throw null; } set { } }

        public System.Collections.Generic.IList<string> OriginalTargetFrameworks { get { throw null; } set { } }

        public string OutputPath { get { throw null; } set { } }

        public string PackagesPath { get { throw null; } set { } }

        public string ProjectJsonPath { get { throw null; } set { } }

        public string ProjectName { get { throw null; } set { } }

        public string ProjectPath { get { throw null; } set { } }

        public ProjectStyle ProjectStyle { get { throw null; } set { } }

        public string ProjectUniqueName { get { throw null; } set { } }

        public WarningProperties ProjectWideWarningProperties { get { throw null; } set { } }

        public RestoreAuditProperties RestoreAuditProperties { get { throw null; } set { } }

        public RestoreLockProperties RestoreLockProperties { get { throw null; } set { } }

        public Versioning.NuGetVersion SdkAnalysisLevel { get { throw null; } set { } }

        public bool SkipContentFileWrite { get { throw null; } set { } }

        public System.Collections.Generic.IList<Configuration.PackageSource> Sources { get { throw null; } set { } }

        public System.Collections.Generic.IList<ProjectRestoreMetadataFrameworkInfo> TargetFrameworks { get { throw null; } set { } }

        public bool UseLegacyDependencyResolver { get { throw null; } set { } }

        public bool UsingMicrosoftNETSdk { get { throw null; } set { } }

        public bool ValidateRuntimeAssets { get { throw null; } set { } }

        public virtual ProjectRestoreMetadata Clone() { throw null; }

        public bool Equals(ProjectRestoreMetadata other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        protected void FillClone(ProjectRestoreMetadata clone) { }

        public override int GetHashCode() { throw null; }
    }

    public partial class ProjectRestoreMetadataFile : System.IEquatable<ProjectRestoreMetadataFile>, System.IComparable<ProjectRestoreMetadataFile>
    {
        public ProjectRestoreMetadataFile(string packagePath, string absolutePath) { }

        public string AbsolutePath { get { throw null; } }

        public string PackagePath { get { throw null; } }

        public ProjectRestoreMetadataFile Clone() { throw null; }

        public int CompareTo(ProjectRestoreMetadataFile other) { throw null; }

        public bool Equals(ProjectRestoreMetadataFile other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class ProjectRestoreMetadataFrameworkInfo : System.IEquatable<ProjectRestoreMetadataFrameworkInfo>
    {
        public ProjectRestoreMetadataFrameworkInfo() { }

        public ProjectRestoreMetadataFrameworkInfo(Frameworks.NuGetFramework frameworkName) { }

        public Frameworks.NuGetFramework FrameworkName { get { throw null; } set { } }

        public System.Collections.Generic.IList<ProjectRestoreReference> ProjectReferences { get { throw null; } set { } }

        public string TargetAlias { get { throw null; } set { } }

        public ProjectRestoreMetadataFrameworkInfo Clone() { throw null; }

        public bool Equals(ProjectRestoreMetadataFrameworkInfo other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class ProjectRestoreReference : System.IEquatable<ProjectRestoreReference>
    {
        public LibraryModel.LibraryIncludeFlags ExcludeAssets { get { throw null; } set { } }

        public LibraryModel.LibraryIncludeFlags IncludeAssets { get { throw null; } set { } }

        public LibraryModel.LibraryIncludeFlags PrivateAssets { get { throw null; } set { } }

        public string ProjectPath { get { throw null; } set { } }

        public string ProjectUniqueName { get { throw null; } set { } }

        public ProjectRestoreReference Clone() { throw null; }

        public bool Equals(ProjectRestoreReference other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class ProjectRestoreSettings
    {
        public bool HideWarningsAndErrors { get { throw null; } set { } }

        public Versioning.NuGetVersion SdkVersion { get { throw null; } set { } }

        public ProjectRestoreSettings Clone() { throw null; }

        public bool Equals(ProjectRestoreSettings other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public enum ProjectStyle : ushort
    {
        Unknown = 0,
        ProjectJson = 1,
        PackageReference = 2,
        DotnetCliTool = 3,
        Standalone = 4,
        PackagesConfig = 5,
        DotnetToolReference = 6
    }

    public partial class RestoreAuditProperties : System.IEquatable<RestoreAuditProperties>
    {
        public string? AuditLevel { get { throw null; } set { } }

        public string? AuditMode { get { throw null; } set { } }

        public string? EnableAudit { get { throw null; } set { } }

        public System.Collections.Generic.HashSet<string>? SuppressedAdvisories { get { throw null; } set { } }

        public bool Equals(RestoreAuditProperties? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(RestoreAuditProperties? x, RestoreAuditProperties? y) { throw null; }

        public static bool operator !=(RestoreAuditProperties? x, RestoreAuditProperties? y) { throw null; }

        public bool TryParseAuditLevel(out Protocol.PackageVulnerabilitySeverity result) { throw null; }

        public bool TryParseEnableAudit(out bool result) { throw null; }
    }

    public partial class RestoreLockProperties : System.IEquatable<RestoreLockProperties>
    {
        public RestoreLockProperties() { }

        public RestoreLockProperties(string restorePackagesWithLockFile, string nuGetLockFilePath, bool restoreLockedMode) { }

        public string NuGetLockFilePath { get { throw null; } }

        public bool RestoreLockedMode { get { throw null; } }

        public string RestorePackagesWithLockFile { get { throw null; } }

        public RestoreLockProperties Clone() { throw null; }

        public bool Equals(RestoreLockProperties other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class TargetFrameworkInformation : System.IEquatable<TargetFrameworkInformation>
    {
        public TargetFrameworkInformation() { }

        public TargetFrameworkInformation(TargetFrameworkInformation cloneFrom) { }

        public bool AssetTargetFallback { get { throw null; } init { } }

        public System.Collections.Generic.IReadOnlyDictionary<string, LibraryModel.CentralPackageVersion> CentralPackageVersions { get { throw null; } init { } }

        public System.Collections.Immutable.ImmutableArray<LibraryModel.LibraryDependency> Dependencies { get { throw null; } init { } }

        public System.Collections.Immutable.ImmutableArray<LibraryModel.DownloadDependency> DownloadDependencies { get { throw null; } init { } }

        public Frameworks.NuGetFramework FrameworkName { get { throw null; } init { } }

        public System.Collections.Generic.IReadOnlyCollection<LibraryModel.FrameworkDependency> FrameworkReferences { get { throw null; } init { } }

        public System.Collections.Immutable.ImmutableArray<Frameworks.NuGetFramework> Imports { get { throw null; } init { } }

        public System.Collections.Generic.IReadOnlyDictionary<string, LibraryModel.PrunePackageReference> PackagesToPrune { get { throw null; } init { } }

        public string RuntimeIdentifierGraphPath { get { throw null; } init { } }

        public string TargetAlias { get { throw null; } init { } }

        public bool Warn { get { throw null; } init { } }

        public bool Equals(TargetFrameworkInformation other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class ToolPathResolver
    {
        public ToolPathResolver(string packagesDirectory, bool isLowercase) { }

        public ToolPathResolver(string packagesDirectory) { }

        public string GetBestToolDirectoryPath(string packageId, Versioning.VersionRange versionRange, Frameworks.NuGetFramework framework) { throw null; }

        public string GetLockFilePath(string packageId, Versioning.NuGetVersion version, Frameworks.NuGetFramework framework) { throw null; }

        public string GetLockFilePath(string toolDirectory) { throw null; }

        public string GetToolDirectoryPath(string packageId, Versioning.NuGetVersion version, Frameworks.NuGetFramework framework) { throw null; }
    }

    public partial class WarningProperties : System.IEquatable<WarningProperties>
    {
        public WarningProperties() { }

        public WarningProperties(System.Collections.Generic.ISet<Common.NuGetLogCode> warningsAsErrors, System.Collections.Generic.ISet<Common.NuGetLogCode> noWarn, bool allWarningsAsErrors, System.Collections.Generic.ISet<Common.NuGetLogCode> warningsNotAsErrors) { }

        [System.Obsolete("Use the constructor with 4 instead.")]
        public WarningProperties(System.Collections.Generic.ISet<Common.NuGetLogCode> warningsAsErrors, System.Collections.Generic.ISet<Common.NuGetLogCode> noWarn, bool allWarningsAsErrors) { }

        public bool AllWarningsAsErrors { get { throw null; } set { } }

        public System.Collections.Generic.ISet<Common.NuGetLogCode> NoWarn { get { throw null; } }

        public System.Collections.Generic.ISet<Common.NuGetLogCode> WarningsAsErrors { get { throw null; } }

        public System.Collections.Generic.ISet<Common.NuGetLogCode> WarningsNotAsErrors { get { throw null; } }

        public WarningProperties Clone() { throw null; }

        public bool Equals(WarningProperties other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, System.Collections.Immutable.ImmutableArray<Common.NuGetLogCode> warningsAsErrors, System.Collections.Immutable.ImmutableArray<Common.NuGetLogCode> noWarn, System.Collections.Immutable.ImmutableArray<Common.NuGetLogCode> warningsNotAsErrors) { throw null; }

        [System.Obsolete]
        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, System.Collections.Immutable.ImmutableArray<Common.NuGetLogCode> warningsAsErrors, System.Collections.Immutable.ImmutableArray<Common.NuGetLogCode> noWarn) { throw null; }

        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, string warningsAsErrors, string noWarn, string warningsNotAsErrors) { throw null; }

        [System.Obsolete]
        public static WarningProperties GetWarningProperties(string treatWarningsAsErrors, string warningsAsErrors, string noWarn) { throw null; }
    }
}

namespace NuGet.ProjectModel.ProjectLockFile
{
    public partial class LockFileDependencyComparerWithoutContentHash : System.Collections.Generic.IEqualityComparer<LockFileDependency>
    {
        public static LockFileDependencyComparerWithoutContentHash Default { get { throw null; } }

        public bool Equals(LockFileDependency x, LockFileDependency y) { throw null; }

        public int GetHashCode(LockFileDependency obj) { throw null; }
    }
}