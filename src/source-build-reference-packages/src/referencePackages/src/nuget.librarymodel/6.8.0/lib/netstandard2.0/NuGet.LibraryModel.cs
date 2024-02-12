// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NuGet.ProjectModel.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NuGet.Commands.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("NuGet's types and interfaces for understanding dependencies.")]
[assembly: System.Reflection.AssemblyFileVersion("6.8.0.131")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.8.0+993ad4ee2350e33c7821d6609e0099971b534a6a.993ad4ee2350e33c7821d6609e0099971b534a6a")]
[assembly: System.Reflection.AssemblyProduct("NuGet")]
[assembly: System.Reflection.AssemblyTitle("NuGet.LibraryModel")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/NuGet/NuGet.Client")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.8.0.131")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace NuGet.LibraryModel
{
    public sealed partial class CentralPackageVersion : System.IEquatable<CentralPackageVersion>
    {
        public CentralPackageVersion(string name, Versioning.VersionRange versionRange) { }

        public string Name { get { throw null; } }

        public Versioning.VersionRange VersionRange { get { throw null; } }

        public bool Equals(CentralPackageVersion other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class CentralPackageVersionNameComparer : System.Collections.Generic.IEqualityComparer<CentralPackageVersion>
    {
        internal CentralPackageVersionNameComparer() { }

        public static CentralPackageVersionNameComparer Default { get { throw null; } }

        public bool Equals(CentralPackageVersion x, CentralPackageVersion y) { throw null; }

        public int GetHashCode(CentralPackageVersion obj) { throw null; }
    }

    public sealed partial class DownloadDependency : System.IEquatable<DownloadDependency>, System.IComparable<DownloadDependency>
    {
        public DownloadDependency(string name, Versioning.VersionRange versionRange) { }

        public string Name { get { throw null; } }

        public Versioning.VersionRange VersionRange { get { throw null; } }

        [System.Obsolete("This type is immutable, so there is no need or point to clone it.")]
        public DownloadDependency Clone() { throw null; }

        public int CompareTo(DownloadDependency other) { throw null; }

        public bool Equals(DownloadDependency other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static implicit operator LibraryRange(DownloadDependency library) { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class FrameworkDependency : System.IEquatable<FrameworkDependency>, System.IComparable<FrameworkDependency>
    {
        public FrameworkDependency(string name, FrameworkDependencyFlags privateAssets) { }

        public string Name { get { throw null; } }

        public FrameworkDependencyFlags PrivateAssets { get { throw null; } }

        public int CompareTo(FrameworkDependency other) { throw null; }

        public bool Equals(FrameworkDependency other) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    [System.Flags]
    public enum FrameworkDependencyFlags : ushort
    {
        None = 0,
        All = ushort.MaxValue
    }

    public static partial class FrameworkDependencyFlagsUtils
    {
        public static readonly FrameworkDependencyFlags Default;
        public static FrameworkDependencyFlags GetFlags(System.Collections.Generic.IEnumerable<string> values) { throw null; }

        public static FrameworkDependencyFlags GetFlags(string flags) { throw null; }

        public static string GetFlagString(FrameworkDependencyFlags flags) { throw null; }
    }

    public static partial class KnownLibraryProperties
    {
        public static readonly string AssemblyPath;
        public static readonly string FrameworkAssemblies;
        public static readonly string FrameworkReferences;
        public static readonly string LockFileLibrary;
        public static readonly string LockFileTargetLibrary;
        public static readonly string MSBuildProjectPath;
        public static readonly string PackageSpec;
        public static readonly string ProjectFrameworks;
        public static readonly string ProjectRestoreMetadataFiles;
        public static readonly string ProjectStyle;
        public static readonly string TargetFrameworkInformation;
    }

    public partial class Library
    {
        public static readonly System.Collections.Generic.IEqualityComparer<Library> IdentityComparer;
        public System.Collections.Generic.IEnumerable<LibraryDependency> Dependencies { get { throw null; } set { } }

        public LibraryIdentity Identity { get { throw null; } set { } }

        public object this[string key] { get { throw null; } set { } }

        public System.Collections.Generic.IDictionary<string, object> Items { get { throw null; } set { } }

        public LibraryRange LibraryRange { get { throw null; } set { } }

        public string Path { get { throw null; } set { } }

        public bool Resolved { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public partial class LibraryDependency : System.IEquatable<LibraryDependency>
    {
        public string Aliases { get { throw null; } set { } }

        public bool AutoReferenced { get { throw null; } set { } }

        public bool GeneratePathProperty { get { throw null; } set { } }

        public LibraryIncludeFlags IncludeType { get { throw null; } set { } }

        public LibraryRange LibraryRange { get { throw null; } set { } }

        public string Name { get { throw null; } }

        public System.Collections.Generic.IList<Common.NuGetLogCode> NoWarn { get { throw null; } set { } }

        public LibraryDependencyReferenceType ReferenceType { get { throw null; } set { } }

        public LibraryIncludeFlags SuppressParent { get { throw null; } set { } }

        public bool VersionCentrallyManaged { get { throw null; } set { } }

        public Versioning.VersionRange VersionOverride { get { throw null; } set { } }

        public static void ApplyCentralVersionInformation(System.Collections.Generic.IList<LibraryDependency> packageReferences, System.Collections.Generic.IDictionary<string, CentralPackageVersion> centralPackageVersions) { }

        public LibraryDependency Clone() { throw null; }

        public bool Equals(LibraryDependency other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class LibraryDependencyInfo
    {
        public LibraryDependencyInfo(LibraryIdentity library, bool resolved, Frameworks.NuGetFramework framework, System.Collections.Generic.IEnumerable<LibraryDependency> dependencies) { }

        public System.Collections.Generic.IEnumerable<LibraryDependency> Dependencies { get { throw null; } }

        public Frameworks.NuGetFramework Framework { get { throw null; } }

        public LibraryIdentity Library { get { throw null; } }

        public bool Resolved { get { throw null; } }

        public static LibraryDependencyInfo Create(LibraryIdentity library, Frameworks.NuGetFramework framework, System.Collections.Generic.IEnumerable<LibraryDependency> dependencies) { throw null; }

        public static LibraryDependencyInfo CreateUnresolved(LibraryIdentity library, Frameworks.NuGetFramework framework) { throw null; }
    }

    public enum LibraryDependencyReferenceType
    {
        None = 0,
        Transitive = 1,
        Direct = 2
    }

    [System.Flags]
    public enum LibraryDependencyTarget : ushort
    {
        None = 0,
        Package = 1,
        Project = 2,
        ExternalProject = 4,
        PackageProjectExternal = 7,
        Assembly = 8,
        Reference = 16,
        WinMD = 32,
        All = 63
    }

    public static partial class LibraryDependencyTargetUtils
    {
        public static string AsString(this LibraryDependencyTarget includeFlags) { throw null; }

        public static string GetFlagString(LibraryDependencyTarget flags) { throw null; }

        public static LibraryDependencyTarget Parse(string flag) { throw null; }
    }

    public static partial class LibraryExtensions
    {
        public static T GetItem<T>(this Library library, string key) { throw null; }

        public static T GetRequiredItem<T>(this Library library, string key) { throw null; }

        public static bool IsEclipsedBy(this LibraryRange library, LibraryRange other) { throw null; }
    }

    public partial class LibraryIdentity : System.IEquatable<LibraryIdentity>, System.IComparable<LibraryIdentity>
    {
        public LibraryIdentity() { }

        public LibraryIdentity(string name, Versioning.NuGetVersion version, LibraryType type) { }

        public string Name { get { throw null; } set { } }

        public LibraryType Type { get { throw null; } set { } }

        public Versioning.NuGetVersion Version { get { throw null; } set { } }

        public int CompareTo(LibraryIdentity other) { throw null; }

        public bool Equals(LibraryIdentity other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(LibraryIdentity left, LibraryIdentity right) { throw null; }

        public static implicit operator LibraryRange(LibraryIdentity library) { throw null; }

        public static bool operator !=(LibraryIdentity left, LibraryIdentity right) { throw null; }

        public override string ToString() { throw null; }
    }

    [System.Flags]
    public enum LibraryIncludeFlags : ushort
    {
        None = 0,
        Runtime = 1,
        Compile = 2,
        Build = 4,
        Native = 8,
        ContentFiles = 16,
        Analyzers = 32,
        BuildTransitive = 64,
        All = 127
    }

    public static partial class LibraryIncludeFlagUtils
    {
        public static readonly LibraryIncludeFlags DefaultSuppressParent;
        public static readonly LibraryIncludeFlags NoContent;
        public static string AsString(this LibraryIncludeFlags includeFlags) { throw null; }

        public static LibraryIncludeFlags GetFlags(System.Collections.Generic.IEnumerable<string> flags) { throw null; }

        public static LibraryIncludeFlags GetFlags(string flags, LibraryIncludeFlags defaultFlags) { throw null; }

        public static string GetFlagString(LibraryIncludeFlags flags) { throw null; }
    }

    public partial class LibraryRange : System.IEquatable<LibraryRange>
    {
        public LibraryRange() { }

        public LibraryRange(string name, LibraryDependencyTarget typeConstraint) { }

        public LibraryRange(string name, Versioning.VersionRange versionRange, LibraryDependencyTarget typeConstraint) { }

        public string Name { get { throw null; } set { } }

        public LibraryDependencyTarget TypeConstraint { get { throw null; } set { } }

        public Versioning.VersionRange VersionRange { get { throw null; } set { } }

        public bool Equals(LibraryRange other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(LibraryRange left, LibraryRange right) { throw null; }

        public static bool operator !=(LibraryRange left, LibraryRange right) { throw null; }

        public string ToLockFileDependencyGroupString() { throw null; }

        public override string ToString() { throw null; }

        public bool TypeConstraintAllows(LibraryDependencyTarget flag) { throw null; }

        public bool TypeConstraintAllowsAnyOf(LibraryDependencyTarget flag) { throw null; }
    }

    public partial struct LibraryType : System.IEquatable<LibraryType>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public static readonly LibraryType Assembly;
        public static readonly LibraryType ExternalProject;
        public static readonly LibraryType Package;
        public static readonly LibraryType Project;
        public static readonly LibraryType Reference;
        public static readonly LibraryType Unresolved;
        public static readonly LibraryType WinMD;
        public bool IsKnown { get { throw null; } }

        public string Value { get { throw null; } }

        public bool Equals(LibraryType other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(LibraryType left, LibraryType right) { throw null; }

        public static implicit operator string(LibraryType libraryType) { throw null; }

        public static bool operator !=(LibraryType left, LibraryType right) { throw null; }

        public static LibraryType Parse(string value) { throw null; }

        public override string ToString() { throw null; }
    }
}