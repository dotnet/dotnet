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
[assembly: System.Reflection.AssemblyDescription("NuGet's understanding of target frameworks.")]
[assembly: System.Reflection.AssemblyFileVersion("6.13.1.3")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.13.1+3fd0e588e53525f0cd037d7b91174c0ca78ac65c.3fd0e588e53525f0cd037d7b91174c0ca78ac65c")]
[assembly: System.Reflection.AssemblyProduct("NuGet")]
[assembly: System.Reflection.AssemblyTitle("NuGet.Frameworks")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/NuGet/NuGet.Client")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.13.1.3")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace NuGet.Frameworks
{
    public partial class AssetTargetFallbackFramework : NuGetFramework, System.IEquatable<AssetTargetFallbackFramework>
    {
        public AssetTargetFallbackFramework(NuGetFramework framework, System.Collections.Generic.IReadOnlyList<NuGetFramework> fallbackFrameworks) : base(default(NuGetFramework)!) { }

        public System.Collections.Generic.IReadOnlyList<NuGetFramework> Fallback { get { throw null; } }

        public NuGetFramework RootFramework { get { throw null; } }

        public FallbackFramework AsFallbackFramework() { throw null; }

        public bool Equals(AssetTargetFallbackFramework? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public sealed partial class CompatibilityListProvider : IFrameworkCompatibilityListProvider
    {
        public CompatibilityListProvider(IFrameworkNameProvider nameProvider, IFrameworkCompatibilityProvider compatibilityProvider) { }

        public static IFrameworkCompatibilityListProvider Default { get { throw null; } }

        public System.Collections.Generic.IEnumerable<NuGetFramework> GetFrameworksSupporting(NuGetFramework target) { throw null; }
    }

    public partial class CompatibilityMappingComparer : System.Collections.Generic.IEqualityComparer<OneWayCompatibilityMappingEntry>
    {
        public static CompatibilityMappingComparer Instance { get { throw null; } }

        public bool Equals(OneWayCompatibilityMappingEntry? x, OneWayCompatibilityMappingEntry? y) { throw null; }

        public int GetHashCode(OneWayCompatibilityMappingEntry obj) { throw null; }
    }

    public partial class CompatibilityProvider : IFrameworkCompatibilityProvider
    {
        public CompatibilityProvider(IFrameworkNameProvider mappings) { }

        public bool IsCompatible(NuGetFramework target, NuGetFramework candidate) { throw null; }
    }

    public partial class CompatibilityTable
    {
        public CompatibilityTable(System.Collections.Generic.IEnumerable<NuGetFramework> frameworks, IFrameworkNameProvider mappings, IFrameworkCompatibilityProvider compat) { }

        public CompatibilityTable(System.Collections.Generic.IEnumerable<NuGetFramework> frameworks) { }

        public System.Collections.Generic.IEnumerable<NuGetFramework> GetNearest(NuGetFramework framework) { throw null; }

        public bool HasFramework(NuGetFramework framework) { throw null; }

        public bool TryGetCompatible(NuGetFramework framework, out System.Collections.Generic.IEnumerable<NuGetFramework>? compatible) { throw null; }
    }

    public sealed partial class DefaultCompatibilityProvider : CompatibilityProvider
    {
        public DefaultCompatibilityProvider() : base(default!) { }

        public static IFrameworkCompatibilityProvider Instance { get { throw null; } }
    }

    public sealed partial class DefaultFrameworkMappings : IFrameworkMappings
    {
        public System.Collections.Generic.IEnumerable<OneWayCompatibilityMappingEntry> CompatibilityMappings { get { throw null; } }

        public System.Collections.Generic.IEnumerable<string> EquivalentFrameworkPrecedence { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<NuGetFramework, NuGetFramework>> EquivalentFrameworks { get { throw null; } }

        public System.Collections.Generic.IEnumerable<FrameworkSpecificMapping> EquivalentProfiles { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<NuGetFramework, NuGetFramework>> FullNameReplacements { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> IdentifierShortNames { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> IdentifierSynonyms { get { throw null; } }

        public static IFrameworkMappings Instance { get { throw null; } }

        public System.Collections.Generic.IEnumerable<string> NonPackageBasedFrameworkPrecedence { get { throw null; } }

        public System.Collections.Generic.IEnumerable<string> PackageBasedFrameworkPrecedence { get { throw null; } }

        public System.Collections.Generic.IEnumerable<FrameworkSpecificMapping> ProfileShortNames { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<NuGetFramework, NuGetFramework>> ShortNameReplacements { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> SubSetFrameworks { get { throw null; } }
    }

    public sealed partial class DefaultFrameworkNameProvider : FrameworkNameProvider
    {
        public DefaultFrameworkNameProvider() : base(default, default) { }

        public static IFrameworkNameProvider Instance { get { throw null; } }
    }

    public partial class DefaultPortableFrameworkMappings : IPortableFrameworkMappings
    {
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, FrameworkRange>> CompatibilityMappings { get { throw null; } }

        public static IPortableFrameworkMappings Instance { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, NuGetFramework[]>> ProfileFrameworks { get { throw null; } }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, NuGetFramework[]>> ProfileOptionalFrameworks { get { throw null; } }
    }

    public partial class DualCompatibilityFramework : NuGetFramework
    {
        public DualCompatibilityFramework(NuGetFramework framework, NuGetFramework secondaryFramework) : base(default(NuGetFramework)!) { }

        public NuGetFramework RootFramework { get { throw null; } }

        public NuGetFramework SecondaryFramework { get { throw null; } }

        public FallbackFramework AsFallbackFramework() { throw null; }

        public bool Equals(DualCompatibilityFramework? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class FallbackFramework : NuGetFramework, System.IEquatable<FallbackFramework>
    {
        public FallbackFramework(NuGetFramework framework, System.Collections.Generic.IReadOnlyList<NuGetFramework> fallbackFrameworks) : base(default(NuGetFramework)!) { }

        public System.Collections.Generic.IReadOnlyList<NuGetFramework> Fallback { get { throw null; } }

        public bool Equals(FallbackFramework? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public static partial class FrameworkConstants
    {
        public static readonly FrameworkRange DotNetAll;
        public static readonly System.Version EmptyVersion;
        public static readonly System.Version MaxVersion;
        public static readonly System.Version Version10;
        public static readonly System.Version Version5;
        public static readonly System.Version Version6;
        public static readonly System.Version Version7;
        public static readonly System.Version Version8;
        public static partial class CommonFrameworks
        {
            public static readonly NuGetFramework AspNet;
            public static readonly NuGetFramework AspNet50;
            public static readonly NuGetFramework AspNetCore;
            public static readonly NuGetFramework AspNetCore50;
            public static readonly NuGetFramework Dnx;
            public static readonly NuGetFramework Dnx45;
            public static readonly NuGetFramework Dnx451;
            public static readonly NuGetFramework Dnx452;
            public static readonly NuGetFramework DnxCore;
            public static readonly NuGetFramework DnxCore50;
            public static readonly NuGetFramework DotNet;
            public static readonly NuGetFramework DotNet50;
            public static readonly NuGetFramework DotNet51;
            public static readonly NuGetFramework DotNet52;
            public static readonly NuGetFramework DotNet53;
            public static readonly NuGetFramework DotNet54;
            public static readonly NuGetFramework DotNet55;
            public static readonly NuGetFramework DotNet56;
            public static readonly NuGetFramework Native;
            public static readonly NuGetFramework Net11;
            public static readonly NuGetFramework Net2;
            public static readonly NuGetFramework Net35;
            public static readonly NuGetFramework Net4;
            public static readonly NuGetFramework Net403;
            public static readonly NuGetFramework Net45;
            public static readonly NuGetFramework Net451;
            public static readonly NuGetFramework Net452;
            public static readonly NuGetFramework Net46;
            public static readonly NuGetFramework Net461;
            public static readonly NuGetFramework Net462;
            public static readonly NuGetFramework Net463;
            public static readonly NuGetFramework Net47;
            public static readonly NuGetFramework Net471;
            public static readonly NuGetFramework Net472;
            public static readonly NuGetFramework Net50;
            public static readonly NuGetFramework Net60;
            public static readonly NuGetFramework Net70;
            public static readonly NuGetFramework Net80;
            public static readonly NuGetFramework NetCore45;
            public static readonly NuGetFramework NetCore451;
            public static readonly NuGetFramework NetCore50;
            public static readonly NuGetFramework NetCoreApp10;
            public static readonly NuGetFramework NetCoreApp11;
            public static readonly NuGetFramework NetCoreApp20;
            public static readonly NuGetFramework NetCoreApp21;
            public static readonly NuGetFramework NetCoreApp22;
            public static readonly NuGetFramework NetCoreApp30;
            public static readonly NuGetFramework NetCoreApp31;
            public static readonly NuGetFramework NetStandard;
            public static readonly NuGetFramework NetStandard10;
            public static readonly NuGetFramework NetStandard11;
            public static readonly NuGetFramework NetStandard12;
            public static readonly NuGetFramework NetStandard13;
            public static readonly NuGetFramework NetStandard14;
            public static readonly NuGetFramework NetStandard15;
            public static readonly NuGetFramework NetStandard16;
            public static readonly NuGetFramework NetStandard17;
            public static readonly NuGetFramework NetStandard20;
            public static readonly NuGetFramework NetStandard21;
            public static readonly NuGetFramework NetStandardApp15;
            public static readonly NuGetFramework SL4;
            public static readonly NuGetFramework SL5;
            public static readonly NuGetFramework Tizen3;
            public static readonly NuGetFramework Tizen4;
            public static readonly NuGetFramework Tizen6;
            public static readonly NuGetFramework UAP10;
            public static readonly NuGetFramework Win10;
            public static readonly NuGetFramework Win8;
            public static readonly NuGetFramework Win81;
            public static readonly NuGetFramework WP7;
            public static readonly NuGetFramework WP75;
            public static readonly NuGetFramework WP8;
            public static readonly NuGetFramework WP81;
            public static readonly NuGetFramework WPA81;
        }

        public static partial class FrameworkIdentifiers
        {
            public const string AspNet = "ASP.NET";
            public const string AspNetCore = "ASP.NETCore";
            public const string Dnx = "DNX";
            public const string DnxCore = "DNXCore";
            public const string DotNet = "dotnet";
            public const string MonoAndroid = "MonoAndroid";
            public const string MonoMac = "MonoMac";
            public const string MonoTouch = "MonoTouch";
            public const string NanoFramework = ".NETnanoFramework";
            public const string Native = "native";
            public const string Net = ".NETFramework";
            public const string NetCore = ".NETCore";
            public const string NetCoreApp = ".NETCoreApp";
            public const string NetMicro = ".NETMicroFramework";
            public const string NetPlatform = ".NETPlatform";
            public const string NetStandard = ".NETStandard";
            public const string NetStandardApp = ".NETStandardApp";
            public const string Portable = ".NETPortable";
            public const string Silverlight = "Silverlight";
            public const string Tizen = "Tizen";
            public const string UAP = "UAP";
            public const string Windows = "Windows";
            public const string WindowsPhone = "WindowsPhone";
            public const string WindowsPhoneApp = "WindowsPhoneApp";
            public const string WinRT = "WinRT";
            public const string XamarinIOs = "Xamarin.iOS";
            public const string XamarinMac = "Xamarin.Mac";
            public const string XamarinPlayStation3 = "Xamarin.PlayStation3";
            public const string XamarinPlayStation4 = "Xamarin.PlayStation4";
            public const string XamarinPlayStationVita = "Xamarin.PlayStationVita";
            public const string XamarinTVOS = "Xamarin.TVOS";
            public const string XamarinWatchOS = "Xamarin.WatchOS";
            public const string XamarinXbox360 = "Xamarin.Xbox360";
            public const string XamarinXboxOne = "Xamarin.XboxOne";
        }

        public static partial class PlatformIdentifiers
        {
            public const string Windows = "Windows";
            public const string WindowsPhone = "WindowsPhone";
        }

        public static partial class SpecialIdentifiers
        {
            public const string Agnostic = "Agnostic";
            public const string Any = "Any";
            public const string Unsupported = "Unsupported";
        }
    }

    public partial class FrameworkException : System.Exception
    {
        protected FrameworkException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }

        public FrameworkException(string message) { }
    }

    public partial class FrameworkExpander
    {
        public FrameworkExpander() { }

        public FrameworkExpander(IFrameworkNameProvider mappings) { }

        public System.Collections.Generic.IEnumerable<NuGetFramework> Expand(NuGetFramework framework) { throw null; }
    }

    public static partial class FrameworkNameHelpers
    {
        public static string GetFolderName(string identifierShortName, string versionString, string? profileShortName) { throw null; }

        public static string GetPortableProfileNumberString(int profileNumber) { throw null; }

        public static System.Version GetVersion(string? versionString) { throw null; }

        public static string GetVersionString(System.Version version) { throw null; }
    }

    public partial class FrameworkNameProvider : IFrameworkNameProvider
    {
        public FrameworkNameProvider(System.Collections.Generic.IEnumerable<IFrameworkMappings>? mappings, System.Collections.Generic.IEnumerable<IPortableFrameworkMappings>? portableMappings) { }

        public void AddFrameworkPrecedenceMappings(System.Collections.Generic.IDictionary<string, int> destination, System.Collections.Generic.IEnumerable<string> mappings) { }

        public int CompareEquivalentFrameworks(NuGetFramework? x, NuGetFramework? y) { throw null; }

        public int CompareFrameworks(NuGetFramework? x, NuGetFramework? y) { throw null; }

        public System.Collections.Generic.IEnumerable<NuGetFramework> GetCompatibleCandidates() { throw null; }

        public NuGetFramework GetFullNameReplacement(NuGetFramework framework) { throw null; }

        public System.Collections.Generic.IEnumerable<NuGetFramework> GetNetStandardVersions() { throw null; }

        public NuGetFramework GetShortNameReplacement(NuGetFramework framework) { throw null; }

        public string GetVersionString(string framework, System.Version version) { throw null; }

        public bool TryGetCompatibilityMappings(NuGetFramework framework, out System.Collections.Generic.IEnumerable<FrameworkRange>? supportedFrameworkRanges) { throw null; }

        public bool TryGetEquivalentFrameworks(FrameworkRange range, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks) { throw null; }

        public bool TryGetEquivalentFrameworks(NuGetFramework framework, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks) { throw null; }

        public bool TryGetIdentifier(string framework, out string? identifier) { throw null; }

        public bool TryGetPlatformVersion(string versionString, out System.Version? version) { throw null; }

        public bool TryGetPortableCompatibilityMappings(int profile, out System.Collections.Generic.IEnumerable<FrameworkRange>? supportedFrameworkRanges) { throw null; }

        public bool TryGetPortableFrameworks(int profile, bool includeOptional, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks) { throw null; }

        public bool TryGetPortableFrameworks(int profile, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks) { throw null; }

        public bool TryGetPortableFrameworks(string profile, bool includeOptional, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks) { throw null; }

        public bool TryGetPortableFrameworks(string shortPortableProfiles, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks) { throw null; }

        public bool TryGetPortableProfile(System.Collections.Generic.IEnumerable<NuGetFramework> supportedFrameworks, out int profileNumber) { throw null; }

        public bool TryGetPortableProfileNumber(string profile, out int profileNumber) { throw null; }

        public bool TryGetProfile(string frameworkIdentifier, string profileShortName, out string? profile) { throw null; }

        public bool TryGetShortIdentifier(string identifier, out string? identifierShortName) { throw null; }

        public bool TryGetShortProfile(string frameworkIdentifier, string profile, out string? profileShortName) { throw null; }

        public bool TryGetSubSetFrameworks(string frameworkIdentifier, out System.Collections.Generic.IEnumerable<string>? subSetFrameworks) { throw null; }

        public bool TryGetVersion(string versionString, out System.Version? version) { throw null; }
    }

    public partial class FrameworkPrecedenceSorter : System.Collections.Generic.IComparer<NuGetFramework>
    {
        public FrameworkPrecedenceSorter(IFrameworkNameProvider mappings, bool allEquivalent) { }

        public int Compare(NuGetFramework? x, NuGetFramework? y) { throw null; }
    }

    public partial class FrameworkRange : System.IEquatable<FrameworkRange>
    {
        public FrameworkRange(NuGetFramework min, NuGetFramework max, bool includeMin, bool includeMax) { }

        public FrameworkRange(NuGetFramework min, NuGetFramework max) { }

        public string FrameworkIdentifier { get { throw null; } }

        public bool IncludeMax { get { throw null; } }

        public bool IncludeMin { get { throw null; } }

        public NuGetFramework Max { get { throw null; } }

        public NuGetFramework Min { get { throw null; } }

        public bool Equals(FrameworkRange? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public bool Satisfies(NuGetFramework framework) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class FrameworkRangeComparer : System.Collections.Generic.IEqualityComparer<FrameworkRange>
    {
        public static FrameworkRangeComparer Instance { get { throw null; } }

        public bool Equals(FrameworkRange? x, FrameworkRange? y) { throw null; }

        public int GetHashCode(FrameworkRange obj) { throw null; }
    }

    public partial class FrameworkReducer
    {
        public FrameworkReducer() { }

        public FrameworkReducer(IFrameworkNameProvider mappings, IFrameworkCompatibilityProvider compat) { }

        public NuGetFramework? GetNearest(NuGetFramework framework, System.Collections.Generic.IEnumerable<NuGetFramework> possibleFrameworks) { throw null; }

        public System.Collections.Generic.IEnumerable<NuGetFramework> ReduceDownwards(System.Collections.Generic.IEnumerable<NuGetFramework> frameworks) { throw null; }

        public System.Collections.Generic.IEnumerable<NuGetFramework> ReduceEquivalent(System.Collections.Generic.IEnumerable<NuGetFramework> frameworks) { throw null; }

        public System.Collections.Generic.IEnumerable<NuGetFramework> ReduceUpwards(System.Collections.Generic.IEnumerable<NuGetFramework> frameworks) { throw null; }
    }

    public sealed partial class FrameworkRuntimePair : System.IEquatable<FrameworkRuntimePair>, System.IComparable<FrameworkRuntimePair>
    {
        public FrameworkRuntimePair(NuGetFramework framework, string? runtimeIdentifier) { }

        public NuGetFramework Framework { get { throw null; } }

        public string Name { get { throw null; } }

        public string RuntimeIdentifier { get { throw null; } }

        [System.Obsolete("This type is immutable, so there is no need or point to clone it.")]
        public FrameworkRuntimePair Clone() { throw null; }

        public int CompareTo(FrameworkRuntimePair? other) { throw null; }

        public bool Equals(FrameworkRuntimePair? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static string GetName(NuGetFramework framework, string? runtimeIdentifier) { throw null; }

        public static string GetTargetGraphName(NuGetFramework framework, string? runtimeIdentifier) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class FrameworkSpecificMapping
    {
        public FrameworkSpecificMapping(string frameworkIdentifier, System.Collections.Generic.KeyValuePair<string, string> mapping) { }

        public FrameworkSpecificMapping(string frameworkIdentifier, string key, string value) { }

        public string FrameworkIdentifier { get { throw null; } }

        public System.Collections.Generic.KeyValuePair<string, string> Mapping { get { throw null; } }
    }

    public partial interface IFrameworkCompatibilityListProvider
    {
        System.Collections.Generic.IEnumerable<NuGetFramework> GetFrameworksSupporting(NuGetFramework target);
    }

    public partial interface IFrameworkCompatibilityProvider
    {
        bool IsCompatible(NuGetFramework framework, NuGetFramework other);
    }

    public partial interface IFrameworkMappings
    {
        System.Collections.Generic.IEnumerable<OneWayCompatibilityMappingEntry> CompatibilityMappings { get; }

        System.Collections.Generic.IEnumerable<string> EquivalentFrameworkPrecedence { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<NuGetFramework, NuGetFramework>> EquivalentFrameworks { get; }

        System.Collections.Generic.IEnumerable<FrameworkSpecificMapping> EquivalentProfiles { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<NuGetFramework, NuGetFramework>> FullNameReplacements { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> IdentifierShortNames { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> IdentifierSynonyms { get; }

        System.Collections.Generic.IEnumerable<string> NonPackageBasedFrameworkPrecedence { get; }

        System.Collections.Generic.IEnumerable<string> PackageBasedFrameworkPrecedence { get; }

        System.Collections.Generic.IEnumerable<FrameworkSpecificMapping> ProfileShortNames { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<NuGetFramework, NuGetFramework>> ShortNameReplacements { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> SubSetFrameworks { get; }
    }

    public partial interface IFrameworkNameProvider
    {
        int CompareEquivalentFrameworks(NuGetFramework? x, NuGetFramework? y);
        int CompareFrameworks(NuGetFramework? x, NuGetFramework? y);
        System.Collections.Generic.IEnumerable<NuGetFramework> GetCompatibleCandidates();
        NuGetFramework GetFullNameReplacement(NuGetFramework framework);
        System.Collections.Generic.IEnumerable<NuGetFramework> GetNetStandardVersions();
        NuGetFramework GetShortNameReplacement(NuGetFramework framework);
        string GetVersionString(string framework, System.Version version);
        bool TryGetCompatibilityMappings(NuGetFramework framework, out System.Collections.Generic.IEnumerable<FrameworkRange>? supportedFrameworkRanges);
        bool TryGetEquivalentFrameworks(FrameworkRange range, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks);
        bool TryGetEquivalentFrameworks(NuGetFramework framework, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks);
        bool TryGetIdentifier(string identifierShortName, out string? identifier);
        bool TryGetPlatformVersion(string versionString, out System.Version? version);
        bool TryGetPortableCompatibilityMappings(int profile, out System.Collections.Generic.IEnumerable<FrameworkRange>? supportedFrameworkRanges);
        bool TryGetPortableFrameworks(int profile, bool includeOptional, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks);
        bool TryGetPortableFrameworks(int profile, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks);
        bool TryGetPortableFrameworks(string profile, bool includeOptional, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks);
        bool TryGetPortableFrameworks(string shortPortableProfiles, out System.Collections.Generic.IEnumerable<NuGetFramework>? frameworks);
        bool TryGetPortableProfile(System.Collections.Generic.IEnumerable<NuGetFramework> supportedFrameworks, out int profileNumber);
        bool TryGetPortableProfileNumber(string profile, out int profileNumber);
        bool TryGetProfile(string frameworkIdentifier, string profileShortName, out string? profile);
        bool TryGetShortIdentifier(string identifier, out string? identifierShortName);
        bool TryGetShortProfile(string frameworkIdentifier, string profile, out string? profileShortName);
        bool TryGetSubSetFrameworks(string frameworkIdentifier, out System.Collections.Generic.IEnumerable<string>? subSetFrameworkIdentifiers);
        bool TryGetVersion(string versionString, out System.Version? version);
    }

    public partial interface IFrameworkSpecific
    {
        NuGetFramework TargetFramework { get; }
    }

    public partial interface IFrameworkTargetable
    {
        System.Collections.Generic.IEnumerable<NuGetFramework> SupportedFrameworks { get; }
    }

    public partial interface IPortableFrameworkMappings
    {
        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, FrameworkRange>> CompatibilityMappings { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, NuGetFramework[]>> ProfileFrameworks { get; }

        System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, NuGetFramework[]>> ProfileOptionalFrameworks { get; }
    }

    public partial class NuGetFramework : System.IEquatable<NuGetFramework>
    {
        public static readonly NuGetFramework AgnosticFramework;
        public static readonly NuGetFramework AnyFramework;
        public static readonly System.Collections.Generic.IEqualityComparer<NuGetFramework> Comparer;
        public static readonly System.Collections.Generic.IEqualityComparer<NuGetFramework> FrameworkNameComparer;
        public static readonly NuGetFramework UnsupportedFramework;
        public NuGetFramework(NuGetFramework framework) { }

        public NuGetFramework(string frameworkIdentifier, System.Version frameworkVersion, string platform, System.Version platformVersion) { }

        public NuGetFramework(string frameworkIdentifier, System.Version frameworkVersion, string? frameworkProfile) { }

        public NuGetFramework(string framework, System.Version version) { }

        public NuGetFramework(string framework) { }

        public bool AllFrameworkVersions { get { throw null; } }

        public string DotNetFrameworkName { get { throw null; } }

        public string DotNetPlatformName { get { throw null; } }

        public string Framework { get { throw null; } }

        public bool HasPlatform { get { throw null; } }

        public bool HasProfile { get { throw null; } }

        public bool IsAgnostic { get { throw null; } }

        public bool IsAny { get { throw null; } }

        public bool IsPackageBased { get { throw null; } }

        public bool IsPCL { get { throw null; } }

        public bool IsSpecificFramework { get { throw null; } }

        public bool IsUnsupported { get { throw null; } }

        public string Platform { get { throw null; } }

        public System.Version PlatformVersion { get { throw null; } }

        public string Profile { get { throw null; } }

        public System.Version Version { get { throw null; } }

        public bool Equals(NuGetFramework? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public string GetDotNetFrameworkName(IFrameworkNameProvider mappings) { throw null; }

        public override int GetHashCode() { throw null; }

        public string GetShortFolderName() { throw null; }

        public virtual string GetShortFolderName(IFrameworkNameProvider mappings) { throw null; }

        public static bool operator ==(NuGetFramework? left, NuGetFramework? right) { throw null; }

        public static bool operator !=(NuGetFramework? left, NuGetFramework? right) { throw null; }

        public static NuGetFramework Parse(string folderName, IFrameworkNameProvider mappings) { throw null; }

        public static NuGetFramework Parse(string folderName) { throw null; }

        public static NuGetFramework ParseComponents(string targetFrameworkMoniker, string? targetPlatformMoniker) { throw null; }

        public static NuGetFramework ParseFolder(string folderName, IFrameworkNameProvider mappings) { throw null; }

        public static NuGetFramework ParseFolder(string folderName) { throw null; }

        public static NuGetFramework ParseFrameworkName(string frameworkName, IFrameworkNameProvider mappings) { throw null; }

        public override string ToString() { throw null; }
    }

    public static partial class NuGetFrameworkExtensions
    {
        public static T? GetNearest<T>(this System.Collections.Generic.IEnumerable<T> items, NuGetFramework projectFramework)
            where T : class, IFrameworkSpecific { throw null; }

        public static bool IsDesktop(this NuGetFramework framework) { throw null; }
    }

    public partial class NuGetFrameworkFullComparer : System.Collections.Generic.IEqualityComparer<NuGetFramework>
    {
        public static NuGetFrameworkFullComparer Instance { get { throw null; } }

        public bool Equals(NuGetFramework? x, NuGetFramework? y) { throw null; }

        public int GetHashCode(NuGetFramework obj) { throw null; }
    }

    public partial class NuGetFrameworkNameComparer : System.Collections.Generic.IEqualityComparer<NuGetFramework>
    {
        public static NuGetFrameworkNameComparer Instance { get { throw null; } }

        public bool Equals(NuGetFramework? x, NuGetFramework? y) { throw null; }

        public int GetHashCode(NuGetFramework obj) { throw null; }
    }

    public partial class NuGetFrameworkSorter : System.Collections.Generic.IComparer<NuGetFramework>
    {
        public static NuGetFrameworkSorter Instance { get { throw null; } }

        public int Compare(NuGetFramework? x, NuGetFramework? y) { throw null; }
    }

    public static partial class NuGetFrameworkUtility
    {
        public static T? GetNearest<T>(System.Collections.Generic.IEnumerable<T> items, NuGetFramework framework, IFrameworkNameProvider frameworkMappings, IFrameworkCompatibilityProvider compatibilityProvider, System.Func<T, NuGetFramework> selector)
            where T : class { throw null; }

        public static T? GetNearest<T>(System.Collections.Generic.IEnumerable<T> items, NuGetFramework framework, IFrameworkNameProvider frameworkMappings, IFrameworkCompatibilityProvider compatibilityProvider)
            where T : IFrameworkSpecific { throw null; }

        public static T? GetNearest<T>(System.Collections.Generic.IEnumerable<T> items, NuGetFramework framework, System.Func<T, NuGetFramework> selector)
            where T : class { throw null; }

        public static T? GetNearest<T>(System.Collections.Generic.IEnumerable<T> items, NuGetFramework framework)
            where T : IFrameworkSpecific { throw null; }

        public static bool IsCompatibleWithFallbackCheck(NuGetFramework projectFramework, NuGetFramework candidate) { throw null; }

        public static bool IsNetCore50AndUp(NuGetFramework framework) { throw null; }
    }

    public partial class OneWayCompatibilityMappingEntry : System.IEquatable<OneWayCompatibilityMappingEntry>
    {
        public OneWayCompatibilityMappingEntry(FrameworkRange targetFramework, FrameworkRange supportedFramework) { }

        public static CompatibilityMappingComparer Comparer { get { throw null; } }

        public FrameworkRange SupportedFrameworkRange { get { throw null; } }

        public FrameworkRange TargetFrameworkRange { get { throw null; } }

        public bool Equals(OneWayCompatibilityMappingEntry? other) { throw null; }

        public override string ToString() { throw null; }
    }
}