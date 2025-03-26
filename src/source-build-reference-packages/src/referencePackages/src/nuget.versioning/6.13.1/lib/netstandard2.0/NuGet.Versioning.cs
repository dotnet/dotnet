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
[assembly: System.Reflection.AssemblyDescription("NuGet's implementation of Semantic Versioning.")]
[assembly: System.Reflection.AssemblyFileVersion("6.13.1.3")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.13.1+3fd0e588e53525f0cd037d7b91174c0ca78ac65c.3fd0e588e53525f0cd037d7b91174c0ca78ac65c")]
[assembly: System.Reflection.AssemblyProduct("NuGet")]
[assembly: System.Reflection.AssemblyTitle("NuGet.Versioning")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/NuGet/NuGet.Client")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.13.1.3")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace NuGet.Versioning
{
    public partial class FloatRange : System.IEquatable<FloatRange>
    {
        public FloatRange(NuGetVersionFloatBehavior floatBehavior, NuGetVersion minVersion, string? releasePrefix) { }

        public FloatRange(NuGetVersionFloatBehavior floatBehavior, NuGetVersion minVersion) { }

        public FloatRange(NuGetVersionFloatBehavior floatBehavior) { }

        public NuGetVersionFloatBehavior FloatBehavior { get { throw null; } }

        public bool HasMinVersion { get { throw null; } }

        public bool IncludePrerelease { get { throw null; } }

        public NuGetVersion MinVersion { get { throw null; } }

        public string? OriginalReleasePrefix { get { throw null; } }

        public bool Equals(FloatRange? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static FloatRange Parse(string versionString) { throw null; }

        public bool Satisfies(NuGetVersion version) { throw null; }

        public override string ToString() { throw null; }

        public void ToString(System.Text.StringBuilder sb) { }

        public static bool TryParse(string versionString, out FloatRange? range) { throw null; }
    }

    public partial interface INuGetVersionable
    {
        NuGetVersion Version { get; }
    }

    public partial interface IVersionComparer : System.Collections.Generic.IEqualityComparer<SemanticVersion>, System.Collections.Generic.IComparer<SemanticVersion>
    {
    }

    public partial interface IVersionRangeComparer : System.Collections.Generic.IEqualityComparer<VersionRangeBase>
    {
    }

    public partial class NuGetVersion : SemanticVersion
    {
        public NuGetVersion(NuGetVersion version) : base(default!) { }

        public NuGetVersion(int major, int minor, int patch, System.Collections.Generic.IEnumerable<string>? releaseLabels, string? metadata) : base(default!) { }

        public NuGetVersion(int major, int minor, int patch, int revision, System.Collections.Generic.IEnumerable<string>? releaseLabels, string? metadata) : base(default!) { }

        public NuGetVersion(int major, int minor, int patch, int revision, string releaseLabel, string metadata) : base(default!) { }

        public NuGetVersion(int major, int minor, int patch, int revision) : base(default!) { }

        public NuGetVersion(int major, int minor, int patch, string? releaseLabel, string? metadata) : base(default!) { }

        public NuGetVersion(int major, int minor, int patch, string? releaseLabel) : base(default!) { }

        public NuGetVersion(int major, int minor, int patch) : base(default!) { }

        public NuGetVersion(string version) : base(default!) { }

        public NuGetVersion(System.Version version, System.Collections.Generic.IEnumerable<string>? releaseLabels, string? metadata, string? originalVersion) : base(default!) { }

        public NuGetVersion(System.Version version, string? releaseLabel = null, string? metadata = null) : base(default!) { }

        public virtual bool IsLegacyVersion { get { throw null; } }

        public bool IsSemVer2 { get { throw null; } }

        public string? OriginalVersion { get { throw null; } }

        public int Revision { get { throw null; } }

        public System.Version Version { get { throw null; } }

        public new static NuGetVersion Parse(string value) { throw null; }

        public override string ToString() { throw null; }

        public static bool TryParse(string? value, out NuGetVersion? version) { throw null; }

        public static bool TryParseStrict(string value, out NuGetVersion? version) { throw null; }
    }

    public enum NuGetVersionFloatBehavior
    {
        None = 0,
        Prerelease = 1,
        Revision = 2,
        Patch = 3,
        Minor = 4,
        Major = 5,
        AbsoluteLatest = 6,
        PrereleaseRevision = 7,
        PrereleasePatch = 8,
        PrereleaseMinor = 9,
        PrereleaseMajor = 10
    }

    public partial class SemanticVersion : System.IFormattable, System.IComparable, System.IComparable<SemanticVersion>, System.IEquatable<SemanticVersion>
    {
        public SemanticVersion(SemanticVersion version) { }

        public SemanticVersion(int major, int minor, int patch, System.Collections.Generic.IEnumerable<string>? releaseLabels, string? metadata) { }

        protected SemanticVersion(int major, int minor, int patch, int revision, System.Collections.Generic.IEnumerable<string>? releaseLabels, string? metadata) { }

        protected SemanticVersion(int major, int minor, int patch, int revision, string? releaseLabel, string? metadata) { }

        public SemanticVersion(int major, int minor, int patch, string? releaseLabel, string? metadata) { }

        public SemanticVersion(int major, int minor, int patch, string? releaseLabel) { }

        public SemanticVersion(int major, int minor, int patch) { }

        protected SemanticVersion(System.Version version, System.Collections.Generic.IEnumerable<string>? releaseLabels, string? metadata) { }

        protected SemanticVersion(System.Version version, string? releaseLabel = null, string? metadata = null) { }

        public virtual bool HasMetadata { get { throw null; } }

        public virtual bool IsPrerelease { get { throw null; } }

        public int Major { get { throw null; } }

        public virtual string? Metadata { get { throw null; } }

        public int Minor { get { throw null; } }

        public int Patch { get { throw null; } }

        public string Release { get { throw null; } }

        public System.Collections.Generic.IEnumerable<string> ReleaseLabels { get { throw null; } }

        public virtual int CompareTo(SemanticVersion? other, VersionComparison versionComparison) { throw null; }

        public virtual int CompareTo(SemanticVersion? other) { throw null; }

        public virtual int CompareTo(object? obj) { throw null; }

        public virtual bool Equals(SemanticVersion? other, VersionComparison versionComparison) { throw null; }

        public virtual bool Equals(SemanticVersion? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(SemanticVersion? version1, SemanticVersion? version2) { throw null; }

        public static bool operator >(SemanticVersion version1, SemanticVersion version2) { throw null; }

        public static bool operator >=(SemanticVersion version1, SemanticVersion version2) { throw null; }

        public static bool operator !=(SemanticVersion? version1, SemanticVersion? version2) { throw null; }

        public static bool operator <(SemanticVersion version1, SemanticVersion version2) { throw null; }

        public static bool operator <=(SemanticVersion version1, SemanticVersion version2) { throw null; }

        public static SemanticVersion Parse(string value) { throw null; }

        public virtual string ToFullString() { throw null; }

        public virtual string ToNormalizedString() { throw null; }

        public override string ToString() { throw null; }

        public virtual string ToString(string? format, System.IFormatProvider? formatProvider) { throw null; }

        protected bool TryFormatter(string? format, System.IFormatProvider formatProvider, out string? formattedString) { throw null; }

        public static bool TryParse(string value, out SemanticVersion? version) { throw null; }
    }

    public partial class SemanticVersionConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Type sourceType) { throw null; }

        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext? context, System.Type? destinationType) { throw null; }

        public override object? ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value) { throw null; }

        public override object? ConvertTo(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, System.Type destinationType) { throw null; }
    }

    public sealed partial class VersionComparer : IVersionComparer, System.Collections.Generic.IEqualityComparer<SemanticVersion>, System.Collections.Generic.IComparer<SemanticVersion>
    {
        public static readonly IVersionComparer Default;
        public static readonly IVersionComparer Version;
        public static readonly IVersionComparer VersionRelease;
        public static readonly IVersionComparer VersionReleaseMetadata;
        public VersionComparer() { }

        public VersionComparer(VersionComparison versionComparison) { }

        public static int Compare(SemanticVersion? version1, SemanticVersion? version2, VersionComparison versionComparison) { throw null; }

        public int Compare(SemanticVersion? x, SemanticVersion? y) { throw null; }

        public bool Equals(SemanticVersion? x, SemanticVersion? y) { throw null; }

        public static IVersionComparer Get(VersionComparison versionComparison) { throw null; }

        public int GetHashCode(SemanticVersion version) { throw null; }
    }

    public enum VersionComparison
    {
        Default = 0,
        Version = 1,
        VersionRelease = 2,
        VersionReleaseMetadata = 3
    }

    public static partial class VersionExtensions
    {
        public static INuGetVersionable? FindBestMatch(this System.Collections.Generic.IEnumerable<INuGetVersionable> items, VersionRange ideal) { throw null; }

        public static T? FindBestMatch<T>(this System.Collections.Generic.IEnumerable<T> items, VersionRange? ideal, System.Func<T, NuGetVersion> selector)
            where T : class { throw null; }
    }

    public partial class VersionFormatter : System.IFormatProvider, System.ICustomFormatter
    {
        public static readonly VersionFormatter Instance;
        public string Format(string? format, object? arg, System.IFormatProvider? formatProvider) { throw null; }

        public object? GetFormat(System.Type? formatType) { throw null; }
    }

    public partial class VersionRange : VersionRangeBase, System.IFormattable
    {
        public static readonly VersionRange All;
        [System.Obsolete("Consider not using this VersionRange. The lack of a proper normalized version means that it is not round trippable in an assets file.")]
        public static readonly VersionRange AllFloating;
        public static readonly VersionRange AllStable;
        [System.Obsolete("Consider not using this VersionRange. The lack of a proper normalized version means that it is not round trippable in an assets file.")]
        public static readonly VersionRange AllStableFloating;
        public static readonly VersionRange None;
        public VersionRange(NuGetVersion minVersion, FloatRange? floatRange) : base(default, default, default, default) { }

        public VersionRange(NuGetVersion? minVersion = null, bool includeMinVersion = true, NuGetVersion? maxVersion = null, bool includeMaxVersion = false, FloatRange? floatRange = null, string? originalString = null) : base(default, default, default, default) { }

        public VersionRange(NuGetVersion minVersion) : base(default, default, default, default) { }

        public VersionRange(VersionRange range, FloatRange floatRange) : base(default, default, default, default) { }

        public FloatRange? Float { get { throw null; } }

        public new bool HasLowerAndUpperBounds { get { throw null; } }

        public new bool HasLowerBound { get { throw null; } }

        public new bool HasUpperBound { get { throw null; } }

        public bool IsFloating { get { throw null; } }

        public new bool IsMaxInclusive { get { throw null; } }

        public new bool IsMinInclusive { get { throw null; } }

        public new NuGetVersion? MaxVersion { get { throw null; } }

        public new NuGetVersion? MinVersion { get { throw null; } }

        public string? OriginalString { get { throw null; } }

        public static VersionRange Combine(System.Collections.Generic.IEnumerable<NuGetVersion> versions, IVersionComparer comparer) { throw null; }

        public static VersionRange Combine(System.Collections.Generic.IEnumerable<NuGetVersion> versions) { throw null; }

        public static VersionRange Combine(System.Collections.Generic.IEnumerable<VersionRange> ranges, IVersionComparer comparer) { throw null; }

        public static VersionRange Combine(System.Collections.Generic.IEnumerable<VersionRange> ranges) { throw null; }

        public static VersionRange CommonSubSet(System.Collections.Generic.IEnumerable<VersionRange> ranges, IVersionComparer comparer) { throw null; }

        public static VersionRange CommonSubSet(System.Collections.Generic.IEnumerable<VersionRange> ranges) { throw null; }

        public bool Equals(VersionRange? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public NuGetVersion? FindBestMatch(System.Collections.Generic.IEnumerable<NuGetVersion>? versions) { throw null; }

        public override int GetHashCode() { throw null; }

        public bool IsBetter(NuGetVersion? current, NuGetVersion? considering) { throw null; }

        public static VersionRange Parse(string value, bool allowFloating) { throw null; }

        public static VersionRange Parse(string value) { throw null; }

        public string PrettyPrint() { throw null; }

        public virtual string ToLegacyShortString() { throw null; }

        public virtual string ToLegacyString() { throw null; }

        public VersionRange ToNonSnapshotRange() { throw null; }

        public virtual string ToNormalizedString() { throw null; }

        public virtual string ToShortString() { throw null; }

        public override string ToString() { throw null; }

        public string ToString(string? format, System.IFormatProvider? formatProvider) { throw null; }

        protected bool TryFormatter(string? format, System.IFormatProvider formatProvider, out string? formattedString) { throw null; }

        public static bool TryParse(string value, out VersionRange? versionRange) { throw null; }

        public static bool TryParse(string value, bool allowFloating, out VersionRange? versionRange) { throw null; }
    }

    public abstract partial class VersionRangeBase : System.IEquatable<VersionRangeBase>
    {
        public VersionRangeBase(NuGetVersion? minVersion = null, bool includeMinVersion = true, NuGetVersion? maxVersion = null, bool includeMaxVersion = false) { }

        public bool HasLowerAndUpperBounds { get { throw null; } }

        public bool HasLowerBound { get { throw null; } }

        protected bool HasPrereleaseBounds { get { throw null; } }

        public bool HasUpperBound { get { throw null; } }

        public bool IsMaxInclusive { get { throw null; } }

        public bool IsMinInclusive { get { throw null; } }

        public NuGetVersion? MaxVersion { get { throw null; } }

        public NuGetVersion? MinVersion { get { throw null; } }

        public bool Equals(VersionRangeBase? other, IVersionComparer versionComparer) { throw null; }

        public bool Equals(VersionRangeBase? other, IVersionRangeComparer comparer) { throw null; }

        public bool Equals(VersionRangeBase? other, VersionComparison versionComparison) { throw null; }

        public bool Equals(VersionRangeBase? other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public bool IsSubSetOrEqualTo(VersionRangeBase? possibleSuperSet, IVersionComparer comparer) { throw null; }

        public bool IsSubSetOrEqualTo(VersionRangeBase? possibleSuperSet) { throw null; }

        public bool Satisfies(NuGetVersion version, IVersionComparer comparer) { throw null; }

        public bool Satisfies(NuGetVersion version, VersionComparison versionComparison) { throw null; }

        public bool Satisfies(NuGetVersion version) { throw null; }
    }

    public partial class VersionRangeComparer : IVersionRangeComparer, System.Collections.Generic.IEqualityComparer<VersionRangeBase>
    {
        public VersionRangeComparer() { }

        public VersionRangeComparer(IVersionComparer versionComparer) { }

        public VersionRangeComparer(VersionComparison versionComparison) { }

        public static IVersionRangeComparer Default { get { throw null; } }

        public static IVersionRangeComparer VersionRelease { get { throw null; } }

        public bool Equals(VersionRangeBase? x, VersionRangeBase? y) { throw null; }

        public static IVersionRangeComparer Get(VersionComparison versionComparison) { throw null; }

        public int GetHashCode(VersionRangeBase obj) { throw null; }
    }

    public partial class VersionRangeFormatter : System.IFormatProvider, System.ICustomFormatter
    {
        public static readonly VersionRangeFormatter Instance;
        public string Format(string? format, object? arg, System.IFormatProvider? formatProvider) { throw null; }

        public object? GetFormat(System.Type? formatType) { throw null; }
    }
}