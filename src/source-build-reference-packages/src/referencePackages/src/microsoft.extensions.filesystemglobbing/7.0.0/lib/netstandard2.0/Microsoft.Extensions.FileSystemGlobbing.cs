// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Extensions.FileSystemGlobbing.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Extensions.FileSystemGlobbing")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("File system globbing to find files matching a specified pattern.")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Extensions.FileSystemGlobbing")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("7.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Extensions.FileSystemGlobbing
{
    public partial struct FilePatternMatch : System.IEquatable<FilePatternMatch>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public FilePatternMatch(string path, string? stem) { }

        public string Path { get { throw null; } }

        public string? Stem { get { throw null; } }

        public bool Equals(FilePatternMatch other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial class InMemoryDirectoryInfo : Abstractions.DirectoryInfoBase
    {
        public InMemoryDirectoryInfo(string rootDir, System.Collections.Generic.IEnumerable<string>? files) { }

        public override string FullName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override Abstractions.DirectoryInfoBase? ParentDirectory { get { throw null; } }

        public override System.Collections.Generic.IEnumerable<Abstractions.FileSystemInfoBase> EnumerateFileSystemInfos() { throw null; }

        public override Abstractions.DirectoryInfoBase GetDirectory(string path) { throw null; }

        public override Abstractions.FileInfoBase? GetFile(string path) { throw null; }
    }

    public partial class Matcher
    {
        public Matcher() { }

        public Matcher(System.StringComparison comparisonType) { }

        public virtual Matcher AddExclude(string pattern) { throw null; }

        public virtual Matcher AddInclude(string pattern) { throw null; }

        public virtual PatternMatchingResult Execute(Abstractions.DirectoryInfoBase directoryInfo) { throw null; }
    }

    public static partial class MatcherExtensions
    {
        public static void AddExcludePatterns(this Matcher matcher, params System.Collections.Generic.IEnumerable<string>[] excludePatternsGroups) { }

        public static void AddIncludePatterns(this Matcher matcher, params System.Collections.Generic.IEnumerable<string>[] includePatternsGroups) { }

        public static System.Collections.Generic.IEnumerable<string> GetResultsInFullPath(this Matcher matcher, string directoryPath) { throw null; }

        public static PatternMatchingResult Match(this Matcher matcher, System.Collections.Generic.IEnumerable<string>? files) { throw null; }

        public static PatternMatchingResult Match(this Matcher matcher, string rootDir, System.Collections.Generic.IEnumerable<string>? files) { throw null; }

        public static PatternMatchingResult Match(this Matcher matcher, string rootDir, string file) { throw null; }

        public static PatternMatchingResult Match(this Matcher matcher, string file) { throw null; }
    }

    public partial class PatternMatchingResult
    {
        public PatternMatchingResult(System.Collections.Generic.IEnumerable<FilePatternMatch> files, bool hasMatches) { }

        public PatternMatchingResult(System.Collections.Generic.IEnumerable<FilePatternMatch> files) { }

        public System.Collections.Generic.IEnumerable<FilePatternMatch> Files { get { throw null; } set { } }

        public bool HasMatches { get { throw null; } }
    }
}

namespace Microsoft.Extensions.FileSystemGlobbing.Abstractions
{
    public abstract partial class DirectoryInfoBase : FileSystemInfoBase
    {
        protected DirectoryInfoBase() { }

        public abstract System.Collections.Generic.IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos();
        public abstract DirectoryInfoBase? GetDirectory(string path);
        public abstract FileInfoBase? GetFile(string path);
    }

    public partial class DirectoryInfoWrapper : DirectoryInfoBase
    {
        public DirectoryInfoWrapper(System.IO.DirectoryInfo directoryInfo) { }

        public override string FullName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override DirectoryInfoBase? ParentDirectory { get { throw null; } }

        public override System.Collections.Generic.IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos() { throw null; }

        public override DirectoryInfoBase? GetDirectory(string name) { throw null; }

        public override FileInfoBase GetFile(string name) { throw null; }
    }

    public abstract partial class FileInfoBase : FileSystemInfoBase
    {
        protected FileInfoBase() { }
    }

    public partial class FileInfoWrapper : FileInfoBase
    {
        public FileInfoWrapper(System.IO.FileInfo fileInfo) { }

        public override string FullName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override DirectoryInfoBase? ParentDirectory { get { throw null; } }
    }

    public abstract partial class FileSystemInfoBase
    {
        protected FileSystemInfoBase() { }

        public abstract string FullName { get; }
        public abstract string Name { get; }
        public abstract DirectoryInfoBase? ParentDirectory { get; }
    }
}

namespace Microsoft.Extensions.FileSystemGlobbing.Internal
{
    public partial interface ILinearPattern : IPattern
    {
        System.Collections.Generic.IList<IPathSegment> Segments { get; }
    }

    public partial interface IPathSegment
    {
        bool CanProduceStem { get; }

        bool Match(string value);
    }

    public partial interface IPattern
    {
        IPatternContext CreatePatternContextForExclude();
        IPatternContext CreatePatternContextForInclude();
    }

    public partial interface IPatternContext
    {
        void Declare(System.Action<IPathSegment, bool> onDeclare);
        void PopDirectory();
        void PushDirectory(Abstractions.DirectoryInfoBase directory);
        bool Test(Abstractions.DirectoryInfoBase directory);
        PatternTestResult Test(Abstractions.FileInfoBase file);
    }

    public partial interface IRaggedPattern : IPattern
    {
        System.Collections.Generic.IList<System.Collections.Generic.IList<IPathSegment>> Contains { get; }

        System.Collections.Generic.IList<IPathSegment> EndsWith { get; }

        System.Collections.Generic.IList<IPathSegment> Segments { get; }

        System.Collections.Generic.IList<IPathSegment> StartsWith { get; }
    }

    public partial class MatcherContext
    {
        public MatcherContext(System.Collections.Generic.IEnumerable<IPattern> includePatterns, System.Collections.Generic.IEnumerable<IPattern> excludePatterns, Abstractions.DirectoryInfoBase directoryInfo, System.StringComparison comparison) { }

        public PatternMatchingResult Execute() { throw null; }
    }

    public partial struct PatternTestResult
    {
        private object _dummy;
        private int _dummyPrimitive;
        public static readonly PatternTestResult Failed;
        public bool IsSuccessful { get { throw null; } }

        public string? Stem { get { throw null; } }

        public static PatternTestResult Success(string? stem) { throw null; }
    }
}

namespace Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments
{
    public partial class CurrentPathSegment : IPathSegment
    {
        public CurrentPathSegment() { }

        public bool CanProduceStem { get { throw null; } }

        public bool Match(string value) { throw null; }
    }

    public partial class LiteralPathSegment : IPathSegment
    {
        public LiteralPathSegment(string value, System.StringComparison comparisonType) { }

        public bool CanProduceStem { get { throw null; } }

        public string Value { get { throw null; } }

        public override bool Equals(object? obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public bool Match(string value) { throw null; }
    }

    public partial class ParentPathSegment : IPathSegment
    {
        public ParentPathSegment() { }

        public bool CanProduceStem { get { throw null; } }

        public bool Match(string value) { throw null; }
    }

    public partial class RecursiveWildcardSegment : IPathSegment
    {
        public RecursiveWildcardSegment() { }

        public bool CanProduceStem { get { throw null; } }

        public bool Match(string value) { throw null; }
    }

    public partial class WildcardPathSegment : IPathSegment
    {
        public static readonly WildcardPathSegment MatchAll;
        public WildcardPathSegment(string beginsWith, System.Collections.Generic.List<string> contains, string endsWith, System.StringComparison comparisonType) { }

        public string BeginsWith { get { throw null; } }

        public bool CanProduceStem { get { throw null; } }

        public System.Collections.Generic.List<string> Contains { get { throw null; } }

        public string EndsWith { get { throw null; } }

        public bool Match(string value) { throw null; }
    }
}

namespace Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts
{
    public abstract partial class PatternContextLinear : PatternContext<PatternContextLinear.FrameData>
    {
        public PatternContextLinear(ILinearPattern pattern) { }

        protected ILinearPattern Pattern { get { throw null; } }

        protected string CalculateStem(Abstractions.FileInfoBase matchedFile) { throw null; }

        protected bool IsLastSegment() { throw null; }

        public override void PushDirectory(Abstractions.DirectoryInfoBase directory) { }

        public override PatternTestResult Test(Abstractions.FileInfoBase file) { throw null; }

        protected bool TestMatchingSegment(string value) { throw null; }

        public partial struct FrameData
        {
            private object _dummy;
            private int _dummyPrimitive;
            public bool InStem;
            public bool IsNotApplicable;
            public int SegmentIndex;
            public string? Stem { get { throw null; } }

            public System.Collections.Generic.IList<string> StemItems { get { throw null; } }
        }
    }

    public partial class PatternContextLinearExclude : PatternContextLinear
    {
        public PatternContextLinearExclude(ILinearPattern pattern) : base(default!) { }

        public override bool Test(Abstractions.DirectoryInfoBase directory) { throw null; }
    }

    public partial class PatternContextLinearInclude : PatternContextLinear
    {
        public PatternContextLinearInclude(ILinearPattern pattern) : base(default!) { }

        public override void Declare(System.Action<IPathSegment, bool> onDeclare) { }

        public override bool Test(Abstractions.DirectoryInfoBase directory) { throw null; }
    }

    public abstract partial class PatternContextRagged : PatternContext<PatternContextRagged.FrameData>
    {
        public PatternContextRagged(IRaggedPattern pattern) { }

        protected IRaggedPattern Pattern { get { throw null; } }

        protected string CalculateStem(Abstractions.FileInfoBase matchedFile) { throw null; }

        protected bool IsEndingGroup() { throw null; }

        protected bool IsStartingGroup() { throw null; }

        public override void PopDirectory() { }

        public sealed override void PushDirectory(Abstractions.DirectoryInfoBase directory) { }

        public override PatternTestResult Test(Abstractions.FileInfoBase file) { throw null; }

        protected bool TestMatchingGroup(Abstractions.FileSystemInfoBase value) { throw null; }

        protected bool TestMatchingSegment(string value) { throw null; }

        public partial struct FrameData
        {
            private object _dummy;
            private int _dummyPrimitive;
            public int BacktrackAvailable;
            public bool InStem;
            public bool IsNotApplicable;
            public System.Collections.Generic.IList<IPathSegment> SegmentGroup;
            public int SegmentGroupIndex;
            public int SegmentIndex;
            public string? Stem { get { throw null; } }

            public System.Collections.Generic.IList<string> StemItems { get { throw null; } }
        }
    }

    public partial class PatternContextRaggedExclude : PatternContextRagged
    {
        public PatternContextRaggedExclude(IRaggedPattern pattern) : base(default!) { }

        public override bool Test(Abstractions.DirectoryInfoBase directory) { throw null; }
    }

    public partial class PatternContextRaggedInclude : PatternContextRagged
    {
        public PatternContextRaggedInclude(IRaggedPattern pattern) : base(default!) { }

        public override void Declare(System.Action<IPathSegment, bool> onDeclare) { }

        public override bool Test(Abstractions.DirectoryInfoBase directory) { throw null; }
    }

    public abstract partial class PatternContext<TFrame> : IPatternContext where TFrame : struct
    {
        protected TFrame Frame;
        protected PatternContext() { }

        public virtual void Declare(System.Action<IPathSegment, bool> declare) { }

        protected bool IsStackEmpty() { throw null; }

        public virtual void PopDirectory() { }

        protected void PushDataFrame(TFrame frame) { }

        public abstract void PushDirectory(Abstractions.DirectoryInfoBase directory);
        public abstract bool Test(Abstractions.DirectoryInfoBase directory);
        public abstract PatternTestResult Test(Abstractions.FileInfoBase file);
    }
}

namespace Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns
{
    public partial class PatternBuilder
    {
        public PatternBuilder() { }

        public PatternBuilder(System.StringComparison comparisonType) { }

        public System.StringComparison ComparisonType { get { throw null; } }

        public IPattern Build(string pattern) { throw null; }
    }
}