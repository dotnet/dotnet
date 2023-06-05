// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("Microsoft.CodeAnalysis.Common")]
[assembly: AssemblyDescription("Microsoft.CodeAnalysis.Common")]
[assembly: AssemblyDefaultAlias("Microsoft.CodeAnalysis.Common")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.0.121.55815")]
[assembly: AssemblyInformationalVersion("4.0.121.55815 built by: SOURCEBUILD")]
[assembly: CLSCompliant(false)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: InternalsVisibleTo("Microsoft.CodeAnalysis.CSharp, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: InternalsVisibleTo("Microsoft.CodeAnalysis.VisualBasic, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]




namespace Microsoft.CodeAnalysis
{
    public enum Accessibility
    {
        NotApplicable = 0,
        Private = 1,
        ProtectedAndFriend = 2,
        ProtectedAndInternal = 2,
        Protected = 3,
        Friend = 4,
        Internal = 4,
        ProtectedOrFriend = 5,
        ProtectedOrInternal = 5,
        Public = 6,
    }
    public abstract partial class AdditionalText
    {
        protected AdditionalText() { }
        public abstract string Path { get; }
        public abstract Microsoft.CodeAnalysis.Text.SourceText? GetText(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
    public sealed partial class AnalyzerConfig
    {
        internal AnalyzerConfig() { }
        public static Microsoft.CodeAnalysis.AnalyzerConfig Parse(Microsoft.CodeAnalysis.Text.SourceText text, string? pathToFile) { throw null; }
        public static Microsoft.CodeAnalysis.AnalyzerConfig Parse(string text, string? pathToFile) { throw null; }
    }
    public readonly partial struct AnalyzerConfigOptionsResult
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Collections.Immutable.ImmutableDictionary<string, string> AnalyzerOptions { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Diagnostics { get { throw null; } }
        public System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> TreeOptions { get { throw null; } }
    }
    public sealed partial class AnalyzerConfigSet
    {
        internal AnalyzerConfigSet() { }
        public Microsoft.CodeAnalysis.AnalyzerConfigOptionsResult GlobalConfigOptions { get { throw null; } }
        public static Microsoft.CodeAnalysis.AnalyzerConfigSet Create<TList>(TList analyzerConfigs) where TList : System.Collections.Generic.IReadOnlyCollection<Microsoft.CodeAnalysis.AnalyzerConfig> { throw null; }
        public static Microsoft.CodeAnalysis.AnalyzerConfigSet Create<TList>(TList analyzerConfigs, out System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> diagnostics) where TList : System.Collections.Generic.IReadOnlyCollection<Microsoft.CodeAnalysis.AnalyzerConfig> { throw null; }
        public Microsoft.CodeAnalysis.AnalyzerConfigOptionsResult GetOptionsForSourcePath(string sourcePath) { throw null; }
    }
    public static partial class AnnotationExtensions
    {
        public static TNode WithAdditionalAnnotations<TNode>(this TNode node, params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode WithAdditionalAnnotations<TNode>(this TNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode WithoutAnnotations<TNode>(this TNode node, params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode WithoutAnnotations<TNode>(this TNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode WithoutAnnotations<TNode>(this TNode node, string annotationKind) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
    }
    public sealed partial class AssemblyIdentity : System.IEquatable<Microsoft.CodeAnalysis.AssemblyIdentity>
    {
        public AssemblyIdentity(string? name, System.Version? version = null, string? cultureName = null, System.Collections.Immutable.ImmutableArray<byte> publicKeyOrToken = default(System.Collections.Immutable.ImmutableArray<byte>), bool hasPublicKey = false, bool isRetargetable = false, System.Reflection.AssemblyContentType contentType = System.Reflection.AssemblyContentType.Default) { }
        public System.Reflection.AssemblyContentType ContentType { get { throw null; } }
        public string CultureName { get { throw null; } }
        public System.Reflection.AssemblyNameFlags Flags { get { throw null; } }
        public bool HasPublicKey { get { throw null; } }
        public bool IsRetargetable { get { throw null; } }
        public bool IsStrongName { get { throw null; } }
        public string Name { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<byte> PublicKey { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<byte> PublicKeyToken { get { throw null; } }
        public System.Version Version { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.AssemblyIdentity? obj) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyIdentity FromAssemblyDefinition(System.Reflection.Assembly assembly) { throw null; }
        public string GetDisplayName(bool fullKey = false) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.AssemblyIdentity? left, Microsoft.CodeAnalysis.AssemblyIdentity? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.AssemblyIdentity? left, Microsoft.CodeAnalysis.AssemblyIdentity? right) { throw null; }
        public override string ToString() { throw null; }
        public static bool TryParseDisplayName(string displayName, out Microsoft.CodeAnalysis.AssemblyIdentity? identity) { throw null; }
        public static bool TryParseDisplayName(string displayName, out Microsoft.CodeAnalysis.AssemblyIdentity? identity, out Microsoft.CodeAnalysis.AssemblyIdentityParts parts) { throw null; }
    }
    public partial class AssemblyIdentityComparer
    {
        internal AssemblyIdentityComparer() { }
        public static System.StringComparer CultureComparer { get { throw null; } }
        public static Microsoft.CodeAnalysis.AssemblyIdentityComparer Default { get { throw null; } }
        public static System.StringComparer SimpleNameComparer { get { throw null; } }
        public Microsoft.CodeAnalysis.AssemblyIdentityComparer.ComparisonResult Compare(Microsoft.CodeAnalysis.AssemblyIdentity reference, Microsoft.CodeAnalysis.AssemblyIdentity definition) { throw null; }
        public bool ReferenceMatchesDefinition(Microsoft.CodeAnalysis.AssemblyIdentity reference, Microsoft.CodeAnalysis.AssemblyIdentity definition) { throw null; }
        public bool ReferenceMatchesDefinition(string referenceDisplayName, Microsoft.CodeAnalysis.AssemblyIdentity definition) { throw null; }
        public enum ComparisonResult
        {
            NotEquivalent = 0,
            Equivalent = 1,
            EquivalentIgnoringVersion = 2,
        }
    }
    [System.FlagsAttribute]
    public enum AssemblyIdentityParts
    {
        Name = 1,
        VersionMajor = 2,
        VersionMinor = 4,
        VersionBuild = 8,
        VersionRevision = 16,
        Version = 30,
        Culture = 32,
        PublicKey = 64,
        PublicKeyToken = 128,
        PublicKeyOrToken = 192,
        Retargetability = 256,
        ContentType = 512,
        Unknown = 1024,
    }
    public sealed partial class AssemblyMetadata : Microsoft.CodeAnalysis.Metadata
    {
        internal AssemblyMetadata() { }
        public override Microsoft.CodeAnalysis.MetadataImageKind Kind { get { throw null; } }
        protected override Microsoft.CodeAnalysis.Metadata CommonCopy() { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata Create(Microsoft.CodeAnalysis.ModuleMetadata module) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata Create(params Microsoft.CodeAnalysis.ModuleMetadata[] modules) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata Create(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ModuleMetadata> modules) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata Create(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ModuleMetadata> modules) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata CreateFromFile(string path) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata CreateFromImage(System.Collections.Generic.IEnumerable<byte> peImage) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata CreateFromImage(System.Collections.Immutable.ImmutableArray<byte> peImage) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata CreateFromStream(System.IO.Stream peStream, bool leaveOpen = false) { throw null; }
        public static Microsoft.CodeAnalysis.AssemblyMetadata CreateFromStream(System.IO.Stream peStream, System.Reflection.PortableExecutable.PEStreamOptions options) { throw null; }
        public override void Dispose() { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ModuleMetadata> GetModules() { throw null; }
        public Microsoft.CodeAnalysis.PortableExecutableReference GetReference(Microsoft.CodeAnalysis.DocumentationProvider? documentation = null, System.Collections.Immutable.ImmutableArray<string> aliases = default(System.Collections.Immutable.ImmutableArray<string>), bool embedInteropTypes = false, string? filePath = null, string? display = null) { throw null; }
    }
    public abstract partial class AttributeData
    {
        protected AttributeData() { }
        public Microsoft.CodeAnalysis.SyntaxReference? ApplicationSyntaxReference { get { throw null; } }
        public Microsoft.CodeAnalysis.INamedTypeSymbol? AttributeClass { get { throw null; } }
        public Microsoft.CodeAnalysis.IMethodSymbol? AttributeConstructor { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.SyntaxReference? CommonApplicationSyntaxReference { get; }
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol? CommonAttributeClass { get; }
        protected abstract Microsoft.CodeAnalysis.IMethodSymbol? CommonAttributeConstructor { get; }
        protected internal abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.TypedConstant> CommonConstructorArguments { get; }
        protected internal abstract System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, Microsoft.CodeAnalysis.TypedConstant>> CommonNamedArguments { get; }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.TypedConstant> ConstructorArguments { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, Microsoft.CodeAnalysis.TypedConstant>> NamedArguments { get { throw null; } }
    }
    public enum CandidateReason
    {
        None = 0,
        NotATypeOrNamespace = 1,
        NotAnEvent = 2,
        NotAWithEventsMember = 3,
        NotAnAttributeType = 4,
        WrongArity = 5,
        NotCreatable = 6,
        NotReferencable = 7,
        Inaccessible = 8,
        NotAValue = 9,
        NotAVariable = 10,
        NotInvocable = 11,
        StaticInstanceMismatch = 12,
        OverloadResolutionFailure = 13,
        LateBound = 14,
        Ambiguous = 15,
        MemberGroup = 16,
    }
    public static partial class CaseInsensitiveComparison
    {
        public static System.StringComparer Comparer { get { throw null; } }
        public static int Compare(System.ReadOnlySpan<char> left, System.ReadOnlySpan<char> right) { throw null; }
        public static int Compare(string left, string right) { throw null; }
        public static bool EndsWith(string value, string possibleEnd) { throw null; }
        public static bool Equals(System.ReadOnlySpan<char> left, System.ReadOnlySpan<char> right) { throw null; }
        public static bool Equals(string left, string right) { throw null; }
        public static int GetHashCode(string value) { throw null; }
        public static bool StartsWith(string value, string possibleStart) { throw null; }
        public static char ToLower(char c) { throw null; }
        public static string? ToLower(string? value) { throw null; }
        public static void ToLower(System.Text.StringBuilder builder) { }
    }
    public readonly partial struct ChildSyntaxList : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken>, System.Collections.Generic.IReadOnlyCollection<Microsoft.CodeAnalysis.SyntaxNodeOrToken>, System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNodeOrToken>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.ChildSyntaxList>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken this[int index] { get { throw null; } }
        public bool Any() { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.ChildSyntaxList other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken First() { throw null; }
        public Microsoft.CodeAnalysis.ChildSyntaxList.Enumerator GetEnumerator() { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken Last() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.ChildSyntaxList list1, Microsoft.CodeAnalysis.ChildSyntaxList list2) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.ChildSyntaxList list1, Microsoft.CodeAnalysis.ChildSyntaxList list2) { throw null; }
        public Microsoft.CodeAnalysis.ChildSyntaxList.Reversed Reverse() { throw null; }
        System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxNodeOrToken> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Microsoft.CodeAnalysis.SyntaxNodeOrToken Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
        }
        public readonly partial struct Reversed : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.ChildSyntaxList.Reversed>
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public bool Equals(Microsoft.CodeAnalysis.ChildSyntaxList.Reversed other) { throw null; }
            public override bool Equals(object? obj) { throw null; }
            public Microsoft.CodeAnalysis.ChildSyntaxList.Reversed.Enumerator GetEnumerator() { throw null; }
            public override int GetHashCode() { throw null; }
            System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxNodeOrToken> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken>.GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            public partial struct Enumerator
            {
                private object _dummy;
                private int _dummyPrimitive;
                public Microsoft.CodeAnalysis.SyntaxNodeOrToken Current { get { throw null; } }
                public bool MoveNext() { throw null; }
                public void Reset() { }
            }
        }
    }
    public partial struct CommandLineAnalyzerReference : System.IEquatable<Microsoft.CodeAnalysis.CommandLineAnalyzerReference>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public CommandLineAnalyzerReference(string path) { throw null; }
        public string FilePath { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.CommandLineAnalyzerReference other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public abstract partial class CommandLineArguments
    {
        internal CommandLineArguments() { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CommandLineSourceFile> AdditionalFiles { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<string> AnalyzerConfigPaths { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CommandLineAnalyzerReference> AnalyzerReferences { get { throw null; } }
        public string? AppConfigPath { get { throw null; } }
        public string? BaseDirectory { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceHashAlgorithm ChecksumAlgorithm { get { throw null; } }
        public string? CompilationName { get { throw null; } }
        public Microsoft.CodeAnalysis.CompilationOptions CompilationOptions { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CompilationOptionsCore { get; }
        public bool DisplayHelp { get { throw null; } }
        public bool DisplayLangVersions { get { throw null; } }
        public bool DisplayLogo { get { throw null; } }
        public bool DisplayVersion { get { throw null; } }
        public string? DocumentationPath { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CommandLineSourceFile> EmbeddedFiles { get { throw null; } }
        public Microsoft.CodeAnalysis.Emit.EmitOptions EmitOptions { get { throw null; } }
        public bool EmitPdb { get { throw null; } }
        public bool EmitPdbFile { get { throw null; } }
        public System.Text.Encoding? Encoding { get { throw null; } }
        public Microsoft.CodeAnalysis.ErrorLogOptions? ErrorLogOptions { get { throw null; } }
        public string? ErrorLogPath { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Errors { get { throw null; } }
        public string? GeneratedFilesOutputDirectory { get { throw null; } }
        public bool InteractiveMode { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<string> KeyFileSearchPaths { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ResourceDescription> ManifestResources { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CommandLineReference> MetadataReferences { get { throw null; } }
        public bool NoWin32Manifest { get { throw null; } }
        public string OutputDirectory { get { throw null; } }
        public string? OutputFileName { get { throw null; } }
        public string? OutputRefFilePath { get { throw null; } }
        public Microsoft.CodeAnalysis.ParseOptions ParseOptions { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.ParseOptions ParseOptionsCore { get; }
        public System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, string>> PathMap { get { throw null; } }
        public string? PdbPath { get { throw null; } }
        public System.Globalization.CultureInfo? PreferredUILang { get { throw null; } }
        public bool PrintFullPaths { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<string> ReferencePaths { get { throw null; } }
        public bool ReportAnalyzer { get { throw null; } }
        public string? RuleSetPath { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<string> ScriptArguments { get { throw null; } }
        public bool SkipAnalyzers { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CommandLineSourceFile> SourceFiles { get { throw null; } }
        public string? SourceLink { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<string> SourcePaths { get { throw null; } }
        public string? TouchedFilesPath { get { throw null; } }
        public bool Utf8Output { get { throw null; } }
        public string? Win32Icon { get { throw null; } }
        public string? Win32Manifest { get { throw null; } }
        public string? Win32ResourceFile { get { throw null; } }
        public string GetOutputFilePath(string outputFileName) { throw null; }
        public string GetPdbFilePath(string outputFileName) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> ResolveAnalyzerReferences(Microsoft.CodeAnalysis.IAnalyzerAssemblyLoader analyzerLoader) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> ResolveMetadataReferences(Microsoft.CodeAnalysis.MetadataReferenceResolver metadataResolver) { throw null; }
    }
    public abstract partial class CommandLineParser
    {
        internal CommandLineParser() { }
        protected abstract string RegularFileExtension { get; }
        protected abstract string ScriptFileExtension { get; }
        public Microsoft.CodeAnalysis.CommandLineArguments Parse(System.Collections.Generic.IEnumerable<string> args, string baseDirectory, string? sdkDirectory, string? additionalReferenceDirectories) { throw null; }
        protected System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, string>> ParsePathMap(string pathMap, System.Collections.Generic.IList<Microsoft.CodeAnalysis.Diagnostic> errors) { throw null; }
        public static System.Collections.Generic.IEnumerable<string> SplitCommandLineIntoArguments(string commandLine, bool removeHashComments) { throw null; }
    }
    public partial struct CommandLineReference : System.IEquatable<Microsoft.CodeAnalysis.CommandLineReference>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public CommandLineReference(string reference, Microsoft.CodeAnalysis.MetadataReferenceProperties properties) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReferenceProperties Properties { get { throw null; } }
        public string Reference { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.CommandLineReference other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public partial struct CommandLineSourceFile
    {
        private object _dummy;
        private int _dummyPrimitive;
        public CommandLineSourceFile(string path, bool isScript) { throw null; }
        public CommandLineSourceFile(string path, bool isScript, bool isInputRedirected) { throw null; }
        public bool IsInputRedirected { get { throw null; } }
        public bool IsScript { get { throw null; } }
        public string Path { get { throw null; } }
    }
    public abstract partial class Compilation
    {
        internal Compilation() { }
        protected readonly System.Collections.Generic.IReadOnlyDictionary<string, string> _features;
        public Microsoft.CodeAnalysis.IAssemblySymbol Assembly { get { throw null; } }
        public string? AssemblyName { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.IAssemblySymbol CommonAssembly { get; }
        protected abstract Microsoft.CodeAnalysis.ITypeSymbol CommonDynamicType { get; }
        protected abstract Microsoft.CodeAnalysis.INamespaceSymbol CommonGlobalNamespace { get; }
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol CommonObjectType { get; }
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonOptions { get; }
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol? CommonScriptClass { get; }
        protected abstract Microsoft.CodeAnalysis.ITypeSymbol? CommonScriptGlobalsType { get; }
        protected abstract Microsoft.CodeAnalysis.IModuleSymbol CommonSourceModule { get; }
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SyntaxTree> CommonSyntaxTrees { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.MetadataReference> DirectiveReferences { get; }
        public Microsoft.CodeAnalysis.ITypeSymbol DynamicType { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.MetadataReference> ExternalReferences { get { throw null; } }
        public Microsoft.CodeAnalysis.INamespaceSymbol GlobalNamespace { get { throw null; } }
        public abstract bool IsCaseSensitive { get; }
        public abstract string Language { get; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol ObjectType { get { throw null; } }
        public Microsoft.CodeAnalysis.CompilationOptions Options { get { throw null; } }
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.AssemblyIdentity> ReferencedAssemblyNames { get; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> References { get { throw null; } }
        public Microsoft.CodeAnalysis.INamedTypeSymbol? ScriptClass { get { throw null; } }
        public Microsoft.CodeAnalysis.ScriptCompilationInfo? ScriptCompilationInfo { get { throw null; } }
        public Microsoft.CodeAnalysis.IModuleSymbol SourceModule { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> SyntaxTrees { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation AddReferences(params Microsoft.CodeAnalysis.MetadataReference[] references) { throw null; }
        public Microsoft.CodeAnalysis.Compilation AddReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references) { throw null; }
        public Microsoft.CodeAnalysis.Compilation AddSyntaxTrees(params Microsoft.CodeAnalysis.SyntaxTree[] trees) { throw null; }
        public Microsoft.CodeAnalysis.Compilation AddSyntaxTrees(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> trees) { throw null; }
        protected abstract void AppendDefaultVersionResource(System.IO.Stream resourceStream);
        protected static void CheckTupleElementLocations(int cardinality, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location?> elementLocations) { }
        protected static System.Collections.Immutable.ImmutableArray<string?> CheckTupleElementNames(int cardinality, System.Collections.Immutable.ImmutableArray<string?> elementNames) { throw null; }
        protected static void CheckTupleElementNullableAnnotations(int cardinality, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> elementNullableAnnotations) { }
        public abstract Microsoft.CodeAnalysis.Operations.CommonConversion ClassifyCommonConversion(Microsoft.CodeAnalysis.ITypeSymbol source, Microsoft.CodeAnalysis.ITypeSymbol destination);
        public Microsoft.CodeAnalysis.Compilation Clone() { throw null; }
        protected abstract Microsoft.CodeAnalysis.Compilation CommonAddSyntaxTrees(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> trees);
        protected Microsoft.CodeAnalysis.INamedTypeSymbol? CommonBindScriptClass() { throw null; }
        protected abstract Microsoft.CodeAnalysis.Compilation CommonClone();
        protected abstract bool CommonContainsSyntaxTree(Microsoft.CodeAnalysis.SyntaxTree? syntaxTree);
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol CommonCreateAnonymousTypeSymbol(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> memberTypes, System.Collections.Immutable.ImmutableArray<string> memberNames, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location> memberLocations, System.Collections.Immutable.ImmutableArray<bool> memberIsReadOnly, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> memberNullableAnnotations);
        protected abstract Microsoft.CodeAnalysis.IArrayTypeSymbol CommonCreateArrayTypeSymbol(Microsoft.CodeAnalysis.ITypeSymbol elementType, int rank, Microsoft.CodeAnalysis.NullableAnnotation elementNullableAnnotation);
        protected abstract Microsoft.CodeAnalysis.INamespaceSymbol CommonCreateErrorNamespaceSymbol(Microsoft.CodeAnalysis.INamespaceSymbol container, string name);
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol CommonCreateErrorTypeSymbol(Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container, string name, int arity);
        protected abstract Microsoft.CodeAnalysis.IFunctionPointerTypeSymbol CommonCreateFunctionPointerTypeSymbol(Microsoft.CodeAnalysis.ITypeSymbol returnType, Microsoft.CodeAnalysis.RefKind returnRefKind, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> parameterTypes, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.RefKind> parameterRefKinds, System.Reflection.Metadata.SignatureCallingConvention callingConvention, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> callingConventionTypes);
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol CommonCreateNativeIntegerTypeSymbol(bool signed);
        protected abstract Microsoft.CodeAnalysis.IPointerTypeSymbol CommonCreatePointerTypeSymbol(Microsoft.CodeAnalysis.ITypeSymbol elementType);
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol CommonCreateTupleTypeSymbol(Microsoft.CodeAnalysis.INamedTypeSymbol underlyingType, System.Collections.Immutable.ImmutableArray<string?> elementNames, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location?> elementLocations, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> elementNullableAnnotations);
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol CommonCreateTupleTypeSymbol(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> elementTypes, System.Collections.Immutable.ImmutableArray<string?> elementNames, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location?> elementLocations, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> elementNullableAnnotations);
        protected abstract Microsoft.CodeAnalysis.ISymbol? CommonGetAssemblyOrModuleSymbol(Microsoft.CodeAnalysis.MetadataReference reference);
        protected abstract Microsoft.CodeAnalysis.INamespaceSymbol? CommonGetCompilationNamespace(Microsoft.CodeAnalysis.INamespaceSymbol namespaceSymbol);
        protected abstract Microsoft.CodeAnalysis.IMethodSymbol? CommonGetEntryPoint(System.Threading.CancellationToken cancellationToken);
        protected abstract Microsoft.CodeAnalysis.SemanticModel CommonGetSemanticModel(Microsoft.CodeAnalysis.SyntaxTree syntaxTree, bool ignoreAccessibility);
        protected abstract Microsoft.CodeAnalysis.INamedTypeSymbol? CommonGetTypeByMetadataName(string metadataName);
        protected abstract Microsoft.CodeAnalysis.Compilation CommonRemoveAllSyntaxTrees();
        protected abstract Microsoft.CodeAnalysis.Compilation CommonRemoveSyntaxTrees(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> trees);
        protected abstract Microsoft.CodeAnalysis.Compilation CommonReplaceSyntaxTree(Microsoft.CodeAnalysis.SyntaxTree oldTree, Microsoft.CodeAnalysis.SyntaxTree newTree);
        protected abstract Microsoft.CodeAnalysis.Compilation CommonWithAssemblyName(string? outputName);
        protected abstract Microsoft.CodeAnalysis.Compilation CommonWithOptions(Microsoft.CodeAnalysis.CompilationOptions options);
        protected abstract Microsoft.CodeAnalysis.Compilation CommonWithReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> newReferences);
        protected abstract Microsoft.CodeAnalysis.Compilation CommonWithScriptCompilationInfo(Microsoft.CodeAnalysis.ScriptCompilationInfo? info);
        public abstract bool ContainsSymbolsWithName(System.Func<string, bool> predicate, Microsoft.CodeAnalysis.SymbolFilter filter = Microsoft.CodeAnalysis.SymbolFilter.TypeAndMember, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract bool ContainsSymbolsWithName(string name, Microsoft.CodeAnalysis.SymbolFilter filter = Microsoft.CodeAnalysis.SymbolFilter.TypeAndMember, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public bool ContainsSyntaxTree(Microsoft.CodeAnalysis.SyntaxTree syntaxTree) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateAnonymousTypeSymbol(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> memberTypes, System.Collections.Immutable.ImmutableArray<string> memberNames, System.Collections.Immutable.ImmutableArray<bool> memberIsReadOnly, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location> memberLocations) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateAnonymousTypeSymbol(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> memberTypes, System.Collections.Immutable.ImmutableArray<string> memberNames, System.Collections.Immutable.ImmutableArray<bool> memberIsReadOnly = default(System.Collections.Immutable.ImmutableArray<bool>), System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location> memberLocations = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location>), System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> memberNullableAnnotations = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation>)) { throw null; }
        public Microsoft.CodeAnalysis.IArrayTypeSymbol CreateArrayTypeSymbol(Microsoft.CodeAnalysis.ITypeSymbol elementType, int rank) { throw null; }
        public Microsoft.CodeAnalysis.IArrayTypeSymbol CreateArrayTypeSymbol(Microsoft.CodeAnalysis.ITypeSymbol elementType, int rank = 1, Microsoft.CodeAnalysis.NullableAnnotation elementNullableAnnotation = Microsoft.CodeAnalysis.NullableAnnotation.None) { throw null; }
        public System.IO.Stream CreateDefaultWin32Resources(bool versionResource, bool noManifest, System.IO.Stream? manifestContents, System.IO.Stream? iconInIcoFormat) { throw null; }
        public Microsoft.CodeAnalysis.INamespaceSymbol CreateErrorNamespaceSymbol(Microsoft.CodeAnalysis.INamespaceSymbol container, string name) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateErrorTypeSymbol(Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container, string name, int arity) { throw null; }
        public Microsoft.CodeAnalysis.IFunctionPointerTypeSymbol CreateFunctionPointerTypeSymbol(Microsoft.CodeAnalysis.ITypeSymbol returnType, Microsoft.CodeAnalysis.RefKind returnRefKind, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> parameterTypes, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.RefKind> parameterRefKinds, System.Reflection.Metadata.SignatureCallingConvention callingConvention = System.Reflection.Metadata.SignatureCallingConvention.Default, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> callingConventionTypes = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol>)) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateNativeIntegerTypeSymbol(bool signed) { throw null; }
        public Microsoft.CodeAnalysis.IPointerTypeSymbol CreatePointerTypeSymbol(Microsoft.CodeAnalysis.ITypeSymbol pointedAtType) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateTupleTypeSymbol(Microsoft.CodeAnalysis.INamedTypeSymbol underlyingType, System.Collections.Immutable.ImmutableArray<string?> elementNames, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location?> elementLocations) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateTupleTypeSymbol(Microsoft.CodeAnalysis.INamedTypeSymbol underlyingType, System.Collections.Immutable.ImmutableArray<string?> elementNames = default(System.Collections.Immutable.ImmutableArray<string>), System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location?> elementLocations = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location>), System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> elementNullableAnnotations = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation>)) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateTupleTypeSymbol(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> elementTypes, System.Collections.Immutable.ImmutableArray<string?> elementNames, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location?> elementLocations) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol CreateTupleTypeSymbol(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> elementTypes, System.Collections.Immutable.ImmutableArray<string?> elementNames = default(System.Collections.Immutable.ImmutableArray<string>), System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location?> elementLocations = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location>), System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> elementNullableAnnotations = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation>)) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitResult Emit(System.IO.Stream peStream, System.IO.Stream? pdbStream = null, System.IO.Stream? xmlDocumentationStream = null, System.IO.Stream? win32Resources = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ResourceDescription>? manifestResources = null, Microsoft.CodeAnalysis.Emit.EmitOptions? options = null, Microsoft.CodeAnalysis.IMethodSymbol? debugEntryPoint = null, System.IO.Stream? sourceLinkStream = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.EmbeddedText>? embeddedTexts = null, System.IO.Stream? metadataPEStream = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitResult Emit(System.IO.Stream peStream, System.IO.Stream? pdbStream, System.IO.Stream? xmlDocumentationStream, System.IO.Stream? win32Resources, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ResourceDescription>? manifestResources, Microsoft.CodeAnalysis.Emit.EmitOptions options, Microsoft.CodeAnalysis.IMethodSymbol? debugEntryPoint, System.IO.Stream? sourceLinkStream, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.EmbeddedText>? embeddedTexts, System.Threading.CancellationToken cancellationToken) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitResult Emit(System.IO.Stream peStream, System.IO.Stream pdbStream, System.IO.Stream xmlDocumentationStream, System.IO.Stream win32Resources, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ResourceDescription> manifestResources, Microsoft.CodeAnalysis.Emit.EmitOptions options, Microsoft.CodeAnalysis.IMethodSymbol debugEntryPoint, System.Threading.CancellationToken cancellationToken) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitResult Emit(System.IO.Stream peStream, System.IO.Stream? pdbStream, System.IO.Stream? xmlDocumentationStream, System.IO.Stream? win32Resources, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ResourceDescription>? manifestResources, Microsoft.CodeAnalysis.Emit.EmitOptions options, System.Threading.CancellationToken cancellationToken) { throw null; }
        [System.ObsoleteAttribute("UpdatedMethods is now part of EmitDifferenceResult, so you should use an overload that doesn't take it.")]
        public Microsoft.CodeAnalysis.Emit.EmitDifferenceResult EmitDifference(Microsoft.CodeAnalysis.Emit.EmitBaseline baseline, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Emit.SemanticEdit> edits, System.Func<Microsoft.CodeAnalysis.ISymbol, bool> isAddedSymbol, System.IO.Stream metadataStream, System.IO.Stream ilStream, System.IO.Stream pdbStream, System.Collections.Generic.ICollection<System.Reflection.Metadata.MethodDefinitionHandle> updatedMethods, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitDifferenceResult EmitDifference(Microsoft.CodeAnalysis.Emit.EmitBaseline baseline, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Emit.SemanticEdit> edits, System.Func<Microsoft.CodeAnalysis.ISymbol, bool> isAddedSymbol, System.IO.Stream metadataStream, System.IO.Stream ilStream, System.IO.Stream pdbStream, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        [System.ObsoleteAttribute("UpdatedMethods is now part of EmitDifferenceResult, so you should use an overload that doesn't take it.")]
        public Microsoft.CodeAnalysis.Emit.EmitDifferenceResult EmitDifference(Microsoft.CodeAnalysis.Emit.EmitBaseline baseline, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Emit.SemanticEdit> edits, System.IO.Stream metadataStream, System.IO.Stream ilStream, System.IO.Stream pdbStream, System.Collections.Generic.ICollection<System.Reflection.Metadata.MethodDefinitionHandle> updatedMethods, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.ISymbol? GetAssemblyOrModuleSymbol(Microsoft.CodeAnalysis.MetadataReference reference) { throw null; }
        public Microsoft.CodeAnalysis.INamespaceSymbol? GetCompilationNamespace(Microsoft.CodeAnalysis.INamespaceSymbol namespaceSymbol) { throw null; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetDeclarationDiagnostics(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public Microsoft.CodeAnalysis.IMethodSymbol? GetEntryPoint(System.Threading.CancellationToken cancellationToken) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReference? GetMetadataReference(Microsoft.CodeAnalysis.IAssemblySymbol assemblySymbol) { throw null; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetMethodBodyDiagnostics(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetParseDiagnostics(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public static string? GetRequiredLanguageVersion(Microsoft.CodeAnalysis.Diagnostic diagnostic) { throw null; }
        public Microsoft.CodeAnalysis.SemanticModel GetSemanticModel(Microsoft.CodeAnalysis.SyntaxTree syntaxTree, bool ignoreAccessibility = false) { throw null; }
        public Microsoft.CodeAnalysis.INamedTypeSymbol GetSpecialType(Microsoft.CodeAnalysis.SpecialType specialType) { throw null; }
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol> GetSymbolsWithName(System.Func<string, bool> predicate, Microsoft.CodeAnalysis.SymbolFilter filter = Microsoft.CodeAnalysis.SymbolFilter.TypeAndMember, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol> GetSymbolsWithName(string name, Microsoft.CodeAnalysis.SymbolFilter filter = Microsoft.CodeAnalysis.SymbolFilter.TypeAndMember, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public Microsoft.CodeAnalysis.INamedTypeSymbol? GetTypeByMetadataName(string fullyQualifiedMetadataName) { throw null; }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AssemblyIdentity> GetUnreferencedAssemblyIdentities(Microsoft.CodeAnalysis.Diagnostic diagnostic) { throw null; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.MetadataReference> GetUsedAssemblyReferences(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public bool HasImplicitConversion(Microsoft.CodeAnalysis.ITypeSymbol? fromType, Microsoft.CodeAnalysis.ITypeSymbol? toType) { throw null; }
        public bool IsSymbolAccessibleWithin(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.ISymbol within, Microsoft.CodeAnalysis.ITypeSymbol? throughType = null) { throw null; }
        public Microsoft.CodeAnalysis.Compilation RemoveAllReferences() { throw null; }
        public Microsoft.CodeAnalysis.Compilation RemoveAllSyntaxTrees() { throw null; }
        public Microsoft.CodeAnalysis.Compilation RemoveReferences(params Microsoft.CodeAnalysis.MetadataReference[] references) { throw null; }
        public Microsoft.CodeAnalysis.Compilation RemoveReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> references) { throw null; }
        public Microsoft.CodeAnalysis.Compilation RemoveSyntaxTrees(params Microsoft.CodeAnalysis.SyntaxTree[] trees) { throw null; }
        public Microsoft.CodeAnalysis.Compilation RemoveSyntaxTrees(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> trees) { throw null; }
        public Microsoft.CodeAnalysis.Compilation ReplaceReference(Microsoft.CodeAnalysis.MetadataReference oldReference, Microsoft.CodeAnalysis.MetadataReference? newReference) { throw null; }
        public Microsoft.CodeAnalysis.Compilation ReplaceSyntaxTree(Microsoft.CodeAnalysis.SyntaxTree oldTree, Microsoft.CodeAnalysis.SyntaxTree newTree) { throw null; }
        protected static System.Collections.Generic.IReadOnlyDictionary<string, string> SyntaxTreeCommonFeatures(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> trees) { throw null; }
        public abstract Microsoft.CodeAnalysis.CompilationReference ToMetadataReference(System.Collections.Immutable.ImmutableArray<string> aliases = default(System.Collections.Immutable.ImmutableArray<string>), bool embedInteropTypes = false);
        public Microsoft.CodeAnalysis.Compilation WithAssemblyName(string? assemblyName) { throw null; }
        public Microsoft.CodeAnalysis.Compilation WithOptions(Microsoft.CodeAnalysis.CompilationOptions options) { throw null; }
        public Microsoft.CodeAnalysis.Compilation WithReferences(params Microsoft.CodeAnalysis.MetadataReference[] newReferences) { throw null; }
        public Microsoft.CodeAnalysis.Compilation WithReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> newReferences) { throw null; }
        public Microsoft.CodeAnalysis.Compilation WithScriptCompilationInfo(Microsoft.CodeAnalysis.ScriptCompilationInfo? info) { throw null; }
    }
    public abstract partial class CompilationOptions
    {
        internal CompilationOptions() { }
        public Microsoft.CodeAnalysis.AssemblyIdentityComparer AssemblyIdentityComparer { get { throw null; } protected set { } }
        public bool CheckOverflow { get { throw null; } protected set { } }
        public bool ConcurrentBuild { get { throw null; } protected set { } }
        public string? CryptoKeyContainer { get { throw null; } protected set { } }
        public string? CryptoKeyFile { get { throw null; } protected set { } }
        public System.Collections.Immutable.ImmutableArray<byte> CryptoPublicKey { get { throw null; } protected set { } }
        public bool? DelaySign { get { throw null; } protected set { } }
        public bool Deterministic { get { throw null; } protected set { } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Errors { get { throw null; } }
        [System.ObsoleteAttribute]
        protected internal System.Collections.Immutable.ImmutableArray<string> Features { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.ReportDiagnostic GeneralDiagnosticOption { get { throw null; } protected set { } }
        public abstract string Language { get; }
        public string? MainTypeName { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.MetadataImportOptions MetadataImportOptions { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.MetadataReferenceResolver? MetadataReferenceResolver { get { throw null; } protected set { } }
        public string? ModuleName { get { throw null; } protected set { } }
        public abstract Microsoft.CodeAnalysis.NullableContextOptions NullableContextOptions { get; protected set; }
        public Microsoft.CodeAnalysis.OptimizationLevel OptimizationLevel { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.OutputKind OutputKind { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.Platform Platform { get { throw null; } protected set { } }
        public bool PublicSign { get { throw null; } protected set { } }
        public bool ReportSuppressedDiagnostics { get { throw null; } protected set { } }
        public string? ScriptClassName { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.SourceReferenceResolver? SourceReferenceResolver { get { throw null; } protected set { } }
        public System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> SpecificDiagnosticOptions { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.StrongNameProvider? StrongNameProvider { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.SyntaxTreeOptionsProvider? SyntaxTreeOptionsProvider { get { throw null; } protected set { } }
        public int WarningLevel { get { throw null; } protected set { } }
        public Microsoft.CodeAnalysis.XmlReferenceResolver? XmlReferenceResolver { get { throw null; } protected set { } }
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithAssemblyIdentityComparer(Microsoft.CodeAnalysis.AssemblyIdentityComparer? comparer);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithCheckOverflow(bool checkOverflow);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithConcurrentBuild(bool concurrent);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithCryptoKeyContainer(string? cryptoKeyContainer);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithCryptoKeyFile(string? cryptoKeyFile);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithCryptoPublicKey(System.Collections.Immutable.ImmutableArray<byte> cryptoPublicKey);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithDelaySign(bool? delaySign);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithDeterministic(bool deterministic);
        [System.ObsoleteAttribute]
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithFeatures(System.Collections.Immutable.ImmutableArray<string> features);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithGeneralDiagnosticOption(Microsoft.CodeAnalysis.ReportDiagnostic generalDiagnosticOption);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithMainTypeName(string? mainTypeName);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithMetadataImportOptions(Microsoft.CodeAnalysis.MetadataImportOptions value);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithMetadataReferenceResolver(Microsoft.CodeAnalysis.MetadataReferenceResolver? resolver);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithModuleName(string? moduleName);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel value);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithOutputKind(Microsoft.CodeAnalysis.OutputKind kind);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithPlatform(Microsoft.CodeAnalysis.Platform platform);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithPublicSign(bool publicSign);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithReportSuppressedDiagnostics(bool reportSuppressedDiagnostics);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithScriptClassName(string scriptClassName);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithSourceReferenceResolver(Microsoft.CodeAnalysis.SourceReferenceResolver? resolver);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithSpecificDiagnosticOptions(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, Microsoft.CodeAnalysis.ReportDiagnostic>> specificDiagnosticOptions);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithSpecificDiagnosticOptions(System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic>? specificDiagnosticOptions);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithStrongNameProvider(Microsoft.CodeAnalysis.StrongNameProvider? provider);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithSyntaxTreeOptionsProvider(Microsoft.CodeAnalysis.SyntaxTreeOptionsProvider? resolver);
        protected abstract Microsoft.CodeAnalysis.CompilationOptions CommonWithXmlReferenceResolver(Microsoft.CodeAnalysis.XmlReferenceResolver? resolver);
        public abstract override bool Equals(object? obj);
        protected bool EqualsHelper(Microsoft.CodeAnalysis.CompilationOptions? other) { throw null; }
        public abstract override int GetHashCode();
        protected int GetHashCodeHelper() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.CompilationOptions? left, Microsoft.CodeAnalysis.CompilationOptions? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.CompilationOptions? left, Microsoft.CodeAnalysis.CompilationOptions? right) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithAssemblyIdentityComparer(Microsoft.CodeAnalysis.AssemblyIdentityComparer comparer) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithConcurrentBuild(bool concurrent) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithCryptoKeyContainer(string? cryptoKeyContainer) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithCryptoKeyFile(string? cryptoKeyFile) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithCryptoPublicKey(System.Collections.Immutable.ImmutableArray<byte> cryptoPublicKey) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithDelaySign(bool? delaySign) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithDeterministic(bool deterministic) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithGeneralDiagnosticOption(Microsoft.CodeAnalysis.ReportDiagnostic value) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithMainTypeName(string? mainTypeName) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithMetadataImportOptions(Microsoft.CodeAnalysis.MetadataImportOptions value) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithMetadataReferenceResolver(Microsoft.CodeAnalysis.MetadataReferenceResolver? resolver) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithModuleName(string? moduleName) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel value) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithOutputKind(Microsoft.CodeAnalysis.OutputKind kind) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithOverflowChecks(bool checkOverflow) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithPlatform(Microsoft.CodeAnalysis.Platform platform) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithPublicSign(bool publicSign) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithReportSuppressedDiagnostics(bool value) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithScriptClassName(string scriptClassName) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithSourceReferenceResolver(Microsoft.CodeAnalysis.SourceReferenceResolver? resolver) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithSpecificDiagnosticOptions(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, Microsoft.CodeAnalysis.ReportDiagnostic>> value) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithSpecificDiagnosticOptions(System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic>? value) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithStrongNameProvider(Microsoft.CodeAnalysis.StrongNameProvider? provider) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithSyntaxTreeOptionsProvider(Microsoft.CodeAnalysis.SyntaxTreeOptionsProvider? provider) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOptions WithXmlReferenceResolver(Microsoft.CodeAnalysis.XmlReferenceResolver? resolver) { throw null; }
    }
    public abstract partial class CompilationReference : Microsoft.CodeAnalysis.MetadataReference, System.IEquatable<Microsoft.CodeAnalysis.CompilationReference>
    {
        internal CompilationReference() : base (default(Microsoft.CodeAnalysis.MetadataReferenceProperties)) { }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public override string? Display { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.CompilationReference? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public new Microsoft.CodeAnalysis.CompilationReference WithAliases(System.Collections.Generic.IEnumerable<string> aliases) { throw null; }
        public new Microsoft.CodeAnalysis.CompilationReference WithAliases(System.Collections.Immutable.ImmutableArray<string> aliases) { throw null; }
        public new Microsoft.CodeAnalysis.CompilationReference WithEmbedInteropTypes(bool value) { throw null; }
        public new Microsoft.CodeAnalysis.CompilationReference WithProperties(Microsoft.CodeAnalysis.MetadataReferenceProperties properties) { throw null; }
    }
    public abstract partial class ControlFlowAnalysis
    {
        protected ControlFlowAnalysis() { }
        public abstract bool EndPointIsReachable { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SyntaxNode> EntryPoints { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SyntaxNode> ExitPoints { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SyntaxNode> ReturnStatements { get; }
        public abstract bool StartPointIsReachable { get; }
        public abstract bool Succeeded { get; }
    }
    public abstract partial class CustomModifier
    {
        protected CustomModifier() { }
        public abstract bool IsOptional { get; }
        public abstract Microsoft.CodeAnalysis.INamedTypeSymbol Modifier { get; }
    }
    public abstract partial class DataFlowAnalysis
    {
        protected DataFlowAnalysis() { }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> AlwaysAssigned { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> Captured { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> CapturedInside { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> CapturedOutside { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> DataFlowsIn { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> DataFlowsOut { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> DefinitelyAssignedOnEntry { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> DefinitelyAssignedOnExit { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> ReadInside { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> ReadOutside { get; }
        public abstract bool Succeeded { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> UnsafeAddressTaken { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IMethodSymbol> UsedLocalFunctions { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> VariablesDeclared { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> WrittenInside { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> WrittenOutside { get; }
    }
    public sealed partial class DesktopAssemblyIdentityComparer : Microsoft.CodeAnalysis.AssemblyIdentityComparer
    {
        internal DesktopAssemblyIdentityComparer() { }
        public static new Microsoft.CodeAnalysis.DesktopAssemblyIdentityComparer Default { get { throw null; } }
        public static Microsoft.CodeAnalysis.DesktopAssemblyIdentityComparer LoadFromXml(System.IO.Stream input) { throw null; }
    }
    public partial class DesktopStrongNameProvider : Microsoft.CodeAnalysis.StrongNameProvider
    {
        public DesktopStrongNameProvider(System.Collections.Immutable.ImmutableArray<string> keyFileSearchPaths) { }
        public DesktopStrongNameProvider(System.Collections.Immutable.ImmutableArray<string> keyFileSearchPaths = default(System.Collections.Immutable.ImmutableArray<string>), string? tempPath = null) { }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public abstract partial class Diagnostic : System.IEquatable<Microsoft.CodeAnalysis.Diagnostic?>, System.IFormattable
    {
        protected Diagnostic() { }
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Location> AdditionalLocations { get; }
        public virtual Microsoft.CodeAnalysis.DiagnosticSeverity DefaultSeverity { get { throw null; } }
        public abstract Microsoft.CodeAnalysis.DiagnosticDescriptor Descriptor { get; }
        public abstract string Id { get; }
        public abstract bool IsSuppressed { get; }
        public bool IsWarningAsError { get { throw null; } }
        public abstract Microsoft.CodeAnalysis.Location Location { get; }
        public virtual System.Collections.Immutable.ImmutableDictionary<string, string?> Properties { get { throw null; } }
        public abstract Microsoft.CodeAnalysis.DiagnosticSeverity Severity { get; }
        public abstract int WarningLevel { get; }
        public static Microsoft.CodeAnalysis.Diagnostic Create(Microsoft.CodeAnalysis.DiagnosticDescriptor descriptor, Microsoft.CodeAnalysis.Location? location, Microsoft.CodeAnalysis.DiagnosticSeverity effectiveSeverity, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Location>? additionalLocations, System.Collections.Immutable.ImmutableDictionary<string, string?>? properties, params object?[]? messageArgs) { throw null; }
        public static Microsoft.CodeAnalysis.Diagnostic Create(Microsoft.CodeAnalysis.DiagnosticDescriptor descriptor, Microsoft.CodeAnalysis.Location? location, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Location>? additionalLocations, System.Collections.Immutable.ImmutableDictionary<string, string?>? properties, params object?[]? messageArgs) { throw null; }
        public static Microsoft.CodeAnalysis.Diagnostic Create(Microsoft.CodeAnalysis.DiagnosticDescriptor descriptor, Microsoft.CodeAnalysis.Location? location, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Location>? additionalLocations, params object?[]? messageArgs) { throw null; }
        public static Microsoft.CodeAnalysis.Diagnostic Create(Microsoft.CodeAnalysis.DiagnosticDescriptor descriptor, Microsoft.CodeAnalysis.Location? location, System.Collections.Immutable.ImmutableDictionary<string, string?>? properties, params object?[]? messageArgs) { throw null; }
        public static Microsoft.CodeAnalysis.Diagnostic Create(Microsoft.CodeAnalysis.DiagnosticDescriptor descriptor, Microsoft.CodeAnalysis.Location? location, params object?[]? messageArgs) { throw null; }
        public static Microsoft.CodeAnalysis.Diagnostic Create(string id, string category, Microsoft.CodeAnalysis.LocalizableString message, Microsoft.CodeAnalysis.DiagnosticSeverity severity, Microsoft.CodeAnalysis.DiagnosticSeverity defaultSeverity, bool isEnabledByDefault, int warningLevel, Microsoft.CodeAnalysis.LocalizableString? title = null, Microsoft.CodeAnalysis.LocalizableString? description = null, string? helpLink = null, Microsoft.CodeAnalysis.Location? location = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Location>? additionalLocations = null, System.Collections.Generic.IEnumerable<string>? customTags = null, System.Collections.Immutable.ImmutableDictionary<string, string?>? properties = null) { throw null; }
        public static Microsoft.CodeAnalysis.Diagnostic Create(string id, string category, Microsoft.CodeAnalysis.LocalizableString message, Microsoft.CodeAnalysis.DiagnosticSeverity severity, Microsoft.CodeAnalysis.DiagnosticSeverity defaultSeverity, bool isEnabledByDefault, int warningLevel, bool isSuppressed, Microsoft.CodeAnalysis.LocalizableString? title = null, Microsoft.CodeAnalysis.LocalizableString? description = null, string? helpLink = null, Microsoft.CodeAnalysis.Location? location = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Location>? additionalLocations = null, System.Collections.Generic.IEnumerable<string>? customTags = null, System.Collections.Immutable.ImmutableDictionary<string, string?>? properties = null) { throw null; }
        public abstract bool Equals(Microsoft.CodeAnalysis.Diagnostic? obj);
        public abstract override bool Equals(object? obj);
        public abstract override int GetHashCode();
        public abstract string GetMessage(System.IFormatProvider? formatProvider = null);
        public Microsoft.CodeAnalysis.Diagnostics.SuppressionInfo? GetSuppressionInfo(Microsoft.CodeAnalysis.Compilation compilation) { throw null; }
        string System.IFormattable.ToString(string? ignored, System.IFormatProvider? formatProvider) { throw null; }
        public override string ToString() { throw null; }
        internal abstract Microsoft.CodeAnalysis.Diagnostic WithIsSuppressed(bool isSuppressed);
        internal abstract Microsoft.CodeAnalysis.Diagnostic WithLocation(Microsoft.CodeAnalysis.Location location);
        internal abstract Microsoft.CodeAnalysis.Diagnostic WithSeverity(Microsoft.CodeAnalysis.DiagnosticSeverity severity);
    }
    public sealed partial class DiagnosticDescriptor : System.IEquatable<Microsoft.CodeAnalysis.DiagnosticDescriptor?>
    {
        public DiagnosticDescriptor(string id, Microsoft.CodeAnalysis.LocalizableString title, Microsoft.CodeAnalysis.LocalizableString messageFormat, string category, Microsoft.CodeAnalysis.DiagnosticSeverity defaultSeverity, bool isEnabledByDefault, Microsoft.CodeAnalysis.LocalizableString? description = null, string? helpLinkUri = null, params string[] customTags) { }
        public DiagnosticDescriptor(string id, string title, string messageFormat, string category, Microsoft.CodeAnalysis.DiagnosticSeverity defaultSeverity, bool isEnabledByDefault, string? description = null, string? helpLinkUri = null, params string[] customTags) { }
        public string Category { get { throw null; } }
        public System.Collections.Generic.IEnumerable<string> CustomTags { get { throw null; } }
        public Microsoft.CodeAnalysis.DiagnosticSeverity DefaultSeverity { get { throw null; } }
        public Microsoft.CodeAnalysis.LocalizableString Description { get { throw null; } }
        public string HelpLinkUri { get { throw null; } }
        public string Id { get { throw null; } }
        public bool IsEnabledByDefault { get { throw null; } }
        public Microsoft.CodeAnalysis.LocalizableString MessageFormat { get { throw null; } }
        public Microsoft.CodeAnalysis.LocalizableString Title { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.DiagnosticDescriptor? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public Microsoft.CodeAnalysis.ReportDiagnostic GetEffectiveSeverity(Microsoft.CodeAnalysis.CompilationOptions compilationOptions) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public partial class DiagnosticFormatter
    {
        public DiagnosticFormatter() { }
        public virtual string Format(Microsoft.CodeAnalysis.Diagnostic diagnostic, System.IFormatProvider? formatter = null) { throw null; }
    }
    public enum DiagnosticSeverity
    {
        Hidden = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
    }
    public sealed partial class DllImportData
    {
        internal DllImportData() { }
        public bool? BestFitMapping { get { throw null; } }
        public System.Runtime.InteropServices.CallingConvention CallingConvention { get { throw null; } }
        public System.Runtime.InteropServices.CharSet CharacterSet { get { throw null; } }
        public string? EntryPointName { get { throw null; } }
        public bool ExactSpelling { get { throw null; } }
        public string? ModuleName { get { throw null; } }
        public bool SetLastError { get { throw null; } }
        public bool? ThrowOnUnmappableCharacter { get { throw null; } }
    }
    public static partial class DocumentationCommentId
    {
        public static string CreateDeclarationId(Microsoft.CodeAnalysis.ISymbol symbol) { throw null; }
        public static string CreateReferenceId(Microsoft.CodeAnalysis.ISymbol symbol) { throw null; }
        public static Microsoft.CodeAnalysis.ISymbol? GetFirstSymbolForDeclarationId(string id, Microsoft.CodeAnalysis.Compilation compilation) { throw null; }
        public static Microsoft.CodeAnalysis.ISymbol? GetFirstSymbolForReferenceId(string id, Microsoft.CodeAnalysis.Compilation compilation) { throw null; }
        public static System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> GetSymbolsForDeclarationId(string id, Microsoft.CodeAnalysis.Compilation compilation) { throw null; }
        public static System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> GetSymbolsForReferenceId(string id, Microsoft.CodeAnalysis.Compilation compilation) { throw null; }
    }
    public enum DocumentationMode : byte
    {
        None = (byte)0,
        Parse = (byte)1,
        Diagnose = (byte)2,
    }
    public abstract partial class DocumentationProvider
    {
        protected DocumentationProvider() { }
        public static Microsoft.CodeAnalysis.DocumentationProvider Default { get { throw null; } }
        public abstract override bool Equals(object? obj);
        protected internal abstract string? GetDocumentationForSymbol(string documentationMemberID, System.Globalization.CultureInfo preferredCulture, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract override int GetHashCode();
    }
    public sealed partial class EmbeddedText
    {
        internal EmbeddedText() { }
        public System.Collections.Immutable.ImmutableArray<byte> Checksum { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceHashAlgorithm ChecksumAlgorithm { get { throw null; } }
        public string FilePath { get { throw null; } }
        public static Microsoft.CodeAnalysis.EmbeddedText FromBytes(string filePath, System.ArraySegment<byte> bytes, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1) { throw null; }
        public static Microsoft.CodeAnalysis.EmbeddedText FromSource(string filePath, Microsoft.CodeAnalysis.Text.SourceText text) { throw null; }
        public static Microsoft.CodeAnalysis.EmbeddedText FromStream(string filePath, System.IO.Stream stream, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1) { throw null; }
    }
    public sealed partial class ErrorLogOptions
    {
        public ErrorLogOptions(string path, Microsoft.CodeAnalysis.SarifVersion sarifVersion) { }
        public string Path { get { throw null; } }
        public Microsoft.CodeAnalysis.SarifVersion SarifVersion { get { throw null; } }
    }
    public readonly partial struct FileLinePositionSpan : System.IEquatable<Microsoft.CodeAnalysis.FileLinePositionSpan>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public FileLinePositionSpan(string path, Microsoft.CodeAnalysis.Text.LinePosition start, Microsoft.CodeAnalysis.Text.LinePosition end) { throw null; }
        public FileLinePositionSpan(string path, Microsoft.CodeAnalysis.Text.LinePositionSpan span) { throw null; }
        public Microsoft.CodeAnalysis.Text.LinePosition EndLinePosition { get { throw null; } }
        public bool HasMappedPath { get { throw null; } }
        public bool IsValid { get { throw null; } }
        public string Path { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.LinePositionSpan Span { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.LinePosition StartLinePosition { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.FileLinePositionSpan other) { throw null; }
        public override bool Equals(object? other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.FileLinePositionSpan left, Microsoft.CodeAnalysis.FileLinePositionSpan right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.FileLinePositionSpan left, Microsoft.CodeAnalysis.FileLinePositionSpan right) { throw null; }
        public override string ToString() { throw null; }
    }
    public static partial class FileSystemExtensions
    {
        public static Microsoft.CodeAnalysis.Emit.EmitResult Emit(this Microsoft.CodeAnalysis.Compilation compilation, string outputPath, string? pdbPath = null, string? xmlDocPath = null, string? win32ResourcesPath = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ResourceDescription>? manifestResources = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public enum GeneratedKind
    {
        Unknown = 0,
        NotGenerated = 1,
        MarkedGenerated = 2,
    }
    public readonly partial struct GeneratedSourceResult
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public string HintName { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceText SourceText { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTree SyntaxTree { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    public sealed partial class GeneratorAttribute : System.Attribute
    {
        public GeneratorAttribute() { }
        public GeneratorAttribute(string firstLanguage, params string[] additionalLanguages) { }
        public string[] Languages { get { throw null; } }
    }
    public abstract partial class GeneratorDriver
    {
        internal GeneratorDriver() { }
        public Microsoft.CodeAnalysis.GeneratorDriver AddAdditionalTexts(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AdditionalText> additionalTexts) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver AddGenerators(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> generators) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriverRunResult GetRunResult() { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver RemoveAdditionalTexts(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AdditionalText> additionalTexts) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver RemoveGenerators(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> generators) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver ReplaceAdditionalText(Microsoft.CodeAnalysis.AdditionalText oldText, Microsoft.CodeAnalysis.AdditionalText newText) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver RunGenerators(Microsoft.CodeAnalysis.Compilation compilation, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver RunGeneratorsAndUpdateCompilation(Microsoft.CodeAnalysis.Compilation compilation, out Microsoft.CodeAnalysis.Compilation outputCompilation, out System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> diagnostics, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver WithUpdatedAnalyzerConfigOptions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptionsProvider newOptions) { throw null; }
        public Microsoft.CodeAnalysis.GeneratorDriver WithUpdatedParseOptions(Microsoft.CodeAnalysis.ParseOptions newOptions) { throw null; }
    }
    public readonly partial struct GeneratorDriverOptions
    {
        public readonly Microsoft.CodeAnalysis.IncrementalGeneratorOutputKind DisabledOutputs;
        public GeneratorDriverOptions(Microsoft.CodeAnalysis.IncrementalGeneratorOutputKind disabledOutputs) { throw null; }
    }
    public partial class GeneratorDriverRunResult
    {
        internal GeneratorDriverRunResult() { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Diagnostics { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SyntaxTree> GeneratedTrees { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.GeneratorRunResult> Results { get { throw null; } }
    }
    public readonly partial struct GeneratorExecutionContext
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AdditionalText> AdditionalFiles { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptionsProvider AnalyzerConfigOptions { get { throw null; } }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.ParseOptions ParseOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.ISyntaxContextReceiver? SyntaxContextReceiver { get { throw null; } }
        public Microsoft.CodeAnalysis.ISyntaxReceiver? SyntaxReceiver { get { throw null; } }
        public void AddSource(string hintName, Microsoft.CodeAnalysis.Text.SourceText sourceText) { }
        public void AddSource(string hintName, string source) { }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public static partial class GeneratorExtensions
    {
        public static Microsoft.CodeAnalysis.ISourceGenerator AsSourceGenerator(this Microsoft.CodeAnalysis.IIncrementalGenerator incrementalGenerator) { throw null; }
        public static System.Type GetGeneratorType(this Microsoft.CodeAnalysis.ISourceGenerator generator) { throw null; }
    }
    public partial struct GeneratorInitializationContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public void RegisterForPostInitialization(System.Action<Microsoft.CodeAnalysis.GeneratorPostInitializationContext> callback) { }
        public void RegisterForSyntaxNotifications(Microsoft.CodeAnalysis.SyntaxContextReceiverCreator receiverCreator) { }
        public void RegisterForSyntaxNotifications(Microsoft.CodeAnalysis.SyntaxReceiverCreator receiverCreator) { }
    }
    public readonly partial struct GeneratorPostInitializationContext
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public void AddSource(string hintName, Microsoft.CodeAnalysis.Text.SourceText sourceText) { }
        public void AddSource(string hintName, string source) { }
    }
    public readonly partial struct GeneratorRunResult
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Diagnostics { get { throw null; } }
        public System.Exception? Exception { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.GeneratedSourceResult> GeneratedSources { get { throw null; } }
        public Microsoft.CodeAnalysis.ISourceGenerator Generator { get { throw null; } }
    }
    public readonly partial struct GeneratorSyntaxContext
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Microsoft.CodeAnalysis.SyntaxNode Node { get { throw null; } }
        public Microsoft.CodeAnalysis.SemanticModel SemanticModel { get { throw null; } }
    }
    public partial interface IAliasSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.INamespaceOrTypeSymbol Target { get; }
    }
    public partial interface IAnalyzerAssemblyLoader
    {
        void AddDependencyLocation(string fullPath);
        System.Reflection.Assembly LoadFromPath(string fullPath);
    }
    public partial interface IArrayTypeSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, Microsoft.CodeAnalysis.ITypeSymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> CustomModifiers { get; }
        Microsoft.CodeAnalysis.NullableAnnotation ElementNullableAnnotation { get; }
        Microsoft.CodeAnalysis.ITypeSymbol ElementType { get; }
        bool IsSZArray { get; }
        System.Collections.Immutable.ImmutableArray<int> LowerBounds { get; }
        int Rank { get; }
        System.Collections.Immutable.ImmutableArray<int> Sizes { get; }
        bool Equals(Microsoft.CodeAnalysis.IArrayTypeSymbol? other);
    }
    public partial interface IAssemblySymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.INamespaceSymbol GlobalNamespace { get; }
        Microsoft.CodeAnalysis.AssemblyIdentity Identity { get; }
        bool IsInteractive { get; }
        bool MightContainExtensionMethods { get; }
        System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.IModuleSymbol> Modules { get; }
        System.Collections.Generic.ICollection<string> NamespaceNames { get; }
        System.Collections.Generic.ICollection<string> TypeNames { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> GetForwardedTypes();
        Microsoft.CodeAnalysis.AssemblyMetadata? GetMetadata();
        Microsoft.CodeAnalysis.INamedTypeSymbol? GetTypeByMetadataName(string fullyQualifiedMetadataName);
        bool GivesAccessTo(Microsoft.CodeAnalysis.IAssemblySymbol toAssembly);
        Microsoft.CodeAnalysis.INamedTypeSymbol? ResolveForwardedType(string fullyQualifiedMetadataName);
    }
    public partial interface ICompilationUnitSyntax
    {
        Microsoft.CodeAnalysis.SyntaxToken EndOfFileToken { get; }
    }
    public partial interface IDiscardSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.NullableAnnotation NullableAnnotation { get; }
        Microsoft.CodeAnalysis.ITypeSymbol Type { get; }
    }
    public partial interface IDynamicTypeSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, Microsoft.CodeAnalysis.ITypeSymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
    }
    public partial interface IErrorTypeSymbol : Microsoft.CodeAnalysis.INamedTypeSymbol, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, Microsoft.CodeAnalysis.ITypeSymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.CandidateReason CandidateReason { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> CandidateSymbols { get; }
    }
    public partial interface IEventSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.IMethodSymbol? AddMethod { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IEventSymbol> ExplicitInterfaceImplementations { get; }
        bool IsWindowsRuntimeEvent { get; }
        Microsoft.CodeAnalysis.NullableAnnotation NullableAnnotation { get; }
        new Microsoft.CodeAnalysis.IEventSymbol OriginalDefinition { get; }
        Microsoft.CodeAnalysis.IEventSymbol? OverriddenEvent { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? RaiseMethod { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? RemoveMethod { get; }
        Microsoft.CodeAnalysis.ITypeSymbol Type { get; }
    }
    public partial interface IFieldSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.ISymbol? AssociatedSymbol { get; }
        object? ConstantValue { get; }
        Microsoft.CodeAnalysis.IFieldSymbol? CorrespondingTupleField { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> CustomModifiers { get; }
        int FixedSize { get; }
        bool HasConstantValue { get; }
        bool IsConst { get; }
        bool IsExplicitlyNamedTupleElement { get; }
        bool IsFixedSizeBuffer { get; }
        bool IsReadOnly { get; }
        bool IsVolatile { get; }
        Microsoft.CodeAnalysis.NullableAnnotation NullableAnnotation { get; }
        new Microsoft.CodeAnalysis.IFieldSymbol OriginalDefinition { get; }
        Microsoft.CodeAnalysis.ITypeSymbol Type { get; }
    }
    public partial interface IFunctionPointerTypeSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, Microsoft.CodeAnalysis.ITypeSymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.IMethodSymbol Signature { get; }
    }
    public partial interface IIncrementalGenerator
    {
        void Initialize(Microsoft.CodeAnalysis.IncrementalGeneratorInitializationContext context);
    }
    public partial interface ILabelSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.IMethodSymbol ContainingMethod { get; }
    }
    public partial interface ILocalSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        object? ConstantValue { get; }
        bool HasConstantValue { get; }
        bool IsConst { get; }
        bool IsFixed { get; }
        bool IsFunctionValue { get; }
        bool IsRef { get; }
        Microsoft.CodeAnalysis.NullableAnnotation NullableAnnotation { get; }
        Microsoft.CodeAnalysis.RefKind RefKind { get; }
        Microsoft.CodeAnalysis.ITypeSymbol Type { get; }
    }
    public partial interface IMethodSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        int Arity { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol? AssociatedAnonymousDelegate { get; }
        Microsoft.CodeAnalysis.ISymbol? AssociatedSymbol { get; }
        System.Reflection.Metadata.SignatureCallingConvention CallingConvention { get; }
        Microsoft.CodeAnalysis.IMethodSymbol ConstructedFrom { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IMethodSymbol> ExplicitInterfaceImplementations { get; }
        bool HidesBaseMethodsByName { get; }
        bool IsAsync { get; }
        bool IsCheckedBuiltin { get; }
        bool IsConditional { get; }
        bool IsExtensionMethod { get; }
        bool IsGenericMethod { get; }
        bool IsInitOnly { get; }
        bool IsPartialDefinition { get; }
        bool IsReadOnly { get; }
        bool IsVararg { get; }
        System.Reflection.MethodImplAttributes MethodImplementationFlags { get; }
        Microsoft.CodeAnalysis.MethodKind MethodKind { get; }
        new Microsoft.CodeAnalysis.IMethodSymbol OriginalDefinition { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? OverriddenMethod { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IParameterSymbol> Parameters { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? PartialDefinitionPart { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? PartialImplementationPart { get; }
        Microsoft.CodeAnalysis.NullableAnnotation ReceiverNullableAnnotation { get; }
        Microsoft.CodeAnalysis.ITypeSymbol? ReceiverType { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? ReducedFrom { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> RefCustomModifiers { get; }
        Microsoft.CodeAnalysis.RefKind RefKind { get; }
        Microsoft.CodeAnalysis.NullableAnnotation ReturnNullableAnnotation { get; }
        bool ReturnsByRef { get; }
        bool ReturnsByRefReadonly { get; }
        bool ReturnsVoid { get; }
        Microsoft.CodeAnalysis.ITypeSymbol ReturnType { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> ReturnTypeCustomModifiers { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> TypeArgumentNullableAnnotations { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> TypeArguments { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeParameterSymbol> TypeParameters { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> UnmanagedCallingConventionTypes { get; }
        Microsoft.CodeAnalysis.IMethodSymbol Construct(params Microsoft.CodeAnalysis.ITypeSymbol[] typeArguments);
        Microsoft.CodeAnalysis.IMethodSymbol Construct(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> typeArguments, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> typeArgumentNullableAnnotations);
        Microsoft.CodeAnalysis.DllImportData? GetDllImportData();
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AttributeData> GetReturnTypeAttributes();
        Microsoft.CodeAnalysis.ITypeSymbol? GetTypeInferredDuringReduction(Microsoft.CodeAnalysis.ITypeParameterSymbol reducedFromTypeParameter);
        Microsoft.CodeAnalysis.IMethodSymbol? ReduceExtensionMethod(Microsoft.CodeAnalysis.ITypeSymbol receiverType);
    }
    public partial interface IModuleSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.INamespaceSymbol GlobalNamespace { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AssemblyIdentity> ReferencedAssemblies { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IAssemblySymbol> ReferencedAssemblySymbols { get; }
        Microsoft.CodeAnalysis.ModuleMetadata? GetMetadata();
        Microsoft.CodeAnalysis.INamespaceSymbol? GetModuleNamespace(Microsoft.CodeAnalysis.INamespaceSymbol namespaceSymbol);
    }
    public partial interface INamedTypeSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, Microsoft.CodeAnalysis.ITypeSymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        int Arity { get; }
        Microsoft.CodeAnalysis.ISymbol? AssociatedSymbol { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol ConstructedFrom { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IMethodSymbol> Constructors { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? DelegateInvokeMethod { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol? EnumUnderlyingType { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IMethodSymbol> InstanceConstructors { get; }
        bool IsComImport { get; }
        bool IsGenericType { get; }
        bool IsImplicitClass { get; }
        bool IsScriptClass { get; }
        bool IsSerializable { get; }
        bool IsUnboundGenericType { get; }
        System.Collections.Generic.IEnumerable<string> MemberNames { get; }
        bool MightContainExtensionMethods { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol? NativeIntegerUnderlyingType { get; }
        new Microsoft.CodeAnalysis.INamedTypeSymbol OriginalDefinition { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IMethodSymbol> StaticConstructors { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IFieldSymbol> TupleElements { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol? TupleUnderlyingType { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> TypeArgumentNullableAnnotations { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> TypeArguments { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeParameterSymbol> TypeParameters { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol Construct(params Microsoft.CodeAnalysis.ITypeSymbol[] typeArguments);
        Microsoft.CodeAnalysis.INamedTypeSymbol Construct(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> typeArguments, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> typeArgumentNullableAnnotations);
        Microsoft.CodeAnalysis.INamedTypeSymbol ConstructUnboundGenericType();
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> GetTypeArgumentCustomModifiers(int ordinal);
    }
    public partial interface INamespaceOrTypeSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        bool IsNamespace { get; }
        bool IsType { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> GetMembers();
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> GetMembers(string name);
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> GetTypeMembers();
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> GetTypeMembers(string name);
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> GetTypeMembers(string name, int arity);
    }
    public partial interface INamespaceSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamespaceSymbol> ConstituentNamespaces { get; }
        Microsoft.CodeAnalysis.Compilation? ContainingCompilation { get; }
        bool IsGlobalNamespace { get; }
        Microsoft.CodeAnalysis.NamespaceKind NamespaceKind { get; }
        new System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.INamespaceOrTypeSymbol> GetMembers();
        new System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.INamespaceOrTypeSymbol> GetMembers(string name);
        System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.INamespaceSymbol> GetNamespaceMembers();
    }
    public readonly partial struct IncrementalGeneratorInitializationContext
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Microsoft.CodeAnalysis.IncrementalValuesProvider<Microsoft.CodeAnalysis.AdditionalText> AdditionalTextsProvider { get { throw null; } }
        public Microsoft.CodeAnalysis.IncrementalValueProvider<Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptionsProvider> AnalyzerConfigOptionsProvider { get { throw null; } }
        public Microsoft.CodeAnalysis.IncrementalValueProvider<Microsoft.CodeAnalysis.Compilation> CompilationProvider { get { throw null; } }
        public Microsoft.CodeAnalysis.IncrementalValueProvider<Microsoft.CodeAnalysis.MetadataReference> MetadataReferencesProvider { get { throw null; } }
        public Microsoft.CodeAnalysis.IncrementalValueProvider<Microsoft.CodeAnalysis.ParseOptions> ParseOptionsProvider { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxValueProvider SyntaxProvider { get { throw null; } }
        public void RegisterImplementationSourceOutput<TSource>(Microsoft.CodeAnalysis.IncrementalValueProvider<TSource> source, System.Action<Microsoft.CodeAnalysis.SourceProductionContext, TSource> action) { }
        public void RegisterImplementationSourceOutput<TSource>(Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source, System.Action<Microsoft.CodeAnalysis.SourceProductionContext, TSource> action) { }
        public void RegisterPostInitializationOutput(System.Action<Microsoft.CodeAnalysis.IncrementalGeneratorPostInitializationContext> callback) { }
        public void RegisterSourceOutput<TSource>(Microsoft.CodeAnalysis.IncrementalValueProvider<TSource> source, System.Action<Microsoft.CodeAnalysis.SourceProductionContext, TSource> action) { }
        public void RegisterSourceOutput<TSource>(Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source, System.Action<Microsoft.CodeAnalysis.SourceProductionContext, TSource> action) { }
    }
    [System.FlagsAttribute]
    public enum IncrementalGeneratorOutputKind
    {
        None = 0,
        Source = 1,
        PostInit = 2,
        Implementation = 4,
    }
    public readonly partial struct IncrementalGeneratorPostInitializationContext
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public void AddSource(string hintName, Microsoft.CodeAnalysis.Text.SourceText sourceText) { }
        public void AddSource(string hintName, string source) { }
    }
    public static partial class IncrementalValueProviderExtensions
    {
        public static Microsoft.CodeAnalysis.IncrementalValueProvider<System.Collections.Immutable.ImmutableArray<TSource>> Collect<TSource>(this Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValueProvider<(TLeft Left, TRight Right)> Combine<TLeft, TRight>(this Microsoft.CodeAnalysis.IncrementalValueProvider<TLeft> provider1, Microsoft.CodeAnalysis.IncrementalValueProvider<TRight> provider2) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<(TLeft Left, TRight Right)> Combine<TLeft, TRight>(this Microsoft.CodeAnalysis.IncrementalValuesProvider<TLeft> provider1, Microsoft.CodeAnalysis.IncrementalValueProvider<TRight> provider2) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<TResult> SelectMany<TSource, TResult>(this Microsoft.CodeAnalysis.IncrementalValueProvider<TSource> source, System.Func<TSource, System.Threading.CancellationToken, System.Collections.Generic.IEnumerable<TResult>> selector) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<TResult> SelectMany<TSource, TResult>(this Microsoft.CodeAnalysis.IncrementalValueProvider<TSource> source, System.Func<TSource, System.Threading.CancellationToken, System.Collections.Immutable.ImmutableArray<TResult>> selector) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<TResult> SelectMany<TSource, TResult>(this Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source, System.Func<TSource, System.Threading.CancellationToken, System.Collections.Generic.IEnumerable<TResult>> selector) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<TResult> SelectMany<TSource, TResult>(this Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source, System.Func<TSource, System.Threading.CancellationToken, System.Collections.Immutable.ImmutableArray<TResult>> selector) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValueProvider<TResult> Select<TSource, TResult>(this Microsoft.CodeAnalysis.IncrementalValueProvider<TSource> source, System.Func<TSource, System.Threading.CancellationToken, TResult> selector) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<TResult> Select<TSource, TResult>(this Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source, System.Func<TSource, System.Threading.CancellationToken, TResult> selector) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> Where<TSource>(this Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source, System.Func<TSource, bool> predicate) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValueProvider<TSource> WithComparer<TSource>(this Microsoft.CodeAnalysis.IncrementalValueProvider<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }
        public static Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> WithComparer<TSource>(this Microsoft.CodeAnalysis.IncrementalValuesProvider<TSource> source, System.Collections.Generic.IEqualityComparer<TSource> comparer) { throw null; }
    }
    public readonly partial struct IncrementalValueProvider<TValue>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
    }
    public readonly partial struct IncrementalValuesProvider<TValues>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
    }
    public partial interface IOperation
    {
        System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.IOperation> Children { get; }
        Microsoft.CodeAnalysis.Optional<object?> ConstantValue { get; }
        bool IsImplicit { get; }
        Microsoft.CodeAnalysis.OperationKind Kind { get; }
        string Language { get; }
        Microsoft.CodeAnalysis.IOperation? Parent { get; }
        Microsoft.CodeAnalysis.SemanticModel? SemanticModel { get; }
        Microsoft.CodeAnalysis.SyntaxNode Syntax { get; }
        Microsoft.CodeAnalysis.ITypeSymbol? Type { get; }
        void Accept(Microsoft.CodeAnalysis.Operations.OperationVisitor visitor);
        TResult? Accept<TArgument, TResult>(Microsoft.CodeAnalysis.Operations.OperationVisitor<TArgument, TResult> visitor, TArgument argument);
    }
    public partial interface IParameterSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> CustomModifiers { get; }
        object? ExplicitDefaultValue { get; }
        bool HasExplicitDefaultValue { get; }
        bool IsDiscard { get; }
        bool IsOptional { get; }
        bool IsParams { get; }
        bool IsThis { get; }
        Microsoft.CodeAnalysis.NullableAnnotation NullableAnnotation { get; }
        int Ordinal { get; }
        new Microsoft.CodeAnalysis.IParameterSymbol OriginalDefinition { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> RefCustomModifiers { get; }
        Microsoft.CodeAnalysis.RefKind RefKind { get; }
        Microsoft.CodeAnalysis.ITypeSymbol Type { get; }
    }
    public partial interface IPointerTypeSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, Microsoft.CodeAnalysis.ITypeSymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> CustomModifiers { get; }
        Microsoft.CodeAnalysis.ITypeSymbol PointedAtType { get; }
    }
    public partial interface IPreprocessingSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
    }
    public partial interface IPropertySymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IPropertySymbol> ExplicitInterfaceImplementations { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? GetMethod { get; }
        bool IsIndexer { get; }
        bool IsReadOnly { get; }
        bool IsWithEvents { get; }
        bool IsWriteOnly { get; }
        Microsoft.CodeAnalysis.NullableAnnotation NullableAnnotation { get; }
        new Microsoft.CodeAnalysis.IPropertySymbol OriginalDefinition { get; }
        Microsoft.CodeAnalysis.IPropertySymbol? OverriddenProperty { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IParameterSymbol> Parameters { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> RefCustomModifiers { get; }
        Microsoft.CodeAnalysis.RefKind RefKind { get; }
        bool ReturnsByRef { get; }
        bool ReturnsByRefReadonly { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? SetMethod { get; }
        Microsoft.CodeAnalysis.ITypeSymbol Type { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CustomModifier> TypeCustomModifiers { get; }
    }
    public partial interface IRangeVariableSymbol : Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
    }
    public partial interface ISkippedTokensTriviaSyntax
    {
        Microsoft.CodeAnalysis.SyntaxTokenList Tokens { get; }
    }
    public partial interface ISourceAssemblySymbol : Microsoft.CodeAnalysis.IAssemblySymbol, Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        Microsoft.CodeAnalysis.Compilation Compilation { get; }
    }
    public partial interface ISourceGenerator
    {
        void Execute(Microsoft.CodeAnalysis.GeneratorExecutionContext context);
        void Initialize(Microsoft.CodeAnalysis.GeneratorInitializationContext context);
    }
    public partial interface IStructuredTriviaSyntax
    {
        Microsoft.CodeAnalysis.SyntaxTrivia ParentTrivia { get; }
    }
    public partial interface ISymbol : System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        bool CanBeReferencedByName { get; }
        Microsoft.CodeAnalysis.IAssemblySymbol ContainingAssembly { get; }
        Microsoft.CodeAnalysis.IModuleSymbol ContainingModule { get; }
        Microsoft.CodeAnalysis.INamespaceSymbol ContainingNamespace { get; }
        Microsoft.CodeAnalysis.ISymbol ContainingSymbol { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol ContainingType { get; }
        Microsoft.CodeAnalysis.Accessibility DeclaredAccessibility { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SyntaxReference> DeclaringSyntaxReferences { get; }
        bool HasUnsupportedMetadata { get; }
        bool IsAbstract { get; }
        bool IsDefinition { get; }
        bool IsExtern { get; }
        bool IsImplicitlyDeclared { get; }
        bool IsOverride { get; }
        bool IsSealed { get; }
        bool IsStatic { get; }
        bool IsVirtual { get; }
        Microsoft.CodeAnalysis.SymbolKind Kind { get; }
        string Language { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Location> Locations { get; }
        string MetadataName { get; }
        int MetadataToken { get; }
        string Name { get; }
        Microsoft.CodeAnalysis.ISymbol OriginalDefinition { get; }
        void Accept(Microsoft.CodeAnalysis.SymbolVisitor visitor);
        TResult? Accept<TResult>(Microsoft.CodeAnalysis.SymbolVisitor<TResult> visitor);
        bool Equals(Microsoft.CodeAnalysis.ISymbol? other, Microsoft.CodeAnalysis.SymbolEqualityComparer equalityComparer);
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AttributeData> GetAttributes();
        string? GetDocumentationCommentId();
        string? GetDocumentationCommentXml(System.Globalization.CultureInfo? preferredCulture = null, bool expandIncludes = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SymbolDisplayPart> ToDisplayParts(Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
        string ToDisplayString(Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SymbolDisplayPart> ToMinimalDisplayParts(Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
        string ToMinimalDisplayString(Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
    }
    public static partial class ISymbolExtensions
    {
        public static Microsoft.CodeAnalysis.IMethodSymbol? GetConstructedReducedFrom(this Microsoft.CodeAnalysis.IMethodSymbol method) { throw null; }
    }
    public partial interface ISyntaxContextReceiver
    {
        void OnVisitSyntaxNode(Microsoft.CodeAnalysis.GeneratorSyntaxContext context);
    }
    public partial interface ISyntaxReceiver
    {
        void OnVisitSyntaxNode(Microsoft.CodeAnalysis.SyntaxNode syntaxNode);
    }
    public partial interface ITypeParameterSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, Microsoft.CodeAnalysis.ITypeSymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.NullableAnnotation> ConstraintNullableAnnotations { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> ConstraintTypes { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? DeclaringMethod { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol? DeclaringType { get; }
        bool HasConstructorConstraint { get; }
        bool HasNotNullConstraint { get; }
        bool HasReferenceTypeConstraint { get; }
        bool HasUnmanagedTypeConstraint { get; }
        bool HasValueTypeConstraint { get; }
        int Ordinal { get; }
        new Microsoft.CodeAnalysis.ITypeParameterSymbol OriginalDefinition { get; }
        Microsoft.CodeAnalysis.ITypeParameterSymbol? ReducedFrom { get; }
        Microsoft.CodeAnalysis.NullableAnnotation ReferenceTypeConstraintNullableAnnotation { get; }
        Microsoft.CodeAnalysis.TypeParameterKind TypeParameterKind { get; }
        Microsoft.CodeAnalysis.VarianceKind Variance { get; }
    }
    public partial interface ITypeSymbol : Microsoft.CodeAnalysis.INamespaceOrTypeSymbol, Microsoft.CodeAnalysis.ISymbol, System.IEquatable<Microsoft.CodeAnalysis.ISymbol?>
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> AllInterfaces { get; }
        Microsoft.CodeAnalysis.INamedTypeSymbol? BaseType { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.INamedTypeSymbol> Interfaces { get; }
        bool IsAnonymousType { get; }
        bool IsNativeIntegerType { get; }
        bool IsReadOnly { get; }
        bool IsRecord { get; }
        bool IsReferenceType { get; }
        bool IsRefLikeType { get; }
        bool IsTupleType { get; }
        bool IsUnmanagedType { get; }
        bool IsValueType { get; }
        Microsoft.CodeAnalysis.NullableAnnotation NullableAnnotation { get; }
        new Microsoft.CodeAnalysis.ITypeSymbol OriginalDefinition { get; }
        Microsoft.CodeAnalysis.SpecialType SpecialType { get; }
        Microsoft.CodeAnalysis.TypeKind TypeKind { get; }
        Microsoft.CodeAnalysis.ISymbol? FindImplementationForInterfaceMember(Microsoft.CodeAnalysis.ISymbol interfaceMember);
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SymbolDisplayPart> ToDisplayParts(Microsoft.CodeAnalysis.NullableFlowState topLevelNullability, Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
        string ToDisplayString(Microsoft.CodeAnalysis.NullableFlowState topLevelNullability, Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SymbolDisplayPart> ToMinimalDisplayParts(Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.NullableFlowState topLevelNullability, int position, Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
        string ToMinimalDisplayString(Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.NullableFlowState topLevelNullability, int position, Microsoft.CodeAnalysis.SymbolDisplayFormat? format = null);
        Microsoft.CodeAnalysis.ITypeSymbol WithNullableAnnotation(Microsoft.CodeAnalysis.NullableAnnotation nullableAnnotation);
    }
    public static partial class LanguageNames
    {
        public const string CSharp = "C#";
        public const string FSharp = "F#";
        public const string VisualBasic = "Visual Basic";
    }
    public readonly partial struct LineMapping : System.IEquatable<Microsoft.CodeAnalysis.LineMapping>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public LineMapping(Microsoft.CodeAnalysis.Text.LinePositionSpan span, int? characterOffset, Microsoft.CodeAnalysis.FileLinePositionSpan mappedSpan) { throw null; }
        public int? CharacterOffset { get { throw null; } }
        public bool IsHidden { get { throw null; } }
        public Microsoft.CodeAnalysis.FileLinePositionSpan MappedSpan { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.LinePositionSpan Span { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.LineMapping other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.LineMapping left, Microsoft.CodeAnalysis.LineMapping right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.LineMapping left, Microsoft.CodeAnalysis.LineMapping right) { throw null; }
        public override string? ToString() { throw null; }
    }
    public enum LineVisibility
    {
        BeforeFirstLineDirective = 0,
        Hidden = 1,
        Visible = 2,
    }
    public sealed partial class LocalizableResourceString : Microsoft.CodeAnalysis.LocalizableString
    {
        public LocalizableResourceString(string nameOfLocalizableResource, System.Resources.ResourceManager resourceManager, System.Type resourceSource) { }
        public LocalizableResourceString(string nameOfLocalizableResource, System.Resources.ResourceManager resourceManager, System.Type resourceSource, params string[] formatArguments) { }
        protected override bool AreEqual(object? other) { throw null; }
        protected override int GetHash() { throw null; }
        protected override string GetText(System.IFormatProvider? formatProvider) { throw null; }
    }
    public abstract partial class LocalizableString : System.IEquatable<Microsoft.CodeAnalysis.LocalizableString?>, System.IFormattable
    {
        protected LocalizableString() { }
        public event System.EventHandler<System.Exception>? OnException { add { } remove { } }
        protected abstract bool AreEqual(object? other);
        public bool Equals(Microsoft.CodeAnalysis.LocalizableString? other) { throw null; }
        public sealed override bool Equals(object? other) { throw null; }
        protected abstract int GetHash();
        public sealed override int GetHashCode() { throw null; }
        protected abstract string GetText(System.IFormatProvider? formatProvider);
        public static explicit operator string (Microsoft.CodeAnalysis.LocalizableString localizableResource) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.LocalizableString (string? fixedResource) { throw null; }
        string System.IFormattable.ToString(string? ignored, System.IFormatProvider? formatProvider) { throw null; }
        public sealed override string ToString() { throw null; }
        public string ToString(System.IFormatProvider? formatProvider) { throw null; }
    }
    public abstract partial class Location
    {
        internal Location() { }
        public bool IsInMetadata { get { throw null; } }
        public bool IsInSource { get { throw null; } }
        public abstract Microsoft.CodeAnalysis.LocationKind Kind { get; }
        public Microsoft.CodeAnalysis.IModuleSymbol? MetadataModule { get { throw null; } }
        public static Microsoft.CodeAnalysis.Location None { get { throw null; } }
        public virtual Microsoft.CodeAnalysis.Text.TextSpan SourceSpan { get { throw null; } }
        public virtual Microsoft.CodeAnalysis.SyntaxTree? SourceTree { get { throw null; } }
        public static Microsoft.CodeAnalysis.Location Create(Microsoft.CodeAnalysis.SyntaxTree syntaxTree, Microsoft.CodeAnalysis.Text.TextSpan textSpan) { throw null; }
        public static Microsoft.CodeAnalysis.Location Create(string filePath, Microsoft.CodeAnalysis.Text.TextSpan textSpan, Microsoft.CodeAnalysis.Text.LinePositionSpan lineSpan) { throw null; }
        public abstract override bool Equals(object? obj);
        protected virtual string GetDebuggerDisplay() { throw null; }
        public abstract override int GetHashCode();
        public virtual Microsoft.CodeAnalysis.FileLinePositionSpan GetLineSpan() { throw null; }
        public virtual Microsoft.CodeAnalysis.FileLinePositionSpan GetMappedLineSpan() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Location? left, Microsoft.CodeAnalysis.Location? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Location? left, Microsoft.CodeAnalysis.Location? right) { throw null; }
        public override string ToString() { throw null; }
    }
    public enum LocationKind : byte
    {
        None = (byte)0,
        SourceFile = (byte)1,
        MetadataFile = (byte)2,
        XmlFile = (byte)3,
        ExternalFile = (byte)4,
    }
    public abstract partial class Metadata : System.IDisposable
    {
        internal Metadata() { }
        public Microsoft.CodeAnalysis.MetadataId Id { get { throw null; } }
        public abstract Microsoft.CodeAnalysis.MetadataImageKind Kind { get; }
        protected abstract Microsoft.CodeAnalysis.Metadata CommonCopy();
        public Microsoft.CodeAnalysis.Metadata Copy() { throw null; }
        public abstract void Dispose();
    }
    public sealed partial class MetadataId
    {
        internal MetadataId() { }
    }
    public enum MetadataImageKind : byte
    {
        Assembly = (byte)0,
        Module = (byte)1,
    }
    public enum MetadataImportOptions : byte
    {
        Public = (byte)0,
        Internal = (byte)1,
        All = (byte)2,
    }
    public abstract partial class MetadataReference
    {
        protected MetadataReference(Microsoft.CodeAnalysis.MetadataReferenceProperties properties) { }
        public virtual string? Display { get { throw null; } }
        public Microsoft.CodeAnalysis.MetadataReferenceProperties Properties { get { throw null; } }
        [System.ObsoleteAttribute("Use CreateFromFile(assembly.Location) instead", true)]
        public static Microsoft.CodeAnalysis.MetadataReference CreateFromAssembly(System.Reflection.Assembly assembly) { throw null; }
        [System.ObsoleteAttribute("Use CreateFromFile(assembly.Location) instead", true)]
        public static Microsoft.CodeAnalysis.MetadataReference CreateFromAssembly(System.Reflection.Assembly assembly, Microsoft.CodeAnalysis.MetadataReferenceProperties properties, Microsoft.CodeAnalysis.DocumentationProvider? documentation = null) { throw null; }
        public static Microsoft.CodeAnalysis.PortableExecutableReference CreateFromFile(string path, Microsoft.CodeAnalysis.MetadataReferenceProperties properties = default(Microsoft.CodeAnalysis.MetadataReferenceProperties), Microsoft.CodeAnalysis.DocumentationProvider? documentation = null) { throw null; }
        public static Microsoft.CodeAnalysis.PortableExecutableReference CreateFromImage(System.Collections.Generic.IEnumerable<byte> peImage, Microsoft.CodeAnalysis.MetadataReferenceProperties properties = default(Microsoft.CodeAnalysis.MetadataReferenceProperties), Microsoft.CodeAnalysis.DocumentationProvider? documentation = null, string? filePath = null) { throw null; }
        public static Microsoft.CodeAnalysis.PortableExecutableReference CreateFromImage(System.Collections.Immutable.ImmutableArray<byte> peImage, Microsoft.CodeAnalysis.MetadataReferenceProperties properties = default(Microsoft.CodeAnalysis.MetadataReferenceProperties), Microsoft.CodeAnalysis.DocumentationProvider? documentation = null, string? filePath = null) { throw null; }
        public static Microsoft.CodeAnalysis.PortableExecutableReference CreateFromStream(System.IO.Stream peStream, Microsoft.CodeAnalysis.MetadataReferenceProperties properties = default(Microsoft.CodeAnalysis.MetadataReferenceProperties), Microsoft.CodeAnalysis.DocumentationProvider? documentation = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReference WithAliases(System.Collections.Generic.IEnumerable<string> aliases) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReference WithAliases(System.Collections.Immutable.ImmutableArray<string> aliases) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReference WithEmbedInteropTypes(bool value) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReference WithProperties(Microsoft.CodeAnalysis.MetadataReferenceProperties properties) { throw null; }
        internal abstract Microsoft.CodeAnalysis.MetadataReference WithPropertiesImplReturningMetadataReference(Microsoft.CodeAnalysis.MetadataReferenceProperties properties);
    }
    public partial struct MetadataReferenceProperties : System.IEquatable<Microsoft.CodeAnalysis.MetadataReferenceProperties>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public MetadataReferenceProperties(Microsoft.CodeAnalysis.MetadataImageKind kind = Microsoft.CodeAnalysis.MetadataImageKind.Assembly, System.Collections.Immutable.ImmutableArray<string> aliases = default(System.Collections.Immutable.ImmutableArray<string>), bool embedInteropTypes = false) { throw null; }
        public System.Collections.Immutable.ImmutableArray<string> Aliases { get { throw null; } }
        public static Microsoft.CodeAnalysis.MetadataReferenceProperties Assembly { get { throw null; } }
        public bool EmbedInteropTypes { get { throw null; } }
        public static string GlobalAlias { get { throw null; } }
        public Microsoft.CodeAnalysis.MetadataImageKind Kind { get { throw null; } }
        public static Microsoft.CodeAnalysis.MetadataReferenceProperties Module { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.MetadataReferenceProperties other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.MetadataReferenceProperties left, Microsoft.CodeAnalysis.MetadataReferenceProperties right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.MetadataReferenceProperties left, Microsoft.CodeAnalysis.MetadataReferenceProperties right) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReferenceProperties WithAliases(System.Collections.Generic.IEnumerable<string> aliases) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReferenceProperties WithAliases(System.Collections.Immutable.ImmutableArray<string> aliases) { throw null; }
        public Microsoft.CodeAnalysis.MetadataReferenceProperties WithEmbedInteropTypes(bool embedInteropTypes) { throw null; }
    }
    public abstract partial class MetadataReferenceResolver
    {
        protected MetadataReferenceResolver() { }
        public virtual bool ResolveMissingAssemblies { get { throw null; } }
        public abstract override bool Equals(object? other);
        public abstract override int GetHashCode();
        public virtual Microsoft.CodeAnalysis.PortableExecutableReference? ResolveMissingAssembly(Microsoft.CodeAnalysis.MetadataReference definition, Microsoft.CodeAnalysis.AssemblyIdentity referenceIdentity) { throw null; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.PortableExecutableReference> ResolveReference(string reference, string? baseFilePath, Microsoft.CodeAnalysis.MetadataReferenceProperties properties);
    }
    public enum MethodKind
    {
        AnonymousFunction = 0,
        LambdaMethod = 0,
        Constructor = 1,
        Conversion = 2,
        DelegateInvoke = 3,
        Destructor = 4,
        EventAdd = 5,
        EventRaise = 6,
        EventRemove = 7,
        ExplicitInterfaceImplementation = 8,
        UserDefinedOperator = 9,
        Ordinary = 10,
        PropertyGet = 11,
        PropertySet = 12,
        ReducedExtension = 13,
        SharedConstructor = 14,
        StaticConstructor = 14,
        BuiltinOperator = 15,
        DeclareMethod = 16,
        LocalFunction = 17,
        FunctionPointerSignature = 18,
    }
    public static partial class ModelExtensions
    {
        public static Microsoft.CodeAnalysis.ControlFlowAnalysis AnalyzeControlFlow(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode statement) { throw null; }
        public static Microsoft.CodeAnalysis.ControlFlowAnalysis AnalyzeControlFlow(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode firstStatement, Microsoft.CodeAnalysis.SyntaxNode lastStatement) { throw null; }
        public static Microsoft.CodeAnalysis.DataFlowAnalysis AnalyzeDataFlow(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode statementOrExpression) { throw null; }
        public static Microsoft.CodeAnalysis.DataFlowAnalysis AnalyzeDataFlow(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode firstStatement, Microsoft.CodeAnalysis.SyntaxNode lastStatement) { throw null; }
        public static Microsoft.CodeAnalysis.IAliasSymbol? GetAliasInfo(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode nameSyntax, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.ISymbol? GetDeclaredSymbol(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode declaration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> GetMemberGroup(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.IAliasSymbol? GetSpeculativeAliasInfo(this Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.SyntaxNode nameSyntax, Microsoft.CodeAnalysis.SpeculativeBindingOption bindingOption) { throw null; }
        public static Microsoft.CodeAnalysis.SymbolInfo GetSpeculativeSymbolInfo(this Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SpeculativeBindingOption bindingOption) { throw null; }
        public static Microsoft.CodeAnalysis.TypeInfo GetSpeculativeTypeInfo(this Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SpeculativeBindingOption bindingOption) { throw null; }
        public static Microsoft.CodeAnalysis.SymbolInfo GetSymbolInfo(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.TypeInfo GetTypeInfo(this Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public sealed partial class ModuleMetadata : Microsoft.CodeAnalysis.Metadata
    {
        internal ModuleMetadata() { }
        public bool IsDisposed { get { throw null; } }
        public override Microsoft.CodeAnalysis.MetadataImageKind Kind { get { throw null; } }
        public string Name { get { throw null; } }
        protected override Microsoft.CodeAnalysis.Metadata CommonCopy() { throw null; }
        public static Microsoft.CodeAnalysis.ModuleMetadata CreateFromFile(string path) { throw null; }
        public static Microsoft.CodeAnalysis.ModuleMetadata CreateFromImage(System.Collections.Generic.IEnumerable<byte> peImage) { throw null; }
        public static Microsoft.CodeAnalysis.ModuleMetadata CreateFromImage(System.Collections.Immutable.ImmutableArray<byte> peImage) { throw null; }
        public static Microsoft.CodeAnalysis.ModuleMetadata CreateFromImage(System.IntPtr peImage, int size) { throw null; }
        public static Microsoft.CodeAnalysis.ModuleMetadata CreateFromMetadata(System.IntPtr metadata, int size) { throw null; }
        public static Microsoft.CodeAnalysis.ModuleMetadata CreateFromStream(System.IO.Stream peStream, bool leaveOpen = false) { throw null; }
        public static Microsoft.CodeAnalysis.ModuleMetadata CreateFromStream(System.IO.Stream peStream, System.Reflection.PortableExecutable.PEStreamOptions options) { throw null; }
        public override void Dispose() { }
        public System.Reflection.Metadata.MetadataReader GetMetadataReader() { throw null; }
        public System.Collections.Immutable.ImmutableArray<string> GetModuleNames() { throw null; }
        public System.Guid GetModuleVersionId() { throw null; }
        public Microsoft.CodeAnalysis.PortableExecutableReference GetReference(Microsoft.CodeAnalysis.DocumentationProvider? documentation = null, string? filePath = null, string? display = null) { throw null; }
    }
    public enum NamespaceKind
    {
        Module = 1,
        Assembly = 2,
        Compilation = 3,
    }
    public readonly partial struct NullabilityInfo : System.IEquatable<Microsoft.CodeAnalysis.NullabilityInfo>
    {
        private readonly int _dummyPrimitive;
        public Microsoft.CodeAnalysis.NullableAnnotation Annotation { get { throw null; } }
        public Microsoft.CodeAnalysis.NullableFlowState FlowState { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.NullabilityInfo other) { throw null; }
        public override bool Equals(object? other) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum NullableAnnotation : byte
    {
        None = (byte)0,
        NotAnnotated = (byte)1,
        Annotated = (byte)2,
    }
    [System.FlagsAttribute]
    public enum NullableContext
    {
        Disabled = 0,
        WarningsEnabled = 1,
        AnnotationsEnabled = 2,
        Enabled = 3,
        WarningsContextInherited = 4,
        AnnotationsContextInherited = 8,
        ContextInherited = 12,
    }
    public static partial class NullableContextExtensions
    {
        public static bool AnnotationsEnabled(this Microsoft.CodeAnalysis.NullableContext context) { throw null; }
        public static bool AnnotationsInherited(this Microsoft.CodeAnalysis.NullableContext context) { throw null; }
        public static bool WarningsEnabled(this Microsoft.CodeAnalysis.NullableContext context) { throw null; }
        public static bool WarningsInherited(this Microsoft.CodeAnalysis.NullableContext context) { throw null; }
    }
    [System.FlagsAttribute]
    public enum NullableContextOptions
    {
        Disable = 0,
        Warnings = 1,
        Annotations = 2,
        Enable = 3,
    }
    public static partial class NullableContextOptionsExtensions
    {
        public static bool AnnotationsEnabled(this Microsoft.CodeAnalysis.NullableContextOptions context) { throw null; }
        public static bool WarningsEnabled(this Microsoft.CodeAnalysis.NullableContextOptions context) { throw null; }
    }
    public enum NullableFlowState : byte
    {
        None = (byte)0,
        NotNull = (byte)1,
        MaybeNull = (byte)2,
    }
    public enum OperationKind
    {
        None = 0,
        Invalid = 1,
        Block = 2,
        VariableDeclarationGroup = 3,
        Switch = 4,
        Loop = 5,
        Labeled = 6,
        Branch = 7,
        Empty = 8,
        Return = 9,
        YieldBreak = 10,
        Lock = 11,
        Try = 12,
        Using = 13,
        YieldReturn = 14,
        ExpressionStatement = 15,
        LocalFunction = 16,
        Stop = 17,
        End = 18,
        RaiseEvent = 19,
        Literal = 20,
        Conversion = 21,
        Invocation = 22,
        ArrayElementReference = 23,
        LocalReference = 24,
        ParameterReference = 25,
        FieldReference = 26,
        MethodReference = 27,
        PropertyReference = 28,
        EventReference = 30,
        Unary = 31,
        UnaryOperator = 31,
        Binary = 32,
        BinaryOperator = 32,
        Conditional = 33,
        Coalesce = 34,
        AnonymousFunction = 35,
        ObjectCreation = 36,
        TypeParameterObjectCreation = 37,
        ArrayCreation = 38,
        InstanceReference = 39,
        IsType = 40,
        Await = 41,
        SimpleAssignment = 42,
        CompoundAssignment = 43,
        Parenthesized = 44,
        EventAssignment = 45,
        ConditionalAccess = 46,
        ConditionalAccessInstance = 47,
        InterpolatedString = 48,
        AnonymousObjectCreation = 49,
        ObjectOrCollectionInitializer = 50,
        MemberInitializer = 51,
        [System.ObsoleteAttribute("ICollectionElementInitializerOperation has been replaced with IInvocationOperation and IDynamicInvocationOperation", true)]
        CollectionElementInitializer = 52,
        NameOf = 53,
        Tuple = 54,
        DynamicObjectCreation = 55,
        DynamicMemberReference = 56,
        DynamicInvocation = 57,
        DynamicIndexerAccess = 58,
        TranslatedQuery = 59,
        DelegateCreation = 60,
        DefaultValue = 61,
        TypeOf = 62,
        SizeOf = 63,
        AddressOf = 64,
        IsPattern = 65,
        Increment = 66,
        Throw = 67,
        Decrement = 68,
        DeconstructionAssignment = 69,
        DeclarationExpression = 70,
        OmittedArgument = 71,
        FieldInitializer = 72,
        VariableInitializer = 73,
        PropertyInitializer = 74,
        ParameterInitializer = 75,
        ArrayInitializer = 76,
        VariableDeclarator = 77,
        VariableDeclaration = 78,
        Argument = 79,
        CatchClause = 80,
        SwitchCase = 81,
        CaseClause = 82,
        InterpolatedStringText = 83,
        Interpolation = 84,
        ConstantPattern = 85,
        DeclarationPattern = 86,
        TupleBinary = 87,
        TupleBinaryOperator = 87,
        MethodBody = 88,
        MethodBodyOperation = 88,
        ConstructorBody = 89,
        ConstructorBodyOperation = 89,
        Discard = 90,
        FlowCapture = 91,
        FlowCaptureReference = 92,
        IsNull = 93,
        CaughtException = 94,
        StaticLocalInitializationSemaphore = 95,
        FlowAnonymousFunction = 96,
        CoalesceAssignment = 97,
        Range = 99,
        ReDim = 101,
        ReDimClause = 102,
        RecursivePattern = 103,
        DiscardPattern = 104,
        SwitchExpression = 105,
        SwitchExpressionArm = 106,
        PropertySubpattern = 107,
        UsingDeclaration = 108,
        NegatedPattern = 109,
        BinaryPattern = 110,
        TypePattern = 111,
        RelationalPattern = 112,
        With = 113,
    }
    public enum OptimizationLevel
    {
        Debug = 0,
        Release = 1,
    }
    public readonly partial struct Optional<T>
    {
        private readonly T _value;
        private readonly int _dummyPrimitive;
        public Optional(T value) { throw null; }
        public bool HasValue { get { throw null; } }
        public T Value { get { throw null; } }
        public static implicit operator Microsoft.CodeAnalysis.Optional<T> (T value) { throw null; }
        public override string ToString() { throw null; }
    }
    public enum OutputKind
    {
        ConsoleApplication = 0,
        WindowsApplication = 1,
        DynamicallyLinkedLibrary = 2,
        NetModule = 3,
        WindowsRuntimeMetadata = 4,
        WindowsRuntimeApplication = 5,
    }
    public abstract partial class ParseOptions
    {
        internal ParseOptions() { }
        public Microsoft.CodeAnalysis.DocumentationMode DocumentationMode { get { throw null; } protected set { } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Errors { get { throw null; } }
        public abstract System.Collections.Generic.IReadOnlyDictionary<string, string> Features { get; }
        public Microsoft.CodeAnalysis.SourceCodeKind Kind { get { throw null; } protected set { } }
        public abstract string Language { get; }
        public abstract System.Collections.Generic.IEnumerable<string> PreprocessorSymbolNames { get; }
        public Microsoft.CodeAnalysis.SourceCodeKind SpecifiedKind { get { throw null; } protected set { } }
        protected abstract Microsoft.CodeAnalysis.ParseOptions CommonWithDocumentationMode(Microsoft.CodeAnalysis.DocumentationMode documentationMode);
        protected abstract Microsoft.CodeAnalysis.ParseOptions CommonWithFeatures(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> features);
        public abstract Microsoft.CodeAnalysis.ParseOptions CommonWithKind(Microsoft.CodeAnalysis.SourceCodeKind kind);
        public abstract override bool Equals(object? obj);
        protected bool EqualsHelper(Microsoft.CodeAnalysis.ParseOptions? other) { throw null; }
        public abstract override int GetHashCode();
        protected int GetHashCodeHelper() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.ParseOptions? left, Microsoft.CodeAnalysis.ParseOptions? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.ParseOptions? left, Microsoft.CodeAnalysis.ParseOptions? right) { throw null; }
        public Microsoft.CodeAnalysis.ParseOptions WithDocumentationMode(Microsoft.CodeAnalysis.DocumentationMode documentationMode) { throw null; }
        public Microsoft.CodeAnalysis.ParseOptions WithFeatures(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> features) { throw null; }
        public Microsoft.CodeAnalysis.ParseOptions WithKind(Microsoft.CodeAnalysis.SourceCodeKind kind) { throw null; }
    }
    public enum Platform
    {
        AnyCpu = 0,
        X86 = 1,
        X64 = 2,
        Itanium = 3,
        AnyCpu32BitPreferred = 4,
        Arm = 5,
        Arm64 = 6,
    }
    public abstract partial class PortableExecutableReference : Microsoft.CodeAnalysis.MetadataReference
    {
        protected PortableExecutableReference(Microsoft.CodeAnalysis.MetadataReferenceProperties properties, string? fullPath = null, Microsoft.CodeAnalysis.DocumentationProvider? initialDocumentation = null) : base (default(Microsoft.CodeAnalysis.MetadataReferenceProperties)) { }
        public override string? Display { get { throw null; } }
        public string? FilePath { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.DocumentationProvider CreateDocumentationProvider();
        public Microsoft.CodeAnalysis.Metadata GetMetadata() { throw null; }
        public Microsoft.CodeAnalysis.MetadataId GetMetadataId() { throw null; }
        protected abstract Microsoft.CodeAnalysis.Metadata GetMetadataImpl();
        public new Microsoft.CodeAnalysis.PortableExecutableReference WithAliases(System.Collections.Generic.IEnumerable<string> aliases) { throw null; }
        public new Microsoft.CodeAnalysis.PortableExecutableReference WithAliases(System.Collections.Immutable.ImmutableArray<string> aliases) { throw null; }
        public new Microsoft.CodeAnalysis.PortableExecutableReference WithEmbedInteropTypes(bool value) { throw null; }
        public new Microsoft.CodeAnalysis.PortableExecutableReference WithProperties(Microsoft.CodeAnalysis.MetadataReferenceProperties properties) { throw null; }
        protected abstract Microsoft.CodeAnalysis.PortableExecutableReference WithPropertiesImpl(Microsoft.CodeAnalysis.MetadataReferenceProperties properties);
    }
    public partial struct PreprocessingSymbolInfo : System.IEquatable<Microsoft.CodeAnalysis.PreprocessingSymbolInfo>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public bool IsDefined { get { throw null; } }
        public Microsoft.CodeAnalysis.IPreprocessingSymbol? Symbol { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.PreprocessingSymbolInfo other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum RefKind : byte
    {
        None = (byte)0,
        Ref = (byte)1,
        Out = (byte)2,
        In = (byte)3,
        RefReadOnly = (byte)3,
    }
    public enum ReportDiagnostic
    {
        Default = 0,
        Error = 1,
        Warn = 2,
        Info = 3,
        Hidden = 4,
        Suppress = 5,
    }
    public sealed partial class ResourceDescription
    {
        public ResourceDescription(string resourceName, System.Func<System.IO.Stream> dataProvider, bool isPublic) { }
        public ResourceDescription(string resourceName, string? fileName, System.Func<System.IO.Stream> dataProvider, bool isPublic) { }
    }
    public partial class RuleSet
    {
        public RuleSet(string filePath, Microsoft.CodeAnalysis.ReportDiagnostic generalOption, System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> specificOptions, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.RuleSetInclude> includes) { }
        public string FilePath { get { throw null; } }
        public Microsoft.CodeAnalysis.ReportDiagnostic GeneralDiagnosticOption { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.RuleSetInclude> Includes { get { throw null; } }
        public System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> SpecificDiagnosticOptions { get { throw null; } }
        public static Microsoft.CodeAnalysis.ReportDiagnostic GetDiagnosticOptionsFromRulesetFile(string? rulesetFileFullPath, out System.Collections.Generic.Dictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> specificDiagnosticOptions) { throw null; }
        public static System.Collections.Immutable.ImmutableArray<string> GetEffectiveIncludesFromFile(string filePath) { throw null; }
        public static Microsoft.CodeAnalysis.RuleSet LoadEffectiveRuleSetFromFile(string filePath) { throw null; }
        public Microsoft.CodeAnalysis.RuleSet? WithEffectiveAction(Microsoft.CodeAnalysis.ReportDiagnostic action) { throw null; }
    }
    public partial class RuleSetInclude
    {
        public RuleSetInclude(string includePath, Microsoft.CodeAnalysis.ReportDiagnostic action) { }
        public Microsoft.CodeAnalysis.ReportDiagnostic Action { get { throw null; } }
        public string IncludePath { get { throw null; } }
        public Microsoft.CodeAnalysis.RuleSet? LoadRuleSet(Microsoft.CodeAnalysis.RuleSet parent) { throw null; }
    }
    public enum SarifVersion
    {
        Default = 1,
        Sarif1 = 1,
        Sarif2 = 2,
        Latest = 2147483647,
    }
    public static partial class SarifVersionFacts
    {
        public static bool TryParse(string version, out Microsoft.CodeAnalysis.SarifVersion result) { throw null; }
    }
    public abstract partial class ScriptCompilationInfo
    {
        internal ScriptCompilationInfo() { }
        public System.Type? GlobalsType { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation? PreviousScriptCompilation { get { throw null; } }
        public System.Type ReturnType { get { throw null; } }
        public Microsoft.CodeAnalysis.ScriptCompilationInfo WithPreviousScriptCompilation(Microsoft.CodeAnalysis.Compilation? compilation) { throw null; }
    }
    public abstract partial class SemanticModel
    {
        protected SemanticModel() { }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.Compilation CompilationCore { get; }
        internal abstract Microsoft.CodeAnalysis.SemanticModel ContainingModelOrSelf { get; }
        public virtual bool IgnoresAccessibility { get { throw null; } }
        public abstract bool IsSpeculativeSemanticModel { get; }
        public abstract string Language { get; }
        public abstract int OriginalPositionForSpeculation { get; }
        public Microsoft.CodeAnalysis.SemanticModel? ParentModel { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.SemanticModel? ParentModelCore { get; }
        protected abstract Microsoft.CodeAnalysis.SyntaxNode RootCore { get; }
        public Microsoft.CodeAnalysis.SyntaxTree SyntaxTree { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.SyntaxTree SyntaxTreeCore { get; }
        protected abstract Microsoft.CodeAnalysis.ControlFlowAnalysis AnalyzeControlFlowCore(Microsoft.CodeAnalysis.SyntaxNode statement);
        protected abstract Microsoft.CodeAnalysis.ControlFlowAnalysis AnalyzeControlFlowCore(Microsoft.CodeAnalysis.SyntaxNode firstStatement, Microsoft.CodeAnalysis.SyntaxNode lastStatement);
        protected abstract Microsoft.CodeAnalysis.DataFlowAnalysis AnalyzeDataFlowCore(Microsoft.CodeAnalysis.SyntaxNode statementOrExpression);
        protected abstract Microsoft.CodeAnalysis.DataFlowAnalysis AnalyzeDataFlowCore(Microsoft.CodeAnalysis.SyntaxNode firstStatement, Microsoft.CodeAnalysis.SyntaxNode lastStatement);
        internal abstract void ComputeDeclarationsInNode(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.ISymbol associatedSymbol, bool getSymbol, Microsoft.CodeAnalysis.PooledObjects.ArrayBuilder<Microsoft.CodeAnalysis.DeclarationInfo> builder, System.Threading.CancellationToken cancellationToken, int? levelsToCompute = default(int?));
        internal abstract void ComputeDeclarationsInSpan(Microsoft.CodeAnalysis.Text.TextSpan span, bool getSymbol, Microsoft.CodeAnalysis.PooledObjects.ArrayBuilder<Microsoft.CodeAnalysis.DeclarationInfo> builder, System.Threading.CancellationToken cancellationToken);
        protected abstract Microsoft.CodeAnalysis.IAliasSymbol? GetAliasInfoCore(Microsoft.CodeAnalysis.SyntaxNode nameSyntax, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public Microsoft.CodeAnalysis.Optional<object?> GetConstantValue(Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        protected abstract Microsoft.CodeAnalysis.Optional<object?> GetConstantValueCore(Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetDeclarationDiagnostics(Microsoft.CodeAnalysis.Text.TextSpan? span = default(Microsoft.CodeAnalysis.Text.TextSpan?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        protected abstract Microsoft.CodeAnalysis.ISymbol? GetDeclaredSymbolCore(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> GetDeclaredSymbolsCore(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics(Microsoft.CodeAnalysis.Text.TextSpan? span = default(Microsoft.CodeAnalysis.Text.TextSpan?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public Microsoft.CodeAnalysis.ISymbol? GetEnclosingSymbol(int position, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        protected abstract Microsoft.CodeAnalysis.ISymbol? GetEnclosingSymbolCore(int position, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> GetMemberGroupCore(Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetMethodBodyDiagnostics(Microsoft.CodeAnalysis.Text.TextSpan? span = default(Microsoft.CodeAnalysis.Text.TextSpan?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract Microsoft.CodeAnalysis.NullableContext GetNullableContext(int position);
        public Microsoft.CodeAnalysis.IOperation? GetOperation(Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        protected abstract Microsoft.CodeAnalysis.IOperation? GetOperationCore(Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken);
        public Microsoft.CodeAnalysis.PreprocessingSymbolInfo GetPreprocessingSymbolInfo(Microsoft.CodeAnalysis.SyntaxNode nameSyntax) { throw null; }
        protected abstract Microsoft.CodeAnalysis.PreprocessingSymbolInfo GetPreprocessingSymbolInfoCore(Microsoft.CodeAnalysis.SyntaxNode nameSyntax);
        protected abstract Microsoft.CodeAnalysis.IAliasSymbol? GetSpeculativeAliasInfoCore(int position, Microsoft.CodeAnalysis.SyntaxNode nameSyntax, Microsoft.CodeAnalysis.SpeculativeBindingOption bindingOption);
        protected abstract Microsoft.CodeAnalysis.SymbolInfo GetSpeculativeSymbolInfoCore(int position, Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SpeculativeBindingOption bindingOption);
        protected abstract Microsoft.CodeAnalysis.TypeInfo GetSpeculativeTypeInfoCore(int position, Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SpeculativeBindingOption bindingOption);
        protected abstract Microsoft.CodeAnalysis.SymbolInfo GetSymbolInfoCore(Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetSyntaxDiagnostics(Microsoft.CodeAnalysis.Text.TextSpan? span = default(Microsoft.CodeAnalysis.Text.TextSpan?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        protected internal virtual Microsoft.CodeAnalysis.SyntaxNode GetTopmostNodeForDiagnosticAnalysis(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.SyntaxNode declaringSyntax) { throw null; }
        protected abstract Microsoft.CodeAnalysis.TypeInfo GetTypeInfoCore(Microsoft.CodeAnalysis.SyntaxNode node, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public bool IsAccessible(int position, Microsoft.CodeAnalysis.ISymbol symbol) { throw null; }
        protected abstract bool IsAccessibleCore(int position, Microsoft.CodeAnalysis.ISymbol symbol);
        public bool IsEventUsableAsField(int position, Microsoft.CodeAnalysis.IEventSymbol eventSymbol) { throw null; }
        protected abstract bool IsEventUsableAsFieldCore(int position, Microsoft.CodeAnalysis.IEventSymbol eventSymbol);
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupBaseMembers(int position, string? name = null) { throw null; }
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupBaseMembersCore(int position, string? name);
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupLabels(int position, string? name = null) { throw null; }
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupLabelsCore(int position, string? name);
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupNamespacesAndTypes(int position, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container = null, string? name = null) { throw null; }
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupNamespacesAndTypesCore(int position, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container, string? name);
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupStaticMembers(int position, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container = null, string? name = null) { throw null; }
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupStaticMembersCore(int position, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container, string? name);
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupSymbols(int position, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container = null, string? name = null, bool includeReducedExtensionMethods = false) { throw null; }
        protected abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> LookupSymbolsCore(int position, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol? container, string? name, bool includeReducedExtensionMethods);
    }
    public readonly partial struct SeparatedSyntaxList<TNode> : System.Collections.Generic.IEnumerable<TNode>, System.Collections.Generic.IReadOnlyCollection<TNode>, System.Collections.Generic.IReadOnlyList<TNode>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode>> where TNode : Microsoft.CodeAnalysis.SyntaxNode
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public TNode this[int index] { get { throw null; } }
        public int SeparatorCount { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> Add(TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> AddRange(System.Collections.Generic.IEnumerable<TNode> nodes) { throw null; }
        public bool Any() { throw null; }
        public bool Contains(TNode node) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public TNode First() { throw null; }
        public TNode? FirstOrDefault() { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode>.Enumerator GetEnumerator() { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken GetSeparator(int index) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> GetSeparators() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList GetWithSeparators() { throw null; }
        public int IndexOf(System.Func<TNode, bool> predicate) { throw null; }
        public int IndexOf(TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> Insert(int index, TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> InsertRange(int index, System.Collections.Generic.IEnumerable<TNode> nodes) { throw null; }
        public TNode Last() { throw null; }
        public int LastIndexOf(System.Func<TNode, bool> predicate) { throw null; }
        public int LastIndexOf(TNode node) { throw null; }
        public TNode? LastOrDefault() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> left, Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> right) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> (Microsoft.CodeAnalysis.SeparatedSyntaxList<Microsoft.CodeAnalysis.SyntaxNode> nodes) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.SeparatedSyntaxList<Microsoft.CodeAnalysis.SyntaxNode> (Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> nodes) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> left, Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> right) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> Remove(TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> RemoveAt(int index) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> Replace(TNode nodeInList, TNode newNode) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> ReplaceRange(TNode nodeInList, System.Collections.Generic.IEnumerable<TNode> newNodes) { throw null; }
        public Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> ReplaceSeparator(Microsoft.CodeAnalysis.SyntaxToken separatorToken, Microsoft.CodeAnalysis.SyntaxToken newSeparator) { throw null; }
        System.Collections.Generic.IEnumerator<TNode> System.Collections.Generic.IEnumerable<TNode>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public TNode Current { get { throw null; } }
            public override bool Equals(object? obj) { throw null; }
            public override int GetHashCode() { throw null; }
            public bool MoveNext() { throw null; }
            public void Reset() { }
        }
    }
    public enum SourceCodeKind
    {
        Regular = 0,
        Script = 1,
        [System.ObsoleteAttribute("Use Script instead", false)]
        Interactive = 2,
    }
    public partial class SourceFileResolver : Microsoft.CodeAnalysis.SourceReferenceResolver, System.IEquatable<Microsoft.CodeAnalysis.SourceFileResolver>
    {
        public SourceFileResolver(System.Collections.Generic.IEnumerable<string> searchPaths, string? baseDirectory) { }
        public SourceFileResolver(System.Collections.Immutable.ImmutableArray<string> searchPaths, string? baseDirectory) { }
        public SourceFileResolver(System.Collections.Immutable.ImmutableArray<string> searchPaths, string? baseDirectory, System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, string>> pathMap) { }
        public string? BaseDirectory { get { throw null; } }
        public static Microsoft.CodeAnalysis.SourceFileResolver Default { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, string>> PathMap { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<string> SearchPaths { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.SourceFileResolver? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        protected virtual bool FileExists(string? resolvedPath) { throw null; }
        public override int GetHashCode() { throw null; }
        public override string? NormalizePath(string path, string? baseFilePath) { throw null; }
        public override System.IO.Stream OpenRead(string resolvedPath) { throw null; }
        public override string? ResolveReference(string path, string? baseFilePath) { throw null; }
    }
    public readonly partial struct SourceProductionContext
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public void AddSource(string hintName, Microsoft.CodeAnalysis.Text.SourceText sourceText) { }
        public void AddSource(string hintName, string source) { }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public abstract partial class SourceReferenceResolver
    {
        protected SourceReferenceResolver() { }
        public abstract override bool Equals(object? other);
        public abstract override int GetHashCode();
        public abstract string? NormalizePath(string path, string? baseFilePath);
        public abstract System.IO.Stream OpenRead(string resolvedPath);
        public virtual Microsoft.CodeAnalysis.Text.SourceText ReadText(string resolvedPath) { throw null; }
        public abstract string? ResolveReference(string path, string? baseFilePath);
    }
    public enum SpecialType : sbyte
    {
        None = (sbyte)0,
        System_Object = (sbyte)1,
        System_Enum = (sbyte)2,
        System_MulticastDelegate = (sbyte)3,
        System_Delegate = (sbyte)4,
        System_ValueType = (sbyte)5,
        System_Void = (sbyte)6,
        System_Boolean = (sbyte)7,
        System_Char = (sbyte)8,
        System_SByte = (sbyte)9,
        System_Byte = (sbyte)10,
        System_Int16 = (sbyte)11,
        System_UInt16 = (sbyte)12,
        System_Int32 = (sbyte)13,
        System_UInt32 = (sbyte)14,
        System_Int64 = (sbyte)15,
        System_UInt64 = (sbyte)16,
        System_Decimal = (sbyte)17,
        System_Single = (sbyte)18,
        System_Double = (sbyte)19,
        System_String = (sbyte)20,
        System_IntPtr = (sbyte)21,
        System_UIntPtr = (sbyte)22,
        System_Array = (sbyte)23,
        System_Collections_IEnumerable = (sbyte)24,
        System_Collections_Generic_IEnumerable_T = (sbyte)25,
        System_Collections_Generic_IList_T = (sbyte)26,
        System_Collections_Generic_ICollection_T = (sbyte)27,
        System_Collections_IEnumerator = (sbyte)28,
        System_Collections_Generic_IEnumerator_T = (sbyte)29,
        System_Collections_Generic_IReadOnlyList_T = (sbyte)30,
        System_Collections_Generic_IReadOnlyCollection_T = (sbyte)31,
        System_Nullable_T = (sbyte)32,
        System_DateTime = (sbyte)33,
        System_Runtime_CompilerServices_IsVolatile = (sbyte)34,
        System_IDisposable = (sbyte)35,
        System_TypedReference = (sbyte)36,
        System_ArgIterator = (sbyte)37,
        System_RuntimeArgumentHandle = (sbyte)38,
        System_RuntimeFieldHandle = (sbyte)39,
        System_RuntimeMethodHandle = (sbyte)40,
        System_RuntimeTypeHandle = (sbyte)41,
        System_IAsyncResult = (sbyte)42,
        System_AsyncCallback = (sbyte)43,
        System_Runtime_CompilerServices_RuntimeFeature = (sbyte)44,
        Count = (sbyte)45,
        System_Runtime_CompilerServices_PreserveBaseOverridesAttribute = (sbyte)45,
    }
    public enum SpeculativeBindingOption
    {
        BindAsExpression = 0,
        BindAsTypeOrNamespace = 1,
    }
    public abstract partial class StrongNameProvider
    {
        protected StrongNameProvider() { }
        internal abstract Microsoft.CodeAnalysis.StrongNameFileSystem FileSystem { get; }
        internal abstract Microsoft.CodeAnalysis.StrongNameKeys CreateKeys(string? keyFilePath, string? keyContainerName, bool hasCounterSignature, Microsoft.CodeAnalysis.CommonMessageProvider messageProvider);
        public abstract override bool Equals(object? other);
        public abstract override int GetHashCode();
        internal abstract void SignBuilder(Microsoft.Cci.ExtendedPEBuilder peBuilder, System.Reflection.Metadata.BlobBuilder peBlob, System.Security.Cryptography.RSAParameters privateKey);
        internal abstract void SignFile(Microsoft.CodeAnalysis.StrongNameKeys keys, string filePath);
    }
    public partial struct SubsystemVersion : System.IEquatable<Microsoft.CodeAnalysis.SubsystemVersion>
    {
        private int _dummyPrimitive;
        public bool IsValid { get { throw null; } }
        public int Major { get { throw null; } }
        public int Minor { get { throw null; } }
        public static Microsoft.CodeAnalysis.SubsystemVersion None { get { throw null; } }
        public static Microsoft.CodeAnalysis.SubsystemVersion Windows2000 { get { throw null; } }
        public static Microsoft.CodeAnalysis.SubsystemVersion Windows7 { get { throw null; } }
        public static Microsoft.CodeAnalysis.SubsystemVersion Windows8 { get { throw null; } }
        public static Microsoft.CodeAnalysis.SubsystemVersion WindowsVista { get { throw null; } }
        public static Microsoft.CodeAnalysis.SubsystemVersion WindowsXP { get { throw null; } }
        public static Microsoft.CodeAnalysis.SubsystemVersion Create(int major, int minor) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SubsystemVersion other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public override string ToString() { throw null; }
        public static bool TryParse(string str, out Microsoft.CodeAnalysis.SubsystemVersion version) { throw null; }
    }
    public sealed partial class SuppressionDescriptor : System.IEquatable<Microsoft.CodeAnalysis.SuppressionDescriptor?>
    {
        public SuppressionDescriptor(string id, string suppressedDiagnosticId, Microsoft.CodeAnalysis.LocalizableString justification) { }
        public SuppressionDescriptor(string id, string suppressedDiagnosticId, string justification) { }
        public string Id { get { throw null; } }
        public Microsoft.CodeAnalysis.LocalizableString Justification { get { throw null; } }
        public string SuppressedDiagnosticId { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.SuppressionDescriptor? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum SymbolDisplayDelegateStyle
    {
        NameOnly = 0,
        NameAndParameters = 1,
        NameAndSignature = 2,
    }
    public enum SymbolDisplayExtensionMethodStyle
    {
        Default = 0,
        InstanceMethod = 1,
        StaticMethod = 2,
    }
    public static partial class SymbolDisplayExtensions
    {
        public static string ToDisplayString(this System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SymbolDisplayPart> parts) { throw null; }
    }
    public partial class SymbolDisplayFormat
    {
        public SymbolDisplayFormat(Microsoft.CodeAnalysis.SymbolDisplayGlobalNamespaceStyle globalNamespaceStyle = Microsoft.CodeAnalysis.SymbolDisplayGlobalNamespaceStyle.Omitted, Microsoft.CodeAnalysis.SymbolDisplayTypeQualificationStyle typeQualificationStyle = Microsoft.CodeAnalysis.SymbolDisplayTypeQualificationStyle.NameOnly, Microsoft.CodeAnalysis.SymbolDisplayGenericsOptions genericsOptions = Microsoft.CodeAnalysis.SymbolDisplayGenericsOptions.None, Microsoft.CodeAnalysis.SymbolDisplayMemberOptions memberOptions = Microsoft.CodeAnalysis.SymbolDisplayMemberOptions.None, Microsoft.CodeAnalysis.SymbolDisplayDelegateStyle delegateStyle = Microsoft.CodeAnalysis.SymbolDisplayDelegateStyle.NameOnly, Microsoft.CodeAnalysis.SymbolDisplayExtensionMethodStyle extensionMethodStyle = Microsoft.CodeAnalysis.SymbolDisplayExtensionMethodStyle.Default, Microsoft.CodeAnalysis.SymbolDisplayParameterOptions parameterOptions = Microsoft.CodeAnalysis.SymbolDisplayParameterOptions.None, Microsoft.CodeAnalysis.SymbolDisplayPropertyStyle propertyStyle = Microsoft.CodeAnalysis.SymbolDisplayPropertyStyle.NameOnly, Microsoft.CodeAnalysis.SymbolDisplayLocalOptions localOptions = Microsoft.CodeAnalysis.SymbolDisplayLocalOptions.None, Microsoft.CodeAnalysis.SymbolDisplayKindOptions kindOptions = Microsoft.CodeAnalysis.SymbolDisplayKindOptions.None, Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions miscellaneousOptions = Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions.None) { }
        public static Microsoft.CodeAnalysis.SymbolDisplayFormat CSharpErrorMessageFormat { get { throw null; } }
        public static Microsoft.CodeAnalysis.SymbolDisplayFormat CSharpShortErrorMessageFormat { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayDelegateStyle DelegateStyle { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayExtensionMethodStyle ExtensionMethodStyle { get { throw null; } }
        public static Microsoft.CodeAnalysis.SymbolDisplayFormat FullyQualifiedFormat { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayGenericsOptions GenericsOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayGlobalNamespaceStyle GlobalNamespaceStyle { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayKindOptions KindOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayLocalOptions LocalOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayMemberOptions MemberOptions { get { throw null; } }
        public static Microsoft.CodeAnalysis.SymbolDisplayFormat MinimallyQualifiedFormat { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions MiscellaneousOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayParameterOptions ParameterOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayPropertyStyle PropertyStyle { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayTypeQualificationStyle TypeQualificationStyle { get { throw null; } }
        public static Microsoft.CodeAnalysis.SymbolDisplayFormat VisualBasicErrorMessageFormat { get { throw null; } }
        public static Microsoft.CodeAnalysis.SymbolDisplayFormat VisualBasicShortErrorMessageFormat { get { throw null; } }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat AddGenericsOptions(Microsoft.CodeAnalysis.SymbolDisplayGenericsOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat AddKindOptions(Microsoft.CodeAnalysis.SymbolDisplayKindOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat AddLocalOptions(Microsoft.CodeAnalysis.SymbolDisplayLocalOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat AddMemberOptions(Microsoft.CodeAnalysis.SymbolDisplayMemberOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat AddMiscellaneousOptions(Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat AddParameterOptions(Microsoft.CodeAnalysis.SymbolDisplayParameterOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat RemoveGenericsOptions(Microsoft.CodeAnalysis.SymbolDisplayGenericsOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat RemoveKindOptions(Microsoft.CodeAnalysis.SymbolDisplayKindOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat RemoveLocalOptions(Microsoft.CodeAnalysis.SymbolDisplayLocalOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat RemoveMemberOptions(Microsoft.CodeAnalysis.SymbolDisplayMemberOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat RemoveMiscellaneousOptions(Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat RemoveParameterOptions(Microsoft.CodeAnalysis.SymbolDisplayParameterOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat WithGenericsOptions(Microsoft.CodeAnalysis.SymbolDisplayGenericsOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat WithGlobalNamespaceStyle(Microsoft.CodeAnalysis.SymbolDisplayGlobalNamespaceStyle style) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat WithKindOptions(Microsoft.CodeAnalysis.SymbolDisplayKindOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat WithLocalOptions(Microsoft.CodeAnalysis.SymbolDisplayLocalOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat WithMemberOptions(Microsoft.CodeAnalysis.SymbolDisplayMemberOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat WithMiscellaneousOptions(Microsoft.CodeAnalysis.SymbolDisplayMiscellaneousOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayFormat WithParameterOptions(Microsoft.CodeAnalysis.SymbolDisplayParameterOptions options) { throw null; }
    }
    [System.FlagsAttribute]
    public enum SymbolDisplayGenericsOptions
    {
        None = 0,
        IncludeTypeParameters = 1,
        IncludeTypeConstraints = 2,
        IncludeVariance = 4,
    }
    public enum SymbolDisplayGlobalNamespaceStyle
    {
        Omitted = 0,
        OmittedAsContaining = 1,
        Included = 2,
    }
    [System.FlagsAttribute]
    public enum SymbolDisplayKindOptions
    {
        None = 0,
        IncludeNamespaceKeyword = 1,
        IncludeTypeKeyword = 2,
        IncludeMemberKeyword = 4,
    }
    [System.FlagsAttribute]
    public enum SymbolDisplayLocalOptions
    {
        None = 0,
        IncludeType = 1,
        IncludeConstantValue = 2,
        IncludeRef = 4,
    }
    [System.FlagsAttribute]
    public enum SymbolDisplayMemberOptions
    {
        None = 0,
        IncludeType = 1,
        IncludeModifiers = 2,
        IncludeAccessibility = 4,
        IncludeExplicitInterface = 8,
        IncludeParameters = 16,
        IncludeContainingType = 32,
        IncludeConstantValue = 64,
        IncludeRef = 128,
    }
    [System.FlagsAttribute]
    public enum SymbolDisplayMiscellaneousOptions
    {
        None = 0,
        UseSpecialTypes = 1,
        EscapeKeywordIdentifiers = 2,
        UseAsterisksInMultiDimensionalArrays = 4,
        UseErrorTypeSymbolName = 8,
        RemoveAttributeSuffix = 16,
        ExpandNullable = 32,
        IncludeNullableReferenceTypeModifier = 64,
        AllowDefaultLiteral = 128,
        IncludeNotNullableReferenceTypeModifier = 256,
    }
    [System.FlagsAttribute]
    public enum SymbolDisplayParameterOptions
    {
        None = 0,
        IncludeExtensionThis = 1,
        IncludeParamsRefOut = 2,
        IncludeType = 4,
        IncludeName = 8,
        IncludeDefaultValue = 16,
        IncludeOptionalBrackets = 32,
    }
    public partial struct SymbolDisplayPart
    {
        private object _dummy;
        private int _dummyPrimitive;
        public SymbolDisplayPart(Microsoft.CodeAnalysis.SymbolDisplayPartKind kind, Microsoft.CodeAnalysis.ISymbol? symbol, string text) { throw null; }
        public Microsoft.CodeAnalysis.SymbolDisplayPartKind Kind { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol? Symbol { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public enum SymbolDisplayPartKind
    {
        AliasName = 0,
        AssemblyName = 1,
        ClassName = 2,
        DelegateName = 3,
        EnumName = 4,
        ErrorTypeName = 5,
        EventName = 6,
        FieldName = 7,
        InterfaceName = 8,
        Keyword = 9,
        LabelName = 10,
        LineBreak = 11,
        NumericLiteral = 12,
        StringLiteral = 13,
        LocalName = 14,
        MethodName = 15,
        ModuleName = 16,
        NamespaceName = 17,
        Operator = 18,
        ParameterName = 19,
        PropertyName = 20,
        Punctuation = 21,
        Space = 22,
        StructName = 23,
        AnonymousTypeIndicator = 24,
        Text = 25,
        TypeParameterName = 26,
        RangeVariableName = 27,
        EnumMemberName = 28,
        ExtensionMethodName = 29,
        ConstantName = 30,
        RecordClassName = 31,
        RecordStructName = 32,
    }
    public enum SymbolDisplayPropertyStyle
    {
        NameOnly = 0,
        ShowReadWriteDescriptor = 1,
    }
    public enum SymbolDisplayTypeQualificationStyle
    {
        NameOnly = 0,
        NameAndContainingTypes = 1,
        NameAndContainingTypesAndNamespaces = 2,
    }
    public sealed partial class SymbolEqualityComparer : System.Collections.Generic.IEqualityComparer<Microsoft.CodeAnalysis.ISymbol?>
    {
        internal SymbolEqualityComparer() { }
        public static readonly Microsoft.CodeAnalysis.SymbolEqualityComparer Default;
        public static readonly Microsoft.CodeAnalysis.SymbolEqualityComparer IncludeNullability;
        public bool Equals(Microsoft.CodeAnalysis.ISymbol? x, Microsoft.CodeAnalysis.ISymbol? y) { throw null; }
        public int GetHashCode(Microsoft.CodeAnalysis.ISymbol? obj) { throw null; }
    }
    [System.FlagsAttribute]
    public enum SymbolFilter
    {
        None = 0,
        Namespace = 1,
        Type = 2,
        Member = 4,
        TypeAndMember = 6,
        All = 7,
    }
    public partial struct SymbolInfo : System.IEquatable<Microsoft.CodeAnalysis.SymbolInfo>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Microsoft.CodeAnalysis.CandidateReason CandidateReason { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> CandidateSymbols { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol? Symbol { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.SymbolInfo other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum SymbolKind
    {
        Alias = 0,
        ArrayType = 1,
        Assembly = 2,
        DynamicType = 3,
        ErrorType = 4,
        Event = 5,
        Field = 6,
        Label = 7,
        Local = 8,
        Method = 9,
        NetModule = 10,
        NamedType = 11,
        Namespace = 12,
        Parameter = 13,
        PointerType = 14,
        Property = 15,
        RangeVariable = 16,
        TypeParameter = 17,
        Preprocessing = 18,
        Discard = 19,
        FunctionPointerType = 20,
    }
    public abstract partial class SymbolVisitor
    {
        protected SymbolVisitor() { }
        public virtual void DefaultVisit(Microsoft.CodeAnalysis.ISymbol symbol) { }
        public virtual void Visit(Microsoft.CodeAnalysis.ISymbol? symbol) { }
        public virtual void VisitAlias(Microsoft.CodeAnalysis.IAliasSymbol symbol) { }
        public virtual void VisitArrayType(Microsoft.CodeAnalysis.IArrayTypeSymbol symbol) { }
        public virtual void VisitAssembly(Microsoft.CodeAnalysis.IAssemblySymbol symbol) { }
        public virtual void VisitDiscard(Microsoft.CodeAnalysis.IDiscardSymbol symbol) { }
        public virtual void VisitDynamicType(Microsoft.CodeAnalysis.IDynamicTypeSymbol symbol) { }
        public virtual void VisitEvent(Microsoft.CodeAnalysis.IEventSymbol symbol) { }
        public virtual void VisitField(Microsoft.CodeAnalysis.IFieldSymbol symbol) { }
        public virtual void VisitFunctionPointerType(Microsoft.CodeAnalysis.IFunctionPointerTypeSymbol symbol) { }
        public virtual void VisitLabel(Microsoft.CodeAnalysis.ILabelSymbol symbol) { }
        public virtual void VisitLocal(Microsoft.CodeAnalysis.ILocalSymbol symbol) { }
        public virtual void VisitMethod(Microsoft.CodeAnalysis.IMethodSymbol symbol) { }
        public virtual void VisitModule(Microsoft.CodeAnalysis.IModuleSymbol symbol) { }
        public virtual void VisitNamedType(Microsoft.CodeAnalysis.INamedTypeSymbol symbol) { }
        public virtual void VisitNamespace(Microsoft.CodeAnalysis.INamespaceSymbol symbol) { }
        public virtual void VisitParameter(Microsoft.CodeAnalysis.IParameterSymbol symbol) { }
        public virtual void VisitPointerType(Microsoft.CodeAnalysis.IPointerTypeSymbol symbol) { }
        public virtual void VisitProperty(Microsoft.CodeAnalysis.IPropertySymbol symbol) { }
        public virtual void VisitRangeVariable(Microsoft.CodeAnalysis.IRangeVariableSymbol symbol) { }
        public virtual void VisitTypeParameter(Microsoft.CodeAnalysis.ITypeParameterSymbol symbol) { }
    }
    public abstract partial class SymbolVisitor<TResult>
    {
        protected SymbolVisitor() { }
        public virtual TResult? DefaultVisit(Microsoft.CodeAnalysis.ISymbol symbol) { throw null; }
        public virtual TResult? Visit(Microsoft.CodeAnalysis.ISymbol? symbol) { throw null; }
        public virtual TResult? VisitAlias(Microsoft.CodeAnalysis.IAliasSymbol symbol) { throw null; }
        public virtual TResult? VisitArrayType(Microsoft.CodeAnalysis.IArrayTypeSymbol symbol) { throw null; }
        public virtual TResult? VisitAssembly(Microsoft.CodeAnalysis.IAssemblySymbol symbol) { throw null; }
        public virtual TResult? VisitDiscard(Microsoft.CodeAnalysis.IDiscardSymbol symbol) { throw null; }
        public virtual TResult? VisitDynamicType(Microsoft.CodeAnalysis.IDynamicTypeSymbol symbol) { throw null; }
        public virtual TResult? VisitEvent(Microsoft.CodeAnalysis.IEventSymbol symbol) { throw null; }
        public virtual TResult? VisitField(Microsoft.CodeAnalysis.IFieldSymbol symbol) { throw null; }
        public virtual TResult? VisitFunctionPointerType(Microsoft.CodeAnalysis.IFunctionPointerTypeSymbol symbol) { throw null; }
        public virtual TResult? VisitLabel(Microsoft.CodeAnalysis.ILabelSymbol symbol) { throw null; }
        public virtual TResult? VisitLocal(Microsoft.CodeAnalysis.ILocalSymbol symbol) { throw null; }
        public virtual TResult? VisitMethod(Microsoft.CodeAnalysis.IMethodSymbol symbol) { throw null; }
        public virtual TResult? VisitModule(Microsoft.CodeAnalysis.IModuleSymbol symbol) { throw null; }
        public virtual TResult? VisitNamedType(Microsoft.CodeAnalysis.INamedTypeSymbol symbol) { throw null; }
        public virtual TResult? VisitNamespace(Microsoft.CodeAnalysis.INamespaceSymbol symbol) { throw null; }
        public virtual TResult? VisitParameter(Microsoft.CodeAnalysis.IParameterSymbol symbol) { throw null; }
        public virtual TResult? VisitPointerType(Microsoft.CodeAnalysis.IPointerTypeSymbol symbol) { throw null; }
        public virtual TResult? VisitProperty(Microsoft.CodeAnalysis.IPropertySymbol symbol) { throw null; }
        public virtual TResult? VisitRangeVariable(Microsoft.CodeAnalysis.IRangeVariableSymbol symbol) { throw null; }
        public virtual TResult? VisitTypeParameter(Microsoft.CodeAnalysis.ITypeParameterSymbol symbol) { throw null; }
    }
    public sealed partial class SyntaxAnnotation : System.IEquatable<Microsoft.CodeAnalysis.SyntaxAnnotation?>
    {
        public SyntaxAnnotation() { }
        public SyntaxAnnotation(string? kind) { }
        public SyntaxAnnotation(string? kind, string? data) { }
        public string? Data { get { throw null; } }
        public static Microsoft.CodeAnalysis.SyntaxAnnotation ElasticAnnotation { get { throw null; } }
        public string? Kind { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxAnnotation? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxAnnotation? left, Microsoft.CodeAnalysis.SyntaxAnnotation? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxAnnotation? left, Microsoft.CodeAnalysis.SyntaxAnnotation? right) { throw null; }
    }
    public delegate Microsoft.CodeAnalysis.ISyntaxContextReceiver? SyntaxContextReceiverCreator();
    public readonly partial struct SyntaxList<TNode> : System.Collections.Generic.IEnumerable<TNode>, System.Collections.Generic.IReadOnlyCollection<TNode>, System.Collections.Generic.IReadOnlyList<TNode>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.SyntaxList<TNode>> where TNode : Microsoft.CodeAnalysis.SyntaxNode
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SyntaxList(System.Collections.Generic.IEnumerable<TNode>? nodes) { throw null; }
        public SyntaxList(TNode? node) { throw null; }
        public int Count { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public TNode this[int index] { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> Add(TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> AddRange(System.Collections.Generic.IEnumerable<TNode> nodes) { throw null; }
        public bool Any() { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxList<TNode> other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public TNode First() { throw null; }
        public TNode? FirstOrDefault() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode>.Enumerator GetEnumerator() { throw null; }
        public override int GetHashCode() { throw null; }
        public int IndexOf(System.Func<TNode, bool> predicate) { throw null; }
        public int IndexOf(TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> Insert(int index, TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> InsertRange(int index, System.Collections.Generic.IEnumerable<TNode> nodes) { throw null; }
        public TNode Last() { throw null; }
        public int LastIndexOf(System.Func<TNode, bool> predicate) { throw null; }
        public int LastIndexOf(TNode node) { throw null; }
        public TNode? LastOrDefault() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxList<TNode> left, Microsoft.CodeAnalysis.SyntaxList<TNode> right) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.SyntaxList<TNode> (Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.SyntaxNode> nodes) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.SyntaxNode> (Microsoft.CodeAnalysis.SyntaxList<TNode> nodes) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxList<TNode> left, Microsoft.CodeAnalysis.SyntaxList<TNode> right) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> Remove(TNode node) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> RemoveAt(int index) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> Replace(TNode nodeInList, TNode newNode) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxList<TNode> ReplaceRange(TNode nodeInList, System.Collections.Generic.IEnumerable<TNode> newNodes) { throw null; }
        System.Collections.Generic.IEnumerator<TNode> System.Collections.Generic.IEnumerable<TNode>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public TNode Current { get { throw null; } }
            public override bool Equals(object? obj) { throw null; }
            public override int GetHashCode() { throw null; }
            public bool MoveNext() { throw null; }
            public void Reset() { }
        }
    }
    public abstract partial class SyntaxNode
    {
        internal SyntaxNode() { }
        public bool ContainsAnnotations { get { throw null; } }
        public bool ContainsDiagnostics { get { throw null; } }
        public bool ContainsDirectives { get { throw null; } }
        public bool ContainsSkippedText { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public bool HasLeadingTrivia { get { throw null; } }
        public bool HasStructuredTrivia { get { throw null; } }
        public bool HasTrailingTrivia { get { throw null; } }
        public bool IsMissing { get { throw null; } }
        public bool IsStructuredTrivia { get { throw null; } }
        protected string KindText { get { throw null; } }
        public abstract string Language { get; }
        public Microsoft.CodeAnalysis.SyntaxNode? Parent { get { throw null; } }
        public virtual Microsoft.CodeAnalysis.SyntaxTrivia ParentTrivia { get { throw null; } }
        public int RawKind { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public int SpanStart { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTree SyntaxTree { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.SyntaxTree SyntaxTreeCore { get; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> Ancestors(bool ascendOutOfTrivia = true) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> AncestorsAndSelf(bool ascendOutOfTrivia = true) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> ChildNodes() { throw null; }
        public Microsoft.CodeAnalysis.ChildSyntaxList ChildNodesAndTokens() { throw null; }
        public virtual Microsoft.CodeAnalysis.SyntaxNodeOrToken ChildThatContainsPosition(int position) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> ChildTokens() { throw null; }
        public bool Contains(Microsoft.CodeAnalysis.SyntaxNode? node) { throw null; }
        public T? CopyAnnotationsTo<T>(T? node) where T : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> DescendantNodes(Microsoft.CodeAnalysis.Text.TextSpan span, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> DescendantNodes(System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> DescendantNodesAndSelf(Microsoft.CodeAnalysis.Text.TextSpan span, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> DescendantNodesAndSelf(System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> DescendantNodesAndTokens(Microsoft.CodeAnalysis.Text.TextSpan span, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> DescendantNodesAndTokens(System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> DescendantNodesAndTokensAndSelf(Microsoft.CodeAnalysis.Text.TextSpan span, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> DescendantNodesAndTokensAndSelf(System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> DescendantTokens(Microsoft.CodeAnalysis.Text.TextSpan span, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> DescendantTokens(System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> DescendantTrivia(Microsoft.CodeAnalysis.Text.TextSpan span, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> DescendantTrivia(System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool>? descendIntoChildren = null, bool descendIntoTrivia = false) { throw null; }
        protected virtual bool EquivalentToCore(Microsoft.CodeAnalysis.SyntaxNode other) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode FindNode(Microsoft.CodeAnalysis.Text.TextSpan span, bool findInsideTrivia = false, bool getInnermostNodeForTie = false) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken FindToken(int position, bool findInsideTrivia = false) { throw null; }
        protected virtual Microsoft.CodeAnalysis.SyntaxToken FindTokenCore(int position, bool findInsideTrivia) { throw null; }
        protected virtual Microsoft.CodeAnalysis.SyntaxToken FindTokenCore(int position, System.Func<Microsoft.CodeAnalysis.SyntaxTrivia, bool> stepInto) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia FindTrivia(int position, bool findInsideTrivia = false) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia FindTrivia(int position, System.Func<Microsoft.CodeAnalysis.SyntaxTrivia, bool>? stepInto) { throw null; }
        protected virtual Microsoft.CodeAnalysis.SyntaxTrivia FindTriviaCore(int position, bool findInsideTrivia) { throw null; }
        public TNode? FirstAncestorOrSelf<TNode>(System.Func<TNode, bool>? predicate = null, bool ascendOutOfTrivia = true) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public TNode? FirstAncestorOrSelf<TNode, TArg>(System.Func<TNode, TArg, bool> predicate, TArg argument, bool ascendOutOfTrivia = true) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> GetAnnotatedNodes(Microsoft.CodeAnalysis.SyntaxAnnotation syntaxAnnotation) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> GetAnnotatedNodes(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> GetAnnotatedNodesAndTokens(Microsoft.CodeAnalysis.SyntaxAnnotation annotation) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> GetAnnotatedNodesAndTokens(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> GetAnnotatedNodesAndTokens(params string[] annotationKinds) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> GetAnnotatedTokens(Microsoft.CodeAnalysis.SyntaxAnnotation syntaxAnnotation) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> GetAnnotatedTokens(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> GetAnnotatedTrivia(Microsoft.CodeAnalysis.SyntaxAnnotation annotation) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> GetAnnotatedTrivia(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> GetAnnotatedTrivia(params string[] annotationKinds) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(System.Collections.Generic.IEnumerable<string> annotationKinds) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken GetFirstToken(bool includeZeroWidth = false, bool includeSkipped = false, bool includeDirectives = false, bool includeDocumentationComments = false) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken GetLastToken(bool includeZeroWidth = false, bool includeSkipped = false, bool includeDirectives = false, bool includeDocumentationComments = false) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList GetLeadingTrivia() { throw null; }
        public Microsoft.CodeAnalysis.Location GetLocation() { throw null; }
        protected T? GetRedAtZero<T>(ref T? field) where T : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        protected T? GetRed<T>(ref T? field, int slot) where T : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public Microsoft.CodeAnalysis.SyntaxReference GetReference() { throw null; }
        public Microsoft.CodeAnalysis.Text.SourceText GetText(System.Text.Encoding? encoding = null, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList GetTrailingTrivia() { throw null; }
        public bool HasAnnotation(Microsoft.CodeAnalysis.SyntaxAnnotation? annotation) { throw null; }
        public bool HasAnnotations(System.Collections.Generic.IEnumerable<string> annotationKinds) { throw null; }
        public bool HasAnnotations(string annotationKind) { throw null; }
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode InsertNodesInListCore(Microsoft.CodeAnalysis.SyntaxNode nodeInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> nodesToInsert, bool insertBefore);
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode InsertTokensInListCore(Microsoft.CodeAnalysis.SyntaxToken originalToken, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> newTokens, bool insertBefore);
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode InsertTriviaInListCore(Microsoft.CodeAnalysis.SyntaxTrivia originalTrivia, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> newTrivia, bool insertBefore);
        public bool IsEquivalentTo(Microsoft.CodeAnalysis.SyntaxNode? other) { throw null; }
        public bool IsEquivalentTo(Microsoft.CodeAnalysis.SyntaxNode node, bool topLevel = false) { throw null; }
        protected abstract bool IsEquivalentToCore(Microsoft.CodeAnalysis.SyntaxNode node, bool topLevel = false);
        public bool IsIncrementallyIdenticalTo(Microsoft.CodeAnalysis.SyntaxNode? other) { throw null; }
        public bool IsPartOfStructuredTrivia() { throw null; }
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode NormalizeWhitespaceCore(string indentation, string eol, bool elasticTrivia);
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode? RemoveNodesCore(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> nodes, Microsoft.CodeAnalysis.SyntaxRemoveOptions options);
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode ReplaceCore<TNode>(System.Collections.Generic.IEnumerable<TNode>? nodes = null, System.Func<TNode, TNode, Microsoft.CodeAnalysis.SyntaxNode>? computeReplacementNode = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken>? tokens = null, System.Func<Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken>? computeReplacementToken = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>? trivia = null, System.Func<Microsoft.CodeAnalysis.SyntaxTrivia, Microsoft.CodeAnalysis.SyntaxTrivia, Microsoft.CodeAnalysis.SyntaxTrivia>? computeReplacementTrivia = null) where TNode : Microsoft.CodeAnalysis.SyntaxNode;
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode ReplaceNodeInListCore(Microsoft.CodeAnalysis.SyntaxNode originalNode, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> replacementNodes);
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode ReplaceTokenInListCore(Microsoft.CodeAnalysis.SyntaxToken originalToken, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> newTokens);
        protected internal abstract Microsoft.CodeAnalysis.SyntaxNode ReplaceTriviaInListCore(Microsoft.CodeAnalysis.SyntaxTrivia originalTrivia, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> newTrivia);
        public virtual void SerializeTo(System.IO.Stream stream, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { }
        public virtual string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public virtual void WriteTo(System.IO.TextWriter writer) { }
    }
    public static partial class SyntaxNodeExtensions
    {
        public static System.Collections.Generic.IEnumerable<TNode> GetCurrentNodes<TNode>(this Microsoft.CodeAnalysis.SyntaxNode root, System.Collections.Generic.IEnumerable<TNode> nodes) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static System.Collections.Generic.IEnumerable<TNode> GetCurrentNodes<TNode>(this Microsoft.CodeAnalysis.SyntaxNode root, TNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode? GetCurrentNode<TNode>(this Microsoft.CodeAnalysis.SyntaxNode root, TNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot InsertNodesAfter<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxNode nodeInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newNodes) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot InsertNodesBefore<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxNode nodeInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newNodes) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot InsertTokensAfter<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxToken tokenInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> newTokens) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot InsertTokensBefore<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxToken tokenInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> newTokens) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot InsertTriviaAfter<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxTrivia trivia, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> newTrivia) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot InsertTriviaBefore<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxTrivia trivia, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> newTrivia) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode NormalizeWhitespace<TNode>(this TNode node, string indentation, bool elasticTrivia) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode NormalizeWhitespace<TNode>(this TNode node, string indentation = "    ", string eol = "\r\n", bool elasticTrivia = false) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot? RemoveNodes<TRoot>(this TRoot root, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> nodes, Microsoft.CodeAnalysis.SyntaxRemoveOptions options) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot? RemoveNode<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxRemoveOptions options) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceNodes<TRoot, TNode>(this TRoot root, System.Collections.Generic.IEnumerable<TNode> nodes, System.Func<TNode, TNode, Microsoft.CodeAnalysis.SyntaxNode> computeReplacementNode) where TRoot : Microsoft.CodeAnalysis.SyntaxNode where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceNode<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxNode oldNode, Microsoft.CodeAnalysis.SyntaxNode newNode) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceNode<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxNode oldNode, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newNodes) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceSyntax<TRoot>(this TRoot root, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> nodes, System.Func<Microsoft.CodeAnalysis.SyntaxNode, Microsoft.CodeAnalysis.SyntaxNode, Microsoft.CodeAnalysis.SyntaxNode> computeReplacementNode, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> tokens, System.Func<Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken> computeReplacementToken, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> trivia, System.Func<Microsoft.CodeAnalysis.SyntaxTrivia, Microsoft.CodeAnalysis.SyntaxTrivia, Microsoft.CodeAnalysis.SyntaxTrivia> computeReplacementTrivia) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceTokens<TRoot>(this TRoot root, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> tokens, System.Func<Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken> computeReplacementToken) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceToken<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxToken oldToken, Microsoft.CodeAnalysis.SyntaxToken newToken) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceToken<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxToken tokenInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> newTokens) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceTrivia<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxTrivia trivia, Microsoft.CodeAnalysis.SyntaxTrivia newTrivia) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceTrivia<TRoot>(this TRoot root, Microsoft.CodeAnalysis.SyntaxTrivia oldTrivia, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> newTrivia) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot ReplaceTrivia<TRoot>(this TRoot root, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> trivia, System.Func<Microsoft.CodeAnalysis.SyntaxTrivia, Microsoft.CodeAnalysis.SyntaxTrivia, Microsoft.CodeAnalysis.SyntaxTrivia> computeReplacementTrivia) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot TrackNodes<TRoot>(this TRoot root, params Microsoft.CodeAnalysis.SyntaxNode[] nodes) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TRoot TrackNodes<TRoot>(this TRoot root, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> nodes) where TRoot : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithLeadingTrivia<TSyntax>(this TSyntax node, Microsoft.CodeAnalysis.SyntaxTriviaList trivia) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithLeadingTrivia<TSyntax>(this TSyntax node, params Microsoft.CodeAnalysis.SyntaxTrivia[]? trivia) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithLeadingTrivia<TSyntax>(this TSyntax node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>? trivia) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithoutLeadingTrivia<TSyntax>(this TSyntax node) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithoutTrailingTrivia<TSyntax>(this TSyntax node) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static Microsoft.CodeAnalysis.SyntaxToken WithoutTrivia(this Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public static TSyntax WithoutTrivia<TSyntax>(this TSyntax syntax) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithTrailingTrivia<TSyntax>(this TSyntax node, Microsoft.CodeAnalysis.SyntaxTriviaList trivia) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithTrailingTrivia<TSyntax>(this TSyntax node, params Microsoft.CodeAnalysis.SyntaxTrivia[]? trivia) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithTrailingTrivia<TSyntax>(this TSyntax node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>? trivia) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TSyntax WithTriviaFrom<TSyntax>(this TSyntax syntax, Microsoft.CodeAnalysis.SyntaxNode node) where TSyntax : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
    }
    public readonly partial struct SyntaxNodeOrToken : System.IEquatable<Microsoft.CodeAnalysis.SyntaxNodeOrToken>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public bool ContainsAnnotations { get { throw null; } }
        public bool ContainsDiagnostics { get { throw null; } }
        public bool ContainsDirectives { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public bool HasLeadingTrivia { get { throw null; } }
        public bool HasTrailingTrivia { get { throw null; } }
        public bool IsMissing { get { throw null; } }
        public bool IsNode { get { throw null; } }
        public bool IsToken { get { throw null; } }
        public string Language { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode? Parent { get { throw null; } }
        public int RawKind { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public int SpanStart { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTree? SyntaxTree { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode? AsNode() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken AsToken() { throw null; }
        public Microsoft.CodeAnalysis.ChildSyntaxList ChildNodesAndTokens() { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxNodeOrToken other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(System.Collections.Generic.IEnumerable<string> annotationKinds) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics() { throw null; }
        public static int GetFirstChildIndexSpanningPosition(Microsoft.CodeAnalysis.SyntaxNode node, int position) { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList GetLeadingTrivia() { throw null; }
        public Microsoft.CodeAnalysis.Location? GetLocation() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken GetNextSibling() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken GetPreviousSibling() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList GetTrailingTrivia() { throw null; }
        public bool HasAnnotation(Microsoft.CodeAnalysis.SyntaxAnnotation? annotation) { throw null; }
        public bool HasAnnotations(System.Collections.Generic.IEnumerable<string> annotationKinds) { throw null; }
        public bool HasAnnotations(string annotationKind) { throw null; }
        public bool IsEquivalentTo(Microsoft.CodeAnalysis.SyntaxNodeOrToken other) { throw null; }
        public bool IsIncrementallyIdenticalTo(Microsoft.CodeAnalysis.SyntaxNodeOrToken other) { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxNodeOrToken left, Microsoft.CodeAnalysis.SyntaxNodeOrToken right) { throw null; }
        public static explicit operator Microsoft.CodeAnalysis.SyntaxNode? (Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrToken) { throw null; }
        public static explicit operator Microsoft.CodeAnalysis.SyntaxToken (Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrToken) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.SyntaxNodeOrToken (Microsoft.CodeAnalysis.SyntaxNode? node) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.SyntaxNodeOrToken (Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxNodeOrToken left, Microsoft.CodeAnalysis.SyntaxNodeOrToken right) { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithAdditionalAnnotations(params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithAdditionalAnnotations(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithLeadingTrivia(params Microsoft.CodeAnalysis.SyntaxTrivia[] trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithLeadingTrivia(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithoutAnnotations(params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithoutAnnotations(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithoutAnnotations(string annotationKind) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithTrailingTrivia(params Microsoft.CodeAnalysis.SyntaxTrivia[] trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken WithTrailingTrivia(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> trivia) { throw null; }
        public void WriteTo(System.IO.TextWriter writer) { }
    }
    public readonly partial struct SyntaxNodeOrTokenList : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken>, System.Collections.Generic.IReadOnlyCollection<Microsoft.CodeAnalysis.SyntaxNodeOrToken>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.SyntaxNodeOrTokenList>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SyntaxNodeOrTokenList(params Microsoft.CodeAnalysis.SyntaxNodeOrToken[] nodesAndTokens) { throw null; }
        public SyntaxNodeOrTokenList(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> nodesAndTokens) { throw null; }
        public int Count { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken this[int index] { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList Add(Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrToken) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList AddRange(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> nodesOrTokens) { throw null; }
        public bool Any() { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxNodeOrTokenList other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken First() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken FirstOrDefault() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList.Enumerator GetEnumerator() { throw null; }
        public override int GetHashCode() { throw null; }
        public int IndexOf(Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrToken) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList Insert(int index, Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrToken) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList InsertRange(int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> nodesAndTokens) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken Last() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrToken LastOrDefault() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxNodeOrTokenList left, Microsoft.CodeAnalysis.SyntaxNodeOrTokenList right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxNodeOrTokenList left, Microsoft.CodeAnalysis.SyntaxNodeOrTokenList right) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList Remove(Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrTokenInList) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList RemoveAt(int index) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList Replace(Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrTokenInList, Microsoft.CodeAnalysis.SyntaxNodeOrToken newNodeOrToken) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNodeOrTokenList ReplaceRange(Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrTokenInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken> newNodesAndTokens) { throw null; }
        System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxNodeOrToken> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNodeOrToken>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxNodeOrToken>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Microsoft.CodeAnalysis.SyntaxNodeOrToken Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public override bool Equals(object? obj) { throw null; }
            public override int GetHashCode() { throw null; }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public delegate Microsoft.CodeAnalysis.ISyntaxReceiver SyntaxReceiverCreator();
    public abstract partial class SyntaxReference
    {
        protected SyntaxReference() { }
        public abstract Microsoft.CodeAnalysis.Text.TextSpan Span { get; }
        public abstract Microsoft.CodeAnalysis.SyntaxTree SyntaxTree { get; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode GetSyntax(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public virtual System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SyntaxNode> GetSyntaxAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    [System.FlagsAttribute]
    public enum SyntaxRemoveOptions
    {
        KeepNoTrivia = 0,
        KeepLeadingTrivia = 1,
        KeepTrailingTrivia = 2,
        KeepExteriorTrivia = 3,
        KeepUnbalancedDirectives = 4,
        KeepDirectives = 8,
        KeepEndOfLine = 16,
        AddElasticMarker = 32,
    }
    public readonly partial struct SyntaxToken : System.IEquatable<Microsoft.CodeAnalysis.SyntaxToken>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public bool ContainsAnnotations { get { throw null; } }
        public bool ContainsDiagnostics { get { throw null; } }
        public bool ContainsDirectives { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public bool HasLeadingTrivia { get { throw null; } }
        public bool HasStructuredTrivia { get { throw null; } }
        public bool HasTrailingTrivia { get { throw null; } }
        public bool IsMissing { get { throw null; } }
        public string Language { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTriviaList LeadingTrivia { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode? Parent { get { throw null; } }
        public int RawKind { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public int SpanStart { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTree? SyntaxTree { get { throw null; } }
        public string Text { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTriviaList TrailingTrivia { get { throw null; } }
        public object? Value { get { throw null; } }
        public string ValueText { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxToken CopyAnnotationsTo(Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxToken other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> GetAllTrivia() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(System.Collections.Generic.IEnumerable<string> annotationKinds) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(params string[] annotationKinds) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics() { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.Location GetLocation() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken GetNextToken(bool includeZeroWidth = false, bool includeSkipped = false, bool includeDirectives = false, bool includeDocumentationComments = false) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken GetPreviousToken(bool includeZeroWidth = false, bool includeSkipped = false, bool includeDirectives = false, bool includeDocumentationComments = false) { throw null; }
        public bool HasAnnotation(Microsoft.CodeAnalysis.SyntaxAnnotation? annotation) { throw null; }
        public bool HasAnnotations(string annotationKind) { throw null; }
        public bool HasAnnotations(params string[] annotationKinds) { throw null; }
        public bool IsEquivalentTo(Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public bool IsIncrementallyIdenticalTo(Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public bool IsPartOfStructuredTrivia() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxToken left, Microsoft.CodeAnalysis.SyntaxToken right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxToken left, Microsoft.CodeAnalysis.SyntaxToken right) { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithAdditionalAnnotations(params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithAdditionalAnnotations(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithLeadingTrivia(Microsoft.CodeAnalysis.SyntaxTriviaList trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithLeadingTrivia(params Microsoft.CodeAnalysis.SyntaxTrivia[]? trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithLeadingTrivia(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>? trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithoutAnnotations(params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithoutAnnotations(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithoutAnnotations(string annotationKind) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithTrailingTrivia(Microsoft.CodeAnalysis.SyntaxTriviaList trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithTrailingTrivia(params Microsoft.CodeAnalysis.SyntaxTrivia[]? trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithTrailingTrivia(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>? trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken WithTriviaFrom(Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public void WriteTo(System.IO.TextWriter writer) { }
    }
    public readonly partial struct SyntaxTokenList : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken>, System.Collections.Generic.IReadOnlyCollection<Microsoft.CodeAnalysis.SyntaxToken>, System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxToken>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.SyntaxTokenList>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SyntaxTokenList(Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public SyntaxTokenList(params Microsoft.CodeAnalysis.SyntaxToken[] tokens) { throw null; }
        public SyntaxTokenList(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> tokens) { throw null; }
        public int Count { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxToken this[int index] { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTokenList Add(Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList AddRange(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> tokens) { throw null; }
        public bool Any() { throw null; }
        public static Microsoft.CodeAnalysis.SyntaxTokenList Create(Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxTokenList other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken First() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList.Enumerator GetEnumerator() { throw null; }
        public override int GetHashCode() { throw null; }
        public int IndexOf(Microsoft.CodeAnalysis.SyntaxToken tokenInList) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList Insert(int index, Microsoft.CodeAnalysis.SyntaxToken token) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList InsertRange(int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> tokens) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxToken Last() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxTokenList left, Microsoft.CodeAnalysis.SyntaxTokenList right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxTokenList left, Microsoft.CodeAnalysis.SyntaxTokenList right) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList Remove(Microsoft.CodeAnalysis.SyntaxToken tokenInList) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList RemoveAt(int index) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList Replace(Microsoft.CodeAnalysis.SyntaxToken tokenInList, Microsoft.CodeAnalysis.SyntaxToken newToken) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList ReplaceRange(Microsoft.CodeAnalysis.SyntaxToken tokenInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> newTokens) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTokenList.Reversed Reverse() { throw null; }
        System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxToken> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Microsoft.CodeAnalysis.SyntaxToken Current { get { throw null; } }
            public override bool Equals(object? obj) { throw null; }
            public override int GetHashCode() { throw null; }
            public bool MoveNext() { throw null; }
        }
        public readonly partial struct Reversed : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.SyntaxTokenList.Reversed>
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public Reversed(Microsoft.CodeAnalysis.SyntaxTokenList list) { throw null; }
            public bool Equals(Microsoft.CodeAnalysis.SyntaxTokenList.Reversed other) { throw null; }
            public override bool Equals(object? obj) { throw null; }
            public Microsoft.CodeAnalysis.SyntaxTokenList.Reversed.Enumerator GetEnumerator() { throw null; }
            public override int GetHashCode() { throw null; }
            System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxToken> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken>.GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            public partial struct Enumerator
            {
                private object _dummy;
                private int _dummyPrimitive;
                public Microsoft.CodeAnalysis.SyntaxToken Current { get { throw null; } }
                public override bool Equals(object? obj) { throw null; }
                public override int GetHashCode() { throw null; }
                public bool MoveNext() { throw null; }
            }
        }
    }
    public abstract partial class SyntaxTree
    {
        protected internal static readonly System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> EmptyDiagnosticOptions;
        protected SyntaxTree() { }
        [System.ObsoleteAttribute("Obsolete due to performance problems, use CompilationOptions.SyntaxTreeOptionsProvider instead", false)]
        public virtual System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> DiagnosticOptions { get { throw null; } }
        public abstract System.Text.Encoding? Encoding { get; }
        public abstract string FilePath { get; }
        public abstract bool HasCompilationUnitRoot { get; }
        public abstract int Length { get; }
        public Microsoft.CodeAnalysis.ParseOptions Options { get { throw null; } }
        protected abstract Microsoft.CodeAnalysis.ParseOptions OptionsCore { get; }
        public abstract System.Collections.Generic.IList<Microsoft.CodeAnalysis.Text.TextSpan> GetChangedSpans(Microsoft.CodeAnalysis.SyntaxTree syntaxTree);
        public abstract System.Collections.Generic.IList<Microsoft.CodeAnalysis.Text.TextChange> GetChanges(Microsoft.CodeAnalysis.SyntaxTree oldTree);
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics(Microsoft.CodeAnalysis.SyntaxNode node);
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics(Microsoft.CodeAnalysis.SyntaxNodeOrToken nodeOrToken);
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics(Microsoft.CodeAnalysis.SyntaxToken token);
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics(Microsoft.CodeAnalysis.SyntaxTrivia trivia);
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.LineMapping> GetLineMappings(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract Microsoft.CodeAnalysis.FileLinePositionSpan GetLineSpan(Microsoft.CodeAnalysis.Text.TextSpan span, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public virtual Microsoft.CodeAnalysis.LineVisibility GetLineVisibility(int position, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public abstract Microsoft.CodeAnalysis.Location GetLocation(Microsoft.CodeAnalysis.Text.TextSpan span);
        public abstract Microsoft.CodeAnalysis.FileLinePositionSpan GetMappedLineSpan(Microsoft.CodeAnalysis.Text.TextSpan span, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public abstract Microsoft.CodeAnalysis.SyntaxReference GetReference(Microsoft.CodeAnalysis.SyntaxNode node);
        public Microsoft.CodeAnalysis.SyntaxNode GetRoot(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SyntaxNode> GetRootAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        protected abstract System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SyntaxNode> GetRootAsyncCore(System.Threading.CancellationToken cancellationToken);
        protected abstract Microsoft.CodeAnalysis.SyntaxNode GetRootCore(System.Threading.CancellationToken cancellationToken);
        public abstract Microsoft.CodeAnalysis.Text.SourceText GetText(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        public virtual System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Text.SourceText> GetTextAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public abstract bool HasHiddenRegions();
        public abstract bool IsEquivalentTo(Microsoft.CodeAnalysis.SyntaxTree tree, bool topLevel = false);
        public override string ToString() { throw null; }
        public bool TryGetRoot(out Microsoft.CodeAnalysis.SyntaxNode? root) { throw null; }
        protected abstract bool TryGetRootCore(out Microsoft.CodeAnalysis.SyntaxNode? root);
        public abstract bool TryGetText(out Microsoft.CodeAnalysis.Text.SourceText? text);
        public abstract Microsoft.CodeAnalysis.SyntaxTree WithChangedText(Microsoft.CodeAnalysis.Text.SourceText newText);
        [System.ObsoleteAttribute("Obsolete due to performance problems, use CompilationOptions.SyntaxTreeOptionsProvider instead", false)]
        public virtual Microsoft.CodeAnalysis.SyntaxTree WithDiagnosticOptions(System.Collections.Immutable.ImmutableDictionary<string, Microsoft.CodeAnalysis.ReportDiagnostic> options) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxTree WithFilePath(string path);
        public abstract Microsoft.CodeAnalysis.SyntaxTree WithRootAndOptions(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.ParseOptions options);
    }
    public abstract partial class SyntaxTreeOptionsProvider
    {
        protected SyntaxTreeOptionsProvider() { }
        public abstract Microsoft.CodeAnalysis.GeneratedKind IsGenerated(Microsoft.CodeAnalysis.SyntaxTree tree, System.Threading.CancellationToken cancellationToken);
        public abstract bool TryGetDiagnosticValue(Microsoft.CodeAnalysis.SyntaxTree tree, string diagnosticId, System.Threading.CancellationToken cancellationToken, out Microsoft.CodeAnalysis.ReportDiagnostic severity);
        public abstract bool TryGetGlobalDiagnosticValue(string diagnosticId, System.Threading.CancellationToken cancellationToken, out Microsoft.CodeAnalysis.ReportDiagnostic severity);
    }
    public readonly partial struct SyntaxTrivia : System.IEquatable<Microsoft.CodeAnalysis.SyntaxTrivia>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public bool ContainsDiagnostics { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public bool HasStructure { get { throw null; } }
        public bool IsDirective { get { throw null; } }
        public string Language { get { throw null; } }
        public int RawKind { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public int SpanStart { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTree? SyntaxTree { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxToken Token { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTrivia CopyAnnotationsTo(Microsoft.CodeAnalysis.SyntaxTrivia trivia) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxTrivia other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(string annotationKind) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> GetAnnotations(params string[] annotationKinds) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetDiagnostics() { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.Location GetLocation() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode? GetStructure() { throw null; }
        public bool HasAnnotation(Microsoft.CodeAnalysis.SyntaxAnnotation? annotation) { throw null; }
        public bool HasAnnotations(string annotationKind) { throw null; }
        public bool HasAnnotations(params string[] annotationKinds) { throw null; }
        public bool IsEquivalentTo(Microsoft.CodeAnalysis.SyntaxTrivia trivia) { throw null; }
        public bool IsPartOfStructuredTrivia() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxTrivia left, Microsoft.CodeAnalysis.SyntaxTrivia right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxTrivia left, Microsoft.CodeAnalysis.SyntaxTrivia right) { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia WithAdditionalAnnotations(params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia WithAdditionalAnnotations(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia WithoutAnnotations(params Microsoft.CodeAnalysis.SyntaxAnnotation[] annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia WithoutAnnotations(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxAnnotation> annotations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia WithoutAnnotations(string annotationKind) { throw null; }
        public void WriteTo(System.IO.TextWriter writer) { }
    }
    public readonly partial struct SyntaxTriviaList : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>, System.Collections.Generic.IReadOnlyCollection<Microsoft.CodeAnalysis.SyntaxTrivia>, System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxTrivia>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.SyntaxTriviaList>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SyntaxTriviaList(Microsoft.CodeAnalysis.SyntaxTrivia trivia) { throw null; }
        public SyntaxTriviaList(params Microsoft.CodeAnalysis.SyntaxTrivia[] trivias) { throw null; }
        public SyntaxTriviaList(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>? trivias) { throw null; }
        public int Count { get { throw null; } }
        public static Microsoft.CodeAnalysis.SyntaxTriviaList Empty { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan FullSpan { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTrivia this[int index] { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTriviaList Add(Microsoft.CodeAnalysis.SyntaxTrivia trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList AddRange(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> trivia) { throw null; }
        public bool Any() { throw null; }
        public static Microsoft.CodeAnalysis.SyntaxTriviaList Create(Microsoft.CodeAnalysis.SyntaxTrivia trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia ElementAt(int index) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SyntaxTriviaList other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia First() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList.Enumerator GetEnumerator() { throw null; }
        public override int GetHashCode() { throw null; }
        public int IndexOf(Microsoft.CodeAnalysis.SyntaxTrivia triviaInList) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList Insert(int index, Microsoft.CodeAnalysis.SyntaxTrivia trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList InsertRange(int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> trivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTrivia Last() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SyntaxTriviaList left, Microsoft.CodeAnalysis.SyntaxTriviaList right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SyntaxTriviaList left, Microsoft.CodeAnalysis.SyntaxTriviaList right) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList Remove(Microsoft.CodeAnalysis.SyntaxTrivia triviaInList) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList RemoveAt(int index) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList Replace(Microsoft.CodeAnalysis.SyntaxTrivia triviaInList, Microsoft.CodeAnalysis.SyntaxTrivia newTrivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList ReplaceRange(Microsoft.CodeAnalysis.SyntaxTrivia triviaInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia> newTrivia) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxTriviaList.Reversed Reverse() { throw null; }
        System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxTrivia> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public string ToFullString() { throw null; }
        public override string ToString() { throw null; }
        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Microsoft.CodeAnalysis.SyntaxTrivia Current { get { throw null; } }
            public bool MoveNext() { throw null; }
        }
        public readonly partial struct Reversed : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>, System.Collections.IEnumerable, System.IEquatable<Microsoft.CodeAnalysis.SyntaxTriviaList.Reversed>
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public Reversed(Microsoft.CodeAnalysis.SyntaxTriviaList list) { throw null; }
            public bool Equals(Microsoft.CodeAnalysis.SyntaxTriviaList.Reversed other) { throw null; }
            public override bool Equals(object? obj) { throw null; }
            public Microsoft.CodeAnalysis.SyntaxTriviaList.Reversed.Enumerator GetEnumerator() { throw null; }
            public override int GetHashCode() { throw null; }
            System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.SyntaxTrivia> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxTrivia>.GetEnumerator() { throw null; }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
            public partial struct Enumerator
            {
                private object _dummy;
                private int _dummyPrimitive;
                public Microsoft.CodeAnalysis.SyntaxTrivia Current { get { throw null; } }
                public bool MoveNext() { throw null; }
            }
        }
    }
    public readonly partial struct SyntaxValueProvider
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Microsoft.CodeAnalysis.IncrementalValuesProvider<T> CreateSyntaxProvider<T>(System.Func<Microsoft.CodeAnalysis.SyntaxNode, System.Threading.CancellationToken, bool> predicate, System.Func<Microsoft.CodeAnalysis.GeneratorSyntaxContext, System.Threading.CancellationToken, T> transform) { throw null; }
    }
    public abstract partial class SyntaxWalker
    {
        protected SyntaxWalker(Microsoft.CodeAnalysis.SyntaxWalkerDepth depth = Microsoft.CodeAnalysis.SyntaxWalkerDepth.Node) { }
        protected Microsoft.CodeAnalysis.SyntaxWalkerDepth Depth { get { throw null; } }
        public virtual void Visit(Microsoft.CodeAnalysis.SyntaxNode node) { }
        protected virtual void VisitToken(Microsoft.CodeAnalysis.SyntaxToken token) { }
        protected virtual void VisitTrivia(Microsoft.CodeAnalysis.SyntaxTrivia trivia) { }
    }
    public enum SyntaxWalkerDepth
    {
        Node = 0,
        Token = 1,
        Trivia = 2,
        StructuredTrivia = 3,
    }
    public partial struct TypedConstant : System.IEquatable<Microsoft.CodeAnalysis.TypedConstant>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public bool IsNull { get { throw null; } }
        public Microsoft.CodeAnalysis.TypedConstantKind Kind { get { throw null; } }
        public Microsoft.CodeAnalysis.ITypeSymbol? Type { get { throw null; } }
        public object? Value { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.TypedConstant> Values { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.TypedConstant other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum TypedConstantKind
    {
        Error = 0,
        Primitive = 1,
        Enum = 2,
        Type = 3,
        Array = 4,
    }
    public readonly partial struct TypeInfo : System.IEquatable<Microsoft.CodeAnalysis.TypeInfo>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Microsoft.CodeAnalysis.NullabilityInfo ConvertedNullability { get { throw null; } }
        public Microsoft.CodeAnalysis.ITypeSymbol? ConvertedType { get { throw null; } }
        public Microsoft.CodeAnalysis.NullabilityInfo Nullability { get { throw null; } }
        public Microsoft.CodeAnalysis.ITypeSymbol? Type { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.TypeInfo other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum TypeKind : byte
    {
        Unknown = (byte)0,
        Array = (byte)1,
        Class = (byte)2,
        Delegate = (byte)3,
        Dynamic = (byte)4,
        Enum = (byte)5,
        Error = (byte)6,
        Interface = (byte)7,
        Module = (byte)8,
        Pointer = (byte)9,
        Struct = (byte)10,
        Structure = (byte)10,
        TypeParameter = (byte)11,
        Submission = (byte)12,
        FunctionPointer = (byte)13,
    }
    public enum TypeParameterKind
    {
        Type = 0,
        Method = 1,
        Cref = 2,
    }
    public sealed partial class UnresolvedMetadataReference : Microsoft.CodeAnalysis.MetadataReference
    {
        internal UnresolvedMetadataReference() : base (default(Microsoft.CodeAnalysis.MetadataReferenceProperties)) { }
        public override string Display { get { throw null; } }
        public string Reference { get { throw null; } }
    }
    public enum VarianceKind : short
    {
        None = (short)0,
        Out = (short)1,
        In = (short)2,
    }
    public static partial class WellKnownDiagnosticTags
    {
        public const string AnalyzerException = "AnalyzerException";
        public const string Build = "Build";
        public const string CompilationEnd = "CompilationEnd";
        public const string Compiler = "Compiler";
        public const string CustomObsolete = "CustomObsolete";
        public const string EditAndContinue = "EditAndContinue";
        public const string NotConfigurable = "NotConfigurable";
        public const string Telemetry = "Telemetry";
        public const string Unnecessary = "Unnecessary";
    }
    public static partial class WellKnownMemberNames
    {
        public const string AdditionOperatorName = "op_Addition";
        public const string BitwiseAndOperatorName = "op_BitwiseAnd";
        public const string BitwiseOrOperatorName = "op_BitwiseOr";
        public const string CollectionInitializerAddMethodName = "Add";
        public const string ConcatenateOperatorName = "op_Concatenate";
        public const string CountPropertyName = "Count";
        public const string CurrentPropertyName = "Current";
        public const string DeconstructMethodName = "Deconstruct";
        public const string DecrementOperatorName = "op_Decrement";
        public const string DefaultScriptClassName = "Script";
        public const string DelegateBeginInvokeName = "BeginInvoke";
        public const string DelegateEndInvokeName = "EndInvoke";
        public const string DelegateInvokeName = "Invoke";
        public const string DestructorName = "Finalize";
        public const string DisposeAsyncMethodName = "DisposeAsync";
        public const string DisposeMethodName = "Dispose";
        public const string DivisionOperatorName = "op_Division";
        public const string EntryPointMethodName = "Main";
        public const string EnumBackingFieldName = "value__";
        public const string EqualityOperatorName = "op_Equality";
        public const string ExclusiveOrOperatorName = "op_ExclusiveOr";
        public const string ExplicitConversionName = "op_Explicit";
        public const string ExponentOperatorName = "op_Exponent";
        public const string FalseOperatorName = "op_False";
        public const string GetAsyncEnumeratorMethodName = "GetAsyncEnumerator";
        public const string GetAwaiter = "GetAwaiter";
        public const string GetEnumeratorMethodName = "GetEnumerator";
        public const string GetResult = "GetResult";
        public const string GreaterThanOperatorName = "op_GreaterThan";
        public const string GreaterThanOrEqualOperatorName = "op_GreaterThanOrEqual";
        public const string ImplicitConversionName = "op_Implicit";
        public const string IncrementOperatorName = "op_Increment";
        public const string Indexer = "this[]";
        public const string InequalityOperatorName = "op_Inequality";
        public const string InstanceConstructorName = ".ctor";
        public const string IntegerDivisionOperatorName = "op_IntegerDivision";
        public const string IsCompleted = "IsCompleted";
        public const string LeftShiftOperatorName = "op_LeftShift";
        public const string LengthPropertyName = "Length";
        public const string LessThanOperatorName = "op_LessThan";
        public const string LessThanOrEqualOperatorName = "op_LessThanOrEqual";
        public const string LikeOperatorName = "op_Like";
        public const string LogicalAndOperatorName = "op_LogicalAnd";
        public const string LogicalNotOperatorName = "op_LogicalNot";
        public const string LogicalOrOperatorName = "op_LogicalOr";
        public const string ModulusOperatorName = "op_Modulus";
        public const string MoveNextAsyncMethodName = "MoveNextAsync";
        public const string MoveNextMethodName = "MoveNext";
        public const string MultiplyOperatorName = "op_Multiply";
        public const string ObjectEquals = "Equals";
        public const string ObjectGetHashCode = "GetHashCode";
        public const string ObjectToString = "ToString";
        public const string OnCompleted = "OnCompleted";
        public const string OnesComplementOperatorName = "op_OnesComplement";
        public const string PrintMembersMethodName = "PrintMembers";
        public const string RightShiftOperatorName = "op_RightShift";
        public const string SliceMethodName = "Slice";
        public const string StaticConstructorName = ".cctor";
        public const string SubtractionOperatorName = "op_Subtraction";
        public const string TopLevelStatementsEntryPointMethodName = "<Main>$";
        public const string TopLevelStatementsEntryPointTypeName = "Program";
        public const string TrueOperatorName = "op_True";
        public const string UnaryNegationOperatorName = "op_UnaryNegation";
        public const string UnaryPlusOperatorName = "op_UnaryPlus";
        public const string UnsignedLeftShiftOperatorName = "op_UnsignedLeftShift";
        public const string UnsignedRightShiftOperatorName = "op_UnsignedRightShift";
        public const string ValuePropertyName = "Value";
    }
    public partial class XmlFileResolver : Microsoft.CodeAnalysis.XmlReferenceResolver
    {
        public XmlFileResolver(string? baseDirectory) { }
        public string? BaseDirectory { get { throw null; } }
        public static Microsoft.CodeAnalysis.XmlFileResolver Default { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        protected virtual bool FileExists(string? resolvedPath) { throw null; }
        public override int GetHashCode() { throw null; }
        public override System.IO.Stream OpenRead(string resolvedPath) { throw null; }
        public override string? ResolveReference(string path, string? baseFilePath) { throw null; }
    }
    public abstract partial class XmlReferenceResolver
    {
        protected XmlReferenceResolver() { }
        public abstract override bool Equals(object? other);
        public abstract override int GetHashCode();
        public abstract System.IO.Stream OpenRead(string resolvedPath);
        public abstract string? ResolveReference(string path, string? baseFilePath);
    }
}
namespace Microsoft.CodeAnalysis.Diagnostics
{
    public readonly partial struct AdditionalFileAnalysisContext
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Microsoft.CodeAnalysis.AdditionalText AdditionalFile { get { throw null; } }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public abstract partial class AnalysisContext
    {
        protected AnalysisContext() { }
        public virtual void ConfigureGeneratedCodeAnalysis(Microsoft.CodeAnalysis.Diagnostics.GeneratedCodeAnalysisFlags analysisMode) { }
        public virtual void EnableConcurrentExecution() { }
        public virtual void RegisterAdditionalFileAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.AdditionalFileAnalysisContext> action) { }
        public abstract void RegisterCodeBlockAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.CodeBlockAnalysisContext> action);
        public abstract void RegisterCodeBlockStartAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext<TLanguageKindEnum>> action) where TLanguageKindEnum : struct;
        public abstract void RegisterCompilationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.CompilationAnalysisContext> action);
        public abstract void RegisterCompilationStartAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext> action);
        public void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, params Microsoft.CodeAnalysis.OperationKind[] operationKinds) { }
        public virtual void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.OperationKind> operationKinds) { }
        public virtual void RegisterOperationBlockAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationBlockAnalysisContext> action) { }
        public virtual void RegisterOperationBlockStartAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationBlockStartAnalysisContext> action) { }
        public abstract void RegisterSemanticModelAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext> action);
        public void RegisterSymbolAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext> action, params Microsoft.CodeAnalysis.SymbolKind[] symbolKinds) { }
        public abstract void RegisterSymbolAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext> action, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SymbolKind> symbolKinds);
        public virtual void RegisterSymbolStartAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SymbolStartAnalysisContext> action, Microsoft.CodeAnalysis.SymbolKind symbolKind) { }
        public abstract void RegisterSyntaxNodeAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, System.Collections.Immutable.ImmutableArray<TLanguageKindEnum> syntaxKinds) where TLanguageKindEnum : struct;
        public void RegisterSyntaxNodeAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, params TLanguageKindEnum[] syntaxKinds) where TLanguageKindEnum : struct { }
        public abstract void RegisterSyntaxTreeAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext> action);
        public bool TryGetValue<TValue>(Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.Diagnostics.SourceTextValueProvider<TValue> valueProvider, out TValue value) { throw null; }
    }
    public partial class AnalysisResult
    {
        internal AnalysisResult() { }
        public System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.AdditionalText, System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>>> AdditionalFileDiagnostics { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> Analyzers { get { throw null; } }
        public System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, Microsoft.CodeAnalysis.Diagnostics.Telemetry.AnalyzerTelemetryInfo> AnalyzerTelemetryInfo { get { throw null; } }
        public System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> CompilationDiagnostics { get { throw null; } }
        public System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.SyntaxTree, System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>>> SemanticDiagnostics { get { throw null; } }
        public System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.SyntaxTree, System.Collections.Immutable.ImmutableDictionary<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>>> SyntaxDiagnostics { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetAllDiagnostics() { throw null; }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> GetAllDiagnostics(Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer analyzer) { throw null; }
    }
    public abstract partial class AnalyzerConfigOptions
    {
        protected AnalyzerConfigOptions() { }
        public static System.StringComparer KeyComparer { get { throw null; } }
        public abstract bool TryGetValue(string key, out string? value);
    }
    public abstract partial class AnalyzerConfigOptionsProvider
    {
        protected AnalyzerConfigOptionsProvider() { }
        public abstract Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptions GlobalOptions { get; }
        public abstract Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptions GetOptions(Microsoft.CodeAnalysis.AdditionalText textFile);
        public abstract Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptions GetOptions(Microsoft.CodeAnalysis.SyntaxTree tree);
    }
    public sealed partial class AnalyzerFileReference : Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference, System.IEquatable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference>
    {
        public AnalyzerFileReference(string fullPath, Microsoft.CodeAnalysis.IAnalyzerAssemblyLoader assemblyLoader) { }
        public Microsoft.CodeAnalysis.IAnalyzerAssemblyLoader AssemblyLoader { get { throw null; } }
        public override string Display { get { throw null; } }
        public override string FullPath { get { throw null; } }
        public override object Id { get { throw null; } }
        public event System.EventHandler<Microsoft.CodeAnalysis.Diagnostics.AnalyzerLoadFailureEventArgs>? AnalyzerLoadFailed { add { } remove { } }
        public bool Equals(Microsoft.CodeAnalysis.Diagnostics.AnalyzerFileReference? other) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzers(string language) { throw null; }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzersForAllLanguages() { throw null; }
        public System.Reflection.Assembly GetAssembly() { throw null; }
        [System.ObsoleteAttribute("Use GetGenerators(string language) or GetGeneratorsForAllLanguages()")]
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> GetGenerators() { throw null; }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> GetGenerators(string language) { throw null; }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> GetGeneratorsForAllLanguages() { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public sealed partial class AnalyzerImageReference : Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference
    {
        public AnalyzerImageReference(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, string? fullPath = null, string? display = null) { }
        public override string Display { get { throw null; } }
        public override string? FullPath { get { throw null; } }
        public override object Id { get { throw null; } }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzers(string language) { throw null; }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzersForAllLanguages() { throw null; }
    }
    public sealed partial class AnalyzerLoadFailureEventArgs : System.EventArgs
    {
        public AnalyzerLoadFailureEventArgs(Microsoft.CodeAnalysis.Diagnostics.AnalyzerLoadFailureEventArgs.FailureErrorCode errorCode, string message, System.Exception? exceptionOpt = null, string? typeNameOpt = null) { }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerLoadFailureEventArgs.FailureErrorCode ErrorCode { get { throw null; } }
        public System.Exception? Exception { get { throw null; } }
        public string Message { get { throw null; } }
        public string? TypeName { get { throw null; } }
        public enum FailureErrorCode
        {
            None = 0,
            UnableToLoadAnalyzer = 1,
            UnableToCreateAnalyzer = 2,
            NoAnalyzers = 3,
            ReferencesFramework = 4,
        }
    }
    public partial class AnalyzerOptions
    {
        public AnalyzerOptions(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AdditionalText> additionalFiles) { }
        public AnalyzerOptions(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AdditionalText> additionalFiles, Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptionsProvider optionsProvider) { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AdditionalText> AdditionalFiles { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerConfigOptionsProvider AnalyzerConfigOptionsProvider { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions WithAdditionalFiles(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.AdditionalText> additionalFiles) { throw null; }
    }
    public abstract partial class AnalyzerReference
    {
        protected AnalyzerReference() { }
        public virtual string Display { get { throw null; } }
        public abstract string? FullPath { get; }
        public abstract object Id { get; }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzers(string language);
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzersForAllLanguages();
        [System.ObsoleteAttribute("Use GetGenerators(string language) or GetGeneratorsForAllLanguages()")]
        public virtual System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> GetGenerators() { throw null; }
        public virtual System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> GetGenerators(string language) { throw null; }
        public virtual System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISourceGenerator> GetGeneratorsForAllLanguages() { throw null; }
    }
    public partial struct CodeBlockAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public CodeBlockAnalysisContext(Microsoft.CodeAnalysis.SyntaxNode codeBlock, Microsoft.CodeAnalysis.ISymbol owningSymbol, Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode CodeBlock { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol OwningSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.SemanticModel SemanticModel { get { throw null; } }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public abstract partial class CodeBlockStartAnalysisContext<TLanguageKindEnum> where TLanguageKindEnum : struct
    {
        protected CodeBlockStartAnalysisContext(Microsoft.CodeAnalysis.SyntaxNode codeBlock, Microsoft.CodeAnalysis.ISymbol owningSymbol, Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Threading.CancellationToken cancellationToken) { }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode CodeBlock { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol OwningSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.SemanticModel SemanticModel { get { throw null; } }
        public abstract void RegisterCodeBlockEndAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.CodeBlockAnalysisContext> action);
        public abstract void RegisterSyntaxNodeAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, System.Collections.Immutable.ImmutableArray<TLanguageKindEnum> syntaxKinds);
        public void RegisterSyntaxNodeAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, params TLanguageKindEnum[] syntaxKinds) { }
    }
    public partial struct CompilationAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public CompilationAnalysisContext(Microsoft.CodeAnalysis.Compilation compilation, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
        public bool TryGetValue<TValue>(Microsoft.CodeAnalysis.SyntaxTree tree, Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeValueProvider<TValue> valueProvider, out TValue value) { throw null; }
        public bool TryGetValue<TValue>(Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.Diagnostics.SourceTextValueProvider<TValue> valueProvider, out TValue value) { throw null; }
    }
    public abstract partial class CompilationStartAnalysisContext
    {
        protected CompilationStartAnalysisContext(Microsoft.CodeAnalysis.Compilation compilation, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Threading.CancellationToken cancellationToken) { }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public virtual void RegisterAdditionalFileAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.AdditionalFileAnalysisContext> action) { }
        public abstract void RegisterCodeBlockAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.CodeBlockAnalysisContext> action);
        public abstract void RegisterCodeBlockStartAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext<TLanguageKindEnum>> action) where TLanguageKindEnum : struct;
        public abstract void RegisterCompilationEndAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.CompilationAnalysisContext> action);
        public void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, params Microsoft.CodeAnalysis.OperationKind[] operationKinds) { }
        public virtual void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.OperationKind> operationKinds) { }
        public virtual void RegisterOperationBlockAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationBlockAnalysisContext> action) { }
        public virtual void RegisterOperationBlockStartAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationBlockStartAnalysisContext> action) { }
        public abstract void RegisterSemanticModelAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext> action);
        public void RegisterSymbolAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext> action, params Microsoft.CodeAnalysis.SymbolKind[] symbolKinds) { }
        public abstract void RegisterSymbolAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext> action, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SymbolKind> symbolKinds);
        public virtual void RegisterSymbolStartAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SymbolStartAnalysisContext> action, Microsoft.CodeAnalysis.SymbolKind symbolKind) { }
        public abstract void RegisterSyntaxNodeAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, System.Collections.Immutable.ImmutableArray<TLanguageKindEnum> syntaxKinds) where TLanguageKindEnum : struct;
        public void RegisterSyntaxNodeAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, params TLanguageKindEnum[] syntaxKinds) where TLanguageKindEnum : struct { }
        public abstract void RegisterSyntaxTreeAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext> action);
        public bool TryGetValue<TValue>(Microsoft.CodeAnalysis.SyntaxTree tree, Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeValueProvider<TValue> valueProvider, out TValue value) { throw null; }
        public bool TryGetValue<TValue>(Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.Diagnostics.SourceTextValueProvider<TValue> valueProvider, out TValue value) { throw null; }
    }
    public partial class CompilationWithAnalyzers
    {
        public CompilationWithAnalyzers(Microsoft.CodeAnalysis.Compilation compilation, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions? options, System.Threading.CancellationToken cancellationToken) { }
        public CompilationWithAnalyzers(Microsoft.CodeAnalysis.Compilation compilation, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, Microsoft.CodeAnalysis.Diagnostics.CompilationWithAnalyzersOptions analysisOptions) { }
        public Microsoft.CodeAnalysis.Diagnostics.CompilationWithAnalyzersOptions AnalysisOptions { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> Analyzers { get { throw null; } }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        [System.ObsoleteAttribute("This API is no longer required to be invoked. Analyzer state is automatically cleaned up when CompilationWithAnalyzers instance is released.")]
        public static void ClearAnalyzerState(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers) { }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAllDiagnosticsAsync() { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAllDiagnosticsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(Microsoft.CodeAnalysis.AdditionalText file, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(Microsoft.CodeAnalysis.AdditionalText file, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(Microsoft.CodeAnalysis.SemanticModel model, Microsoft.CodeAnalysis.Text.TextSpan? filterSpan, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(Microsoft.CodeAnalysis.SemanticModel model, Microsoft.CodeAnalysis.Text.TextSpan? filterSpan, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(Microsoft.CodeAnalysis.SyntaxTree tree, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(Microsoft.CodeAnalysis.SyntaxTree tree, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.AnalysisResult> GetAnalysisResultAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        [System.ObsoleteAttribute("This API was found to have performance issues and hence has been deprecated. Instead, invoke the API 'GetAnalysisResultAsync' and access the property 'CompilationDiagnostics' on the returned 'AnalysisResult' to fetch the compilation diagnostics.")]
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerCompilationDiagnosticsAsync(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        [System.ObsoleteAttribute("This API was found to have performance issues and hence has been deprecated. Instead, invoke the API 'GetAnalysisResultAsync' and access the property 'CompilationDiagnostics' on the returned 'AnalysisResult' to fetch the compilation diagnostics.")]
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerCompilationDiagnosticsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerDiagnosticsAsync() { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerDiagnosticsAsync(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerDiagnosticsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerSemanticDiagnosticsAsync(Microsoft.CodeAnalysis.SemanticModel model, Microsoft.CodeAnalysis.Text.TextSpan? filterSpan, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerSemanticDiagnosticsAsync(Microsoft.CodeAnalysis.SemanticModel model, Microsoft.CodeAnalysis.Text.TextSpan? filterSpan, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerSyntaxDiagnosticsAsync(Microsoft.CodeAnalysis.SyntaxTree tree, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAnalyzerSyntaxDiagnosticsAsync(Microsoft.CodeAnalysis.SyntaxTree tree, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Diagnostics.Telemetry.AnalyzerTelemetryInfo> GetAnalyzerTelemetryInfoAsync(Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer analyzer, System.Threading.CancellationToken cancellationToken) { throw null; }
        public static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetEffectiveDiagnostics(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> diagnostics, Microsoft.CodeAnalysis.Compilation compilation) { throw null; }
        public static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> GetEffectiveDiagnostics(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> diagnostics, Microsoft.CodeAnalysis.Compilation compilation) { throw null; }
        public static bool IsDiagnosticAnalyzerSuppressed(Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer analyzer, Microsoft.CodeAnalysis.CompilationOptions options, System.Action<System.Exception, Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, Microsoft.CodeAnalysis.Diagnostic>? onAnalyzerException = null) { throw null; }
    }
    public sealed partial class CompilationWithAnalyzersOptions
    {
        public CompilationWithAnalyzersOptions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<System.Exception, Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, Microsoft.CodeAnalysis.Diagnostic>? onAnalyzerException, bool concurrentAnalysis, bool logAnalyzerExecutionTime) { }
        public CompilationWithAnalyzersOptions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<System.Exception, Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, Microsoft.CodeAnalysis.Diagnostic>? onAnalyzerException, bool concurrentAnalysis, bool logAnalyzerExecutionTime, bool reportSuppressedDiagnostics) { }
        public CompilationWithAnalyzersOptions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions? options, System.Action<System.Exception, Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, Microsoft.CodeAnalysis.Diagnostic>? onAnalyzerException, bool concurrentAnalysis, bool logAnalyzerExecutionTime, bool reportSuppressedDiagnostics, System.Func<System.Exception, bool>? analyzerExceptionFilter) { }
        public System.Func<System.Exception, bool>? AnalyzerExceptionFilter { get { throw null; } }
        public bool ConcurrentAnalysis { get { throw null; } }
        public bool LogAnalyzerExecutionTime { get { throw null; } }
        public System.Action<System.Exception, Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, Microsoft.CodeAnalysis.Diagnostic>? OnAnalyzerException { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions? Options { get { throw null; } }
        public bool ReportSuppressedDiagnostics { get { throw null; } }
    }
    public abstract partial class DiagnosticAnalyzer
    {
        protected DiagnosticAnalyzer() { }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DiagnosticDescriptor> SupportedDiagnostics { get; }
        public sealed override bool Equals(object? obj) { throw null; }
        public sealed override int GetHashCode() { throw null; }
        public abstract void Initialize(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext context);
        public sealed override string ToString() { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    public sealed partial class DiagnosticAnalyzerAttribute : System.Attribute
    {
        public DiagnosticAnalyzerAttribute(string firstLanguage, params string[] additionalLanguages) { }
        public string[] Languages { get { throw null; } }
    }
    public static partial class DiagnosticAnalyzerExtensions
    {
        public static Microsoft.CodeAnalysis.Diagnostics.CompilationWithAnalyzers WithAnalyzers(this Microsoft.CodeAnalysis.Compilation compilation, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.Diagnostics.CompilationWithAnalyzers WithAnalyzers(this Microsoft.CodeAnalysis.Compilation compilation, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> analyzers, Microsoft.CodeAnalysis.Diagnostics.CompilationWithAnalyzersOptions analysisOptions) { throw null; }
    }
    public abstract partial class DiagnosticSuppressor : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer
    {
        protected DiagnosticSuppressor() { }
        public sealed override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DiagnosticDescriptor> SupportedDiagnostics { get { throw null; } }
        public abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SuppressionDescriptor> SupportedSuppressions { get; }
        public sealed override void Initialize(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext context) { }
        public abstract void ReportSuppressions(Microsoft.CodeAnalysis.Diagnostics.SuppressionAnalysisContext context);
    }
    [System.FlagsAttribute]
    public enum GeneratedCodeAnalysisFlags
    {
        None = 0,
        Analyze = 1,
        ReportDiagnostics = 2,
    }
    public partial struct OperationAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public OperationAnalysisContext(Microsoft.CodeAnalysis.IOperation operation, Microsoft.CodeAnalysis.ISymbol containingSymbol, Microsoft.CodeAnalysis.Compilation compilation, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol ContainingSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.IOperation Operation { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph GetControlFlowGraph() { throw null; }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public partial struct OperationBlockAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public OperationBlockAnalysisContext(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> operationBlocks, Microsoft.CodeAnalysis.ISymbol owningSymbol, Microsoft.CodeAnalysis.Compilation compilation, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> OperationBlocks { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol OwningSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph GetControlFlowGraph(Microsoft.CodeAnalysis.IOperation operationBlock) { throw null; }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public abstract partial class OperationBlockStartAnalysisContext
    {
        protected OperationBlockStartAnalysisContext(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> operationBlocks, Microsoft.CodeAnalysis.ISymbol owningSymbol, Microsoft.CodeAnalysis.Compilation compilation, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Threading.CancellationToken cancellationToken) { }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> OperationBlocks { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol OwningSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph GetControlFlowGraph(Microsoft.CodeAnalysis.IOperation operationBlock) { throw null; }
        public void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, params Microsoft.CodeAnalysis.OperationKind[] operationKinds) { }
        public abstract void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.OperationKind> operationKinds);
        public abstract void RegisterOperationBlockEndAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationBlockAnalysisContext> action);
    }
    public partial struct SemanticModelAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public SemanticModelAnalysisContext(Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.SemanticModel SemanticModel { get { throw null; } }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public sealed partial class SourceTextValueProvider<TValue>
    {
        public SourceTextValueProvider(System.Func<Microsoft.CodeAnalysis.Text.SourceText, TValue> computeValue, System.Collections.Generic.IEqualityComparer<Microsoft.CodeAnalysis.Text.SourceText>? sourceTextComparer = null) { }
    }
    public partial struct Suppression : System.IEquatable<Microsoft.CodeAnalysis.Diagnostics.Suppression>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Microsoft.CodeAnalysis.SuppressionDescriptor Descriptor { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostic SuppressedDiagnostic { get { throw null; } }
        public static Microsoft.CodeAnalysis.Diagnostics.Suppression Create(Microsoft.CodeAnalysis.SuppressionDescriptor descriptor, Microsoft.CodeAnalysis.Diagnostic suppressedDiagnostic) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.Diagnostics.Suppression other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Diagnostics.Suppression left, Microsoft.CodeAnalysis.Diagnostics.Suppression right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Diagnostics.Suppression left, Microsoft.CodeAnalysis.Diagnostics.Suppression right) { throw null; }
    }
    public partial struct SuppressionAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> ReportedDiagnostics { get { throw null; } }
        public Microsoft.CodeAnalysis.SemanticModel GetSemanticModel(Microsoft.CodeAnalysis.SyntaxTree syntaxTree) { throw null; }
        public void ReportSuppression(Microsoft.CodeAnalysis.Diagnostics.Suppression suppression) { }
    }
    public sealed partial class SuppressionInfo
    {
        internal SuppressionInfo() { }
        public Microsoft.CodeAnalysis.AttributeData? Attribute { get { throw null; } }
        public string Id { get { throw null; } }
    }
    public partial struct SymbolAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public SymbolAnalysisContext(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Compilation compilation, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol Symbol { get { throw null; } }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public abstract partial class SymbolStartAnalysisContext
    {
        public SymbolStartAnalysisContext(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Compilation compilation, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Threading.CancellationToken cancellationToken) { }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol Symbol { get { throw null; } }
        public abstract void RegisterCodeBlockAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.CodeBlockAnalysisContext> action);
        public abstract void RegisterCodeBlockStartAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext<TLanguageKindEnum>> action) where TLanguageKindEnum : struct;
        public void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, params Microsoft.CodeAnalysis.OperationKind[] operationKinds) { }
        public abstract void RegisterOperationAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationAnalysisContext> action, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.OperationKind> operationKinds);
        public abstract void RegisterOperationBlockAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationBlockAnalysisContext> action);
        public abstract void RegisterOperationBlockStartAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.OperationBlockStartAnalysisContext> action);
        public abstract void RegisterSymbolEndAction(System.Action<Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext> action);
        public abstract void RegisterSyntaxNodeAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, System.Collections.Immutable.ImmutableArray<TLanguageKindEnum> syntaxKinds) where TLanguageKindEnum : struct;
        public void RegisterSyntaxNodeAction<TLanguageKindEnum>(System.Action<Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext> action, params TLanguageKindEnum[] syntaxKinds) where TLanguageKindEnum : struct { }
    }
    public partial struct SyntaxNodeAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public SyntaxNodeAnalysisContext(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.ISymbol? containingSymbol, Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public SyntaxNodeAnalysisContext(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Compilation Compilation { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol? ContainingSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode Node { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.SemanticModel SemanticModel { get { throw null; } }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public partial struct SyntaxTreeAnalysisContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public SyntaxTreeAnalysisContext(Microsoft.CodeAnalysis.SyntaxTree tree, Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions options, System.Action<Microsoft.CodeAnalysis.Diagnostic> reportDiagnostic, System.Func<Microsoft.CodeAnalysis.Diagnostic, bool> isSupportedDiagnostic, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions Options { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxTree Tree { get { throw null; } }
        public void ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
    }
    public sealed partial class SyntaxTreeValueProvider<TValue>
    {
        public SyntaxTreeValueProvider(System.Func<Microsoft.CodeAnalysis.SyntaxTree, TValue> computeValue, System.Collections.Generic.IEqualityComparer<Microsoft.CodeAnalysis.SyntaxTree>? syntaxTreeComparer = null) { }
    }
    public sealed partial class UnresolvedAnalyzerReference : Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference
    {
        public UnresolvedAnalyzerReference(string unresolvedPath) { }
        public override string Display { get { throw null; } }
        public override string FullPath { get { throw null; } }
        public override object Id { get { throw null; } }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzers(string language) { throw null; }
        public override System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer> GetAnalyzersForAllLanguages() { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.Diagnostics.Telemetry
{
    [System.Runtime.Serialization.DataContractAttribute]
    public sealed partial class AnalyzerTelemetryInfo
    {
        public AnalyzerTelemetryInfo() { }
        [System.Runtime.Serialization.DataMemberAttribute(Order=4)]
        public int AdditionalFileActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=12)]
        public int CodeBlockActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=11)]
        public int CodeBlockEndActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=10)]
        public int CodeBlockStartActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public int CompilationActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int CompilationEndActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int CompilationStartActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=19)]
        public bool Concurrent { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=18)]
        public System.TimeSpan ExecutionTime { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=13)]
        public int OperationActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=16)]
        public int OperationBlockActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=15)]
        public int OperationBlockEndActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=14)]
        public int OperationBlockStartActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public int SemanticModelActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=17)]
        public int SuppressionActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=6)]
        public int SymbolActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=8)]
        public int SymbolEndActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=7)]
        public int SymbolStartActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=9)]
        public int SyntaxNodeActionsCount { get { throw null; } set { } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public int SyntaxTreeActionsCount { get { throw null; } set { } }
    }
}
namespace Microsoft.CodeAnalysis.Emit
{
    public enum DebugInformationFormat
    {
        Pdb = 1,
        PortablePdb = 2,
        Embedded = 3,
    }
    public readonly partial struct EditAndContinueMethodDebugInformation
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public static Microsoft.CodeAnalysis.Emit.EditAndContinueMethodDebugInformation Create(System.Collections.Immutable.ImmutableArray<byte> compressedSlotMap, System.Collections.Immutable.ImmutableArray<byte> compressedLambdaMap) { throw null; }
    }
    public sealed partial class EmitBaseline
    {
        internal EmitBaseline() { }
        public Microsoft.CodeAnalysis.ModuleMetadata OriginalMetadata { get { throw null; } }
        public static Microsoft.CodeAnalysis.Emit.EmitBaseline CreateInitialBaseline(Microsoft.CodeAnalysis.ModuleMetadata module, System.Func<System.Reflection.Metadata.MethodDefinitionHandle, Microsoft.CodeAnalysis.Emit.EditAndContinueMethodDebugInformation> debugInformationProvider) { throw null; }
        public static Microsoft.CodeAnalysis.Emit.EmitBaseline CreateInitialBaseline(Microsoft.CodeAnalysis.ModuleMetadata module, System.Func<System.Reflection.Metadata.MethodDefinitionHandle, Microsoft.CodeAnalysis.Emit.EditAndContinueMethodDebugInformation> debugInformationProvider, System.Func<System.Reflection.Metadata.MethodDefinitionHandle, System.Reflection.Metadata.StandaloneSignatureHandle> localSignatureProvider, bool hasPortableDebugInformation) { throw null; }
    }
    public sealed partial class EmitDifferenceResult : Microsoft.CodeAnalysis.Emit.EmitResult
    {
        internal EmitDifferenceResult() { }
        public Microsoft.CodeAnalysis.Emit.EmitBaseline? Baseline { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.Metadata.TypeDefinitionHandle> ChangedTypes { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.Metadata.MethodDefinitionHandle> UpdatedMethods { get { throw null; } }
    }
    public sealed partial class EmitOptions : System.IEquatable<Microsoft.CodeAnalysis.Emit.EmitOptions>
    {
        public EmitOptions(bool metadataOnly, Microsoft.CodeAnalysis.Emit.DebugInformationFormat debugInformationFormat, string pdbFilePath, string outputNameOverride, int fileAlignment, ulong baseAddress, bool highEntropyVirtualAddressSpace, Microsoft.CodeAnalysis.SubsystemVersion subsystemVersion, string runtimeMetadataVersion, bool tolerateErrors, bool includePrivateMembers) { }
        public EmitOptions(bool metadataOnly, Microsoft.CodeAnalysis.Emit.DebugInformationFormat debugInformationFormat, string pdbFilePath, string outputNameOverride, int fileAlignment, ulong baseAddress, bool highEntropyVirtualAddressSpace, Microsoft.CodeAnalysis.SubsystemVersion subsystemVersion, string runtimeMetadataVersion, bool tolerateErrors, bool includePrivateMembers, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Emit.InstrumentationKind> instrumentationKinds) { }
        public EmitOptions(bool metadataOnly, Microsoft.CodeAnalysis.Emit.DebugInformationFormat debugInformationFormat, string? pdbFilePath, string? outputNameOverride, int fileAlignment, ulong baseAddress, bool highEntropyVirtualAddressSpace, Microsoft.CodeAnalysis.SubsystemVersion subsystemVersion, string? runtimeMetadataVersion, bool tolerateErrors, bool includePrivateMembers, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Emit.InstrumentationKind> instrumentationKinds, System.Security.Cryptography.HashAlgorithmName? pdbChecksumAlgorithm) { }
        public EmitOptions(bool metadataOnly = false, Microsoft.CodeAnalysis.Emit.DebugInformationFormat debugInformationFormat = default(Microsoft.CodeAnalysis.Emit.DebugInformationFormat), string? pdbFilePath = null, string? outputNameOverride = null, int fileAlignment = 0, ulong baseAddress = (ulong)0, bool highEntropyVirtualAddressSpace = false, Microsoft.CodeAnalysis.SubsystemVersion subsystemVersion = default(Microsoft.CodeAnalysis.SubsystemVersion), string? runtimeMetadataVersion = null, bool tolerateErrors = false, bool includePrivateMembers = true, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Emit.InstrumentationKind> instrumentationKinds = default(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Emit.InstrumentationKind>), System.Security.Cryptography.HashAlgorithmName? pdbChecksumAlgorithm = default(System.Security.Cryptography.HashAlgorithmName?), System.Text.Encoding? defaultSourceFileEncoding = null, System.Text.Encoding? fallbackSourceFileEncoding = null) { }
        public ulong BaseAddress { get { throw null; } }
        public Microsoft.CodeAnalysis.Emit.DebugInformationFormat DebugInformationFormat { get { throw null; } }
        public System.Text.Encoding? DefaultSourceFileEncoding { get { throw null; } }
        public bool EmitMetadataOnly { get { throw null; } }
        public System.Text.Encoding? FallbackSourceFileEncoding { get { throw null; } }
        public int FileAlignment { get { throw null; } }
        public bool HighEntropyVirtualAddressSpace { get { throw null; } }
        public bool IncludePrivateMembers { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Emit.InstrumentationKind> InstrumentationKinds { get { throw null; } }
        public string? OutputNameOverride { get { throw null; } }
        public System.Security.Cryptography.HashAlgorithmName PdbChecksumAlgorithm { get { throw null; } }
        public string? PdbFilePath { get { throw null; } }
        public string? RuntimeMetadataVersion { get { throw null; } }
        public Microsoft.CodeAnalysis.SubsystemVersion SubsystemVersion { get { throw null; } }
        public bool TolerateErrors { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Emit.EmitOptions? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Emit.EmitOptions? left, Microsoft.CodeAnalysis.Emit.EmitOptions? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Emit.EmitOptions? left, Microsoft.CodeAnalysis.Emit.EmitOptions? right) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithBaseAddress(ulong value) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithDebugInformationFormat(Microsoft.CodeAnalysis.Emit.DebugInformationFormat format) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithDefaultSourceFileEncoding(System.Text.Encoding? defaultSourceFileEncoding) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithEmitMetadataOnly(bool value) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithFallbackSourceFileEncoding(System.Text.Encoding? fallbackSourceFileEncoding) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithFileAlignment(int value) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithHighEntropyVirtualAddressSpace(bool value) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithIncludePrivateMembers(bool value) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithInstrumentationKinds(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Emit.InstrumentationKind> instrumentationKinds) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithOutputNameOverride(string outputName) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithPdbChecksumAlgorithm(System.Security.Cryptography.HashAlgorithmName name) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithPdbFilePath(string path) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithRuntimeMetadataVersion(string version) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithSubsystemVersion(Microsoft.CodeAnalysis.SubsystemVersion subsystemVersion) { throw null; }
        public Microsoft.CodeAnalysis.Emit.EmitOptions WithTolerateErrors(bool value) { throw null; }
    }
    public partial class EmitResult
    {
        internal EmitResult() { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Diagnostics { get { throw null; } }
        public bool Success { get { throw null; } }
        protected virtual string GetDebuggerDisplay() { throw null; }
    }
    public enum InstrumentationKind
    {
        None = 0,
        TestCoverage = 1,
    }
    public readonly partial struct SemanticEdit : System.IEquatable<Microsoft.CodeAnalysis.Emit.SemanticEdit>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SemanticEdit(Microsoft.CodeAnalysis.Emit.SemanticEditKind kind, Microsoft.CodeAnalysis.ISymbol? oldSymbol, Microsoft.CodeAnalysis.ISymbol? newSymbol, System.Func<Microsoft.CodeAnalysis.SyntaxNode, Microsoft.CodeAnalysis.SyntaxNode?>? syntaxMap = null, bool preserveLocalVariables = false) { throw null; }
        public Microsoft.CodeAnalysis.Emit.SemanticEditKind Kind { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol? NewSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol? OldSymbol { get { throw null; } }
        public bool PreserveLocalVariables { get { throw null; } }
        public System.Func<Microsoft.CodeAnalysis.SyntaxNode, Microsoft.CodeAnalysis.SyntaxNode?>? SyntaxMap { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Emit.SemanticEdit other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Emit.SemanticEdit left, Microsoft.CodeAnalysis.Emit.SemanticEdit right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Emit.SemanticEdit left, Microsoft.CodeAnalysis.Emit.SemanticEdit right) { throw null; }
    }
    public enum SemanticEditKind
    {
        None = 0,
        Update = 1,
        Insert = 2,
        Delete = 3,
        Replace = 4,
    }
}
namespace Microsoft.CodeAnalysis.FlowAnalysis
{
    public sealed partial class BasicBlock
    {
        internal BasicBlock() { }
        public Microsoft.CodeAnalysis.IOperation? BranchValue { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowBranch? ConditionalSuccessor { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowConditionKind ConditionKind { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegion EnclosingRegion { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowBranch? FallThroughSuccessor { get { throw null; } }
        public bool IsReachable { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.BasicBlockKind Kind { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Operations { get { throw null; } }
        public int Ordinal { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowBranch> Predecessors { get { throw null; } }
    }
    public enum BasicBlockKind
    {
        Entry = 0,
        Exit = 1,
        Block = 2,
    }
    public partial struct CaptureId : System.IEquatable<Microsoft.CodeAnalysis.FlowAnalysis.CaptureId>
    {
        private int _dummyPrimitive;
        public bool Equals(Microsoft.CodeAnalysis.FlowAnalysis.CaptureId other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public sealed partial class ControlFlowBranch
    {
        internal ControlFlowBranch() { }
        public Microsoft.CodeAnalysis.FlowAnalysis.BasicBlock? Destination { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegion> EnteringRegions { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegion> FinallyRegions { get { throw null; } }
        public bool IsConditionalSuccessor { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegion> LeavingRegions { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowBranchSemantics Semantics { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.BasicBlock Source { get { throw null; } }
    }
    public enum ControlFlowBranchSemantics
    {
        None = 0,
        Regular = 1,
        Return = 2,
        StructuredExceptionHandling = 3,
        ProgramTermination = 4,
        Throw = 5,
        Rethrow = 6,
        Error = 7,
    }
    public enum ControlFlowConditionKind
    {
        None = 0,
        WhenFalse = 1,
        WhenTrue = 2,
    }
    public sealed partial class ControlFlowGraph
    {
        internal ControlFlowGraph() { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.FlowAnalysis.BasicBlock> Blocks { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IMethodSymbol> LocalFunctions { get { throw null; } }
        public Microsoft.CodeAnalysis.IOperation OriginalOperation { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph? Parent { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegion Root { get { throw null; } }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph Create(Microsoft.CodeAnalysis.Operations.IBlockOperation body, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph Create(Microsoft.CodeAnalysis.Operations.IConstructorBodyOperation constructorBody, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph Create(Microsoft.CodeAnalysis.Operations.IFieldInitializerOperation initializer, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph Create(Microsoft.CodeAnalysis.Operations.IMethodBodyOperation methodBody, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph Create(Microsoft.CodeAnalysis.Operations.IParameterInitializerOperation initializer, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph Create(Microsoft.CodeAnalysis.Operations.IPropertyInitializerOperation initializer, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph? Create(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SemanticModel semanticModel, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph GetAnonymousFunctionControlFlowGraph(Microsoft.CodeAnalysis.FlowAnalysis.IFlowAnonymousFunctionOperation anonymousFunction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph GetLocalFunctionControlFlowGraph(Microsoft.CodeAnalysis.IMethodSymbol localFunction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public static partial class ControlFlowGraphExtensions
    {
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph GetAnonymousFunctionControlFlowGraphInScope(this Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph controlFlowGraph, Microsoft.CodeAnalysis.FlowAnalysis.IFlowAnonymousFunctionOperation anonymousFunction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph GetLocalFunctionControlFlowGraphInScope(this Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowGraph controlFlowGraph, Microsoft.CodeAnalysis.IMethodSymbol localFunction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public sealed partial class ControlFlowRegion
    {
        internal ControlFlowRegion() { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.FlowAnalysis.CaptureId> CaptureIds { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegion? EnclosingRegion { get { throw null; } }
        public Microsoft.CodeAnalysis.ITypeSymbol? ExceptionType { get { throw null; } }
        public int FirstBlockOrdinal { get { throw null; } }
        public Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegionKind Kind { get { throw null; } }
        public int LastBlockOrdinal { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IMethodSymbol> LocalFunctions { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.FlowAnalysis.ControlFlowRegion> NestedRegions { get { throw null; } }
    }
    public enum ControlFlowRegionKind
    {
        Root = 0,
        LocalLifetime = 1,
        Try = 2,
        Filter = 3,
        Catch = 4,
        FilterAndHandler = 5,
        TryAndCatch = 6,
        Finally = 7,
        TryAndFinally = 8,
        StaticLocalInitializer = 9,
        ErroneousBody = 10,
    }
    public partial interface ICaughtExceptionOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IFlowAnonymousFunctionOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IMethodSymbol Symbol { get; }
    }
    public partial interface IFlowCaptureOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.FlowAnalysis.CaptureId Id { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IFlowCaptureReferenceOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.FlowAnalysis.CaptureId Id { get; }
    }
    public partial interface IIsNullOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Operand { get; }
    }
    public partial interface IStaticLocalInitializationSemaphoreOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.ILocalSymbol Local { get; }
    }
}
namespace Microsoft.CodeAnalysis.Operations
{
    public enum ArgumentKind
    {
        None = 0,
        Explicit = 1,
        ParamArray = 2,
        DefaultValue = 3,
    }
    public enum BinaryOperatorKind
    {
        None = 0,
        Add = 1,
        Subtract = 2,
        Multiply = 3,
        Divide = 4,
        IntegerDivide = 5,
        Remainder = 6,
        Power = 7,
        LeftShift = 8,
        RightShift = 9,
        And = 10,
        Or = 11,
        ExclusiveOr = 12,
        ConditionalAnd = 13,
        ConditionalOr = 14,
        Concatenate = 15,
        Equals = 16,
        ObjectValueEquals = 17,
        NotEquals = 18,
        ObjectValueNotEquals = 19,
        LessThan = 20,
        LessThanOrEqual = 21,
        GreaterThanOrEqual = 22,
        GreaterThan = 23,
        Like = 24,
    }
    public enum BranchKind
    {
        None = 0,
        Continue = 1,
        Break = 2,
        GoTo = 3,
    }
    public enum CaseKind
    {
        None = 0,
        SingleValue = 1,
        Relational = 2,
        Range = 3,
        Default = 4,
        Pattern = 5,
    }
    public partial struct CommonConversion
    {
        private object _dummy;
        private int _dummyPrimitive;
        public bool Exists { get { throw null; } }
        public bool IsIdentity { get { throw null; } }
        public bool IsImplicit { get { throw null; } }
        public bool IsNullable { get { throw null; } }
        public bool IsNumeric { get { throw null; } }
        public bool IsReference { get { throw null; } }
        public bool IsUserDefined { get { throw null; } }
        public Microsoft.CodeAnalysis.IMethodSymbol? MethodSymbol { get { throw null; } }
    }
    public partial interface IAddressOfOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Reference { get; }
    }
    public partial interface IAnonymousFunctionOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.IBlockOperation Body { get; }
        Microsoft.CodeAnalysis.IMethodSymbol Symbol { get; }
    }
    public partial interface IAnonymousObjectCreationOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Initializers { get; }
    }
    public partial interface IArgumentOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.ArgumentKind ArgumentKind { get; }
        Microsoft.CodeAnalysis.Operations.CommonConversion InConversion { get; }
        Microsoft.CodeAnalysis.Operations.CommonConversion OutConversion { get; }
        Microsoft.CodeAnalysis.IParameterSymbol? Parameter { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IArrayCreationOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> DimensionSizes { get; }
        Microsoft.CodeAnalysis.Operations.IArrayInitializerOperation? Initializer { get; }
    }
    public partial interface IArrayElementReferenceOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation ArrayReference { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Indices { get; }
    }
    public partial interface IArrayInitializerOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> ElementValues { get; }
    }
    public partial interface IAssignmentOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Target { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IAwaitOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Operation { get; }
    }
    public partial interface IBinaryOperation : Microsoft.CodeAnalysis.IOperation
    {
        bool IsChecked { get; }
        bool IsCompareText { get; }
        bool IsLifted { get; }
        Microsoft.CodeAnalysis.IOperation LeftOperand { get; }
        Microsoft.CodeAnalysis.Operations.BinaryOperatorKind OperatorKind { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? OperatorMethod { get; }
        Microsoft.CodeAnalysis.IOperation RightOperand { get; }
    }
    public partial interface IBinaryPatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
        Microsoft.CodeAnalysis.Operations.IPatternOperation LeftPattern { get; }
        Microsoft.CodeAnalysis.Operations.BinaryOperatorKind OperatorKind { get; }
        Microsoft.CodeAnalysis.Operations.IPatternOperation RightPattern { get; }
    }
    public partial interface IBlockOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Operations { get; }
    }
    public partial interface IBranchOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.BranchKind BranchKind { get; }
        Microsoft.CodeAnalysis.ILabelSymbol Target { get; }
    }
    public partial interface ICaseClauseOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.CaseKind CaseKind { get; }
        Microsoft.CodeAnalysis.ILabelSymbol? Label { get; }
    }
    public partial interface ICatchClauseOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation? ExceptionDeclarationOrExpression { get; }
        Microsoft.CodeAnalysis.ITypeSymbol ExceptionType { get; }
        Microsoft.CodeAnalysis.IOperation? Filter { get; }
        Microsoft.CodeAnalysis.Operations.IBlockOperation Handler { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
    }
    public partial interface ICoalesceAssignmentOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IAssignmentOperation
    {
    }
    public partial interface ICoalesceOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Value { get; }
        Microsoft.CodeAnalysis.Operations.CommonConversion ValueConversion { get; }
        Microsoft.CodeAnalysis.IOperation WhenNull { get; }
    }
    [System.ObsoleteAttribute("ICollectionElementInitializerOperation has been replaced with IInvocationOperation and IDynamicInvocationOperation", true)]
    public partial interface ICollectionElementInitializerOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IMethodSymbol AddMethod { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Arguments { get; }
        bool IsDynamic { get; }
    }
    public partial interface ICompoundAssignmentOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IAssignmentOperation
    {
        Microsoft.CodeAnalysis.Operations.CommonConversion InConversion { get; }
        bool IsChecked { get; }
        bool IsLifted { get; }
        Microsoft.CodeAnalysis.Operations.BinaryOperatorKind OperatorKind { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? OperatorMethod { get; }
        Microsoft.CodeAnalysis.Operations.CommonConversion OutConversion { get; }
    }
    public partial interface IConditionalAccessInstanceOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IConditionalAccessOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Operation { get; }
        Microsoft.CodeAnalysis.IOperation WhenNotNull { get; }
    }
    public partial interface IConditionalOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Condition { get; }
        bool IsRef { get; }
        Microsoft.CodeAnalysis.IOperation? WhenFalse { get; }
        Microsoft.CodeAnalysis.IOperation WhenTrue { get; }
    }
    public partial interface IConstantPatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IConstructorBodyOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IMethodBodyBaseOperation
    {
        Microsoft.CodeAnalysis.IOperation? Initializer { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
    }
    public partial interface IConversionOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.CommonConversion Conversion { get; }
        bool IsChecked { get; }
        bool IsTryCast { get; }
        Microsoft.CodeAnalysis.IOperation Operand { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? OperatorMethod { get; }
    }
    public partial interface IDeclarationExpressionOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Expression { get; }
    }
    public partial interface IDeclarationPatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
        Microsoft.CodeAnalysis.ISymbol? DeclaredSymbol { get; }
        Microsoft.CodeAnalysis.ITypeSymbol? MatchedType { get; }
        bool MatchesNull { get; }
    }
    public partial interface IDeconstructionAssignmentOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IAssignmentOperation
    {
    }
    public partial interface IDefaultCaseClauseOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ICaseClauseOperation
    {
    }
    public partial interface IDefaultValueOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IDelegateCreationOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Target { get; }
    }
    public partial interface IDiscardOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IDiscardSymbol DiscardSymbol { get; }
    }
    public partial interface IDiscardPatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
    }
    public partial interface IDynamicIndexerAccessOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Arguments { get; }
        Microsoft.CodeAnalysis.IOperation Operation { get; }
    }
    public partial interface IDynamicInvocationOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Arguments { get; }
        Microsoft.CodeAnalysis.IOperation Operation { get; }
    }
    public partial interface IDynamicMemberReferenceOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.ITypeSymbol? ContainingType { get; }
        Microsoft.CodeAnalysis.IOperation? Instance { get; }
        string MemberName { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ITypeSymbol> TypeArguments { get; }
    }
    public partial interface IDynamicObjectCreationOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Arguments { get; }
        Microsoft.CodeAnalysis.Operations.IObjectOrCollectionInitializerOperation? Initializer { get; }
    }
    public partial interface IEmptyOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IEndOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IEventAssignmentOperation : Microsoft.CodeAnalysis.IOperation
    {
        bool Adds { get; }
        Microsoft.CodeAnalysis.IOperation EventReference { get; }
        Microsoft.CodeAnalysis.IOperation HandlerValue { get; }
    }
    public partial interface IEventReferenceOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IMemberReferenceOperation
    {
        Microsoft.CodeAnalysis.IEventSymbol Event { get; }
    }
    public partial interface IExpressionStatementOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Operation { get; }
    }
    public partial interface IFieldInitializerOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ISymbolInitializerOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IFieldSymbol> InitializedFields { get; }
    }
    public partial interface IFieldReferenceOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IMemberReferenceOperation
    {
        Microsoft.CodeAnalysis.IFieldSymbol Field { get; }
        bool IsDeclaration { get; }
    }
    public partial interface IForEachLoopOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ILoopOperation
    {
        Microsoft.CodeAnalysis.IOperation Collection { get; }
        bool IsAsynchronous { get; }
        Microsoft.CodeAnalysis.IOperation LoopControlVariable { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> NextVariables { get; }
    }
    public partial interface IForLoopOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ILoopOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> AtLoopBottom { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Before { get; }
        Microsoft.CodeAnalysis.IOperation? Condition { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> ConditionLocals { get; }
    }
    public partial interface IForToLoopOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ILoopOperation
    {
        Microsoft.CodeAnalysis.IOperation InitialValue { get; }
        bool IsChecked { get; }
        Microsoft.CodeAnalysis.IOperation LimitValue { get; }
        Microsoft.CodeAnalysis.IOperation LoopControlVariable { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> NextVariables { get; }
        Microsoft.CodeAnalysis.IOperation StepValue { get; }
    }
    public partial interface IIncrementOrDecrementOperation : Microsoft.CodeAnalysis.IOperation
    {
        bool IsChecked { get; }
        bool IsLifted { get; }
        bool IsPostfix { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? OperatorMethod { get; }
        Microsoft.CodeAnalysis.IOperation Target { get; }
    }
    public partial interface IInstanceReferenceOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.InstanceReferenceKind ReferenceKind { get; }
    }
    public partial interface IInterpolatedStringContentOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IInterpolatedStringOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IInterpolatedStringContentOperation> Parts { get; }
    }
    public partial interface IInterpolatedStringTextOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IInterpolatedStringContentOperation
    {
        Microsoft.CodeAnalysis.IOperation Text { get; }
    }
    public partial interface IInterpolationOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IInterpolatedStringContentOperation
    {
        Microsoft.CodeAnalysis.IOperation? Alignment { get; }
        Microsoft.CodeAnalysis.IOperation Expression { get; }
        Microsoft.CodeAnalysis.IOperation? FormatString { get; }
    }
    public partial interface IInvalidOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IInvocationOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IArgumentOperation> Arguments { get; }
        Microsoft.CodeAnalysis.IOperation? Instance { get; }
        bool IsVirtual { get; }
        Microsoft.CodeAnalysis.IMethodSymbol TargetMethod { get; }
    }
    public partial interface IIsPatternOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.IPatternOperation Pattern { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IIsTypeOperation : Microsoft.CodeAnalysis.IOperation
    {
        bool IsNegated { get; }
        Microsoft.CodeAnalysis.ITypeSymbol TypeOperand { get; }
        Microsoft.CodeAnalysis.IOperation ValueOperand { get; }
    }
    public partial interface ILabeledOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.ILabelSymbol Label { get; }
        Microsoft.CodeAnalysis.IOperation? Operation { get; }
    }
    public partial interface ILiteralOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface ILocalFunctionOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.IBlockOperation? Body { get; }
        Microsoft.CodeAnalysis.Operations.IBlockOperation? IgnoredBody { get; }
        Microsoft.CodeAnalysis.IMethodSymbol Symbol { get; }
    }
    public partial interface ILocalReferenceOperation : Microsoft.CodeAnalysis.IOperation
    {
        bool IsDeclaration { get; }
        Microsoft.CodeAnalysis.ILocalSymbol Local { get; }
    }
    public partial interface ILockOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Body { get; }
        Microsoft.CodeAnalysis.IOperation LockedValue { get; }
    }
    public partial interface ILoopOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Body { get; }
        Microsoft.CodeAnalysis.ILabelSymbol ContinueLabel { get; }
        Microsoft.CodeAnalysis.ILabelSymbol ExitLabel { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
        Microsoft.CodeAnalysis.Operations.LoopKind LoopKind { get; }
    }
    public partial interface IMemberInitializerOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation InitializedMember { get; }
        Microsoft.CodeAnalysis.Operations.IObjectOrCollectionInitializerOperation Initializer { get; }
    }
    public partial interface IMemberReferenceOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation? Instance { get; }
        Microsoft.CodeAnalysis.ISymbol Member { get; }
    }
    public partial interface IMethodBodyBaseOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.IBlockOperation? BlockBody { get; }
        Microsoft.CodeAnalysis.Operations.IBlockOperation? ExpressionBody { get; }
    }
    public partial interface IMethodBodyOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IMethodBodyBaseOperation
    {
    }
    public partial interface IMethodReferenceOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IMemberReferenceOperation
    {
        bool IsVirtual { get; }
        Microsoft.CodeAnalysis.IMethodSymbol Method { get; }
    }
    public partial interface INameOfOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Argument { get; }
    }
    public partial interface INegatedPatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
        Microsoft.CodeAnalysis.Operations.IPatternOperation Pattern { get; }
    }
    public enum InstanceReferenceKind
    {
        ContainingTypeInstance = 0,
        ImplicitReceiver = 1,
        PatternInput = 2,
    }
    public partial interface IObjectCreationOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IArgumentOperation> Arguments { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? Constructor { get; }
        Microsoft.CodeAnalysis.Operations.IObjectOrCollectionInitializerOperation? Initializer { get; }
    }
    public partial interface IObjectOrCollectionInitializerOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Initializers { get; }
    }
    public partial interface IOmittedArgumentOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface IParameterInitializerOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ISymbolInitializerOperation
    {
        Microsoft.CodeAnalysis.IParameterSymbol Parameter { get; }
    }
    public partial interface IParameterReferenceOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IParameterSymbol Parameter { get; }
    }
    public partial interface IParenthesizedOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Operand { get; }
    }
    public partial interface IPatternCaseClauseOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ICaseClauseOperation
    {
        Microsoft.CodeAnalysis.IOperation? Guard { get; }
        new Microsoft.CodeAnalysis.ILabelSymbol Label { get; }
        Microsoft.CodeAnalysis.Operations.IPatternOperation Pattern { get; }
    }
    public partial interface IPatternOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.ITypeSymbol InputType { get; }
        Microsoft.CodeAnalysis.ITypeSymbol NarrowedType { get; }
    }
    public partial interface IPropertyInitializerOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ISymbolInitializerOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IPropertySymbol> InitializedProperties { get; }
    }
    public partial interface IPropertyReferenceOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IMemberReferenceOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IArgumentOperation> Arguments { get; }
        Microsoft.CodeAnalysis.IPropertySymbol Property { get; }
    }
    public partial interface IPropertySubpatternOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Member { get; }
        Microsoft.CodeAnalysis.Operations.IPatternOperation Pattern { get; }
    }
    public partial interface IRaiseEventOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IArgumentOperation> Arguments { get; }
        Microsoft.CodeAnalysis.Operations.IEventReferenceOperation EventReference { get; }
    }
    public partial interface IRangeCaseClauseOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ICaseClauseOperation
    {
        Microsoft.CodeAnalysis.IOperation MaximumValue { get; }
        Microsoft.CodeAnalysis.IOperation MinimumValue { get; }
    }
    public partial interface IRangeOperation : Microsoft.CodeAnalysis.IOperation
    {
        bool IsLifted { get; }
        Microsoft.CodeAnalysis.IOperation? LeftOperand { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? Method { get; }
        Microsoft.CodeAnalysis.IOperation? RightOperand { get; }
    }
    public partial interface IRecursivePatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
        Microsoft.CodeAnalysis.ISymbol? DeclaredSymbol { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IPatternOperation> DeconstructionSubpatterns { get; }
        Microsoft.CodeAnalysis.ISymbol? DeconstructSymbol { get; }
        Microsoft.CodeAnalysis.ITypeSymbol MatchedType { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IPropertySubpatternOperation> PropertySubpatterns { get; }
    }
    public partial interface IReDimClauseOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> DimensionSizes { get; }
        Microsoft.CodeAnalysis.IOperation Operand { get; }
    }
    public partial interface IReDimOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IReDimClauseOperation> Clauses { get; }
        bool Preserve { get; }
    }
    public partial interface IRelationalCaseClauseOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ICaseClauseOperation
    {
        Microsoft.CodeAnalysis.Operations.BinaryOperatorKind Relation { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IRelationalPatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
        Microsoft.CodeAnalysis.Operations.BinaryOperatorKind OperatorKind { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IReturnOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation? ReturnedValue { get; }
    }
    public partial interface ISimpleAssignmentOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IAssignmentOperation
    {
        bool IsRef { get; }
    }
    public partial interface ISingleValueCaseClauseOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ICaseClauseOperation
    {
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface ISizeOfOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.ITypeSymbol TypeOperand { get; }
    }
    public partial interface IStopOperation : Microsoft.CodeAnalysis.IOperation
    {
    }
    public partial interface ISwitchCaseOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Body { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.ICaseClauseOperation> Clauses { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
    }
    public partial interface ISwitchExpressionArmOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation? Guard { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
        Microsoft.CodeAnalysis.Operations.IPatternOperation Pattern { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface ISwitchExpressionOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.ISwitchExpressionArmOperation> Arms { get; }
        bool IsExhaustive { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface ISwitchOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.ISwitchCaseOperation> Cases { get; }
        Microsoft.CodeAnalysis.ILabelSymbol ExitLabel { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface ISymbolInitializerOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
        Microsoft.CodeAnalysis.IOperation Value { get; }
    }
    public partial interface IThrowOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation? Exception { get; }
    }
    public partial interface ITranslatedQueryOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Operation { get; }
    }
    public partial interface ITryOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.IBlockOperation Body { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.ICatchClauseOperation> Catches { get; }
        Microsoft.CodeAnalysis.ILabelSymbol? ExitLabel { get; }
        Microsoft.CodeAnalysis.Operations.IBlockOperation? Finally { get; }
    }
    public partial interface ITupleBinaryOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation LeftOperand { get; }
        Microsoft.CodeAnalysis.Operations.BinaryOperatorKind OperatorKind { get; }
        Microsoft.CodeAnalysis.IOperation RightOperand { get; }
    }
    public partial interface ITupleOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> Elements { get; }
        Microsoft.CodeAnalysis.ITypeSymbol? NaturalType { get; }
    }
    public partial interface ITypeOfOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.ITypeSymbol TypeOperand { get; }
    }
    public partial interface ITypeParameterObjectCreationOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.IObjectOrCollectionInitializerOperation? Initializer { get; }
    }
    public partial interface ITypePatternOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.IPatternOperation
    {
        Microsoft.CodeAnalysis.ITypeSymbol MatchedType { get; }
    }
    public partial interface IUnaryOperation : Microsoft.CodeAnalysis.IOperation
    {
        bool IsChecked { get; }
        bool IsLifted { get; }
        Microsoft.CodeAnalysis.IOperation Operand { get; }
        Microsoft.CodeAnalysis.Operations.UnaryOperatorKind OperatorKind { get; }
        Microsoft.CodeAnalysis.IMethodSymbol? OperatorMethod { get; }
    }
    public partial interface IUsingDeclarationOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.Operations.IVariableDeclarationGroupOperation DeclarationGroup { get; }
        bool IsAsynchronous { get; }
    }
    public partial interface IUsingOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IOperation Body { get; }
        bool IsAsynchronous { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> Locals { get; }
        Microsoft.CodeAnalysis.IOperation Resources { get; }
    }
    public partial interface IVariableDeclarationGroupOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IVariableDeclarationOperation> Declarations { get; }
    }
    public partial interface IVariableDeclarationOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Operations.IVariableDeclaratorOperation> Declarators { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> IgnoredDimensions { get; }
        Microsoft.CodeAnalysis.Operations.IVariableInitializerOperation? Initializer { get; }
    }
    public partial interface IVariableDeclaratorOperation : Microsoft.CodeAnalysis.IOperation
    {
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.IOperation> IgnoredArguments { get; }
        Microsoft.CodeAnalysis.Operations.IVariableInitializerOperation? Initializer { get; }
        Microsoft.CodeAnalysis.ILocalSymbol Symbol { get; }
    }
    public partial interface IVariableInitializerOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ISymbolInitializerOperation
    {
    }
    public partial interface IWhileLoopOperation : Microsoft.CodeAnalysis.IOperation, Microsoft.CodeAnalysis.Operations.ILoopOperation
    {
        Microsoft.CodeAnalysis.IOperation? Condition { get; }
        bool ConditionIsTop { get; }
        bool ConditionIsUntil { get; }
        Microsoft.CodeAnalysis.IOperation? IgnoredCondition { get; }
    }
    public partial interface IWithOperation : Microsoft.CodeAnalysis.IOperation
    {
        Microsoft.CodeAnalysis.IMethodSymbol? CloneMethod { get; }
        Microsoft.CodeAnalysis.Operations.IObjectOrCollectionInitializerOperation Initializer { get; }
        Microsoft.CodeAnalysis.IOperation Operand { get; }
    }
    public enum LoopKind
    {
        None = 0,
        While = 1,
        For = 2,
        ForTo = 3,
        ForEach = 4,
    }
    public static partial class OperationExtensions
    {
        public static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.IOperation> Descendants(this Microsoft.CodeAnalysis.IOperation? operation) { throw null; }
        public static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.IOperation> DescendantsAndSelf(this Microsoft.CodeAnalysis.IOperation? operation) { throw null; }
        public static string? GetArgumentName(this Microsoft.CodeAnalysis.Operations.IDynamicIndexerAccessOperation dynamicOperation, int index) { throw null; }
        public static string? GetArgumentName(this Microsoft.CodeAnalysis.Operations.IDynamicInvocationOperation dynamicOperation, int index) { throw null; }
        public static string? GetArgumentName(this Microsoft.CodeAnalysis.Operations.IDynamicObjectCreationOperation dynamicOperation, int index) { throw null; }
        public static Microsoft.CodeAnalysis.RefKind? GetArgumentRefKind(this Microsoft.CodeAnalysis.Operations.IDynamicIndexerAccessOperation dynamicOperation, int index) { throw null; }
        public static Microsoft.CodeAnalysis.RefKind? GetArgumentRefKind(this Microsoft.CodeAnalysis.Operations.IDynamicInvocationOperation dynamicOperation, int index) { throw null; }
        public static Microsoft.CodeAnalysis.RefKind? GetArgumentRefKind(this Microsoft.CodeAnalysis.Operations.IDynamicObjectCreationOperation dynamicOperation, int index) { throw null; }
        public static Microsoft.CodeAnalysis.IOperation? GetCorrespondingOperation(this Microsoft.CodeAnalysis.Operations.IBranchOperation operation) { throw null; }
        public static System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> GetDeclaredVariables(this Microsoft.CodeAnalysis.Operations.IVariableDeclarationGroupOperation declarationGroup) { throw null; }
        public static System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ILocalSymbol> GetDeclaredVariables(this Microsoft.CodeAnalysis.Operations.IVariableDeclarationOperation declaration) { throw null; }
        public static Microsoft.CodeAnalysis.Operations.IVariableInitializerOperation? GetVariableInitializer(this Microsoft.CodeAnalysis.Operations.IVariableDeclaratorOperation declarationOperation) { throw null; }
    }
    public abstract partial class OperationVisitor
    {
        protected OperationVisitor() { }
        public virtual void DefaultVisit(Microsoft.CodeAnalysis.IOperation operation) { }
        public virtual void Visit(Microsoft.CodeAnalysis.IOperation? operation) { }
        public virtual void VisitAddressOf(Microsoft.CodeAnalysis.Operations.IAddressOfOperation operation) { }
        public virtual void VisitAnonymousFunction(Microsoft.CodeAnalysis.Operations.IAnonymousFunctionOperation operation) { }
        public virtual void VisitAnonymousObjectCreation(Microsoft.CodeAnalysis.Operations.IAnonymousObjectCreationOperation operation) { }
        public virtual void VisitArgument(Microsoft.CodeAnalysis.Operations.IArgumentOperation operation) { }
        public virtual void VisitArrayCreation(Microsoft.CodeAnalysis.Operations.IArrayCreationOperation operation) { }
        public virtual void VisitArrayElementReference(Microsoft.CodeAnalysis.Operations.IArrayElementReferenceOperation operation) { }
        public virtual void VisitArrayInitializer(Microsoft.CodeAnalysis.Operations.IArrayInitializerOperation operation) { }
        public virtual void VisitAwait(Microsoft.CodeAnalysis.Operations.IAwaitOperation operation) { }
        public virtual void VisitBinaryOperator(Microsoft.CodeAnalysis.Operations.IBinaryOperation operation) { }
        public virtual void VisitBinaryPattern(Microsoft.CodeAnalysis.Operations.IBinaryPatternOperation operation) { }
        public virtual void VisitBlock(Microsoft.CodeAnalysis.Operations.IBlockOperation operation) { }
        public virtual void VisitBranch(Microsoft.CodeAnalysis.Operations.IBranchOperation operation) { }
        public virtual void VisitCatchClause(Microsoft.CodeAnalysis.Operations.ICatchClauseOperation operation) { }
        public virtual void VisitCaughtException(Microsoft.CodeAnalysis.FlowAnalysis.ICaughtExceptionOperation operation) { }
        public virtual void VisitCoalesce(Microsoft.CodeAnalysis.Operations.ICoalesceOperation operation) { }
        public virtual void VisitCoalesceAssignment(Microsoft.CodeAnalysis.Operations.ICoalesceAssignmentOperation operation) { }
        [System.ObsoleteAttribute("ICollectionElementInitializerOperation has been replaced with IInvocationOperation and IDynamicInvocationOperation", true)]
        public virtual void VisitCollectionElementInitializer(Microsoft.CodeAnalysis.Operations.ICollectionElementInitializerOperation operation) { }
        public virtual void VisitCompoundAssignment(Microsoft.CodeAnalysis.Operations.ICompoundAssignmentOperation operation) { }
        public virtual void VisitConditional(Microsoft.CodeAnalysis.Operations.IConditionalOperation operation) { }
        public virtual void VisitConditionalAccess(Microsoft.CodeAnalysis.Operations.IConditionalAccessOperation operation) { }
        public virtual void VisitConditionalAccessInstance(Microsoft.CodeAnalysis.Operations.IConditionalAccessInstanceOperation operation) { }
        public virtual void VisitConstantPattern(Microsoft.CodeAnalysis.Operations.IConstantPatternOperation operation) { }
        public virtual void VisitConstructorBodyOperation(Microsoft.CodeAnalysis.Operations.IConstructorBodyOperation operation) { }
        public virtual void VisitConversion(Microsoft.CodeAnalysis.Operations.IConversionOperation operation) { }
        public virtual void VisitDeclarationExpression(Microsoft.CodeAnalysis.Operations.IDeclarationExpressionOperation operation) { }
        public virtual void VisitDeclarationPattern(Microsoft.CodeAnalysis.Operations.IDeclarationPatternOperation operation) { }
        public virtual void VisitDeconstructionAssignment(Microsoft.CodeAnalysis.Operations.IDeconstructionAssignmentOperation operation) { }
        public virtual void VisitDefaultCaseClause(Microsoft.CodeAnalysis.Operations.IDefaultCaseClauseOperation operation) { }
        public virtual void VisitDefaultValue(Microsoft.CodeAnalysis.Operations.IDefaultValueOperation operation) { }
        public virtual void VisitDelegateCreation(Microsoft.CodeAnalysis.Operations.IDelegateCreationOperation operation) { }
        public virtual void VisitDiscardOperation(Microsoft.CodeAnalysis.Operations.IDiscardOperation operation) { }
        public virtual void VisitDiscardPattern(Microsoft.CodeAnalysis.Operations.IDiscardPatternOperation operation) { }
        public virtual void VisitDynamicIndexerAccess(Microsoft.CodeAnalysis.Operations.IDynamicIndexerAccessOperation operation) { }
        public virtual void VisitDynamicInvocation(Microsoft.CodeAnalysis.Operations.IDynamicInvocationOperation operation) { }
        public virtual void VisitDynamicMemberReference(Microsoft.CodeAnalysis.Operations.IDynamicMemberReferenceOperation operation) { }
        public virtual void VisitDynamicObjectCreation(Microsoft.CodeAnalysis.Operations.IDynamicObjectCreationOperation operation) { }
        public virtual void VisitEmpty(Microsoft.CodeAnalysis.Operations.IEmptyOperation operation) { }
        public virtual void VisitEnd(Microsoft.CodeAnalysis.Operations.IEndOperation operation) { }
        public virtual void VisitEventAssignment(Microsoft.CodeAnalysis.Operations.IEventAssignmentOperation operation) { }
        public virtual void VisitEventReference(Microsoft.CodeAnalysis.Operations.IEventReferenceOperation operation) { }
        public virtual void VisitExpressionStatement(Microsoft.CodeAnalysis.Operations.IExpressionStatementOperation operation) { }
        public virtual void VisitFieldInitializer(Microsoft.CodeAnalysis.Operations.IFieldInitializerOperation operation) { }
        public virtual void VisitFieldReference(Microsoft.CodeAnalysis.Operations.IFieldReferenceOperation operation) { }
        public virtual void VisitFlowAnonymousFunction(Microsoft.CodeAnalysis.FlowAnalysis.IFlowAnonymousFunctionOperation operation) { }
        public virtual void VisitFlowCapture(Microsoft.CodeAnalysis.FlowAnalysis.IFlowCaptureOperation operation) { }
        public virtual void VisitFlowCaptureReference(Microsoft.CodeAnalysis.FlowAnalysis.IFlowCaptureReferenceOperation operation) { }
        public virtual void VisitForEachLoop(Microsoft.CodeAnalysis.Operations.IForEachLoopOperation operation) { }
        public virtual void VisitForLoop(Microsoft.CodeAnalysis.Operations.IForLoopOperation operation) { }
        public virtual void VisitForToLoop(Microsoft.CodeAnalysis.Operations.IForToLoopOperation operation) { }
        public virtual void VisitIncrementOrDecrement(Microsoft.CodeAnalysis.Operations.IIncrementOrDecrementOperation operation) { }
        public virtual void VisitInstanceReference(Microsoft.CodeAnalysis.Operations.IInstanceReferenceOperation operation) { }
        public virtual void VisitInterpolatedString(Microsoft.CodeAnalysis.Operations.IInterpolatedStringOperation operation) { }
        public virtual void VisitInterpolatedStringText(Microsoft.CodeAnalysis.Operations.IInterpolatedStringTextOperation operation) { }
        public virtual void VisitInterpolation(Microsoft.CodeAnalysis.Operations.IInterpolationOperation operation) { }
        public virtual void VisitInvalid(Microsoft.CodeAnalysis.Operations.IInvalidOperation operation) { }
        public virtual void VisitInvocation(Microsoft.CodeAnalysis.Operations.IInvocationOperation operation) { }
        public virtual void VisitIsNull(Microsoft.CodeAnalysis.FlowAnalysis.IIsNullOperation operation) { }
        public virtual void VisitIsPattern(Microsoft.CodeAnalysis.Operations.IIsPatternOperation operation) { }
        public virtual void VisitIsType(Microsoft.CodeAnalysis.Operations.IIsTypeOperation operation) { }
        public virtual void VisitLabeled(Microsoft.CodeAnalysis.Operations.ILabeledOperation operation) { }
        public virtual void VisitLiteral(Microsoft.CodeAnalysis.Operations.ILiteralOperation operation) { }
        public virtual void VisitLocalFunction(Microsoft.CodeAnalysis.Operations.ILocalFunctionOperation operation) { }
        public virtual void VisitLocalReference(Microsoft.CodeAnalysis.Operations.ILocalReferenceOperation operation) { }
        public virtual void VisitLock(Microsoft.CodeAnalysis.Operations.ILockOperation operation) { }
        public virtual void VisitMemberInitializer(Microsoft.CodeAnalysis.Operations.IMemberInitializerOperation operation) { }
        public virtual void VisitMethodBodyOperation(Microsoft.CodeAnalysis.Operations.IMethodBodyOperation operation) { }
        public virtual void VisitMethodReference(Microsoft.CodeAnalysis.Operations.IMethodReferenceOperation operation) { }
        public virtual void VisitNameOf(Microsoft.CodeAnalysis.Operations.INameOfOperation operation) { }
        public virtual void VisitNegatedPattern(Microsoft.CodeAnalysis.Operations.INegatedPatternOperation operation) { }
        public virtual void VisitObjectCreation(Microsoft.CodeAnalysis.Operations.IObjectCreationOperation operation) { }
        public virtual void VisitObjectOrCollectionInitializer(Microsoft.CodeAnalysis.Operations.IObjectOrCollectionInitializerOperation operation) { }
        public virtual void VisitOmittedArgument(Microsoft.CodeAnalysis.Operations.IOmittedArgumentOperation operation) { }
        public virtual void VisitParameterInitializer(Microsoft.CodeAnalysis.Operations.IParameterInitializerOperation operation) { }
        public virtual void VisitParameterReference(Microsoft.CodeAnalysis.Operations.IParameterReferenceOperation operation) { }
        public virtual void VisitParenthesized(Microsoft.CodeAnalysis.Operations.IParenthesizedOperation operation) { }
        public virtual void VisitPatternCaseClause(Microsoft.CodeAnalysis.Operations.IPatternCaseClauseOperation operation) { }
        public virtual void VisitPropertyInitializer(Microsoft.CodeAnalysis.Operations.IPropertyInitializerOperation operation) { }
        public virtual void VisitPropertyReference(Microsoft.CodeAnalysis.Operations.IPropertyReferenceOperation operation) { }
        public virtual void VisitPropertySubpattern(Microsoft.CodeAnalysis.Operations.IPropertySubpatternOperation operation) { }
        public virtual void VisitRaiseEvent(Microsoft.CodeAnalysis.Operations.IRaiseEventOperation operation) { }
        public virtual void VisitRangeCaseClause(Microsoft.CodeAnalysis.Operations.IRangeCaseClauseOperation operation) { }
        public virtual void VisitRangeOperation(Microsoft.CodeAnalysis.Operations.IRangeOperation operation) { }
        public virtual void VisitRecursivePattern(Microsoft.CodeAnalysis.Operations.IRecursivePatternOperation operation) { }
        public virtual void VisitReDim(Microsoft.CodeAnalysis.Operations.IReDimOperation operation) { }
        public virtual void VisitReDimClause(Microsoft.CodeAnalysis.Operations.IReDimClauseOperation operation) { }
        public virtual void VisitRelationalCaseClause(Microsoft.CodeAnalysis.Operations.IRelationalCaseClauseOperation operation) { }
        public virtual void VisitRelationalPattern(Microsoft.CodeAnalysis.Operations.IRelationalPatternOperation operation) { }
        public virtual void VisitReturn(Microsoft.CodeAnalysis.Operations.IReturnOperation operation) { }
        public virtual void VisitSimpleAssignment(Microsoft.CodeAnalysis.Operations.ISimpleAssignmentOperation operation) { }
        public virtual void VisitSingleValueCaseClause(Microsoft.CodeAnalysis.Operations.ISingleValueCaseClauseOperation operation) { }
        public virtual void VisitSizeOf(Microsoft.CodeAnalysis.Operations.ISizeOfOperation operation) { }
        public virtual void VisitStaticLocalInitializationSemaphore(Microsoft.CodeAnalysis.FlowAnalysis.IStaticLocalInitializationSemaphoreOperation operation) { }
        public virtual void VisitStop(Microsoft.CodeAnalysis.Operations.IStopOperation operation) { }
        public virtual void VisitSwitch(Microsoft.CodeAnalysis.Operations.ISwitchOperation operation) { }
        public virtual void VisitSwitchCase(Microsoft.CodeAnalysis.Operations.ISwitchCaseOperation operation) { }
        public virtual void VisitSwitchExpression(Microsoft.CodeAnalysis.Operations.ISwitchExpressionOperation operation) { }
        public virtual void VisitSwitchExpressionArm(Microsoft.CodeAnalysis.Operations.ISwitchExpressionArmOperation operation) { }
        public virtual void VisitThrow(Microsoft.CodeAnalysis.Operations.IThrowOperation operation) { }
        public virtual void VisitTranslatedQuery(Microsoft.CodeAnalysis.Operations.ITranslatedQueryOperation operation) { }
        public virtual void VisitTry(Microsoft.CodeAnalysis.Operations.ITryOperation operation) { }
        public virtual void VisitTuple(Microsoft.CodeAnalysis.Operations.ITupleOperation operation) { }
        public virtual void VisitTupleBinaryOperator(Microsoft.CodeAnalysis.Operations.ITupleBinaryOperation operation) { }
        public virtual void VisitTypeOf(Microsoft.CodeAnalysis.Operations.ITypeOfOperation operation) { }
        public virtual void VisitTypeParameterObjectCreation(Microsoft.CodeAnalysis.Operations.ITypeParameterObjectCreationOperation operation) { }
        public virtual void VisitTypePattern(Microsoft.CodeAnalysis.Operations.ITypePatternOperation operation) { }
        public virtual void VisitUnaryOperator(Microsoft.CodeAnalysis.Operations.IUnaryOperation operation) { }
        public virtual void VisitUsing(Microsoft.CodeAnalysis.Operations.IUsingOperation operation) { }
        public virtual void VisitUsingDeclaration(Microsoft.CodeAnalysis.Operations.IUsingDeclarationOperation operation) { }
        public virtual void VisitVariableDeclaration(Microsoft.CodeAnalysis.Operations.IVariableDeclarationOperation operation) { }
        public virtual void VisitVariableDeclarationGroup(Microsoft.CodeAnalysis.Operations.IVariableDeclarationGroupOperation operation) { }
        public virtual void VisitVariableDeclarator(Microsoft.CodeAnalysis.Operations.IVariableDeclaratorOperation operation) { }
        public virtual void VisitVariableInitializer(Microsoft.CodeAnalysis.Operations.IVariableInitializerOperation operation) { }
        public virtual void VisitWhileLoop(Microsoft.CodeAnalysis.Operations.IWhileLoopOperation operation) { }
        public virtual void VisitWith(Microsoft.CodeAnalysis.Operations.IWithOperation operation) { }
    }
    public abstract partial class OperationVisitor<TArgument, TResult>
    {
        protected OperationVisitor() { }
        public virtual TResult? DefaultVisit(Microsoft.CodeAnalysis.IOperation operation, TArgument argument) { throw null; }
        public virtual TResult? Visit(Microsoft.CodeAnalysis.IOperation? operation, TArgument argument) { throw null; }
        public virtual TResult? VisitAddressOf(Microsoft.CodeAnalysis.Operations.IAddressOfOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitAnonymousFunction(Microsoft.CodeAnalysis.Operations.IAnonymousFunctionOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitAnonymousObjectCreation(Microsoft.CodeAnalysis.Operations.IAnonymousObjectCreationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitArgument(Microsoft.CodeAnalysis.Operations.IArgumentOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitArrayCreation(Microsoft.CodeAnalysis.Operations.IArrayCreationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitArrayElementReference(Microsoft.CodeAnalysis.Operations.IArrayElementReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitArrayInitializer(Microsoft.CodeAnalysis.Operations.IArrayInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitAwait(Microsoft.CodeAnalysis.Operations.IAwaitOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitBinaryOperator(Microsoft.CodeAnalysis.Operations.IBinaryOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitBinaryPattern(Microsoft.CodeAnalysis.Operations.IBinaryPatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitBlock(Microsoft.CodeAnalysis.Operations.IBlockOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitBranch(Microsoft.CodeAnalysis.Operations.IBranchOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitCatchClause(Microsoft.CodeAnalysis.Operations.ICatchClauseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitCaughtException(Microsoft.CodeAnalysis.FlowAnalysis.ICaughtExceptionOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitCoalesce(Microsoft.CodeAnalysis.Operations.ICoalesceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitCoalesceAssignment(Microsoft.CodeAnalysis.Operations.ICoalesceAssignmentOperation operation, TArgument argument) { throw null; }
        [System.ObsoleteAttribute("ICollectionElementInitializerOperation has been replaced with IInvocationOperation and IDynamicInvocationOperation", true)]
        public virtual TResult? VisitCollectionElementInitializer(Microsoft.CodeAnalysis.Operations.ICollectionElementInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitCompoundAssignment(Microsoft.CodeAnalysis.Operations.ICompoundAssignmentOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitConditional(Microsoft.CodeAnalysis.Operations.IConditionalOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitConditionalAccess(Microsoft.CodeAnalysis.Operations.IConditionalAccessOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitConditionalAccessInstance(Microsoft.CodeAnalysis.Operations.IConditionalAccessInstanceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitConstantPattern(Microsoft.CodeAnalysis.Operations.IConstantPatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitConstructorBodyOperation(Microsoft.CodeAnalysis.Operations.IConstructorBodyOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitConversion(Microsoft.CodeAnalysis.Operations.IConversionOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDeclarationExpression(Microsoft.CodeAnalysis.Operations.IDeclarationExpressionOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDeclarationPattern(Microsoft.CodeAnalysis.Operations.IDeclarationPatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDeconstructionAssignment(Microsoft.CodeAnalysis.Operations.IDeconstructionAssignmentOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDefaultCaseClause(Microsoft.CodeAnalysis.Operations.IDefaultCaseClauseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDefaultValue(Microsoft.CodeAnalysis.Operations.IDefaultValueOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDelegateCreation(Microsoft.CodeAnalysis.Operations.IDelegateCreationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDiscardOperation(Microsoft.CodeAnalysis.Operations.IDiscardOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDiscardPattern(Microsoft.CodeAnalysis.Operations.IDiscardPatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDynamicIndexerAccess(Microsoft.CodeAnalysis.Operations.IDynamicIndexerAccessOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDynamicInvocation(Microsoft.CodeAnalysis.Operations.IDynamicInvocationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDynamicMemberReference(Microsoft.CodeAnalysis.Operations.IDynamicMemberReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitDynamicObjectCreation(Microsoft.CodeAnalysis.Operations.IDynamicObjectCreationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitEmpty(Microsoft.CodeAnalysis.Operations.IEmptyOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitEnd(Microsoft.CodeAnalysis.Operations.IEndOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitEventAssignment(Microsoft.CodeAnalysis.Operations.IEventAssignmentOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitEventReference(Microsoft.CodeAnalysis.Operations.IEventReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitExpressionStatement(Microsoft.CodeAnalysis.Operations.IExpressionStatementOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitFieldInitializer(Microsoft.CodeAnalysis.Operations.IFieldInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitFieldReference(Microsoft.CodeAnalysis.Operations.IFieldReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitFlowAnonymousFunction(Microsoft.CodeAnalysis.FlowAnalysis.IFlowAnonymousFunctionOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitFlowCapture(Microsoft.CodeAnalysis.FlowAnalysis.IFlowCaptureOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitFlowCaptureReference(Microsoft.CodeAnalysis.FlowAnalysis.IFlowCaptureReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitForEachLoop(Microsoft.CodeAnalysis.Operations.IForEachLoopOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitForLoop(Microsoft.CodeAnalysis.Operations.IForLoopOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitForToLoop(Microsoft.CodeAnalysis.Operations.IForToLoopOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitIncrementOrDecrement(Microsoft.CodeAnalysis.Operations.IIncrementOrDecrementOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitInstanceReference(Microsoft.CodeAnalysis.Operations.IInstanceReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitInterpolatedString(Microsoft.CodeAnalysis.Operations.IInterpolatedStringOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitInterpolatedStringText(Microsoft.CodeAnalysis.Operations.IInterpolatedStringTextOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitInterpolation(Microsoft.CodeAnalysis.Operations.IInterpolationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitInvalid(Microsoft.CodeAnalysis.Operations.IInvalidOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitInvocation(Microsoft.CodeAnalysis.Operations.IInvocationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitIsNull(Microsoft.CodeAnalysis.FlowAnalysis.IIsNullOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitIsPattern(Microsoft.CodeAnalysis.Operations.IIsPatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitIsType(Microsoft.CodeAnalysis.Operations.IIsTypeOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitLabeled(Microsoft.CodeAnalysis.Operations.ILabeledOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitLiteral(Microsoft.CodeAnalysis.Operations.ILiteralOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitLocalFunction(Microsoft.CodeAnalysis.Operations.ILocalFunctionOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitLocalReference(Microsoft.CodeAnalysis.Operations.ILocalReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitLock(Microsoft.CodeAnalysis.Operations.ILockOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitMemberInitializer(Microsoft.CodeAnalysis.Operations.IMemberInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitMethodBodyOperation(Microsoft.CodeAnalysis.Operations.IMethodBodyOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitMethodReference(Microsoft.CodeAnalysis.Operations.IMethodReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitNameOf(Microsoft.CodeAnalysis.Operations.INameOfOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitNegatedPattern(Microsoft.CodeAnalysis.Operations.INegatedPatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitObjectCreation(Microsoft.CodeAnalysis.Operations.IObjectCreationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitObjectOrCollectionInitializer(Microsoft.CodeAnalysis.Operations.IObjectOrCollectionInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitOmittedArgument(Microsoft.CodeAnalysis.Operations.IOmittedArgumentOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitParameterInitializer(Microsoft.CodeAnalysis.Operations.IParameterInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitParameterReference(Microsoft.CodeAnalysis.Operations.IParameterReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitParenthesized(Microsoft.CodeAnalysis.Operations.IParenthesizedOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitPatternCaseClause(Microsoft.CodeAnalysis.Operations.IPatternCaseClauseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitPropertyInitializer(Microsoft.CodeAnalysis.Operations.IPropertyInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitPropertyReference(Microsoft.CodeAnalysis.Operations.IPropertyReferenceOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitPropertySubpattern(Microsoft.CodeAnalysis.Operations.IPropertySubpatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitRaiseEvent(Microsoft.CodeAnalysis.Operations.IRaiseEventOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitRangeCaseClause(Microsoft.CodeAnalysis.Operations.IRangeCaseClauseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitRangeOperation(Microsoft.CodeAnalysis.Operations.IRangeOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitRecursivePattern(Microsoft.CodeAnalysis.Operations.IRecursivePatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitReDim(Microsoft.CodeAnalysis.Operations.IReDimOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitReDimClause(Microsoft.CodeAnalysis.Operations.IReDimClauseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitRelationalCaseClause(Microsoft.CodeAnalysis.Operations.IRelationalCaseClauseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitRelationalPattern(Microsoft.CodeAnalysis.Operations.IRelationalPatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitReturn(Microsoft.CodeAnalysis.Operations.IReturnOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitSimpleAssignment(Microsoft.CodeAnalysis.Operations.ISimpleAssignmentOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitSingleValueCaseClause(Microsoft.CodeAnalysis.Operations.ISingleValueCaseClauseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitSizeOf(Microsoft.CodeAnalysis.Operations.ISizeOfOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitStaticLocalInitializationSemaphore(Microsoft.CodeAnalysis.FlowAnalysis.IStaticLocalInitializationSemaphoreOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitStop(Microsoft.CodeAnalysis.Operations.IStopOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitSwitch(Microsoft.CodeAnalysis.Operations.ISwitchOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitSwitchCase(Microsoft.CodeAnalysis.Operations.ISwitchCaseOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitSwitchExpression(Microsoft.CodeAnalysis.Operations.ISwitchExpressionOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitSwitchExpressionArm(Microsoft.CodeAnalysis.Operations.ISwitchExpressionArmOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitThrow(Microsoft.CodeAnalysis.Operations.IThrowOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitTranslatedQuery(Microsoft.CodeAnalysis.Operations.ITranslatedQueryOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitTry(Microsoft.CodeAnalysis.Operations.ITryOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitTuple(Microsoft.CodeAnalysis.Operations.ITupleOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitTupleBinaryOperator(Microsoft.CodeAnalysis.Operations.ITupleBinaryOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitTypeOf(Microsoft.CodeAnalysis.Operations.ITypeOfOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitTypeParameterObjectCreation(Microsoft.CodeAnalysis.Operations.ITypeParameterObjectCreationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitTypePattern(Microsoft.CodeAnalysis.Operations.ITypePatternOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitUnaryOperator(Microsoft.CodeAnalysis.Operations.IUnaryOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitUsing(Microsoft.CodeAnalysis.Operations.IUsingOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitUsingDeclaration(Microsoft.CodeAnalysis.Operations.IUsingDeclarationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitVariableDeclaration(Microsoft.CodeAnalysis.Operations.IVariableDeclarationOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitVariableDeclarationGroup(Microsoft.CodeAnalysis.Operations.IVariableDeclarationGroupOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitVariableDeclarator(Microsoft.CodeAnalysis.Operations.IVariableDeclaratorOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitVariableInitializer(Microsoft.CodeAnalysis.Operations.IVariableInitializerOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitWhileLoop(Microsoft.CodeAnalysis.Operations.IWhileLoopOperation operation, TArgument argument) { throw null; }
        public virtual TResult? VisitWith(Microsoft.CodeAnalysis.Operations.IWithOperation operation, TArgument argument) { throw null; }
    }
    public abstract partial class OperationWalker : Microsoft.CodeAnalysis.Operations.OperationVisitor
    {
        protected OperationWalker() { }
        public override void DefaultVisit(Microsoft.CodeAnalysis.IOperation operation) { }
        public override void Visit(Microsoft.CodeAnalysis.IOperation? operation) { }
    }
    public abstract partial class OperationWalker<TArgument> : Microsoft.CodeAnalysis.Operations.OperationVisitor<TArgument, object?>
    {
        protected OperationWalker() { }
        public override object? DefaultVisit(Microsoft.CodeAnalysis.IOperation operation, TArgument argument) { throw null; }
        public override object? Visit(Microsoft.CodeAnalysis.IOperation? operation, TArgument argument) { throw null; }
    }
    public enum UnaryOperatorKind
    {
        None = 0,
        BitwiseNegation = 1,
        Not = 2,
        Plus = 3,
        Minus = 4,
        True = 5,
        False = 6,
        Hat = 7,
    }
}
namespace Microsoft.CodeAnalysis.Text
{
    [System.Runtime.Serialization.DataContractAttribute]
    public readonly partial struct LinePosition : System.IComparable<Microsoft.CodeAnalysis.Text.LinePosition>, System.IEquatable<Microsoft.CodeAnalysis.Text.LinePosition>
    {
        private readonly int _dummyPrimitive;
        public LinePosition(int line, int character) { throw null; }
        public int Character { get { throw null; } }
        public int Line { get { throw null; } }
        public static Microsoft.CodeAnalysis.Text.LinePosition Zero { get { throw null; } }
        public int CompareTo(Microsoft.CodeAnalysis.Text.LinePosition other) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.Text.LinePosition other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Text.LinePosition left, Microsoft.CodeAnalysis.Text.LinePosition right) { throw null; }
        public static bool operator >(Microsoft.CodeAnalysis.Text.LinePosition left, Microsoft.CodeAnalysis.Text.LinePosition right) { throw null; }
        public static bool operator >=(Microsoft.CodeAnalysis.Text.LinePosition left, Microsoft.CodeAnalysis.Text.LinePosition right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Text.LinePosition left, Microsoft.CodeAnalysis.Text.LinePosition right) { throw null; }
        public static bool operator <(Microsoft.CodeAnalysis.Text.LinePosition left, Microsoft.CodeAnalysis.Text.LinePosition right) { throw null; }
        public static bool operator <=(Microsoft.CodeAnalysis.Text.LinePosition left, Microsoft.CodeAnalysis.Text.LinePosition right) { throw null; }
        public override string ToString() { throw null; }
    }
    [System.Runtime.Serialization.DataContractAttribute]
    public readonly partial struct LinePositionSpan : System.IEquatable<Microsoft.CodeAnalysis.Text.LinePositionSpan>
    {
        private readonly int _dummyPrimitive;
        public LinePositionSpan(Microsoft.CodeAnalysis.Text.LinePosition start, Microsoft.CodeAnalysis.Text.LinePosition end) { throw null; }
        public Microsoft.CodeAnalysis.Text.LinePosition End { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.LinePosition Start { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Text.LinePositionSpan other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Text.LinePositionSpan left, Microsoft.CodeAnalysis.Text.LinePositionSpan right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Text.LinePositionSpan left, Microsoft.CodeAnalysis.Text.LinePositionSpan right) { throw null; }
        public override string ToString() { throw null; }
    }
    public enum SourceHashAlgorithm
    {
        None = 0,
        Sha1 = 1,
        Sha256 = 2,
    }
    public abstract partial class SourceText
    {
        protected SourceText(System.Collections.Immutable.ImmutableArray<byte> checksum = default(System.Collections.Immutable.ImmutableArray<byte>), Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1, Microsoft.CodeAnalysis.Text.SourceTextContainer? container = null) { }
        public bool CanBeEmbedded { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceHashAlgorithm ChecksumAlgorithm { get { throw null; } }
        public virtual Microsoft.CodeAnalysis.Text.SourceTextContainer Container { get { throw null; } }
        public abstract System.Text.Encoding? Encoding { get; }
        public abstract char this[int position] { get; }
        public abstract int Length { get; }
        public Microsoft.CodeAnalysis.Text.TextLineCollection Lines { get { throw null; } }
        public bool ContentEquals(Microsoft.CodeAnalysis.Text.SourceText other) { throw null; }
        protected virtual bool ContentEqualsImpl(Microsoft.CodeAnalysis.Text.SourceText other) { throw null; }
        public abstract void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count);
        public static Microsoft.CodeAnalysis.Text.SourceText From(byte[] buffer, int length, System.Text.Encoding? encoding, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm, bool throwIfBinaryDetected) { throw null; }
        public static Microsoft.CodeAnalysis.Text.SourceText From(byte[] buffer, int length, System.Text.Encoding? encoding = null, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1, bool throwIfBinaryDetected = false, bool canBeEmbedded = false) { throw null; }
        public static Microsoft.CodeAnalysis.Text.SourceText From(System.IO.Stream stream, System.Text.Encoding? encoding, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm, bool throwIfBinaryDetected) { throw null; }
        public static Microsoft.CodeAnalysis.Text.SourceText From(System.IO.Stream stream, System.Text.Encoding? encoding = null, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1, bool throwIfBinaryDetected = false, bool canBeEmbedded = false) { throw null; }
        public static Microsoft.CodeAnalysis.Text.SourceText From(System.IO.TextReader reader, int length, System.Text.Encoding? encoding = null, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1) { throw null; }
        public static Microsoft.CodeAnalysis.Text.SourceText From(string text, System.Text.Encoding? encoding = null, Microsoft.CodeAnalysis.Text.SourceHashAlgorithm checksumAlgorithm = Microsoft.CodeAnalysis.Text.SourceHashAlgorithm.Sha1) { throw null; }
        public virtual System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Text.TextChangeRange> GetChangeRanges(Microsoft.CodeAnalysis.Text.SourceText oldText) { throw null; }
        public System.Collections.Immutable.ImmutableArray<byte> GetChecksum() { throw null; }
        protected virtual Microsoft.CodeAnalysis.Text.TextLineCollection GetLinesCore() { throw null; }
        public virtual Microsoft.CodeAnalysis.Text.SourceText GetSubText(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public Microsoft.CodeAnalysis.Text.SourceText GetSubText(int start) { throw null; }
        public virtual System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Text.TextChange> GetTextChanges(Microsoft.CodeAnalysis.Text.SourceText oldText) { throw null; }
        public Microsoft.CodeAnalysis.Text.SourceText Replace(Microsoft.CodeAnalysis.Text.TextSpan span, string newText) { throw null; }
        public Microsoft.CodeAnalysis.Text.SourceText Replace(int start, int length, string newText) { throw null; }
        public override string ToString() { throw null; }
        public virtual string ToString(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public Microsoft.CodeAnalysis.Text.SourceText WithChanges(params Microsoft.CodeAnalysis.Text.TextChange[] changes) { throw null; }
        public virtual Microsoft.CodeAnalysis.Text.SourceText WithChanges(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextChange> changes) { throw null; }
        public virtual void Write(System.IO.TextWriter writer, Microsoft.CodeAnalysis.Text.TextSpan span, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { }
        public void Write(System.IO.TextWriter textWriter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { }
    }
    public abstract partial class SourceTextContainer
    {
        protected SourceTextContainer() { }
        public abstract Microsoft.CodeAnalysis.Text.SourceText CurrentText { get; }
        public abstract event System.EventHandler<Microsoft.CodeAnalysis.Text.TextChangeEventArgs> TextChanged;
    }
    [System.Runtime.Serialization.DataContractAttribute]
    public readonly partial struct TextChange : System.IEquatable<Microsoft.CodeAnalysis.Text.TextChange>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public TextChange(Microsoft.CodeAnalysis.Text.TextSpan span, string newText) { throw null; }
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public string? NewText { get { throw null; } }
        public static System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Text.TextChange> NoChanges { get { throw null; } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Text.TextChange other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Text.TextChange left, Microsoft.CodeAnalysis.Text.TextChange right) { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.Text.TextChangeRange (Microsoft.CodeAnalysis.Text.TextChange change) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Text.TextChange left, Microsoft.CodeAnalysis.Text.TextChange right) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class TextChangeEventArgs : System.EventArgs
    {
        public TextChangeEventArgs(Microsoft.CodeAnalysis.Text.SourceText oldText, Microsoft.CodeAnalysis.Text.SourceText newText, params Microsoft.CodeAnalysis.Text.TextChangeRange[] changes) { }
        public TextChangeEventArgs(Microsoft.CodeAnalysis.Text.SourceText oldText, Microsoft.CodeAnalysis.Text.SourceText newText, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextChangeRange> changes) { }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Text.TextChangeRange> Changes { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceText NewText { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceText OldText { get { throw null; } }
    }
    public readonly partial struct TextChangeRange : System.IEquatable<Microsoft.CodeAnalysis.Text.TextChangeRange>
    {
        private readonly int _dummyPrimitive;
        public TextChangeRange(Microsoft.CodeAnalysis.Text.TextSpan span, int newLength) { throw null; }
        public int NewLength { get { throw null; } }
        public static System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Text.TextChangeRange> NoChanges { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public static Microsoft.CodeAnalysis.Text.TextChangeRange Collapse(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextChangeRange> changes) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.Text.TextChangeRange other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Text.TextChangeRange left, Microsoft.CodeAnalysis.Text.TextChangeRange right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Text.TextChangeRange left, Microsoft.CodeAnalysis.Text.TextChangeRange right) { throw null; }
        public override string ToString() { throw null; }
    }
    public readonly partial struct TextLine : System.IEquatable<Microsoft.CodeAnalysis.Text.TextLine>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int End { get { throw null; } }
        public int EndIncludingLineBreak { get { throw null; } }
        public int LineNumber { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan SpanIncludingLineBreak { get { throw null; } }
        public int Start { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceText? Text { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Text.TextLine other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public static Microsoft.CodeAnalysis.Text.TextLine FromSpan(Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Text.TextLine left, Microsoft.CodeAnalysis.Text.TextLine right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Text.TextLine left, Microsoft.CodeAnalysis.Text.TextLine right) { throw null; }
        public override string ToString() { throw null; }
    }
    public abstract partial class TextLineCollection : System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextLine>, System.Collections.Generic.IReadOnlyCollection<Microsoft.CodeAnalysis.Text.TextLine>, System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Text.TextLine>, System.Collections.IEnumerable
    {
        protected TextLineCollection() { }
        public abstract int Count { get; }
        public abstract Microsoft.CodeAnalysis.Text.TextLine this[int index] { get; }
        public Microsoft.CodeAnalysis.Text.TextLineCollection.Enumerator GetEnumerator() { throw null; }
        public virtual Microsoft.CodeAnalysis.Text.TextLine GetLineFromPosition(int position) { throw null; }
        public virtual Microsoft.CodeAnalysis.Text.LinePosition GetLinePosition(int position) { throw null; }
        public Microsoft.CodeAnalysis.Text.LinePositionSpan GetLinePositionSpan(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public int GetPosition(Microsoft.CodeAnalysis.Text.LinePosition position) { throw null; }
        public Microsoft.CodeAnalysis.Text.TextSpan GetTextSpan(Microsoft.CodeAnalysis.Text.LinePositionSpan span) { throw null; }
        public abstract int IndexOf(int position);
        System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.Text.TextLine> System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextLine>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<Microsoft.CodeAnalysis.Text.TextLine>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Microsoft.CodeAnalysis.Text.TextLine Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public override bool Equals(object? obj) { throw null; }
            public override int GetHashCode() { throw null; }
            public bool MoveNext() { throw null; }
            bool System.Collections.IEnumerator.MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    [System.Runtime.Serialization.DataContractAttribute]
    public readonly partial struct TextSpan : System.IComparable<Microsoft.CodeAnalysis.Text.TextSpan>, System.IEquatable<Microsoft.CodeAnalysis.Text.TextSpan>
    {
        private readonly int _dummyPrimitive;
        public TextSpan(int start, int length) { throw null; }
        public int End { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=1)]
        public int Length { get { throw null; } }
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int Start { get { throw null; } }
        public int CompareTo(Microsoft.CodeAnalysis.Text.TextSpan other) { throw null; }
        public bool Contains(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public bool Contains(int position) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.Text.TextSpan other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public static Microsoft.CodeAnalysis.Text.TextSpan FromBounds(int start, int end) { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.Text.TextSpan? Intersection(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public bool IntersectsWith(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public bool IntersectsWith(int position) { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Text.TextSpan left, Microsoft.CodeAnalysis.Text.TextSpan right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Text.TextSpan left, Microsoft.CodeAnalysis.Text.TextSpan right) { throw null; }
        public Microsoft.CodeAnalysis.Text.TextSpan? Overlap(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public bool OverlapsWith(Microsoft.CodeAnalysis.Text.TextSpan span) { throw null; }
        public override string ToString() { throw null; }
    }
}


// The following code was manually created to workaround api generator limitations.
// --------------------------------------------------------------------------------

namespace Microsoft.CodeAnalysis
{
    internal readonly struct DeclarationInfo
    {
    }

    internal class CommonMessageProvider
    {
    }

    internal class StrongNameKeys
    {
    }

    internal class StrongNameFileSystem
    {
    }

    public partial class DesktopStrongNameProvider : Microsoft.CodeAnalysis.StrongNameProvider
    {
        internal override StrongNameFileSystem FileSystem { get => throw null; }
        internal override StrongNameKeys CreateKeys(string keyFilePath, string keyContainerName, bool hasCounterSignature, CommonMessageProvider messageProvider) { throw null; }
        internal override void SignFile(StrongNameKeys keys, string filePath) { }
        internal override void SignBuilder(Microsoft.Cci.ExtendedPEBuilder peBuilder, System.Reflection.Metadata.BlobBuilder peBlob, System.Security.Cryptography.RSAParameters privateKey) { }
        internal virtual Microsoft.CodeAnalysis.Interop.IClrStrongName GetStrongNameInterface() { throw null; }
    }

    public sealed partial class UnresolvedMetadataReference : Microsoft.CodeAnalysis.MetadataReference
    {
        internal override MetadataReference WithPropertiesImplReturningMetadataReference(MetadataReferenceProperties properties) { throw null; }
    }
}

namespace Microsoft.CodeAnalysis.Interop
{
    internal interface IClrStrongName
    {
    }
}

namespace Microsoft.Cci
{
    internal class ExtendedPEBuilder
    {
    }
}

namespace Microsoft.CodeAnalysis.PooledObjects
{
    internal struct ArrayBuilder<T> 
    {
    }
}
