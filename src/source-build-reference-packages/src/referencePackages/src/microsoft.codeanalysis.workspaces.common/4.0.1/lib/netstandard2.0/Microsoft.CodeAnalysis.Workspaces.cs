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
[assembly: AssemblyTitle("Microsoft.CodeAnalysis.Workspaces.Common")]
[assembly: AssemblyDescription("Microsoft.CodeAnalysis.Workspaces.Common")]
[assembly: AssemblyDefaultAlias("Microsoft.CodeAnalysis.Workspaces.Common")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.0.121.55815")]
[assembly: AssemblyInformationalVersion("4.0.121.55815 built by: SOURCEBUILD")]
[assembly: CLSCompliant(false)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace Microsoft.CodeAnalysis
{
    public sealed partial class AdditionalDocument : Microsoft.CodeAnalysis.TextDocument
    {
        internal AdditionalDocument() { }
    }
    public sealed partial class AdhocWorkspace : Microsoft.CodeAnalysis.Workspace
    {
        public AdhocWorkspace() : base (default(Microsoft.CodeAnalysis.Host.HostServices), default(string)) { }
        public AdhocWorkspace(Microsoft.CodeAnalysis.Host.HostServices host, string workspaceKind = "Custom") : base (default(Microsoft.CodeAnalysis.Host.HostServices), default(string)) { }
        public override bool CanOpenDocuments { get { throw null; } }
        public Microsoft.CodeAnalysis.Document AddDocument(Microsoft.CodeAnalysis.DocumentInfo documentInfo) { throw null; }
        public Microsoft.CodeAnalysis.Document AddDocument(Microsoft.CodeAnalysis.ProjectId projectId, string name, Microsoft.CodeAnalysis.Text.SourceText text) { throw null; }
        public Microsoft.CodeAnalysis.Project AddProject(Microsoft.CodeAnalysis.ProjectInfo projectInfo) { throw null; }
        public Microsoft.CodeAnalysis.Project AddProject(string name, string language) { throw null; }
        public void AddProjects(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectInfo> projectInfos) { }
        public Microsoft.CodeAnalysis.Solution AddSolution(Microsoft.CodeAnalysis.SolutionInfo solutionInfo) { throw null; }
        public override bool CanApplyChange(Microsoft.CodeAnalysis.ApplyChangesKind feature) { throw null; }
        public new void ClearSolution() { }
        public override void CloseAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId) { }
        public override void CloseAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId) { }
        public override void CloseDocument(Microsoft.CodeAnalysis.DocumentId documentId) { }
        public override void OpenAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId, bool activate = true) { }
        public override void OpenAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId, bool activate = true) { }
        public override void OpenDocument(Microsoft.CodeAnalysis.DocumentId documentId, bool activate = true) { }
    }
    public sealed partial class AnalyzerConfigDocument : Microsoft.CodeAnalysis.TextDocument
    {
        internal AnalyzerConfigDocument() { }
    }
    public enum ApplyChangesKind
    {
        AddProject = 0,
        RemoveProject = 1,
        AddProjectReference = 2,
        RemoveProjectReference = 3,
        AddMetadataReference = 4,
        RemoveMetadataReference = 5,
        AddDocument = 6,
        RemoveDocument = 7,
        ChangeDocument = 8,
        AddAnalyzerReference = 9,
        RemoveAnalyzerReference = 10,
        AddAdditionalDocument = 11,
        RemoveAdditionalDocument = 12,
        ChangeAdditionalDocument = 13,
        ChangeCompilationOptions = 14,
        ChangeParseOptions = 15,
        ChangeDocumentInfo = 16,
        AddAnalyzerConfigDocument = 17,
        RemoveAnalyzerConfigDocument = 18,
        ChangeAnalyzerConfigDocument = 19,
        AddSolutionAnalyzerReference = 20,
        RemoveSolutionAnalyzerReference = 21,
    }
    public static partial class CommandLineProject
    {
        public static Microsoft.CodeAnalysis.ProjectInfo CreateProjectInfo(string projectName, string language, System.Collections.Generic.IEnumerable<string> commandLineArgs, string projectDirectory, Microsoft.CodeAnalysis.Workspace workspace = null) { throw null; }
        public static Microsoft.CodeAnalysis.ProjectInfo CreateProjectInfo(string projectName, string language, string commandLine, string baseDirectory, Microsoft.CodeAnalysis.Workspace workspace = null) { throw null; }
    }
    public readonly partial struct CompilationOutputInfo : System.IEquatable<Microsoft.CodeAnalysis.CompilationOutputInfo>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public string? AssemblyPath { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.CompilationOutputInfo other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(in Microsoft.CodeAnalysis.CompilationOutputInfo left, in Microsoft.CodeAnalysis.CompilationOutputInfo right) { throw null; }
        public static bool operator !=(in Microsoft.CodeAnalysis.CompilationOutputInfo left, in Microsoft.CodeAnalysis.CompilationOutputInfo right) { throw null; }
        public Microsoft.CodeAnalysis.CompilationOutputInfo WithAssemblyPath(string? path) { throw null; }
    }
    public partial class Document : Microsoft.CodeAnalysis.TextDocument
    {
        internal Document() { }
        public Microsoft.CodeAnalysis.SourceCodeKind SourceCodeKind { get { throw null; } }
        public bool SupportsSemanticModel { get { throw null; } }
        public bool SupportsSyntaxTree { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> GetLinkedDocumentIds() { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Options.DocumentOptionSet> GetOptionsAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SemanticModel?> GetSemanticModelAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SyntaxNode?> GetSyntaxRootAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SyntaxTree?> GetSyntaxTreeAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.VersionStamp> GetSyntaxVersionAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextChange>> GetTextChangesAsync(Microsoft.CodeAnalysis.Document oldDocument, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public bool TryGetSemanticModel(out Microsoft.CodeAnalysis.SemanticModel? semanticModel) { throw null; }
        public bool TryGetSyntaxRoot(out Microsoft.CodeAnalysis.SyntaxNode? root) { throw null; }
        public bool TryGetSyntaxTree(out Microsoft.CodeAnalysis.SyntaxTree? syntaxTree) { throw null; }
        public bool TryGetSyntaxVersion(out Microsoft.CodeAnalysis.VersionStamp version) { throw null; }
        public Microsoft.CodeAnalysis.Document WithFilePath(string filePath) { throw null; }
        public Microsoft.CodeAnalysis.Document WithFolders(System.Collections.Generic.IEnumerable<string> folders) { throw null; }
        public Microsoft.CodeAnalysis.Document WithName(string name) { throw null; }
        public Microsoft.CodeAnalysis.Document WithSourceCodeKind(Microsoft.CodeAnalysis.SourceCodeKind kind) { throw null; }
        public Microsoft.CodeAnalysis.Document WithSyntaxRoot(Microsoft.CodeAnalysis.SyntaxNode root) { throw null; }
        public Microsoft.CodeAnalysis.Document WithText(Microsoft.CodeAnalysis.Text.SourceText text) { throw null; }
    }
    public sealed partial class DocumentActiveContextChangedEventArgs : System.EventArgs
    {
        public DocumentActiveContextChangedEventArgs(Microsoft.CodeAnalysis.Solution solution, Microsoft.CodeAnalysis.Text.SourceTextContainer sourceTextContainer, Microsoft.CodeAnalysis.DocumentId oldActiveContextDocumentId, Microsoft.CodeAnalysis.DocumentId newActiveContextDocumentId) { }
        public Microsoft.CodeAnalysis.DocumentId NewActiveContextDocumentId { get { throw null; } }
        public Microsoft.CodeAnalysis.DocumentId OldActiveContextDocumentId { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution Solution { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceTextContainer SourceTextContainer { get { throw null; } }
    }
    public partial class DocumentDiagnostic : Microsoft.CodeAnalysis.WorkspaceDiagnostic
    {
        public DocumentDiagnostic(Microsoft.CodeAnalysis.WorkspaceDiagnosticKind kind, string message, Microsoft.CodeAnalysis.DocumentId documentId) : base (default(Microsoft.CodeAnalysis.WorkspaceDiagnosticKind), default(string)) { }
        public Microsoft.CodeAnalysis.DocumentId DocumentId { get { throw null; } }
    }
    public partial class DocumentEventArgs : System.EventArgs
    {
        public DocumentEventArgs(Microsoft.CodeAnalysis.Document document) { }
        public Microsoft.CodeAnalysis.Document Document { get { throw null; } }
    }
    public sealed partial class DocumentId : System.IEquatable<Microsoft.CodeAnalysis.DocumentId>
    {
        internal DocumentId() { }
        public System.Guid Id { get { throw null; } }
        public Microsoft.CodeAnalysis.ProjectId ProjectId { get { throw null; } }
        public static Microsoft.CodeAnalysis.DocumentId CreateFromSerialized(Microsoft.CodeAnalysis.ProjectId projectId, System.Guid id, string? debugName = null) { throw null; }
        public static Microsoft.CodeAnalysis.DocumentId CreateNewId(Microsoft.CodeAnalysis.ProjectId projectId, string? debugName = null) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.DocumentId? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.DocumentId? left, Microsoft.CodeAnalysis.DocumentId? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.DocumentId? left, Microsoft.CodeAnalysis.DocumentId? right) { throw null; }
        public override string ToString() { throw null; }
    }
    public sealed partial class DocumentInfo
    {
        internal DocumentInfo() { }
        public string? FilePath { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<string> Folders { get { throw null; } }
        public Microsoft.CodeAnalysis.DocumentId Id { get { throw null; } }
        public bool IsGenerated { get { throw null; } }
        public string Name { get { throw null; } }
        public Microsoft.CodeAnalysis.SourceCodeKind SourceCodeKind { get { throw null; } }
        public Microsoft.CodeAnalysis.TextLoader? TextLoader { get { throw null; } }
        public static Microsoft.CodeAnalysis.DocumentInfo Create(Microsoft.CodeAnalysis.DocumentId id, string name, System.Collections.Generic.IEnumerable<string>? folders = null, Microsoft.CodeAnalysis.SourceCodeKind sourceCodeKind = Microsoft.CodeAnalysis.SourceCodeKind.Regular, Microsoft.CodeAnalysis.TextLoader? loader = null, string? filePath = null, bool isGenerated = false) { throw null; }
        public Microsoft.CodeAnalysis.DocumentInfo WithFilePath(string? filePath) { throw null; }
        public Microsoft.CodeAnalysis.DocumentInfo WithFolders(System.Collections.Generic.IEnumerable<string>? folders) { throw null; }
        public Microsoft.CodeAnalysis.DocumentInfo WithId(Microsoft.CodeAnalysis.DocumentId id) { throw null; }
        public Microsoft.CodeAnalysis.DocumentInfo WithName(string name) { throw null; }
        public Microsoft.CodeAnalysis.DocumentInfo WithSourceCodeKind(Microsoft.CodeAnalysis.SourceCodeKind kind) { throw null; }
        public Microsoft.CodeAnalysis.DocumentInfo WithTextLoader(Microsoft.CodeAnalysis.TextLoader? loader) { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, AllowMultiple=true)]
    [System.Composition.MetadataAttributeAttribute]
    public sealed partial class ExtensionOrderAttribute : System.Attribute
    {
        public ExtensionOrderAttribute() { }
        public string After { get { throw null; } set { } }
        public string Before { get { throw null; } set { } }
    }
    public partial class FileTextLoader : Microsoft.CodeAnalysis.TextLoader
    {
        public FileTextLoader(string path, System.Text.Encoding? defaultEncoding) { }
        public System.Text.Encoding? DefaultEncoding { get { throw null; } }
        public string Path { get { throw null; } }
        protected virtual Microsoft.CodeAnalysis.Text.SourceText CreateText(System.IO.Stream stream, Microsoft.CodeAnalysis.Workspace workspace) { throw null; }
        public override System.Threading.Tasks.Task<Microsoft.CodeAnalysis.TextAndVersion> LoadTextAndVersionAsync(Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.DocumentId documentId, System.Threading.CancellationToken cancellationToken) { throw null; }
    }
    public enum PreservationMode
    {
        PreserveValue = 0,
        PreserveIdentity = 1,
    }
    public partial class Project
    {
        internal Project() { }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.DocumentId> AdditionalDocumentIds { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.TextDocument> AdditionalDocuments { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.ProjectReference> AllProjectReferences { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.AnalyzerConfigDocument> AnalyzerConfigDocuments { get { throw null; } }
        public Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions AnalyzerOptions { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> AnalyzerReferences { get { throw null; } }
        public string AssemblyName { get { throw null; } }
        public Microsoft.CodeAnalysis.CompilationOptions? CompilationOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.CompilationOutputInfo CompilationOutputInfo { get { throw null; } }
        public string? DefaultNamespace { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.DocumentId> DocumentIds { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Document> Documents { get { throw null; } }
        public string? FilePath { get { throw null; } }
        public bool HasDocuments { get { throw null; } }
        public Microsoft.CodeAnalysis.ProjectId Id { get { throw null; } }
        public bool IsSubmission { get { throw null; } }
        public string Language { get { throw null; } }
        public Microsoft.CodeAnalysis.Host.HostLanguageServices LanguageServices { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.MetadataReference> MetadataReferences { get { throw null; } }
        public string Name { get { throw null; } }
        public string? OutputFilePath { get { throw null; } }
        public string? OutputRefFilePath { get { throw null; } }
        public Microsoft.CodeAnalysis.ParseOptions? ParseOptions { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference> ProjectReferences { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution Solution { get { throw null; } }
        public bool SupportsCompilation { get { throw null; } }
        public Microsoft.CodeAnalysis.VersionStamp Version { get { throw null; } }
        public Microsoft.CodeAnalysis.TextDocument AddAdditionalDocument(string name, Microsoft.CodeAnalysis.Text.SourceText text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.TextDocument AddAdditionalDocument(string name, string text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.TextDocument AddAnalyzerConfigDocument(string name, Microsoft.CodeAnalysis.Text.SourceText text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Project AddAnalyzerReference(Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { throw null; }
        public Microsoft.CodeAnalysis.Project AddAnalyzerReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> analyzerReferences) { throw null; }
        public Microsoft.CodeAnalysis.Document AddDocument(string name, Microsoft.CodeAnalysis.SyntaxNode syntaxRoot, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Document AddDocument(string name, Microsoft.CodeAnalysis.Text.SourceText text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Document AddDocument(string name, string text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Project AddMetadataReference(Microsoft.CodeAnalysis.MetadataReference metadataReference) { throw null; }
        public Microsoft.CodeAnalysis.Project AddMetadataReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> metadataReferences) { throw null; }
        public Microsoft.CodeAnalysis.Project AddProjectReference(Microsoft.CodeAnalysis.ProjectReference projectReference) { throw null; }
        public Microsoft.CodeAnalysis.Project AddProjectReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference> projectReferences) { throw null; }
        public bool ContainsAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public bool ContainsAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public bool ContainsDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.TextDocument? GetAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.AnalyzerConfigDocument? GetAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.ProjectChanges GetChanges(Microsoft.CodeAnalysis.Project oldProject) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Compilation?> GetCompilationAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.VersionStamp> GetDependentSemanticVersionAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.VersionStamp> GetDependentVersionAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.Document? GetDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.Document? GetDocument(Microsoft.CodeAnalysis.SyntaxTree? syntaxTree) { throw null; }
        public Microsoft.CodeAnalysis.DocumentId? GetDocumentId(Microsoft.CodeAnalysis.SyntaxTree? syntaxTree) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.VersionStamp> GetLatestDocumentVersionAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.VersionStamp> GetSemanticVersionAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.ValueTask<Microsoft.CodeAnalysis.SourceGeneratedDocument?> GetSourceGeneratedDocumentAsync(Microsoft.CodeAnalysis.DocumentId documentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.ValueTask<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SourceGeneratedDocument>> GetSourceGeneratedDocumentsAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveAdditionalDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> documentIds) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveAnalyzerConfigDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> documentIds) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveAnalyzerReference(Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> documentIds) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveMetadataReference(Microsoft.CodeAnalysis.MetadataReference metadataReference) { throw null; }
        public Microsoft.CodeAnalysis.Project RemoveProjectReference(Microsoft.CodeAnalysis.ProjectReference projectReference) { throw null; }
        public bool TryGetCompilation(out Microsoft.CodeAnalysis.Compilation? compilation) { throw null; }
        public Microsoft.CodeAnalysis.Project WithAnalyzerReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> analyzerReferencs) { throw null; }
        public Microsoft.CodeAnalysis.Project WithAssemblyName(string assemblyName) { throw null; }
        public Microsoft.CodeAnalysis.Project WithCompilationOptions(Microsoft.CodeAnalysis.CompilationOptions options) { throw null; }
        public Microsoft.CodeAnalysis.Project WithDefaultNamespace(string defaultNamespace) { throw null; }
        public Microsoft.CodeAnalysis.Project WithMetadataReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> metadataReferences) { throw null; }
        public Microsoft.CodeAnalysis.Project WithParseOptions(Microsoft.CodeAnalysis.ParseOptions options) { throw null; }
        public Microsoft.CodeAnalysis.Project WithProjectReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference> projectReferences) { throw null; }
    }
    public partial struct ProjectChanges
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Microsoft.CodeAnalysis.Project NewProject { get { throw null; } }
        public Microsoft.CodeAnalysis.Project OldProject { get { throw null; } }
        public Microsoft.CodeAnalysis.ProjectId ProjectId { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetAddedAdditionalDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetAddedAnalyzerConfigDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> GetAddedAnalyzerReferences() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetAddedDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> GetAddedMetadataReferences() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference> GetAddedProjectReferences() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetChangedAdditionalDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetChangedAnalyzerConfigDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetChangedDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetChangedDocuments(bool onlyGetDocumentsWithTextChanges) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetRemovedAdditionalDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetRemovedAnalyzerConfigDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> GetRemovedAnalyzerReferences() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetRemovedDocuments() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> GetRemovedMetadataReferences() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference> GetRemovedProjectReferences() { throw null; }
    }
    public partial class ProjectDependencyGraph
    {
        internal ProjectDependencyGraph() { }
        public System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectId>> GetDependencySets(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.ProjectId> GetProjectsThatDirectlyDependOnThisProject(Microsoft.CodeAnalysis.ProjectId projectId) { throw null; }
        public System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.ProjectId> GetProjectsThatThisProjectDirectlyDependsOn(Microsoft.CodeAnalysis.ProjectId projectId) { throw null; }
        public System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.ProjectId> GetProjectsThatThisProjectTransitivelyDependsOn(Microsoft.CodeAnalysis.ProjectId projectId) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectId> GetProjectsThatTransitivelyDependOnThisProject(Microsoft.CodeAnalysis.ProjectId projectId) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectId> GetTopologicallySortedProjects(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public partial class ProjectDiagnostic : Microsoft.CodeAnalysis.WorkspaceDiagnostic
    {
        public ProjectDiagnostic(Microsoft.CodeAnalysis.WorkspaceDiagnosticKind kind, string message, Microsoft.CodeAnalysis.ProjectId projectId) : base (default(Microsoft.CodeAnalysis.WorkspaceDiagnosticKind), default(string)) { }
        public Microsoft.CodeAnalysis.ProjectId ProjectId { get { throw null; } }
    }
    public sealed partial class ProjectId : System.IEquatable<Microsoft.CodeAnalysis.ProjectId>
    {
        internal ProjectId() { }
        public System.Guid Id { get { throw null; } }
        public static Microsoft.CodeAnalysis.ProjectId CreateFromSerialized(System.Guid id, string? debugName = null) { throw null; }
        public static Microsoft.CodeAnalysis.ProjectId CreateNewId(string? debugName = null) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.ProjectId? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.ProjectId? left, Microsoft.CodeAnalysis.ProjectId? right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.ProjectId? left, Microsoft.CodeAnalysis.ProjectId? right) { throw null; }
        public override string ToString() { throw null; }
    }
    public sealed partial class ProjectInfo
    {
        internal ProjectInfo() { }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.DocumentInfo> AdditionalDocuments { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.DocumentInfo> AnalyzerConfigDocuments { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> AnalyzerReferences { get { throw null; } }
        public string AssemblyName { get { throw null; } }
        public Microsoft.CodeAnalysis.CompilationOptions? CompilationOptions { get { throw null; } }
        public Microsoft.CodeAnalysis.CompilationOutputInfo CompilationOutputInfo { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.DocumentInfo> Documents { get { throw null; } }
        public string? FilePath { get { throw null; } }
        public System.Type? HostObjectType { get { throw null; } }
        public Microsoft.CodeAnalysis.ProjectId Id { get { throw null; } }
        public bool IsSubmission { get { throw null; } }
        public string Language { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.MetadataReference> MetadataReferences { get { throw null; } }
        public string Name { get { throw null; } }
        public string? OutputFilePath { get { throw null; } }
        public string? OutputRefFilePath { get { throw null; } }
        public Microsoft.CodeAnalysis.ParseOptions? ParseOptions { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.ProjectReference> ProjectReferences { get { throw null; } }
        public Microsoft.CodeAnalysis.VersionStamp Version { get { throw null; } }
        public static Microsoft.CodeAnalysis.ProjectInfo Create(Microsoft.CodeAnalysis.ProjectId id, Microsoft.CodeAnalysis.VersionStamp version, string name, string assemblyName, string language, string? filePath, string? outputFilePath, Microsoft.CodeAnalysis.CompilationOptions? compilationOptions, Microsoft.CodeAnalysis.ParseOptions? parseOptions, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentInfo>? documents, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference>? projectReferences, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference>? metadataReferences, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference>? analyzerReferences, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentInfo>? additionalDocuments, bool isSubmission, System.Type? hostObjectType) { throw null; }
        public static Microsoft.CodeAnalysis.ProjectInfo Create(Microsoft.CodeAnalysis.ProjectId id, Microsoft.CodeAnalysis.VersionStamp version, string name, string assemblyName, string language, string? filePath = null, string? outputFilePath = null, Microsoft.CodeAnalysis.CompilationOptions? compilationOptions = null, Microsoft.CodeAnalysis.ParseOptions? parseOptions = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentInfo>? documents = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference>? projectReferences = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference>? metadataReferences = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference>? analyzerReferences = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentInfo>? additionalDocuments = null, bool isSubmission = false, System.Type? hostObjectType = null, string? outputRefFilePath = null) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithAdditionalDocuments(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentInfo>? additionalDocuments) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithAnalyzerConfigDocuments(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentInfo>? analyzerConfigDocuments) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithAnalyzerReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference>? analyzerReferences) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithAssemblyName(string assemblyName) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithCompilationOptions(Microsoft.CodeAnalysis.CompilationOptions? compilationOptions) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithCompilationOutputInfo(in Microsoft.CodeAnalysis.CompilationOutputInfo info) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithDefaultNamespace(string? defaultNamespace) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithDocuments(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentInfo>? documents) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithFilePath(string? filePath) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithMetadataReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference>? metadataReferences) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithName(string name) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithOutputFilePath(string? outputFilePath) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithOutputRefFilePath(string? outputRefFilePath) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithParseOptions(Microsoft.CodeAnalysis.ParseOptions? parseOptions) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithProjectReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference>? projectReferences) { throw null; }
        public Microsoft.CodeAnalysis.ProjectInfo WithVersion(Microsoft.CodeAnalysis.VersionStamp version) { throw null; }
    }
    public sealed partial class ProjectReference : System.IEquatable<Microsoft.CodeAnalysis.ProjectReference>
    {
        public ProjectReference(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Immutable.ImmutableArray<string> aliases = default(System.Collections.Immutable.ImmutableArray<string>), bool embedInteropTypes = false) { }
        public System.Collections.Immutable.ImmutableArray<string> Aliases { get { throw null; } }
        public bool EmbedInteropTypes { get { throw null; } }
        public Microsoft.CodeAnalysis.ProjectId ProjectId { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.ProjectReference reference) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.ProjectReference left, Microsoft.CodeAnalysis.ProjectReference right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.ProjectReference left, Microsoft.CodeAnalysis.ProjectReference right) { throw null; }
    }
    public partial class Solution
    {
        internal Solution() { }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> AnalyzerReferences { get { throw null; } }
        public string? FilePath { get { throw null; } }
        public Microsoft.CodeAnalysis.SolutionId Id { get { throw null; } }
        public Microsoft.CodeAnalysis.Options.OptionSet Options { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.ProjectId> ProjectIds { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Project> Projects { get { throw null; } }
        public Microsoft.CodeAnalysis.VersionStamp Version { get { throw null; } }
        public Microsoft.CodeAnalysis.Workspace Workspace { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution AddAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId, string name, Microsoft.CodeAnalysis.Text.SourceText text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId, string name, string text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAdditionalDocument(Microsoft.CodeAnalysis.DocumentInfo documentInfo) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAdditionalDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentInfo> documentInfos) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId, string name, Microsoft.CodeAnalysis.Text.SourceText text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAnalyzerConfigDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentInfo> documentInfos) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAnalyzerReference(Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAnalyzerReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAnalyzerReferences(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> analyzerReferences) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddAnalyzerReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> analyzerReferences) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddDocument(Microsoft.CodeAnalysis.DocumentId documentId, string name, Microsoft.CodeAnalysis.SyntaxNode syntaxRoot, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null, bool isGenerated = false, Microsoft.CodeAnalysis.PreservationMode preservationMode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddDocument(Microsoft.CodeAnalysis.DocumentId documentId, string name, Microsoft.CodeAnalysis.Text.SourceText text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null, bool isGenerated = false) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddDocument(Microsoft.CodeAnalysis.DocumentId documentId, string name, Microsoft.CodeAnalysis.TextLoader loader, System.Collections.Generic.IEnumerable<string>? folders = null) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddDocument(Microsoft.CodeAnalysis.DocumentId documentId, string name, string text, System.Collections.Generic.IEnumerable<string>? folders = null, string? filePath = null) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddDocument(Microsoft.CodeAnalysis.DocumentInfo documentInfo) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentInfo> documentInfos) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddMetadataReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddMetadataReferences(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> metadataReferences) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddProject(Microsoft.CodeAnalysis.ProjectId projectId, string name, string assemblyName, string language) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddProject(Microsoft.CodeAnalysis.ProjectInfo projectInfo) { throw null; }
        public Microsoft.CodeAnalysis.Project AddProject(string name, string assemblyName, string language) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddProjectReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution AddProjectReferences(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference> projectReferences) { throw null; }
        public bool ContainsAdditionalDocument(Microsoft.CodeAnalysis.DocumentId? documentId) { throw null; }
        public bool ContainsAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId? documentId) { throw null; }
        public bool ContainsDocument(Microsoft.CodeAnalysis.DocumentId? documentId) { throw null; }
        public bool ContainsProject(Microsoft.CodeAnalysis.ProjectId? projectId) { throw null; }
        public Microsoft.CodeAnalysis.TextDocument? GetAdditionalDocument(Microsoft.CodeAnalysis.DocumentId? documentId) { throw null; }
        public Microsoft.CodeAnalysis.AnalyzerConfigDocument? GetAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId? documentId) { throw null; }
        public Microsoft.CodeAnalysis.SolutionChanges GetChanges(Microsoft.CodeAnalysis.Solution oldSolution) { throw null; }
        public Microsoft.CodeAnalysis.Document? GetDocument(Microsoft.CodeAnalysis.DocumentId? documentId) { throw null; }
        public Microsoft.CodeAnalysis.Document? GetDocument(Microsoft.CodeAnalysis.SyntaxTree? syntaxTree) { throw null; }
        public Microsoft.CodeAnalysis.DocumentId? GetDocumentId(Microsoft.CodeAnalysis.SyntaxTree? syntaxTree) { throw null; }
        public Microsoft.CodeAnalysis.DocumentId? GetDocumentId(Microsoft.CodeAnalysis.SyntaxTree? syntaxTree, Microsoft.CodeAnalysis.ProjectId? projectId) { throw null; }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> GetDocumentIdsWithFilePath(string? filePath) { throw null; }
        [System.ObsoleteAttribute("This method no longer produces a Solution that does not share state and is no longer necessary to call.", false)]
        public Microsoft.CodeAnalysis.Solution GetIsolatedSolution() { throw null; }
        public Microsoft.CodeAnalysis.VersionStamp GetLatestProjectVersion() { throw null; }
        public Microsoft.CodeAnalysis.Project? GetProject(Microsoft.CodeAnalysis.IAssemblySymbol assemblySymbol, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.Project? GetProject(Microsoft.CodeAnalysis.ProjectId? projectId) { throw null; }
        public Microsoft.CodeAnalysis.ProjectDependencyGraph GetProjectDependencyGraph() { throw null; }
        public System.Threading.Tasks.ValueTask<Microsoft.CodeAnalysis.SourceGeneratedDocument?> GetSourceGeneratedDocumentAsync(Microsoft.CodeAnalysis.DocumentId documentId, System.Threading.CancellationToken cancellationToken) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveAdditionalDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> documentIds) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveAnalyzerConfigDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> documentIds) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveAnalyzerReference(Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveAnalyzerReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveDocument(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveDocuments(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentId> documentIds) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveMetadataReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveProject(Microsoft.CodeAnalysis.ProjectId projectId) { throw null; }
        public Microsoft.CodeAnalysis.Solution RemoveProjectReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithAdditionalDocumentText(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithAdditionalDocumentText(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextAndVersion textAndVersion, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithAdditionalDocumentTextLoader(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader loader, Microsoft.CodeAnalysis.PreservationMode mode) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithAnalyzerConfigDocumentText(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithAnalyzerConfigDocumentText(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextAndVersion textAndVersion, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithAnalyzerConfigDocumentTextLoader(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader loader, Microsoft.CodeAnalysis.PreservationMode mode) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithAnalyzerReferences(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> analyzerReferences) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentFilePath(Microsoft.CodeAnalysis.DocumentId documentId, string filePath) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentFolders(Microsoft.CodeAnalysis.DocumentId documentId, System.Collections.Generic.IEnumerable<string>? folders) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentName(Microsoft.CodeAnalysis.DocumentId documentId, string name) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentSourceCodeKind(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.SourceCodeKind sourceCodeKind) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentSyntaxRoot(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentText(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentText(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextAndVersion textAndVersion, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentText(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId?> documentIds, Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.PreservationMode mode = Microsoft.CodeAnalysis.PreservationMode.PreserveValue) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithDocumentTextLoader(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader loader, Microsoft.CodeAnalysis.PreservationMode mode) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithOptions(Microsoft.CodeAnalysis.Options.OptionSet options) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectAnalyzerReferences(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> analyzerReferences) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectAssemblyName(Microsoft.CodeAnalysis.ProjectId projectId, string assemblyName) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectCompilationOptions(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.CompilationOptions options) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectCompilationOutputInfo(Microsoft.CodeAnalysis.ProjectId projectId, in Microsoft.CodeAnalysis.CompilationOutputInfo info) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectDefaultNamespace(Microsoft.CodeAnalysis.ProjectId projectId, string? defaultNamespace) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectDocumentsOrder(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Immutable.ImmutableList<Microsoft.CodeAnalysis.DocumentId> documentIds) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectFilePath(Microsoft.CodeAnalysis.ProjectId projectId, string? filePath) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectMetadataReferences(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.MetadataReference> metadataReferences) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectName(Microsoft.CodeAnalysis.ProjectId projectId, string name) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectOutputFilePath(Microsoft.CodeAnalysis.ProjectId projectId, string? outputFilePath) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectOutputRefFilePath(Microsoft.CodeAnalysis.ProjectId projectId, string? outputRefFilePath) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectParseOptions(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ParseOptions options) { throw null; }
        public Microsoft.CodeAnalysis.Solution WithProjectReferences(Microsoft.CodeAnalysis.ProjectId projectId, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectReference>? projectReferences) { throw null; }
    }
    public readonly partial struct SolutionChanges
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> GetAddedAnalyzerReferences() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Project> GetAddedProjects() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectChanges> GetProjectChanges() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> GetRemovedAnalyzerReferences() { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Project> GetRemovedProjects() { throw null; }
    }
    public sealed partial class SolutionId : System.IEquatable<Microsoft.CodeAnalysis.SolutionId>
    {
        internal SolutionId() { }
        public System.Guid Id { get { throw null; } }
        public static Microsoft.CodeAnalysis.SolutionId CreateFromSerialized(System.Guid id, string debugName = null) { throw null; }
        public static Microsoft.CodeAnalysis.SolutionId CreateNewId(string debugName = null) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.SolutionId other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.SolutionId left, Microsoft.CodeAnalysis.SolutionId right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.SolutionId left, Microsoft.CodeAnalysis.SolutionId right) { throw null; }
    }
    public sealed partial class SolutionInfo
    {
        internal SolutionInfo() { }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference> AnalyzerReferences { get { throw null; } }
        public string? FilePath { get { throw null; } }
        public Microsoft.CodeAnalysis.SolutionId Id { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.ProjectInfo> Projects { get { throw null; } }
        public Microsoft.CodeAnalysis.VersionStamp Version { get { throw null; } }
        public static Microsoft.CodeAnalysis.SolutionInfo Create(Microsoft.CodeAnalysis.SolutionId id, Microsoft.CodeAnalysis.VersionStamp version, string? filePath, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectInfo>? projects) { throw null; }
        public static Microsoft.CodeAnalysis.SolutionInfo Create(Microsoft.CodeAnalysis.SolutionId id, Microsoft.CodeAnalysis.VersionStamp version, string? filePath = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ProjectInfo>? projects = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference>? analyzerReferences = null) { throw null; }
    }
    public sealed partial class SourceGeneratedDocument : Microsoft.CodeAnalysis.Document
    {
        internal SourceGeneratedDocument() { }
        public string HintName { get { throw null; } }
    }
    public sealed partial class TextAndVersion
    {
        internal TextAndVersion() { }
        public string FilePath { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.SourceText Text { get { throw null; } }
        public Microsoft.CodeAnalysis.VersionStamp Version { get { throw null; } }
        public static Microsoft.CodeAnalysis.TextAndVersion Create(Microsoft.CodeAnalysis.Text.SourceText text, Microsoft.CodeAnalysis.VersionStamp version, string? filePath = null) { throw null; }
    }
    public partial class TextDocument
    {
        internal TextDocument() { }
        public string? FilePath { get { throw null; } }
        public System.Collections.Generic.IReadOnlyList<string> Folders { get { throw null; } }
        public Microsoft.CodeAnalysis.DocumentId Id { get { throw null; } }
        public string Name { get { throw null; } }
        public Microsoft.CodeAnalysis.Project Project { get { throw null; } }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Text.SourceText> GetTextAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.VersionStamp> GetTextVersionAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public bool TryGetText(out Microsoft.CodeAnalysis.Text.SourceText? text) { throw null; }
        public bool TryGetTextVersion(out Microsoft.CodeAnalysis.VersionStamp version) { throw null; }
    }
    public enum TextDocumentKind
    {
        Document = 0,
        AdditionalDocument = 1,
        AnalyzerConfigDocument = 2,
    }
    public abstract partial class TextLoader
    {
        protected TextLoader() { }
        public static Microsoft.CodeAnalysis.TextLoader From(Microsoft.CodeAnalysis.Text.SourceTextContainer container, Microsoft.CodeAnalysis.VersionStamp version, string? filePath = null) { throw null; }
        public static Microsoft.CodeAnalysis.TextLoader From(Microsoft.CodeAnalysis.TextAndVersion textAndVersion) { throw null; }
        public abstract System.Threading.Tasks.Task<Microsoft.CodeAnalysis.TextAndVersion> LoadTextAndVersionAsync(Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.DocumentId documentId, System.Threading.CancellationToken cancellationToken);
    }
    public readonly partial struct VersionStamp : System.IEquatable<Microsoft.CodeAnalysis.VersionStamp>
    {
        private readonly int _dummyPrimitive;
        public static Microsoft.CodeAnalysis.VersionStamp Default { get { throw null; } }
        public static Microsoft.CodeAnalysis.VersionStamp Create() { throw null; }
        public static Microsoft.CodeAnalysis.VersionStamp Create(System.DateTime utcTimeLastModified) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.VersionStamp version) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public Microsoft.CodeAnalysis.VersionStamp GetNewerVersion() { throw null; }
        public Microsoft.CodeAnalysis.VersionStamp GetNewerVersion(Microsoft.CodeAnalysis.VersionStamp version) { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.VersionStamp left, Microsoft.CodeAnalysis.VersionStamp right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.VersionStamp left, Microsoft.CodeAnalysis.VersionStamp right) { throw null; }
        public override string ToString() { throw null; }
    }
    public abstract partial class Workspace : System.IDisposable
    {
        protected Workspace(Microsoft.CodeAnalysis.Host.HostServices host, string? workspaceKind) { }
        public virtual bool CanOpenDocuments { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution CurrentSolution { get { throw null; } }
        public string? Kind { get { throw null; } }
        public Microsoft.CodeAnalysis.Options.OptionSet Options { get { throw null; } [System.ObsoleteAttribute("Workspace options should be set by invoking 'workspace.TryApplyChanges(workspace.CurrentSolution.WithOptions(newOptionSet))'")] set { } }
        protected internal virtual bool PartialSemanticsEnabled { get { throw null; } }
        public Microsoft.CodeAnalysis.Host.HostWorkspaceServices Services { get { throw null; } }
        public event System.EventHandler<Microsoft.CodeAnalysis.DocumentActiveContextChangedEventArgs> DocumentActiveContextChanged { add { } remove { } }
        public event System.EventHandler<Microsoft.CodeAnalysis.DocumentEventArgs> DocumentClosed { add { } remove { } }
        public event System.EventHandler<Microsoft.CodeAnalysis.DocumentEventArgs> DocumentOpened { add { } remove { } }
        public event System.EventHandler<Microsoft.CodeAnalysis.WorkspaceChangeEventArgs> WorkspaceChanged { add { } remove { } }
        public event System.EventHandler<Microsoft.CodeAnalysis.WorkspaceDiagnosticEventArgs> WorkspaceFailed { add { } remove { } }
        protected virtual Microsoft.CodeAnalysis.Project AdjustReloadedProject(Microsoft.CodeAnalysis.Project oldProject, Microsoft.CodeAnalysis.Project reloadedProject) { throw null; }
        protected virtual Microsoft.CodeAnalysis.Solution AdjustReloadedSolution(Microsoft.CodeAnalysis.Solution oldSolution, Microsoft.CodeAnalysis.Solution reloadedSolution) { throw null; }
        protected virtual void ApplyAdditionalDocumentAdded(Microsoft.CodeAnalysis.DocumentInfo info, Microsoft.CodeAnalysis.Text.SourceText text) { }
        protected virtual void ApplyAdditionalDocumentRemoved(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected virtual void ApplyAdditionalDocumentTextChanged(Microsoft.CodeAnalysis.DocumentId id, Microsoft.CodeAnalysis.Text.SourceText text) { }
        protected virtual void ApplyAnalyzerConfigDocumentAdded(Microsoft.CodeAnalysis.DocumentInfo info, Microsoft.CodeAnalysis.Text.SourceText text) { }
        protected virtual void ApplyAnalyzerConfigDocumentRemoved(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected virtual void ApplyAnalyzerConfigDocumentTextChanged(Microsoft.CodeAnalysis.DocumentId id, Microsoft.CodeAnalysis.Text.SourceText text) { }
        protected virtual void ApplyAnalyzerReferenceAdded(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { }
        protected virtual void ApplyAnalyzerReferenceRemoved(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { }
        protected virtual void ApplyCompilationOptionsChanged(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.CompilationOptions options) { }
        protected virtual void ApplyDocumentAdded(Microsoft.CodeAnalysis.DocumentInfo info, Microsoft.CodeAnalysis.Text.SourceText text) { }
        protected virtual void ApplyDocumentInfoChanged(Microsoft.CodeAnalysis.DocumentId id, Microsoft.CodeAnalysis.DocumentInfo info) { }
        protected virtual void ApplyDocumentRemoved(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected virtual void ApplyDocumentTextChanged(Microsoft.CodeAnalysis.DocumentId id, Microsoft.CodeAnalysis.Text.SourceText text) { }
        protected virtual void ApplyMetadataReferenceAdded(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { }
        protected virtual void ApplyMetadataReferenceRemoved(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { }
        protected virtual void ApplyParseOptionsChanged(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ParseOptions options) { }
        protected virtual void ApplyProjectAdded(Microsoft.CodeAnalysis.ProjectInfo project) { }
        protected virtual void ApplyProjectChanges(Microsoft.CodeAnalysis.ProjectChanges projectChanges) { }
        protected virtual void ApplyProjectReferenceAdded(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { }
        protected virtual void ApplyProjectReferenceRemoved(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { }
        protected virtual void ApplyProjectRemoved(Microsoft.CodeAnalysis.ProjectId projectId) { }
        public virtual bool CanApplyChange(Microsoft.CodeAnalysis.ApplyChangesKind feature) { throw null; }
        protected virtual bool CanApplyCompilationOptionChange(Microsoft.CodeAnalysis.CompilationOptions oldOptions, Microsoft.CodeAnalysis.CompilationOptions newOptions, Microsoft.CodeAnalysis.Project project) { throw null; }
        public virtual bool CanApplyParseOptionChange(Microsoft.CodeAnalysis.ParseOptions oldOptions, Microsoft.CodeAnalysis.ParseOptions newOptions, Microsoft.CodeAnalysis.Project project) { throw null; }
        protected void CheckAdditionalDocumentIsInCurrentSolution(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckAdditionalDocumentIsNotInCurrentSolution(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckAnalyzerConfigDocumentIsInCurrentSolution(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckAnalyzerConfigDocumentIsNotInCurrentSolution(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckCanOpenDocuments() { }
        protected virtual void CheckDocumentCanBeRemoved(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckDocumentIsClosed(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckDocumentIsInCurrentSolution(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckDocumentIsNotInCurrentSolution(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void CheckDocumentIsOpen(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected virtual void CheckProjectCanBeRemoved(Microsoft.CodeAnalysis.ProjectId projectId) { }
        protected void CheckProjectDoesNotContainOpenDocuments(Microsoft.CodeAnalysis.ProjectId projectId) { }
        protected void CheckProjectDoesNotHaveAnalyzerReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { }
        protected void CheckProjectDoesNotHaveMetadataReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { }
        protected void CheckProjectDoesNotHaveProjectReference(Microsoft.CodeAnalysis.ProjectId fromProjectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { }
        protected void CheckProjectDoesNotHaveTransitiveProjectReference(Microsoft.CodeAnalysis.ProjectId fromProjectId, Microsoft.CodeAnalysis.ProjectId toProjectId) { }
        protected void CheckProjectHasAnalyzerReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { }
        protected void CheckProjectHasMetadataReference(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { }
        protected void CheckProjectHasProjectReference(Microsoft.CodeAnalysis.ProjectId fromProjectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { }
        protected void CheckProjectIsInCurrentSolution(Microsoft.CodeAnalysis.ProjectId projectId) { }
        protected void CheckProjectIsNotInCurrentSolution(Microsoft.CodeAnalysis.ProjectId projectId) { }
        protected void CheckSolutionIsEmpty() { }
        protected internal virtual void ClearDocumentData(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected void ClearOpenDocument(Microsoft.CodeAnalysis.DocumentId documentId) { }
        [System.ObsoleteAttribute("The isSolutionClosing parameter is now obsolete. Please call the overload without that parameter.")]
        protected void ClearOpenDocument(Microsoft.CodeAnalysis.DocumentId documentId, bool isSolutionClosing) { }
        protected virtual void ClearProjectData(Microsoft.CodeAnalysis.ProjectId projectId) { }
        protected void ClearSolution() { }
        protected virtual void ClearSolutionData() { }
        public virtual void CloseAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId) { }
        public virtual void CloseAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId) { }
        public virtual void CloseDocument(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected internal Microsoft.CodeAnalysis.Solution CreateSolution(Microsoft.CodeAnalysis.SolutionId id) { throw null; }
        protected internal Microsoft.CodeAnalysis.Solution CreateSolution(Microsoft.CodeAnalysis.SolutionInfo solutionInfo) { throw null; }
        public void Dispose() { }
        protected virtual void Dispose(bool finalize) { }
        protected virtual string GetAdditionalDocumentName(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        protected virtual string GetAnalyzerConfigDocumentName(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public virtual Microsoft.CodeAnalysis.DocumentId? GetDocumentIdInCurrentContext(Microsoft.CodeAnalysis.Text.SourceTextContainer container) { throw null; }
        protected virtual string GetDocumentName(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        public virtual System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetOpenDocumentIds(Microsoft.CodeAnalysis.ProjectId? projectId = null) { throw null; }
        protected virtual string GetProjectName(Microsoft.CodeAnalysis.ProjectId projectId) { throw null; }
        public virtual System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.DocumentId> GetRelatedDocumentIds(Microsoft.CodeAnalysis.Text.SourceTextContainer container) { throw null; }
        public static Microsoft.CodeAnalysis.WorkspaceRegistration GetWorkspaceRegistration(Microsoft.CodeAnalysis.Text.SourceTextContainer? textContainer) { throw null; }
        public virtual bool IsDocumentOpen(Microsoft.CodeAnalysis.DocumentId documentId) { throw null; }
        protected internal void OnAdditionalDocumentAdded(Microsoft.CodeAnalysis.DocumentInfo documentInfo) { }
        protected internal void OnAdditionalDocumentClosed(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader reloader) { }
        protected internal void OnAdditionalDocumentOpened(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceTextContainer textContainer, bool isCurrentContext = true) { }
        protected internal void OnAdditionalDocumentRemoved(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected internal void OnAdditionalDocumentTextChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceText newText, Microsoft.CodeAnalysis.PreservationMode mode) { }
        protected internal void OnAdditionalDocumentTextLoaderChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader loader) { }
        protected internal void OnAnalyzerConfigDocumentAdded(Microsoft.CodeAnalysis.DocumentInfo documentInfo) { }
        protected internal void OnAnalyzerConfigDocumentClosed(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader reloader) { }
        protected internal void OnAnalyzerConfigDocumentOpened(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceTextContainer textContainer, bool isCurrentContext = true) { }
        protected internal void OnAnalyzerConfigDocumentRemoved(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected internal void OnAnalyzerConfigDocumentTextChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceText newText, Microsoft.CodeAnalysis.PreservationMode mode) { }
        protected internal void OnAnalyzerConfigDocumentTextLoaderChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader loader) { }
        protected internal void OnAnalyzerReferenceAdded(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { }
        protected internal void OnAnalyzerReferenceRemoved(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference analyzerReference) { }
        protected internal void OnAssemblyNameChanged(Microsoft.CodeAnalysis.ProjectId projectId, string assemblyName) { }
        protected internal void OnCompilationOptionsChanged(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.CompilationOptions options) { }
        protected internal void OnDocumentAdded(Microsoft.CodeAnalysis.DocumentInfo documentInfo) { }
        protected internal void OnDocumentClosed(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader reloader, bool updateActiveContext = false) { }
        protected virtual void OnDocumentClosing(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected internal void OnDocumentContextUpdated(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected internal void OnDocumentInfoChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.DocumentInfo newInfo) { }
        protected internal void OnDocumentOpened(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceTextContainer textContainer, bool isCurrentContext = true) { }
        protected internal void OnDocumentReloaded(Microsoft.CodeAnalysis.DocumentInfo newDocumentInfo) { }
        protected internal void OnDocumentRemoved(Microsoft.CodeAnalysis.DocumentId documentId) { }
        protected internal void OnDocumentsAdded(System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.DocumentInfo> documentInfos) { }
        protected internal void OnDocumentSourceCodeKindChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.SourceCodeKind sourceCodeKind) { }
        protected virtual void OnDocumentTextChanged(Microsoft.CodeAnalysis.Document document) { }
        protected internal void OnDocumentTextChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.Text.SourceText newText, Microsoft.CodeAnalysis.PreservationMode mode) { }
        protected internal void OnDocumentTextLoaderChanged(Microsoft.CodeAnalysis.DocumentId documentId, Microsoft.CodeAnalysis.TextLoader loader) { }
        protected internal void OnMetadataReferenceAdded(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { }
        protected internal void OnMetadataReferenceRemoved(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.MetadataReference metadataReference) { }
        protected internal void OnOutputFilePathChanged(Microsoft.CodeAnalysis.ProjectId projectId, string? outputFilePath) { }
        protected internal void OnOutputRefFilePathChanged(Microsoft.CodeAnalysis.ProjectId projectId, string? outputFilePath) { }
        protected internal void OnParseOptionsChanged(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ParseOptions options) { }
        protected internal void OnProjectAdded(Microsoft.CodeAnalysis.ProjectInfo projectInfo) { }
        protected internal void OnProjectNameChanged(Microsoft.CodeAnalysis.ProjectId projectId, string name, string? filePath) { }
        protected internal void OnProjectReferenceAdded(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { }
        protected internal void OnProjectReferenceRemoved(Microsoft.CodeAnalysis.ProjectId projectId, Microsoft.CodeAnalysis.ProjectReference projectReference) { }
        protected internal virtual void OnProjectReloaded(Microsoft.CodeAnalysis.ProjectInfo reloadedProjectInfo) { }
        protected internal virtual void OnProjectRemoved(Microsoft.CodeAnalysis.ProjectId projectId) { }
        protected internal void OnSolutionAdded(Microsoft.CodeAnalysis.SolutionInfo solutionInfo) { }
        protected internal void OnSolutionReloaded(Microsoft.CodeAnalysis.SolutionInfo reloadedSolutionInfo) { }
        protected internal void OnSolutionRemoved() { }
        protected internal virtual void OnWorkspaceFailed(Microsoft.CodeAnalysis.WorkspaceDiagnostic diagnostic) { }
        public virtual void OpenAdditionalDocument(Microsoft.CodeAnalysis.DocumentId documentId, bool activate = true) { }
        public virtual void OpenAnalyzerConfigDocument(Microsoft.CodeAnalysis.DocumentId documentId, bool activate = true) { }
        public virtual void OpenDocument(Microsoft.CodeAnalysis.DocumentId documentId, bool activate = true) { }
        [System.ObsoleteAttribute("This member is obsolete. Use the RaiseDocumentActiveContextChangedEventAsync(SourceTextContainer, DocumentId, DocumentId) overload instead.", true)]
        protected System.Threading.Tasks.Task RaiseDocumentActiveContextChangedEventAsync(Microsoft.CodeAnalysis.Document document) { throw null; }
        protected System.Threading.Tasks.Task RaiseDocumentActiveContextChangedEventAsync(Microsoft.CodeAnalysis.Text.SourceTextContainer sourceTextContainer, Microsoft.CodeAnalysis.DocumentId oldActiveContextDocumentId, Microsoft.CodeAnalysis.DocumentId newActiveContextDocumentId) { throw null; }
        protected System.Threading.Tasks.Task RaiseDocumentClosedEventAsync(Microsoft.CodeAnalysis.Document document) { throw null; }
        protected System.Threading.Tasks.Task RaiseDocumentOpenedEventAsync(Microsoft.CodeAnalysis.Document document) { throw null; }
        protected System.Threading.Tasks.Task RaiseWorkspaceChangedEventAsync(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.Solution oldSolution, Microsoft.CodeAnalysis.Solution newSolution, Microsoft.CodeAnalysis.ProjectId projectId = null, Microsoft.CodeAnalysis.DocumentId documentId = null) { throw null; }
        protected void RegisterText(Microsoft.CodeAnalysis.Text.SourceTextContainer textContainer) { }
        protected internal System.Threading.Tasks.Task ScheduleTask(System.Action action, string? taskName = "Workspace.Task") { throw null; }
        protected internal System.Threading.Tasks.Task<T> ScheduleTask<T>(System.Func<T> func, string? taskName = "Workspace.Task") { throw null; }
        protected Microsoft.CodeAnalysis.Solution SetCurrentSolution(Microsoft.CodeAnalysis.Solution solution) { throw null; }
        public virtual bool TryApplyChanges(Microsoft.CodeAnalysis.Solution newSolution) { throw null; }
        public static bool TryGetWorkspace(Microsoft.CodeAnalysis.Text.SourceTextContainer textContainer, out Microsoft.CodeAnalysis.Workspace? workspace) { throw null; }
        protected void UnregisterText(Microsoft.CodeAnalysis.Text.SourceTextContainer textContainer) { }
        protected void UpdateReferencesAfterAdd() { }
    }
    public partial class WorkspaceChangeEventArgs : System.EventArgs
    {
        public WorkspaceChangeEventArgs(Microsoft.CodeAnalysis.WorkspaceChangeKind kind, Microsoft.CodeAnalysis.Solution oldSolution, Microsoft.CodeAnalysis.Solution newSolution, Microsoft.CodeAnalysis.ProjectId? projectId = null, Microsoft.CodeAnalysis.DocumentId? documentId = null) { }
        public Microsoft.CodeAnalysis.DocumentId? DocumentId { get { throw null; } }
        public Microsoft.CodeAnalysis.WorkspaceChangeKind Kind { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution NewSolution { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution OldSolution { get { throw null; } }
        public Microsoft.CodeAnalysis.ProjectId? ProjectId { get { throw null; } }
    }
    public enum WorkspaceChangeKind
    {
        SolutionChanged = 0,
        SolutionAdded = 1,
        SolutionRemoved = 2,
        SolutionCleared = 3,
        SolutionReloaded = 4,
        ProjectAdded = 5,
        ProjectRemoved = 6,
        ProjectChanged = 7,
        ProjectReloaded = 8,
        DocumentAdded = 9,
        DocumentRemoved = 10,
        DocumentReloaded = 11,
        DocumentChanged = 12,
        AdditionalDocumentAdded = 13,
        AdditionalDocumentRemoved = 14,
        AdditionalDocumentReloaded = 15,
        AdditionalDocumentChanged = 16,
        DocumentInfoChanged = 17,
        AnalyzerConfigDocumentAdded = 18,
        AnalyzerConfigDocumentRemoved = 19,
        AnalyzerConfigDocumentReloaded = 20,
        AnalyzerConfigDocumentChanged = 21,
    }
    public partial class WorkspaceDiagnostic
    {
        public WorkspaceDiagnostic(Microsoft.CodeAnalysis.WorkspaceDiagnosticKind kind, string message) { }
        public Microsoft.CodeAnalysis.WorkspaceDiagnosticKind Kind { get { throw null; } }
        public string Message { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public partial class WorkspaceDiagnosticEventArgs : System.EventArgs
    {
        public WorkspaceDiagnosticEventArgs(Microsoft.CodeAnalysis.WorkspaceDiagnostic diagnostic) { }
        public Microsoft.CodeAnalysis.WorkspaceDiagnostic Diagnostic { get { throw null; } }
    }
    public enum WorkspaceDiagnosticKind
    {
        Failure = 0,
        Warning = 1,
    }
    public static partial class WorkspaceKind
    {
        public const string Debugger = "Debugger";
        public const string Host = "Host";
        public const string Interactive = "Interactive";
        public const string MetadataAsSource = "MetadataAsSource";
        public const string MiscellaneousFiles = "MiscellaneousFiles";
        public const string MSBuild = "MSBuildWorkspace";
        public const string Preview = "Preview";
    }
    public sealed partial class WorkspaceRegistration
    {
        internal WorkspaceRegistration() { }
        public Microsoft.CodeAnalysis.Workspace? Workspace { get { throw null; } }
        public event System.EventHandler? WorkspaceChanged { add { } remove { } }
    }
    public abstract partial class XmlDocumentationProvider : Microsoft.CodeAnalysis.DocumentationProvider
    {
        protected XmlDocumentationProvider() { }
        public static Microsoft.CodeAnalysis.XmlDocumentationProvider CreateFromBytes(byte[] xmlDocCommentBytes) { throw null; }
        public static Microsoft.CodeAnalysis.XmlDocumentationProvider CreateFromFile(string xmlDocCommentFilePath) { throw null; }
        protected override string GetDocumentationForSymbol(string documentationMemberID, System.Globalization.CultureInfo preferredCulture, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        protected abstract System.IO.Stream GetSourceStream(System.Threading.CancellationToken cancellationToken);
    }
}
namespace Microsoft.CodeAnalysis.Classification
{
    public static partial class ClassificationTypeNames
    {
        public const string ClassName = "class name";
        public const string Comment = "comment";
        public const string ConstantName = "constant name";
        public const string ControlKeyword = "keyword - control";
        public const string DelegateName = "delegate name";
        public const string EnumMemberName = "enum member name";
        public const string EnumName = "enum name";
        public const string EventName = "event name";
        public const string ExcludedCode = "excluded code";
        public const string ExtensionMethodName = "extension method name";
        public const string FieldName = "field name";
        public const string Identifier = "identifier";
        public const string InterfaceName = "interface name";
        public const string Keyword = "keyword";
        public const string LabelName = "label name";
        public const string LocalName = "local name";
        public const string MethodName = "method name";
        public const string ModuleName = "module name";
        public const string NamespaceName = "namespace name";
        public const string NumericLiteral = "number";
        public const string Operator = "operator";
        public const string OperatorOverloaded = "operator - overloaded";
        public const string ParameterName = "parameter name";
        public const string PreprocessorKeyword = "preprocessor keyword";
        public const string PreprocessorText = "preprocessor text";
        public const string PropertyName = "property name";
        public const string Punctuation = "punctuation";
        public const string RecordClassName = "record class name";
        public const string RecordStructName = "record struct name";
        public const string RegexAlternation = "regex - alternation";
        public const string RegexAnchor = "regex - anchor";
        public const string RegexCharacterClass = "regex - character class";
        public const string RegexComment = "regex - comment";
        public const string RegexGrouping = "regex - grouping";
        public const string RegexOtherEscape = "regex - other escape";
        public const string RegexQuantifier = "regex - quantifier";
        public const string RegexSelfEscapedCharacter = "regex - self escaped character";
        public const string RegexText = "regex - text";
        public const string StaticSymbol = "static symbol";
        public const string StringEscapeCharacter = "string - escape character";
        public const string StringLiteral = "string";
        public const string StructName = "struct name";
        public const string Text = "text";
        public const string TypeParameterName = "type parameter name";
        public const string VerbatimStringLiteral = "string - verbatim";
        public const string WhiteSpace = "whitespace";
        public const string XmlDocCommentAttributeName = "xml doc comment - attribute name";
        public const string XmlDocCommentAttributeQuotes = "xml doc comment - attribute quotes";
        public const string XmlDocCommentAttributeValue = "xml doc comment - attribute value";
        public const string XmlDocCommentCDataSection = "xml doc comment - cdata section";
        public const string XmlDocCommentComment = "xml doc comment - comment";
        public const string XmlDocCommentDelimiter = "xml doc comment - delimiter";
        public const string XmlDocCommentEntityReference = "xml doc comment - entity reference";
        public const string XmlDocCommentName = "xml doc comment - name";
        public const string XmlDocCommentProcessingInstruction = "xml doc comment - processing instruction";
        public const string XmlDocCommentText = "xml doc comment - text";
        public const string XmlLiteralAttributeName = "xml literal - attribute name";
        public const string XmlLiteralAttributeQuotes = "xml literal - attribute quotes";
        public const string XmlLiteralAttributeValue = "xml literal - attribute value";
        public const string XmlLiteralCDataSection = "xml literal - cdata section";
        public const string XmlLiteralComment = "xml literal - comment";
        public const string XmlLiteralDelimiter = "xml literal - delimiter";
        public const string XmlLiteralEmbeddedExpression = "xml literal - embedded expression";
        public const string XmlLiteralEntityReference = "xml literal - entity reference";
        public const string XmlLiteralName = "xml literal - name";
        public const string XmlLiteralProcessingInstruction = "xml literal - processing instruction";
        public const string XmlLiteralText = "xml literal - text";
        public static System.Collections.Immutable.ImmutableArray<string> AdditiveTypeNames { get { throw null; } }
    }
    public partial struct ClassifiedSpan : System.IEquatable<Microsoft.CodeAnalysis.Classification.ClassifiedSpan>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public ClassifiedSpan(Microsoft.CodeAnalysis.Text.TextSpan textSpan, string classificationType) { throw null; }
        public ClassifiedSpan(string classificationType, Microsoft.CodeAnalysis.Text.TextSpan textSpan) { throw null; }
        public string ClassificationType { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan TextSpan { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Classification.ClassifiedSpan other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public static partial class Classifier
    {
        public static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Classification.ClassifiedSpan> GetClassifiedSpans(Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Text.TextSpan textSpan, Microsoft.CodeAnalysis.Workspace workspace, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Classification.ClassifiedSpan>> GetClassifiedSpansAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Text.TextSpan textSpan, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.CodeActions
{
    public sealed partial class ApplyChangesOperation : Microsoft.CodeAnalysis.CodeActions.CodeActionOperation
    {
        public ApplyChangesOperation(Microsoft.CodeAnalysis.Solution changedSolution) { }
        public Microsoft.CodeAnalysis.Solution ChangedSolution { get { throw null; } }
        public override void Apply(Microsoft.CodeAnalysis.Workspace workspace, System.Threading.CancellationToken cancellationToken) { }
    }
    public abstract partial class CodeAction
    {
        protected CodeAction() { }
        public virtual string? EquivalenceKey { get { throw null; } }
        public virtual System.Collections.Immutable.ImmutableArray<string> Tags { get { throw null; } }
        public abstract string Title { get; }
        protected virtual System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>> ComputeOperationsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        protected virtual System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>> ComputePreviewOperationsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public static Microsoft.CodeAnalysis.CodeActions.CodeAction Create(string title, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CodeActions.CodeAction> nestedActions, bool isInlinable) { throw null; }
        public static Microsoft.CodeAnalysis.CodeActions.CodeAction Create(string title, System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document>> createChangedDocument, string? equivalenceKey = null) { throw null; }
        public static Microsoft.CodeAnalysis.CodeActions.CodeAction Create(string title, System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Solution>> createChangedSolution, string? equivalenceKey = null) { throw null; }
        protected virtual System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> GetChangedDocumentAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        protected virtual System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Solution?> GetChangedSolutionAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>> GetOperationsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>> GetPreviewOperationsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        protected System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>> PostProcessAsync(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation> operations, System.Threading.CancellationToken cancellationToken) { throw null; }
        protected virtual System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> PostProcessChangesAsync(Microsoft.CodeAnalysis.Document document, System.Threading.CancellationToken cancellationToken) { throw null; }
        protected System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Solution> PostProcessChangesAsync(Microsoft.CodeAnalysis.Solution changedSolution, System.Threading.CancellationToken cancellationToken) { throw null; }
    }
    public abstract partial class CodeActionOperation
    {
        protected CodeActionOperation() { }
        public virtual string? Title { get { throw null; } }
        public virtual void Apply(Microsoft.CodeAnalysis.Workspace workspace, System.Threading.CancellationToken cancellationToken) { }
    }
    public abstract partial class CodeActionWithOptions : Microsoft.CodeAnalysis.CodeActions.CodeAction
    {
        protected CodeActionWithOptions() { }
        protected abstract System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>> ComputeOperationsAsync(object options, System.Threading.CancellationToken cancellationToken);
        protected override System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>> ComputeOperationsAsync(System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeActions.CodeActionOperation>?> GetOperationsAsync(object? options, System.Threading.CancellationToken cancellationToken) { throw null; }
        public abstract object? GetOptions(System.Threading.CancellationToken cancellationToken);
    }
    public static partial class ConflictAnnotation
    {
        public const string Kind = "CodeAction_Conflict";
        public static Microsoft.CodeAnalysis.SyntaxAnnotation Create(string description) { throw null; }
        public static string? GetDescription(Microsoft.CodeAnalysis.SyntaxAnnotation annotation) { throw null; }
    }
    public sealed partial class OpenDocumentOperation : Microsoft.CodeAnalysis.CodeActions.CodeActionOperation
    {
        public OpenDocumentOperation(Microsoft.CodeAnalysis.DocumentId documentId, bool activateIfAlreadyOpen = false) { }
        public Microsoft.CodeAnalysis.DocumentId DocumentId { get { throw null; } }
        public override void Apply(Microsoft.CodeAnalysis.Workspace workspace, System.Threading.CancellationToken cancellationToken) { }
    }
    public abstract partial class PreviewOperation : Microsoft.CodeAnalysis.CodeActions.CodeActionOperation
    {
        protected PreviewOperation() { }
        public abstract System.Threading.Tasks.Task<object?> GetPreviewAsync(System.Threading.CancellationToken cancellationToken);
    }
    public static partial class RenameAnnotation
    {
        public const string Kind = "CodeAction_Rename";
        public static Microsoft.CodeAnalysis.SyntaxAnnotation Create() { throw null; }
    }
    public static partial class WarningAnnotation
    {
        public const string Kind = "CodeAction_Warning";
        public static Microsoft.CodeAnalysis.SyntaxAnnotation Create(string description) { throw null; }
        public static string? GetDescription(Microsoft.CodeAnalysis.SyntaxAnnotation annotation) { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.CodeFixes
{
    public partial struct CodeFixContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public CodeFixContext(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Diagnostic diagnostic, System.Action<Microsoft.CodeAnalysis.CodeActions.CodeAction, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> registerCodeFix, System.Threading.CancellationToken cancellationToken) { throw null; }
        public CodeFixContext(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Text.TextSpan span, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> diagnostics, System.Action<Microsoft.CodeAnalysis.CodeActions.CodeAction, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> registerCodeFix, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> Diagnostics { get { throw null; } }
        public Microsoft.CodeAnalysis.Document Document { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public void RegisterCodeFix(Microsoft.CodeAnalysis.CodeActions.CodeAction action, Microsoft.CodeAnalysis.Diagnostic diagnostic) { }
        public void RegisterCodeFix(Microsoft.CodeAnalysis.CodeActions.CodeAction action, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic> diagnostics) { }
        public void RegisterCodeFix(Microsoft.CodeAnalysis.CodeActions.CodeAction action, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> diagnostics) { }
    }
    public abstract partial class CodeFixProvider
    {
        protected CodeFixProvider() { }
        public abstract System.Collections.Immutable.ImmutableArray<string> FixableDiagnosticIds { get; }
        public virtual Microsoft.CodeAnalysis.CodeFixes.FixAllProvider? GetFixAllProvider() { throw null; }
        public abstract System.Threading.Tasks.Task RegisterCodeFixesAsync(Microsoft.CodeAnalysis.CodeFixes.CodeFixContext context);
    }
    public abstract partial class DocumentBasedFixAllProvider : Microsoft.CodeAnalysis.CodeFixes.FixAllProvider
    {
        protected DocumentBasedFixAllProvider() { }
        protected abstract System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document?> FixAllAsync(Microsoft.CodeAnalysis.CodeFixes.FixAllContext fixAllContext, Microsoft.CodeAnalysis.Document document, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic> diagnostics);
        protected virtual string GetFixAllTitle(Microsoft.CodeAnalysis.CodeFixes.FixAllContext fixAllContext) { throw null; }
        public sealed override System.Threading.Tasks.Task<Microsoft.CodeAnalysis.CodeActions.CodeAction?> GetFixAsync(Microsoft.CodeAnalysis.CodeFixes.FixAllContext fixAllContext) { throw null; }
        public sealed override System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeFixes.FixAllScope> GetSupportedFixAllScopes() { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    [System.Composition.MetadataAttributeAttribute]
    public sealed partial class ExportCodeFixProviderAttribute : System.Composition.ExportAttribute
    {
        public ExportCodeFixProviderAttribute(string firstLanguage, params string[] additionalLanguages) { }
        public string[] Languages { get { throw null; } }
        public string? Name { get { throw null; } set { } }
    }
    public partial class FixAllContext
    {
        public FixAllContext(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider codeFixProvider, Microsoft.CodeAnalysis.CodeFixes.FixAllScope scope, string codeActionEquivalenceKey, System.Collections.Generic.IEnumerable<string> diagnosticIds, Microsoft.CodeAnalysis.CodeFixes.FixAllContext.DiagnosticProvider fixAllDiagnosticProvider, System.Threading.CancellationToken cancellationToken) { }
        public FixAllContext(Microsoft.CodeAnalysis.Project project, Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider codeFixProvider, Microsoft.CodeAnalysis.CodeFixes.FixAllScope scope, string codeActionEquivalenceKey, System.Collections.Generic.IEnumerable<string> diagnosticIds, Microsoft.CodeAnalysis.CodeFixes.FixAllContext.DiagnosticProvider fixAllDiagnosticProvider, System.Threading.CancellationToken cancellationToken) { }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public string? CodeActionEquivalenceKey { get { throw null; } }
        public Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider CodeFixProvider { get { throw null; } }
        public System.Collections.Immutable.ImmutableHashSet<string> DiagnosticIds { get { throw null; } }
        public Microsoft.CodeAnalysis.Document? Document { get { throw null; } }
        public Microsoft.CodeAnalysis.Project Project { get { throw null; } }
        public Microsoft.CodeAnalysis.CodeFixes.FixAllScope Scope { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution Solution { get { throw null; } }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetAllDiagnosticsAsync(Microsoft.CodeAnalysis.Project project) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetDocumentDiagnosticsAsync(Microsoft.CodeAnalysis.Document document) { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>> GetProjectDiagnosticsAsync(Microsoft.CodeAnalysis.Project project) { throw null; }
        public Microsoft.CodeAnalysis.CodeFixes.FixAllContext WithCancellationToken(System.Threading.CancellationToken cancellationToken) { throw null; }
        public abstract partial class DiagnosticProvider
        {
            protected DiagnosticProvider() { }
            public abstract System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic>> GetAllDiagnosticsAsync(Microsoft.CodeAnalysis.Project project, System.Threading.CancellationToken cancellationToken);
            public abstract System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic>> GetDocumentDiagnosticsAsync(Microsoft.CodeAnalysis.Document document, System.Threading.CancellationToken cancellationToken);
            public abstract System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Diagnostic>> GetProjectDiagnosticsAsync(Microsoft.CodeAnalysis.Project project, System.Threading.CancellationToken cancellationToken);
        }
    }
    public abstract partial class FixAllProvider
    {
        protected FixAllProvider() { }
        public static Microsoft.CodeAnalysis.CodeFixes.FixAllProvider Create(System.Func<Microsoft.CodeAnalysis.CodeFixes.FixAllContext, Microsoft.CodeAnalysis.Document, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Diagnostic>, System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document?>> fixAllAsync) { throw null; }
        public abstract System.Threading.Tasks.Task<Microsoft.CodeAnalysis.CodeActions.CodeAction?> GetFixAsync(Microsoft.CodeAnalysis.CodeFixes.FixAllContext fixAllContext);
        public virtual System.Collections.Generic.IEnumerable<string> GetSupportedFixAllDiagnosticIds(Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider originalCodeFixProvider) { throw null; }
        public virtual System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CodeFixes.FixAllScope> GetSupportedFixAllScopes() { throw null; }
    }
    public enum FixAllScope
    {
        Document = 0,
        Project = 1,
        Solution = 2,
        Custom = 3,
    }
    public static partial class WellKnownFixAllProviders
    {
        public static Microsoft.CodeAnalysis.CodeFixes.FixAllProvider BatchFixer { get { throw null; } }
    }
}
namespace Microsoft.CodeAnalysis.CodeRefactorings
{
    public partial struct CodeRefactoringContext
    {
        private object _dummy;
        private int _dummyPrimitive;
        public CodeRefactoringContext(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Text.TextSpan span, System.Action<Microsoft.CodeAnalysis.CodeActions.CodeAction> registerRefactoring, System.Threading.CancellationToken cancellationToken) { throw null; }
        public System.Threading.CancellationToken CancellationToken { get { throw null; } }
        public Microsoft.CodeAnalysis.Document Document { get { throw null; } }
        public Microsoft.CodeAnalysis.Text.TextSpan Span { get { throw null; } }
        public void RegisterRefactoring(Microsoft.CodeAnalysis.CodeActions.CodeAction action) { }
    }
    public abstract partial class CodeRefactoringProvider
    {
        protected CodeRefactoringProvider() { }
        public abstract System.Threading.Tasks.Task ComputeRefactoringsAsync(Microsoft.CodeAnalysis.CodeRefactorings.CodeRefactoringContext context);
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    [System.Composition.MetadataAttributeAttribute]
    public sealed partial class ExportCodeRefactoringProviderAttribute : System.Composition.ExportAttribute
    {
        public ExportCodeRefactoringProviderAttribute(string firstLanguage, params string[] additionalLanguages) { }
        public string[] Languages { get { throw null; } }
        public string? Name { get { throw null; } set { } }
    }
}
namespace Microsoft.CodeAnalysis.CodeStyle
{
    public partial class CodeStyleOptions
    {
        public static readonly Microsoft.CodeAnalysis.Options.PerLanguageOption<Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<bool>> PreferIntrinsicPredefinedTypeKeywordInDeclaration;
        public static readonly Microsoft.CodeAnalysis.Options.PerLanguageOption<Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<bool>> PreferIntrinsicPredefinedTypeKeywordInMemberAccess;
        public static readonly Microsoft.CodeAnalysis.Options.PerLanguageOption<Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<bool>> QualifyEventAccess;
        public static readonly Microsoft.CodeAnalysis.Options.PerLanguageOption<Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<bool>> QualifyFieldAccess;
        public static readonly Microsoft.CodeAnalysis.Options.PerLanguageOption<Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<bool>> QualifyMethodAccess;
        public static readonly Microsoft.CodeAnalysis.Options.PerLanguageOption<Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<bool>> QualifyPropertyAccess;
        public CodeStyleOptions() { }
    }
    public sealed partial class CodeStyleOption<T> : System.IEquatable<Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<T>>
    {
        public CodeStyleOption(T value, Microsoft.CodeAnalysis.CodeStyle.NotificationOption notification) { }
        public static Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<T> Default { get { throw null; } }
        public Microsoft.CodeAnalysis.CodeStyle.NotificationOption Notification { get { throw null; } [System.ObsoleteAttribute("Modifying a CodeStyleOption<T> is not supported.", true)] set { } }
        public T Value { get { throw null; } [System.ObsoleteAttribute("Modifying a CodeStyleOption<T> is not supported.", true)] set { } }
        public bool Equals(Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<T> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public static Microsoft.CodeAnalysis.CodeStyle.CodeStyleOption<T> FromXElement(System.Xml.Linq.XElement element) { throw null; }
        public override int GetHashCode() { throw null; }
        public System.Xml.Linq.XElement ToXElement() { throw null; }
    }
    public partial class NotificationOption
    {
        internal NotificationOption() { }
        public static readonly Microsoft.CodeAnalysis.CodeStyle.NotificationOption Error;
        public static readonly Microsoft.CodeAnalysis.CodeStyle.NotificationOption None;
        public static readonly Microsoft.CodeAnalysis.CodeStyle.NotificationOption Silent;
        public static readonly Microsoft.CodeAnalysis.CodeStyle.NotificationOption Suggestion;
        public static readonly Microsoft.CodeAnalysis.CodeStyle.NotificationOption Warning;
        public string Name { get { throw null; } set { } }
        public Microsoft.CodeAnalysis.ReportDiagnostic Severity { get { throw null; } set { } }
        [System.ObsoleteAttribute("Use Severity instead.")]
        public Microsoft.CodeAnalysis.DiagnosticSeverity Value { get { throw null; } set { } }
        public override string ToString() { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.Differencing
{
    public enum EditKind
    {
        None = 0,
        Update = 1,
        Insert = 2,
        Delete = 3,
        Move = 4,
        Reorder = 5,
    }
    public sealed partial class EditScript<TNode>
    {
        internal EditScript() { }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Differencing.Edit<TNode>> Edits { get { throw null; } }
        public Microsoft.CodeAnalysis.Differencing.Match<TNode> Match { get { throw null; } }
    }
    public partial struct Edit<TNode> : System.IEquatable<Microsoft.CodeAnalysis.Differencing.Edit<TNode>>
    {
        private readonly TNode _oldNode;
        private readonly TNode _newNode;
        private object _dummy;
        private int _dummyPrimitive;
        public Microsoft.CodeAnalysis.Differencing.EditKind Kind { get { throw null; } }
        public TNode NewNode { get { throw null; } }
        public TNode OldNode { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Differencing.Edit<TNode> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public sealed partial class Match<TNode>
    {
        internal Match() { }
        public Microsoft.CodeAnalysis.Differencing.TreeComparer<TNode> Comparer { get { throw null; } }
        public System.Collections.Generic.IReadOnlyDictionary<TNode, TNode> Matches { get { throw null; } }
        public TNode NewRoot { get { throw null; } }
        public TNode OldRoot { get { throw null; } }
        public System.Collections.Generic.IReadOnlyDictionary<TNode, TNode> ReverseMatches { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Differencing.Edit<TNode>> GetSequenceEdits(System.Collections.Generic.IEnumerable<TNode> oldNodes, System.Collections.Generic.IEnumerable<TNode> newNodes) { throw null; }
        public Microsoft.CodeAnalysis.Differencing.EditScript<TNode> GetTreeEdits() { throw null; }
        public bool TryGetNewNode(TNode oldNode, out TNode newNode) { throw null; }
        public bool TryGetOldNode(TNode newNode, out TNode oldNode) { throw null; }
    }
    public abstract partial class TreeComparer<TNode>
    {
        protected TreeComparer() { }
        protected internal abstract int LabelCount { get; }
        public Microsoft.CodeAnalysis.Differencing.EditScript<TNode> ComputeEditScript(TNode oldRoot, TNode newRoot) { throw null; }
        public Microsoft.CodeAnalysis.Differencing.Match<TNode> ComputeMatch(TNode oldRoot, TNode newRoot, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TNode, TNode>>? knownMatches = null) { throw null; }
        protected internal abstract System.Collections.Generic.IEnumerable<TNode>? GetChildren(TNode node);
        protected internal abstract System.Collections.Generic.IEnumerable<TNode> GetDescendants(TNode node);
        public abstract double GetDistance(TNode oldNode, TNode newNode);
        protected internal abstract int GetLabel(TNode node);
        protected internal abstract Microsoft.CodeAnalysis.Text.TextSpan GetSpan(TNode node);
        protected internal abstract int TiedToAncestor(int label);
        protected internal abstract bool TreesEqual(TNode oldNode, TNode newNode);
        protected internal abstract bool TryGetParent(TNode node, out TNode parent);
        public abstract bool ValuesEqual(TNode oldNode, TNode newNode);
    }
}
namespace Microsoft.CodeAnalysis.Editing
{
    public enum DeclarationKind
    {
        None = 0,
        CompilationUnit = 1,
        Class = 2,
        Struct = 3,
        Interface = 4,
        Enum = 5,
        Delegate = 6,
        Method = 7,
        Operator = 8,
        ConversionOperator = 9,
        Constructor = 10,
        Destructor = 11,
        Field = 12,
        Property = 13,
        Indexer = 14,
        EnumMember = 15,
        Event = 16,
        CustomEvent = 17,
        Namespace = 18,
        NamespaceImport = 19,
        Parameter = 20,
        Variable = 21,
        Attribute = 22,
        LambdaExpression = 23,
        GetAccessor = 24,
        SetAccessor = 25,
        AddAccessor = 26,
        RemoveAccessor = 27,
        RaiseAccessor = 28,
        [System.ObsoleteAttribute("This value is not used. Use Class instead.")]
        RecordClass = 29,
    }
    public partial struct DeclarationModifiers : System.IEquatable<Microsoft.CodeAnalysis.Editing.DeclarationModifiers>
    {
        private int _dummyPrimitive;
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Abstract { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Async { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Const { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Extern { get { throw null; } }
        public bool IsAbstract { get { throw null; } }
        public bool IsAsync { get { throw null; } }
        public bool IsConst { get { throw null; } }
        public bool IsExtern { get { throw null; } }
        public bool IsNew { get { throw null; } }
        public bool IsOverride { get { throw null; } }
        public bool IsPartial { get { throw null; } }
        public bool IsReadOnly { get { throw null; } }
        public bool IsRef { get { throw null; } }
        public bool IsSealed { get { throw null; } }
        public bool IsStatic { get { throw null; } }
        public bool IsUnsafe { get { throw null; } }
        public bool IsVirtual { get { throw null; } }
        public bool IsVolatile { get { throw null; } }
        public bool IsWithEvents { get { throw null; } }
        public bool IsWriteOnly { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers New { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers None { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Override { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Partial { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers ReadOnly { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Ref { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Sealed { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Static { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Unsafe { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Virtual { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers Volatile { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithEvents { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers WriteOnly { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers From(Microsoft.CodeAnalysis.ISymbol symbol) { throw null; }
        public override int GetHashCode() { throw null; }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers operator +(Microsoft.CodeAnalysis.Editing.DeclarationModifiers left, Microsoft.CodeAnalysis.Editing.DeclarationModifiers right) { throw null; }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers operator &(Microsoft.CodeAnalysis.Editing.DeclarationModifiers left, Microsoft.CodeAnalysis.Editing.DeclarationModifiers right) { throw null; }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers operator |(Microsoft.CodeAnalysis.Editing.DeclarationModifiers left, Microsoft.CodeAnalysis.Editing.DeclarationModifiers right) { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Editing.DeclarationModifiers left, Microsoft.CodeAnalysis.Editing.DeclarationModifiers right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Editing.DeclarationModifiers left, Microsoft.CodeAnalysis.Editing.DeclarationModifiers right) { throw null; }
        public static Microsoft.CodeAnalysis.Editing.DeclarationModifiers operator -(Microsoft.CodeAnalysis.Editing.DeclarationModifiers left, Microsoft.CodeAnalysis.Editing.DeclarationModifiers right) { throw null; }
        public override string ToString() { throw null; }
        public static bool TryParse(string value, out Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithAsync(bool isAsync) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsAbstract(bool isAbstract) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsConst(bool isConst) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsExtern(bool isExtern) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsNew(bool isNew) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsOverride(bool isOverride) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsReadOnly(bool isReadOnly) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsRef(bool isRef) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsSealed(bool isSealed) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsStatic(bool isStatic) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsUnsafe(bool isUnsafe) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsVirtual(bool isVirtual) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsVolatile(bool isVolatile) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithIsWriteOnly(bool isWriteOnly) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithPartial(bool isPartial) { throw null; }
        public Microsoft.CodeAnalysis.Editing.DeclarationModifiers WithWithEvents(bool withEvents) { throw null; }
    }
    public partial class DocumentEditor : Microsoft.CodeAnalysis.Editing.SyntaxEditor
    {
        internal DocumentEditor() : base (default(Microsoft.CodeAnalysis.SyntaxNode), default(Microsoft.CodeAnalysis.Workspace)) { }
        public Microsoft.CodeAnalysis.Document OriginalDocument { get { throw null; } }
        public Microsoft.CodeAnalysis.SemanticModel SemanticModel { get { throw null; } }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Editing.DocumentEditor> CreateAsync(Microsoft.CodeAnalysis.Document document, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public Microsoft.CodeAnalysis.Document GetChangedDocument() { throw null; }
    }
    public static partial class ImportAdder
    {
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> AddImportsAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> AddImportsAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.SyntaxAnnotation annotation, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> AddImportsAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Text.TextSpan span, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> AddImportsAsync(Microsoft.CodeAnalysis.Document document, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextSpan> spans, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public enum OperatorKind
    {
        ImplicitConversion = 0,
        ExplicitConversion = 1,
        Addition = 2,
        BitwiseAnd = 3,
        BitwiseOr = 4,
        Decrement = 5,
        Division = 6,
        Equality = 7,
        ExclusiveOr = 8,
        False = 9,
        GreaterThan = 10,
        GreaterThanOrEqual = 11,
        Increment = 12,
        Inequality = 13,
        LeftShift = 14,
        LessThan = 15,
        LessThanOrEqual = 16,
        LogicalNot = 17,
        Modulus = 18,
        Multiply = 19,
        OnesComplement = 20,
        RightShift = 21,
        Subtraction = 22,
        True = 23,
        UnaryNegation = 24,
        UnaryPlus = 25,
    }
    public partial class SolutionEditor
    {
        public SolutionEditor(Microsoft.CodeAnalysis.Solution solution) { }
        public Microsoft.CodeAnalysis.Solution OriginalSolution { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution GetChangedSolution() { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Editing.DocumentEditor> GetDocumentEditorAsync(Microsoft.CodeAnalysis.DocumentId id, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    [System.FlagsAttribute]
    public enum SpecialTypeConstraintKind
    {
        None = 0,
        ReferenceType = 1,
        ValueType = 2,
        Constructor = 4,
    }
    public sealed partial class SymbolEditor
    {
        internal SymbolEditor() { }
        public Microsoft.CodeAnalysis.Solution ChangedSolution { get { throw null; } }
        public Microsoft.CodeAnalysis.Solution OriginalSolution { get { throw null; } }
        public static Microsoft.CodeAnalysis.Editing.SymbolEditor Create(Microsoft.CodeAnalysis.Document document) { throw null; }
        public static Microsoft.CodeAnalysis.Editing.SymbolEditor Create(Microsoft.CodeAnalysis.Solution solution) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditAllDeclarationsAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Editing.SymbolEditor.AsyncDeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditAllDeclarationsAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Editing.SymbolEditor.DeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditOneDeclarationAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Editing.SymbolEditor.AsyncDeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditOneDeclarationAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Editing.SymbolEditor.DeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditOneDeclarationAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.ISymbol member, Microsoft.CodeAnalysis.Editing.SymbolEditor.AsyncDeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditOneDeclarationAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.ISymbol member, Microsoft.CodeAnalysis.Editing.SymbolEditor.DeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditOneDeclarationAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Location location, Microsoft.CodeAnalysis.Editing.SymbolEditor.AsyncDeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> EditOneDeclarationAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Location location, Microsoft.CodeAnalysis.Editing.SymbolEditor.DeclarationEditAction editAction, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Document> GetChangedDocuments() { throw null; }
        public System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode>> GetCurrentDeclarationsAsync(Microsoft.CodeAnalysis.ISymbol symbol, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> GetCurrentSymbolAsync(Microsoft.CodeAnalysis.ISymbol symbol, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public delegate System.Threading.Tasks.Task AsyncDeclarationEditAction(Microsoft.CodeAnalysis.Editing.DocumentEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, System.Threading.CancellationToken cancellationToken);
        public delegate void DeclarationEditAction(Microsoft.CodeAnalysis.Editing.DocumentEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration);
    }
    public static partial class SymbolEditorExtensions
    {
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SyntaxNode> GetBaseOrInterfaceDeclarationReferenceAsync(this Microsoft.CodeAnalysis.Editing.SymbolEditor editor, Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.ITypeSymbol baseOrInterfaceType, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> SetBaseTypeAsync(this Microsoft.CodeAnalysis.Editing.SymbolEditor editor, Microsoft.CodeAnalysis.INamedTypeSymbol symbol, Microsoft.CodeAnalysis.ITypeSymbol newBaseType, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> SetBaseTypeAsync(this Microsoft.CodeAnalysis.Editing.SymbolEditor editor, Microsoft.CodeAnalysis.INamedTypeSymbol symbol, System.Func<Microsoft.CodeAnalysis.Editing.SyntaxGenerator, Microsoft.CodeAnalysis.SyntaxNode> getNewBaseType, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public partial class SyntaxEditor
    {
        public SyntaxEditor(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.Workspace workspace) { }
        public Microsoft.CodeAnalysis.Editing.SyntaxGenerator Generator { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode OriginalRoot { get { throw null; } }
        public Microsoft.CodeAnalysis.SyntaxNode GetChangedRoot() { throw null; }
        public void InsertAfter(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxNode newNode) { }
        public void InsertAfter(Microsoft.CodeAnalysis.SyntaxNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newNodes) { }
        public void InsertBefore(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxNode newNode) { }
        public void InsertBefore(Microsoft.CodeAnalysis.SyntaxNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newNodes) { }
        public void RemoveNode(Microsoft.CodeAnalysis.SyntaxNode node) { }
        public void RemoveNode(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxRemoveOptions options) { }
        public void ReplaceNode(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxNode newNode) { }
        public void ReplaceNode(Microsoft.CodeAnalysis.SyntaxNode node, System.Func<Microsoft.CodeAnalysis.SyntaxNode, Microsoft.CodeAnalysis.Editing.SyntaxGenerator, Microsoft.CodeAnalysis.SyntaxNode> computeReplacement) { }
        public void TrackNode(Microsoft.CodeAnalysis.SyntaxNode node) { }
    }
    public static partial class SyntaxEditorExtensions
    {
        public static void AddAttribute(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode attribute) { }
        public static void AddAttributeArgument(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode attributeDeclaration, Microsoft.CodeAnalysis.SyntaxNode attributeArgument) { }
        public static void AddBaseType(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode baseType) { }
        public static void AddInterfaceType(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode interfaceType) { }
        public static void AddMember(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode member) { }
        public static void AddParameter(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode parameter) { }
        public static void AddReturnAttribute(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode attribute) { }
        public static void InsertMembers(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members) { }
        public static void InsertParameter(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, int index, Microsoft.CodeAnalysis.SyntaxNode parameter) { }
        public static void SetAccessibility(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.Accessibility accessibility) { }
        public static void SetExpression(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode expression) { }
        public static void SetGetAccessorStatements(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { }
        public static void SetModifiers(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers) { }
        public static void SetName(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, string name) { }
        public static void SetSetAccessorStatements(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { }
        public static void SetStatements(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { }
        public static void SetType(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode type) { }
        public static void SetTypeConstraint(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, string typeParameterName, Microsoft.CodeAnalysis.Editing.SpecialTypeConstraintKind kind, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> types) { }
        public static void SetTypeParameters(this Microsoft.CodeAnalysis.Editing.SyntaxEditor editor, Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<string> typeParameters) { }
    }
    public abstract partial class SyntaxGenerator : Microsoft.CodeAnalysis.Host.ILanguageService
    {
        public static Microsoft.CodeAnalysis.SyntaxRemoveOptions DefaultRemoveOptions;
        protected SyntaxGenerator() { }
        internal abstract Microsoft.CodeAnalysis.SyntaxTrivia CarriageReturnLineFeed { get; }
        internal abstract Microsoft.CodeAnalysis.SyntaxTrivia ElasticCarriageReturnLineFeed { get; }
        internal abstract bool RequiresExplicitImplementationForInterfaceMembers { get; }
        internal abstract Microsoft.CodeAnalysis.Editing.SyntaxGeneratorInternal SyntaxGeneratorInternal { get; }
        public Microsoft.CodeAnalysis.SyntaxNode AddAccessors(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> accessors) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddAttributeArguments(Microsoft.CodeAnalysis.SyntaxNode attributeDeclaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributeArguments) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, params Microsoft.CodeAnalysis.SyntaxNode[] attributes) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributes) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode AddBaseType(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode baseType);
        public abstract Microsoft.CodeAnalysis.SyntaxNode AddEventHandler(Microsoft.CodeAnalysis.SyntaxNode @event, Microsoft.CodeAnalysis.SyntaxNode handler);
        public abstract Microsoft.CodeAnalysis.SyntaxNode AddExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode AddInterfaceType(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode interfaceType);
        public Microsoft.CodeAnalysis.SyntaxNode AddMembers(Microsoft.CodeAnalysis.SyntaxNode declaration, params Microsoft.CodeAnalysis.SyntaxNode[] members) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddMembers(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddNamespaceImports(Microsoft.CodeAnalysis.SyntaxNode declaration, params Microsoft.CodeAnalysis.SyntaxNode[] imports) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddNamespaceImports(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> imports) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddParameters(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddReturnAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, params Microsoft.CodeAnalysis.SyntaxNode[] attributes) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddReturnAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributes) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AddSwitchSections(Microsoft.CodeAnalysis.SyntaxNode switchStatement, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> switchSections) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AliasImportDeclaration(string aliasIdentifierName, Microsoft.CodeAnalysis.INamespaceOrTypeSymbol symbol) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode AliasImportDeclaration(string aliasIdentifierName, Microsoft.CodeAnalysis.SyntaxNode name);
        public Microsoft.CodeAnalysis.SyntaxNode Argument(Microsoft.CodeAnalysis.RefKind refKind, Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode Argument(Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode Argument(string name, Microsoft.CodeAnalysis.RefKind refKind, Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ArrayCreationExpression(Microsoft.CodeAnalysis.SyntaxNode elementType, Microsoft.CodeAnalysis.SyntaxNode size);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ArrayCreationExpression(Microsoft.CodeAnalysis.SyntaxNode elementType, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> elements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ArrayTypeExpression(Microsoft.CodeAnalysis.SyntaxNode type);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode AsInterfaceMember(Microsoft.CodeAnalysis.SyntaxNode member);
        public Microsoft.CodeAnalysis.SyntaxNode AsPrivateInterfaceImplementation(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode interfaceType) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode AsPrivateInterfaceImplementation(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode interfaceType, string interfaceMemberName);
        public Microsoft.CodeAnalysis.SyntaxNode AsPublicInterfaceImplementation(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode interfaceType) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode AsPublicInterfaceImplementation(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode interfaceType, string interfaceMemberName);
        public abstract Microsoft.CodeAnalysis.SyntaxNode AssignmentStatement(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public Microsoft.CodeAnalysis.SyntaxNode Attribute(Microsoft.CodeAnalysis.AttributeData attribute) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode Attribute(Microsoft.CodeAnalysis.SyntaxNode name, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributeArguments = null);
        public Microsoft.CodeAnalysis.SyntaxNode Attribute(string name, params Microsoft.CodeAnalysis.SyntaxNode[] attributeArguments) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode Attribute(string name, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributeArguments = null) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode AttributeArgument(Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode AttributeArgument(string name, Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode AwaitExpression(Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode BaseExpression();
        public abstract Microsoft.CodeAnalysis.SyntaxNode BitwiseAndExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode BitwiseNotExpression(Microsoft.CodeAnalysis.SyntaxNode operand);
        public abstract Microsoft.CodeAnalysis.SyntaxNode BitwiseOrExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public Microsoft.CodeAnalysis.SyntaxNode CastExpression(Microsoft.CodeAnalysis.ITypeSymbol type, Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode CastExpression(Microsoft.CodeAnalysis.SyntaxNode type, Microsoft.CodeAnalysis.SyntaxNode expression);
        public Microsoft.CodeAnalysis.SyntaxNode CatchClause(Microsoft.CodeAnalysis.ITypeSymbol type, string identifier, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode CatchClause(Microsoft.CodeAnalysis.SyntaxNode type, string identifier, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ClassDeclaration(string name, System.Collections.Generic.IEnumerable<string> typeParameters = null, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), Microsoft.CodeAnalysis.SyntaxNode baseType = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> interfaceTypes = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members = null);
        public abstract TNode ClearTrivia<TNode>(TNode node) where TNode : Microsoft.CodeAnalysis.SyntaxNode;
        public abstract Microsoft.CodeAnalysis.SyntaxNode CoalesceExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public Microsoft.CodeAnalysis.SyntaxNode CompilationUnit(params Microsoft.CodeAnalysis.SyntaxNode[] declarations) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode CompilationUnit(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> declarations);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ConditionalAccessExpression(Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SyntaxNode whenNotNull);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ConditionalExpression(Microsoft.CodeAnalysis.SyntaxNode condition, Microsoft.CodeAnalysis.SyntaxNode whenTrue, Microsoft.CodeAnalysis.SyntaxNode whenFalse);
        public Microsoft.CodeAnalysis.SyntaxNode ConstructorDeclaration(Microsoft.CodeAnalysis.IMethodSymbol constructorMethod, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> baseConstructorArguments = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ConstructorDeclaration(string containingTypeName = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters = null, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> baseConstructorArguments = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null);
        public Microsoft.CodeAnalysis.SyntaxNode ConvertExpression(Microsoft.CodeAnalysis.ITypeSymbol type, Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ConvertExpression(Microsoft.CodeAnalysis.SyntaxNode type, Microsoft.CodeAnalysis.SyntaxNode expression);
        internal abstract Microsoft.CodeAnalysis.SyntaxToken CreateInterpolatedStringEndToken();
        internal abstract Microsoft.CodeAnalysis.SyntaxToken CreateInterpolatedStringStartToken(bool isVerbatim);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode CreateTupleType(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> elements);
        public Microsoft.CodeAnalysis.SyntaxNode CustomEventDeclaration(Microsoft.CodeAnalysis.IEventSymbol symbol, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> addAccessorStatements = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> removeAccessorStatements = null) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode CustomEventDeclaration(string name, Microsoft.CodeAnalysis.SyntaxNode type, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> addAccessorStatements = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> removeAccessorStatements = null);
        public Microsoft.CodeAnalysis.SyntaxNode Declaration(Microsoft.CodeAnalysis.ISymbol symbol) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode DefaultExpression(Microsoft.CodeAnalysis.ITypeSymbol type);
        public abstract Microsoft.CodeAnalysis.SyntaxNode DefaultExpression(Microsoft.CodeAnalysis.SyntaxNode type);
        public abstract Microsoft.CodeAnalysis.SyntaxNode DefaultSwitchSection(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode DelegateDeclaration(string name, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters = null, System.Collections.Generic.IEnumerable<string> typeParameters = null, Microsoft.CodeAnalysis.SyntaxNode returnType = null, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers));
        public abstract Microsoft.CodeAnalysis.SyntaxNode DivideExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode DocumentationCommentTrivia(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> nodes, Microsoft.CodeAnalysis.SyntaxTriviaList trailingTrivia, Microsoft.CodeAnalysis.SyntaxTrivia lastWhitespaceTrivia, string endOfLineString);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode DocumentationCommentTriviaWithUpdatedContent(Microsoft.CodeAnalysis.SyntaxTrivia trivia, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> content);
        public Microsoft.CodeAnalysis.SyntaxNode DottedName(string dottedName) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode ElementAccessExpression(Microsoft.CodeAnalysis.SyntaxNode expression, params Microsoft.CodeAnalysis.SyntaxNode[] arguments) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ElementAccessExpression(Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> arguments);
        public Microsoft.CodeAnalysis.SyntaxNode ElementBindingExpression(params Microsoft.CodeAnalysis.SyntaxNode[] arguments) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ElementBindingExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> arguments);
        public abstract Microsoft.CodeAnalysis.SyntaxNode EnumDeclaration(string name, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members = null);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode EnumDeclaration(string name, Microsoft.CodeAnalysis.SyntaxNode underlyingType, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members = null);
        public abstract Microsoft.CodeAnalysis.SyntaxNode EnumMember(string name, Microsoft.CodeAnalysis.SyntaxNode expression = null);
        public Microsoft.CodeAnalysis.SyntaxNode EventDeclaration(Microsoft.CodeAnalysis.IEventSymbol symbol) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode EventDeclaration(string name, Microsoft.CodeAnalysis.SyntaxNode type, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers));
        public abstract Microsoft.CodeAnalysis.SyntaxNode ExitSwitchStatement();
        public abstract Microsoft.CodeAnalysis.SyntaxNode ExpressionStatement(Microsoft.CodeAnalysis.SyntaxNode expression);
        public Microsoft.CodeAnalysis.SyntaxNode FalseLiteralExpression() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode FieldDeclaration(Microsoft.CodeAnalysis.IFieldSymbol field) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode FieldDeclaration(Microsoft.CodeAnalysis.IFieldSymbol field, Microsoft.CodeAnalysis.SyntaxNode initializer) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode FieldDeclaration(string name, Microsoft.CodeAnalysis.SyntaxNode type, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), Microsoft.CodeAnalysis.SyntaxNode initializer = null);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode GenericName(Microsoft.CodeAnalysis.SyntaxToken identifier, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> typeArguments);
        public Microsoft.CodeAnalysis.SyntaxNode GenericName(string identifier, params Microsoft.CodeAnalysis.ITypeSymbol[] typeArguments) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode GenericName(string identifier, params Microsoft.CodeAnalysis.SyntaxNode[] typeArguments) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode GenericName(string identifier, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ITypeSymbol> typeArguments) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode GenericName(string identifier, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> typeArguments);
        public abstract Microsoft.CodeAnalysis.Accessibility GetAccessibility(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public Microsoft.CodeAnalysis.SyntaxNode GetAccessor(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.Editing.DeclarationKind kind) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode GetAccessorDeclaration(Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetAccessors(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetAttributeArguments(Microsoft.CodeAnalysis.SyntaxNode attributeDeclaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetBaseAndInterfaceTypes(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public Microsoft.CodeAnalysis.SyntaxNode GetDeclaration(Microsoft.CodeAnalysis.SyntaxNode node) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode GetDeclaration(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.Editing.DeclarationKind kind) { throw null; }
        public abstract Microsoft.CodeAnalysis.Editing.DeclarationKind GetDeclarationKind(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract Microsoft.CodeAnalysis.SyntaxNode GetExpression(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public static Microsoft.CodeAnalysis.Editing.SyntaxGenerator GetGenerator(Microsoft.CodeAnalysis.Document document) { throw null; }
        public static Microsoft.CodeAnalysis.Editing.SyntaxGenerator GetGenerator(Microsoft.CodeAnalysis.Project project) { throw null; }
        public static Microsoft.CodeAnalysis.Editing.SyntaxGenerator GetGenerator(Microsoft.CodeAnalysis.Workspace workspace, string language) { throw null; }
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetGetAccessorStatements(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetMembers(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract Microsoft.CodeAnalysis.Editing.DeclarationModifiers GetModifiers(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract string GetName(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetNamespaceImports(Microsoft.CodeAnalysis.SyntaxNode declaration);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode GetParameterListNode(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetParameters(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetReturnAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetSetAccessorStatements(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetStatements(Microsoft.CodeAnalysis.SyntaxNode declaration);
        public abstract System.Collections.Generic.IReadOnlyList<Microsoft.CodeAnalysis.SyntaxNode> GetSwitchSections(Microsoft.CodeAnalysis.SyntaxNode switchStatement);
        public abstract Microsoft.CodeAnalysis.SyntaxNode GetType(Microsoft.CodeAnalysis.SyntaxNode declaration);
        internal abstract System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.SyntaxNode> GetTypeInheritance(Microsoft.CodeAnalysis.SyntaxNode declaration);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode GlobalAliasedName(Microsoft.CodeAnalysis.SyntaxNode name);
        public abstract Microsoft.CodeAnalysis.SyntaxNode GreaterThanExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode GreaterThanOrEqualExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode IdentifierName(Microsoft.CodeAnalysis.SyntaxToken identifier);
        public abstract Microsoft.CodeAnalysis.SyntaxNode IdentifierName(string identifier);
        public Microsoft.CodeAnalysis.SyntaxNode IfStatement(Microsoft.CodeAnalysis.SyntaxNode condition, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> trueStatements, Microsoft.CodeAnalysis.SyntaxNode falseStatement) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode IfStatement(Microsoft.CodeAnalysis.SyntaxNode condition, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> trueStatements, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> falseStatements = null);
        public Microsoft.CodeAnalysis.SyntaxNode IndexerDeclaration(Microsoft.CodeAnalysis.IPropertySymbol indexer, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> getAccessorStatements = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> setAccessorStatements = null) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode IndexerDeclaration(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters, Microsoft.CodeAnalysis.SyntaxNode type, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> getAccessorStatements = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> setAccessorStatements = null);
        protected int IndexOf<T>(System.Collections.Generic.IReadOnlyList<T> list, T element) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertAccessors(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> accessors);
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertAttributeArguments(Microsoft.CodeAnalysis.SyntaxNode attributeDeclaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributeArguments);
        public Microsoft.CodeAnalysis.SyntaxNode InsertAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, params Microsoft.CodeAnalysis.SyntaxNode[] attributes) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributes);
        public Microsoft.CodeAnalysis.SyntaxNode InsertMembers(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, params Microsoft.CodeAnalysis.SyntaxNode[] members) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertMembers(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members);
        public Microsoft.CodeAnalysis.SyntaxNode InsertNamespaceImports(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, params Microsoft.CodeAnalysis.SyntaxNode[] imports) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertNamespaceImports(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> imports);
        public virtual Microsoft.CodeAnalysis.SyntaxNode InsertNodesAfter(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newDeclarations) { throw null; }
        public virtual Microsoft.CodeAnalysis.SyntaxNode InsertNodesBefore(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newDeclarations) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertParameters(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters);
        public Microsoft.CodeAnalysis.SyntaxNode InsertReturnAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, params Microsoft.CodeAnalysis.SyntaxNode[] attributes) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertReturnAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> attributes);
        public abstract Microsoft.CodeAnalysis.SyntaxNode InsertSwitchSections(Microsoft.CodeAnalysis.SyntaxNode switchStatement, int index, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> switchSections);
        public abstract Microsoft.CodeAnalysis.SyntaxNode InterfaceDeclaration(string name, System.Collections.Generic.IEnumerable<string> typeParameters = null, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> interfaceTypes = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members = null);
        public Microsoft.CodeAnalysis.SyntaxNode InvocationExpression(Microsoft.CodeAnalysis.SyntaxNode expression, params Microsoft.CodeAnalysis.SyntaxNode[] arguments) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode InvocationExpression(Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> arguments);
        internal abstract bool IsRegularOrDocComment(Microsoft.CodeAnalysis.SyntaxTrivia trivia);
        public Microsoft.CodeAnalysis.SyntaxNode IsTypeExpression(Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.ITypeSymbol type) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode IsTypeExpression(Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SyntaxNode type);
        public Microsoft.CodeAnalysis.SyntaxNode LambdaParameter(string identifier, Microsoft.CodeAnalysis.ITypeSymbol type) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode LambdaParameter(string identifier, Microsoft.CodeAnalysis.SyntaxNode type = null);
        public abstract Microsoft.CodeAnalysis.SyntaxNode LessThanExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode LessThanOrEqualExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode LiteralExpression(object value);
        public Microsoft.CodeAnalysis.SyntaxNode LocalDeclarationStatement(Microsoft.CodeAnalysis.ITypeSymbol type, string name, Microsoft.CodeAnalysis.SyntaxNode initializer = null, bool isConst = false) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode LocalDeclarationStatement(Microsoft.CodeAnalysis.SyntaxNode type, string identifier, Microsoft.CodeAnalysis.SyntaxNode initializer = null, bool isConst = false);
        public Microsoft.CodeAnalysis.SyntaxNode LocalDeclarationStatement(string name, Microsoft.CodeAnalysis.SyntaxNode initializer) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode LockStatement(Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode LogicalAndExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode LogicalNotExpression(Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode LogicalOrExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public virtual Microsoft.CodeAnalysis.SyntaxNode MemberAccessExpression(Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SyntaxNode memberName) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode MemberAccessExpression(Microsoft.CodeAnalysis.SyntaxNode expression, string memberName) { throw null; }
        internal abstract Microsoft.CodeAnalysis.SyntaxNode MemberAccessExpressionWorker(Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SyntaxNode memberName);
        public abstract Microsoft.CodeAnalysis.SyntaxNode MemberBindingExpression(Microsoft.CodeAnalysis.SyntaxNode name);
        public Microsoft.CodeAnalysis.SyntaxNode MethodDeclaration(Microsoft.CodeAnalysis.IMethodSymbol method, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode MethodDeclaration(string name, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters = null, System.Collections.Generic.IEnumerable<string> typeParameters = null, Microsoft.CodeAnalysis.SyntaxNode returnType = null, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ModuloExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode MultiplyExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode NamedAnonymousObjectMemberDeclarator(Microsoft.CodeAnalysis.SyntaxNode identifier, Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode NameExpression(Microsoft.CodeAnalysis.INamespaceOrTypeSymbol namespaceOrTypeSymbol);
        public abstract Microsoft.CodeAnalysis.SyntaxNode NameOfExpression(Microsoft.CodeAnalysis.SyntaxNode expression);
        public Microsoft.CodeAnalysis.SyntaxNode NamespaceDeclaration(Microsoft.CodeAnalysis.SyntaxNode name, params Microsoft.CodeAnalysis.SyntaxNode[] declarations) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode NamespaceDeclaration(Microsoft.CodeAnalysis.SyntaxNode name, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> declarations);
        public Microsoft.CodeAnalysis.SyntaxNode NamespaceDeclaration(string name, params Microsoft.CodeAnalysis.SyntaxNode[] declarations) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode NamespaceDeclaration(string name, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> declarations) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode NamespaceImportDeclaration(Microsoft.CodeAnalysis.SyntaxNode name);
        public Microsoft.CodeAnalysis.SyntaxNode NamespaceImportDeclaration(string name) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode NegateExpression(Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode NullableTypeExpression(Microsoft.CodeAnalysis.SyntaxNode type);
        public Microsoft.CodeAnalysis.SyntaxNode NullLiteralExpression() { throw null; }
        internal abstract Microsoft.CodeAnalysis.SyntaxToken NumericLiteralToken(string text, ulong value);
        public Microsoft.CodeAnalysis.SyntaxNode ObjectCreationExpression(Microsoft.CodeAnalysis.ITypeSymbol type, params Microsoft.CodeAnalysis.SyntaxNode[] arguments) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode ObjectCreationExpression(Microsoft.CodeAnalysis.ITypeSymbol type, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> arguments) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode ObjectCreationExpression(Microsoft.CodeAnalysis.SyntaxNode type, params Microsoft.CodeAnalysis.SyntaxNode[] arguments) { throw null; }
        internal abstract Microsoft.CodeAnalysis.SyntaxNode ObjectCreationExpression(Microsoft.CodeAnalysis.SyntaxNode namedType, Microsoft.CodeAnalysis.SyntaxToken openParen, Microsoft.CodeAnalysis.SeparatedSyntaxList<Microsoft.CodeAnalysis.SyntaxNode> arguments, Microsoft.CodeAnalysis.SyntaxToken closeParen);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ObjectCreationExpression(Microsoft.CodeAnalysis.SyntaxNode namedType, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> arguments);
        public virtual Microsoft.CodeAnalysis.SyntaxNode OperatorDeclaration(Microsoft.CodeAnalysis.Editing.OperatorKind kind, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> parameters = null, Microsoft.CodeAnalysis.SyntaxNode returnType = null, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode OperatorDeclaration(Microsoft.CodeAnalysis.IMethodSymbol method, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode ParameterDeclaration(Microsoft.CodeAnalysis.IParameterSymbol symbol, Microsoft.CodeAnalysis.SyntaxNode initializer = null) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ParameterDeclaration(string name, Microsoft.CodeAnalysis.SyntaxNode type = null, Microsoft.CodeAnalysis.SyntaxNode initializer = null, Microsoft.CodeAnalysis.RefKind refKind = Microsoft.CodeAnalysis.RefKind.None);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode ParseExpression(string stringToParse);
        protected static Microsoft.CodeAnalysis.SyntaxNode PreserveTrivia<TNode>(TNode node, System.Func<TNode, Microsoft.CodeAnalysis.SyntaxNode> nodeChanger) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode PropertyDeclaration(Microsoft.CodeAnalysis.IPropertySymbol property, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> getAccessorStatements = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> setAccessorStatements = null) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode PropertyDeclaration(string name, Microsoft.CodeAnalysis.SyntaxNode type, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> getAccessorStatements = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> setAccessorStatements = null);
        public abstract Microsoft.CodeAnalysis.SyntaxNode QualifiedName(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ReferenceEqualsExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ReferenceNotEqualsExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public Microsoft.CodeAnalysis.SyntaxNode RemoveAllAttributes(Microsoft.CodeAnalysis.SyntaxNode declaration) { throw null; }
        internal abstract Microsoft.CodeAnalysis.SyntaxNode RemoveAllComments(Microsoft.CodeAnalysis.SyntaxNode node);
        internal abstract Microsoft.CodeAnalysis.SyntaxTriviaList RemoveCommentLines(Microsoft.CodeAnalysis.SyntaxTriviaList syntaxTriviaList);
        public abstract Microsoft.CodeAnalysis.SyntaxNode RemoveEventHandler(Microsoft.CodeAnalysis.SyntaxNode @event, Microsoft.CodeAnalysis.SyntaxNode handler);
        public virtual Microsoft.CodeAnalysis.SyntaxNode RemoveNode(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxNode node) { throw null; }
        public virtual Microsoft.CodeAnalysis.SyntaxNode RemoveNode(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxRemoveOptions options) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode RemoveNodes(Microsoft.CodeAnalysis.SyntaxNode root, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> declarations) { throw null; }
        protected static Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> RemoveRange<TNode>(Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> list, int offset, int count) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        protected static Microsoft.CodeAnalysis.SyntaxList<TNode> RemoveRange<TNode>(Microsoft.CodeAnalysis.SyntaxList<TNode> list, int offset, int count) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public virtual Microsoft.CodeAnalysis.SyntaxNode ReplaceNode(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxNode newDeclaration) { throw null; }
        protected static Microsoft.CodeAnalysis.SyntaxNode ReplaceRange(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> replacements) { throw null; }
        protected static Microsoft.CodeAnalysis.SyntaxNode ReplaceWithTrivia(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxNode original, Microsoft.CodeAnalysis.SyntaxNode replacement) { throw null; }
        protected static Microsoft.CodeAnalysis.SyntaxNode ReplaceWithTrivia(Microsoft.CodeAnalysis.SyntaxNode root, Microsoft.CodeAnalysis.SyntaxToken original, Microsoft.CodeAnalysis.SyntaxToken replacement) { throw null; }
        protected static Microsoft.CodeAnalysis.SyntaxNode ReplaceWithTrivia<TNode>(Microsoft.CodeAnalysis.SyntaxNode root, TNode original, System.Func<TNode, Microsoft.CodeAnalysis.SyntaxNode> replacer) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ReturnStatement(Microsoft.CodeAnalysis.SyntaxNode expression = null);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode ScopeBlock(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        internal abstract Microsoft.CodeAnalysis.SeparatedSyntaxList<TElement> SeparatedList<TElement>(Microsoft.CodeAnalysis.SyntaxNodeOrTokenList list) where TElement : Microsoft.CodeAnalysis.SyntaxNode;
        internal abstract Microsoft.CodeAnalysis.SeparatedSyntaxList<TElement> SeparatedList<TElement>(System.Collections.Generic.IEnumerable<TElement> nodes, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxToken> separators) where TElement : Microsoft.CodeAnalysis.SyntaxNode;
        public abstract Microsoft.CodeAnalysis.SyntaxNode SetAccessorDeclaration(Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements = null);
        internal abstract Microsoft.CodeAnalysis.SyntaxTrivia SingleLineComment(string text);
        public abstract Microsoft.CodeAnalysis.SyntaxNode StructDeclaration(string name, System.Collections.Generic.IEnumerable<string> typeParameters = null, Microsoft.CodeAnalysis.Accessibility accessibility = Microsoft.CodeAnalysis.Accessibility.NotApplicable, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers = default(Microsoft.CodeAnalysis.Editing.DeclarationModifiers), System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> interfaceTypes = null, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> members = null);
        public abstract Microsoft.CodeAnalysis.SyntaxNode SubtractExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        internal abstract bool SupportsThrowExpression();
        public Microsoft.CodeAnalysis.SyntaxNode SwitchSection(Microsoft.CodeAnalysis.SyntaxNode caseExpression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode SwitchSection(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> caseExpressions, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode SwitchSectionFromLabels(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> labels, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public Microsoft.CodeAnalysis.SyntaxNode SwitchStatement(Microsoft.CodeAnalysis.SyntaxNode expression, params Microsoft.CodeAnalysis.SyntaxNode[] sections) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode SwitchStatement(Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> sections);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ThisExpression();
        public abstract Microsoft.CodeAnalysis.SyntaxNode ThrowExpression(Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ThrowStatement(Microsoft.CodeAnalysis.SyntaxNode expression = null);
        internal abstract Microsoft.CodeAnalysis.SyntaxTrivia Trivia(Microsoft.CodeAnalysis.SyntaxNode node);
        public Microsoft.CodeAnalysis.SyntaxNode TrueLiteralExpression() { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode TryCastExpression(Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.ITypeSymbol type) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode TryCastExpression(Microsoft.CodeAnalysis.SyntaxNode expression, Microsoft.CodeAnalysis.SyntaxNode type);
        public Microsoft.CodeAnalysis.SyntaxNode TryCatchStatement(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> tryStatements, params Microsoft.CodeAnalysis.SyntaxNode[] catchClauses) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode TryCatchStatement(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> tryStatements, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> catchClauses, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> finallyStatements = null);
        public Microsoft.CodeAnalysis.SyntaxNode TryFinallyStatement(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> tryStatements, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> finallyStatements) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode TupleElementExpression(Microsoft.CodeAnalysis.ITypeSymbol type, string name = null) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode TupleElementExpression(Microsoft.CodeAnalysis.SyntaxNode type, string name = null);
        public abstract Microsoft.CodeAnalysis.SyntaxNode TupleExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> arguments);
        public Microsoft.CodeAnalysis.SyntaxNode TupleTypeExpression(params Microsoft.CodeAnalysis.SyntaxNode[] elements) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode TupleTypeExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ITypeSymbol> elementTypes, System.Collections.Generic.IEnumerable<string> elementNames = null) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode TupleTypeExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> elements) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode TypedConstantExpression(Microsoft.CodeAnalysis.TypedConstant value);
        public abstract Microsoft.CodeAnalysis.SyntaxNode TypeExpression(Microsoft.CodeAnalysis.ITypeSymbol typeSymbol);
        public Microsoft.CodeAnalysis.SyntaxNode TypeExpression(Microsoft.CodeAnalysis.ITypeSymbol typeSymbol, bool addImport) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode TypeExpression(Microsoft.CodeAnalysis.SpecialType specialType);
        public abstract Microsoft.CodeAnalysis.SyntaxNode TypeOfExpression(Microsoft.CodeAnalysis.SyntaxNode type);
        public abstract Microsoft.CodeAnalysis.SyntaxNode UsingStatement(Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode UsingStatement(Microsoft.CodeAnalysis.SyntaxNode type, string name, Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public Microsoft.CodeAnalysis.SyntaxNode UsingStatement(string name, Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ValueEqualsExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ValueNotEqualsExpression(Microsoft.CodeAnalysis.SyntaxNode left, Microsoft.CodeAnalysis.SyntaxNode right);
        public Microsoft.CodeAnalysis.SyntaxNode ValueReturningLambdaExpression(Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode ValueReturningLambdaExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode ValueReturningLambdaExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> lambdaParameters, Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode ValueReturningLambdaExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> lambdaParameters, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public Microsoft.CodeAnalysis.SyntaxNode ValueReturningLambdaExpression(string parameterName, Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode ValueReturningLambdaExpression(string parameterName, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode VoidReturningLambdaExpression(Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode VoidReturningLambdaExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode VoidReturningLambdaExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> lambdaParameters, Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode VoidReturningLambdaExpression(System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> lambdaParameters, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public Microsoft.CodeAnalysis.SyntaxNode VoidReturningLambdaExpression(string parameterName, Microsoft.CodeAnalysis.SyntaxNode expression) { throw null; }
        public Microsoft.CodeAnalysis.SyntaxNode VoidReturningLambdaExpression(string parameterName, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode WhileStatement(Microsoft.CodeAnalysis.SyntaxNode condition, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        internal abstract Microsoft.CodeAnalysis.SyntaxTrivia Whitespace(string text);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithAccessibility(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.Accessibility accessibility);
        public Microsoft.CodeAnalysis.SyntaxNode WithAccessorDeclarations(Microsoft.CodeAnalysis.SyntaxNode declaration, params Microsoft.CodeAnalysis.SyntaxNode[] accessorDeclarations) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithAccessorDeclarations(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> accessorDeclarations);
        internal abstract Microsoft.CodeAnalysis.SyntaxNode WithExplicitInterfaceImplementations(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.ISymbol> explicitInterfaceImplementations);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithExpression(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode expression);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithGetAccessorStatements(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithModifiers(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.Editing.DeclarationModifiers modifiers);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithName(Microsoft.CodeAnalysis.SyntaxNode declaration, string name);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithSetAccessorStatements(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithStatements(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> statements);
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithType(Microsoft.CodeAnalysis.SyntaxNode declaration, Microsoft.CodeAnalysis.SyntaxNode type);
        public Microsoft.CodeAnalysis.SyntaxNode WithTypeArguments(Microsoft.CodeAnalysis.SyntaxNode expression, params Microsoft.CodeAnalysis.SyntaxNode[] typeArguments) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithTypeArguments(Microsoft.CodeAnalysis.SyntaxNode expression, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> typeArguments);
        public Microsoft.CodeAnalysis.SyntaxNode WithTypeConstraint(Microsoft.CodeAnalysis.SyntaxNode declaration, string typeParameterName, Microsoft.CodeAnalysis.Editing.SpecialTypeConstraintKind kinds, params Microsoft.CodeAnalysis.SyntaxNode[] types) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithTypeConstraint(Microsoft.CodeAnalysis.SyntaxNode declaration, string typeParameterName, Microsoft.CodeAnalysis.Editing.SpecialTypeConstraintKind kinds, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> types = null);
        public Microsoft.CodeAnalysis.SyntaxNode WithTypeConstraint(Microsoft.CodeAnalysis.SyntaxNode declaration, string typeParameterName, params Microsoft.CodeAnalysis.SyntaxNode[] types) { throw null; }
        public abstract Microsoft.CodeAnalysis.SyntaxNode WithTypeParameters(Microsoft.CodeAnalysis.SyntaxNode declaration, System.Collections.Generic.IEnumerable<string> typeParameters);
        public Microsoft.CodeAnalysis.SyntaxNode WithTypeParameters(Microsoft.CodeAnalysis.SyntaxNode declaration, params string[] typeParameters) { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.FindSymbols
{
    public partial interface IFindReferencesProgress
    {
        void OnCompleted();
        void OnDefinitionFound(Microsoft.CodeAnalysis.ISymbol symbol);
        void OnFindInDocumentCompleted(Microsoft.CodeAnalysis.Document document);
        void OnFindInDocumentStarted(Microsoft.CodeAnalysis.Document document);
        void OnReferenceFound(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation location);
        void OnStarted();
        void ReportProgress(int current, int maximum);
    }
    public partial class ReferencedSymbol
    {
        internal ReferencedSymbol() { }
        public Microsoft.CodeAnalysis.ISymbol Definition { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation> Locations { get { throw null; } }
    }
    public readonly partial struct ReferenceLocation : System.IComparable<Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation>, System.IEquatable<Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Microsoft.CodeAnalysis.IAliasSymbol Alias { get { throw null; } }
        public Microsoft.CodeAnalysis.CandidateReason CandidateReason { get { throw null; } }
        public Microsoft.CodeAnalysis.Document Document { get { throw null; } }
        public bool IsCandidateLocation { get { throw null; } }
        public bool IsImplicit { get { throw null; } }
        public Microsoft.CodeAnalysis.Location Location { get { throw null; } }
        public int CompareTo(Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation other) { throw null; }
        public bool Equals(Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation left, Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation left, Microsoft.CodeAnalysis.FindSymbols.ReferenceLocation right) { throw null; }
    }
    public partial struct SymbolCallerInfo
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Microsoft.CodeAnalysis.ISymbol CalledSymbol { get { throw null; } }
        public Microsoft.CodeAnalysis.ISymbol CallingSymbol { get { throw null; } }
        public bool IsDirect { get { throw null; } }
        public System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Location> Locations { get { throw null; } }
    }
    public static partial class SymbolFinder
    {
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.FindSymbols.SymbolCallerInfo>> FindCallersAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Document>? documents, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.FindSymbols.SymbolCallerInfo>> FindCallersAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindDeclarationsAsync(Microsoft.CodeAnalysis.Project project, string name, bool ignoreCase, Microsoft.CodeAnalysis.SymbolFilter filter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindDeclarationsAsync(Microsoft.CodeAnalysis.Project project, string name, bool ignoreCase, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.INamedTypeSymbol>> FindDerivedClassesAsync(Microsoft.CodeAnalysis.INamedTypeSymbol type, Microsoft.CodeAnalysis.Solution solution, bool transitive = true, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Project> projects = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.INamedTypeSymbol>> FindDerivedClassesAsync(Microsoft.CodeAnalysis.INamedTypeSymbol type, Microsoft.CodeAnalysis.Solution solution, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Project> projects, System.Threading.CancellationToken cancellationToken) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.INamedTypeSymbol>> FindDerivedInterfacesAsync(Microsoft.CodeAnalysis.INamedTypeSymbol type, Microsoft.CodeAnalysis.Solution solution, bool transitive = true, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Project> projects = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.INamedTypeSymbol>> FindImplementationsAsync(Microsoft.CodeAnalysis.INamedTypeSymbol type, Microsoft.CodeAnalysis.Solution solution, bool transitive = true, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Project> projects = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindImplementationsAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Project> projects = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindImplementedInterfaceMembersAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Project> projects = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindOverridesAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Project> projects = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.FindSymbols.ReferencedSymbol>> FindReferencesAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, Microsoft.CodeAnalysis.FindSymbols.IFindReferencesProgress? progress, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Document>? documents, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.FindSymbols.ReferencedSymbol>> FindReferencesAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, System.Collections.Immutable.IImmutableSet<Microsoft.CodeAnalysis.Document>? documents, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.FindSymbols.ReferencedSymbol>> FindReferencesAsync(Microsoft.CodeAnalysis.ISymbol symbol, Microsoft.CodeAnalysis.Solution solution, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Collections.Generic.IEnumerable<TSymbol> FindSimilarSymbols<TSymbol>(TSymbol symbol, Microsoft.CodeAnalysis.Compilation compilation, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TSymbol : Microsoft.CodeAnalysis.ISymbol { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Project project, System.Func<string, bool> predicate, Microsoft.CodeAnalysis.SymbolFilter filter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Project project, System.Func<string, bool> predicate, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Project project, string name, bool ignoreCase, Microsoft.CodeAnalysis.SymbolFilter filter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Project project, string name, bool ignoreCase, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Solution solution, System.Func<string, bool> predicate, Microsoft.CodeAnalysis.SymbolFilter filter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Solution solution, System.Func<string, bool> predicate, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Solution solution, string name, bool ignoreCase, Microsoft.CodeAnalysis.SymbolFilter filter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsAsync(Microsoft.CodeAnalysis.Solution solution, string name, bool ignoreCase, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsWithPatternAsync(Microsoft.CodeAnalysis.Project project, string pattern, Microsoft.CodeAnalysis.SymbolFilter filter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsWithPatternAsync(Microsoft.CodeAnalysis.Project project, string pattern, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsWithPatternAsync(Microsoft.CodeAnalysis.Solution solution, string pattern, Microsoft.CodeAnalysis.SymbolFilter filter, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> FindSourceDeclarationsWithPatternAsync(Microsoft.CodeAnalysis.Solution solution, string pattern, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol?> FindSourceDefinitionAsync(Microsoft.CodeAnalysis.ISymbol? symbol, Microsoft.CodeAnalysis.Solution solution, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        [System.ObsoleteAttribute("Use FindSymbolAtPositionAsync instead.")]
        public static Microsoft.CodeAnalysis.ISymbol FindSymbolAtPosition(Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.Workspace workspace, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> FindSymbolAtPositionAsync(Microsoft.CodeAnalysis.Document document, int position, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.ISymbol> FindSymbolAtPositionAsync(Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.Workspace workspace, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.Formatting
{
    public static partial class Formatter
    {
        public static Microsoft.CodeAnalysis.SyntaxAnnotation Annotation { get { throw null; } }
        public static Microsoft.CodeAnalysis.SyntaxNode Format(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.SyntaxAnnotation annotation, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.SyntaxNode Format(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.Text.TextSpan span, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.SyntaxNode Format(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static Microsoft.CodeAnalysis.SyntaxNode Format(Microsoft.CodeAnalysis.SyntaxNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextSpan>? spans, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> FormatAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> FormatAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.SyntaxAnnotation annotation, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> FormatAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Text.TextSpan span, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> FormatAsync(Microsoft.CodeAnalysis.Document document, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextSpan>? spans, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Collections.Generic.IList<Microsoft.CodeAnalysis.Text.TextChange> GetFormattedTextChanges(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.Text.TextSpan span, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Collections.Generic.IList<Microsoft.CodeAnalysis.Text.TextChange> GetFormattedTextChanges(Microsoft.CodeAnalysis.SyntaxNode node, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Collections.Generic.IList<Microsoft.CodeAnalysis.Text.TextChange> GetFormattedTextChanges(Microsoft.CodeAnalysis.SyntaxNode node, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextSpan>? spans, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> OrganizeImportsAsync(Microsoft.CodeAnalysis.Document document, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public static partial class FormattingOptions
    {
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<int> IndentationSize { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<string> NewLine { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<Microsoft.CodeAnalysis.Formatting.FormattingOptions.IndentStyle> SmartIndent { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<int> TabSize { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> UseTabs { get { throw null; } }
        public enum IndentStyle
        {
            None = 0,
            Block = 1,
            Smart = 2,
        }
    }
}
namespace Microsoft.CodeAnalysis.Host
{
    public abstract partial class HostLanguageServices
    {
        protected HostLanguageServices() { }
        public abstract string Language { get; }
        public abstract Microsoft.CodeAnalysis.Host.HostWorkspaceServices WorkspaceServices { get; }
        public TLanguageService GetRequiredService<TLanguageService>() where TLanguageService : Microsoft.CodeAnalysis.Host.ILanguageService { throw null; }
        public abstract TLanguageService? GetService<TLanguageService>() where TLanguageService : Microsoft.CodeAnalysis.Host.ILanguageService;
    }
    public abstract partial class HostServices
    {
        protected HostServices() { }
        protected internal abstract Microsoft.CodeAnalysis.Host.HostWorkspaceServices CreateWorkspaceServices(Microsoft.CodeAnalysis.Workspace workspace);
    }
    public abstract partial class HostWorkspaceServices
    {
        protected HostWorkspaceServices() { }
        public abstract Microsoft.CodeAnalysis.Host.HostServices HostServices { get; }
        public virtual Microsoft.CodeAnalysis.Host.IPersistentStorageService PersistentStorage { get { throw null; } }
        public virtual System.Collections.Generic.IEnumerable<string> SupportedLanguages { get { throw null; } }
        public virtual Microsoft.CodeAnalysis.Host.ITemporaryStorageService TemporaryStorage { get { throw null; } }
        public abstract Microsoft.CodeAnalysis.Workspace Workspace { get; }
        public abstract System.Collections.Generic.IEnumerable<TLanguageService> FindLanguageServices<TLanguageService>(Microsoft.CodeAnalysis.Host.HostWorkspaceServices.MetadataFilter filter);
        public virtual Microsoft.CodeAnalysis.Host.HostLanguageServices GetLanguageServices(string languageName) { throw null; }
        public TWorkspaceService GetRequiredService<TWorkspaceService>() where TWorkspaceService : Microsoft.CodeAnalysis.Host.IWorkspaceService { throw null; }
        public abstract TWorkspaceService? GetService<TWorkspaceService>() where TWorkspaceService : Microsoft.CodeAnalysis.Host.IWorkspaceService;
        public virtual bool IsSupported(string languageName) { throw null; }
        public delegate bool MetadataFilter(System.Collections.Generic.IReadOnlyDictionary<string, object> metadata);
    }
    public partial interface IAnalyzerService : Microsoft.CodeAnalysis.Host.IWorkspaceService
    {
        Microsoft.CodeAnalysis.IAnalyzerAssemblyLoader GetLoader();
    }
    public partial interface ILanguageService
    {
    }
    public partial interface IPersistentStorage : System.IAsyncDisposable, System.IDisposable
    {
        System.Threading.Tasks.Task<System.IO.Stream?> ReadStreamAsync(Microsoft.CodeAnalysis.Document document, string name, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task<System.IO.Stream?> ReadStreamAsync(Microsoft.CodeAnalysis.Project project, string name, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task<System.IO.Stream?> ReadStreamAsync(string name, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task<bool> WriteStreamAsync(Microsoft.CodeAnalysis.Document document, string name, System.IO.Stream stream, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task<bool> WriteStreamAsync(Microsoft.CodeAnalysis.Project project, string name, System.IO.Stream stream, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task<bool> WriteStreamAsync(string name, System.IO.Stream stream, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
    public partial interface IPersistentStorageService : Microsoft.CodeAnalysis.Host.IWorkspaceService
    {
        [System.ObsoleteAttribute("Use GetStorageAsync instead", false)]
        Microsoft.CodeAnalysis.Host.IPersistentStorage GetStorage(Microsoft.CodeAnalysis.Solution solution);
        System.Threading.Tasks.ValueTask<Microsoft.CodeAnalysis.Host.IPersistentStorage> GetStorageAsync(Microsoft.CodeAnalysis.Solution solution, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
    public partial interface ITemporaryStorageService : Microsoft.CodeAnalysis.Host.IWorkspaceService
    {
        Microsoft.CodeAnalysis.Host.ITemporaryStreamStorage CreateTemporaryStreamStorage(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        Microsoft.CodeAnalysis.Host.ITemporaryTextStorage CreateTemporaryTextStorage(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
    public partial interface ITemporaryStreamStorage : System.IDisposable
    {
        System.IO.Stream ReadStream(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task<System.IO.Stream> ReadStreamAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        void WriteStream(System.IO.Stream stream, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task WriteStreamAsync(System.IO.Stream stream, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
    public partial interface ITemporaryTextStorage : System.IDisposable
    {
        Microsoft.CodeAnalysis.Text.SourceText ReadText(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Text.SourceText> ReadTextAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        void WriteText(Microsoft.CodeAnalysis.Text.SourceText text, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        System.Threading.Tasks.Task WriteTextAsync(Microsoft.CodeAnalysis.Text.SourceText text, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
    public partial interface IWorkspaceService
    {
    }
}
namespace Microsoft.CodeAnalysis.Host.Mef
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    [System.Composition.MetadataAttributeAttribute]
    public partial class ExportLanguageServiceAttribute : System.Composition.ExportAttribute
    {
        public ExportLanguageServiceAttribute(System.Type type, string language, string layer = "Default") { }
        public string Language { get { throw null; } }
        public string Layer { get { throw null; } }
        public string ServiceType { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    [System.Composition.MetadataAttributeAttribute]
    public partial class ExportLanguageServiceFactoryAttribute : System.Composition.ExportAttribute
    {
        public ExportLanguageServiceFactoryAttribute(System.Type type, string language, string layer = "Default") { }
        public string Language { get { throw null; } }
        public string Layer { get { throw null; } }
        public string ServiceType { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    [System.Composition.MetadataAttributeAttribute]
    public partial class ExportWorkspaceServiceAttribute : System.Composition.ExportAttribute
    {
        public ExportWorkspaceServiceAttribute(System.Type serviceType, string layer = "Default") { }
        public string Layer { get { throw null; } }
        public string ServiceType { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class)]
    [System.Composition.MetadataAttributeAttribute]
    public partial class ExportWorkspaceServiceFactoryAttribute : System.Composition.ExportAttribute
    {
        public ExportWorkspaceServiceFactoryAttribute(System.Type serviceType, string layer = "Default") { }
        public string Layer { get { throw null; } }
        public string ServiceType { get { throw null; } }
    }
    public partial interface ILanguageServiceFactory
    {
        Microsoft.CodeAnalysis.Host.ILanguageService CreateLanguageService(Microsoft.CodeAnalysis.Host.HostLanguageServices languageServices);
    }
    public partial interface IWorkspaceServiceFactory
    {
        Microsoft.CodeAnalysis.Host.IWorkspaceService CreateService(Microsoft.CodeAnalysis.Host.HostWorkspaceServices workspaceServices);
    }
    public partial class MefHostServices : Microsoft.CodeAnalysis.Host.HostServices
    {
        public MefHostServices(System.Composition.CompositionContext compositionContext) { }
        public static System.Collections.Immutable.ImmutableArray<System.Reflection.Assembly> DefaultAssemblies { get { throw null; } }
        public static Microsoft.CodeAnalysis.Host.Mef.MefHostServices DefaultHost { get { throw null; } }
        public static Microsoft.CodeAnalysis.Host.Mef.MefHostServices Create(System.Collections.Generic.IEnumerable<System.Reflection.Assembly> assemblies) { throw null; }
        public static Microsoft.CodeAnalysis.Host.Mef.MefHostServices Create(System.Composition.CompositionContext compositionContext) { throw null; }
        protected internal override Microsoft.CodeAnalysis.Host.HostWorkspaceServices CreateWorkspaceServices(Microsoft.CodeAnalysis.Workspace workspace) { throw null; }
    }
    public static partial class ServiceLayer
    {
        public const string Default = "Default";
        public const string Desktop = "Desktop";
        public const string Editor = "Editor";
        public const string Host = "Host";
    }
}
namespace Microsoft.CodeAnalysis.Options
{
    public sealed partial class DocumentOptionSet : Microsoft.CodeAnalysis.Options.OptionSet
    {
        internal DocumentOptionSet() { }
        internal override System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Options.OptionKey> GetChangedOptions(Microsoft.CodeAnalysis.Options.OptionSet optionSet) { throw null; }
        private protected override object? GetOptionCore(Microsoft.CodeAnalysis.Options.OptionKey optionKey) { throw null; }
        public T GetOption<T>(Microsoft.CodeAnalysis.Options.PerLanguageOption<T> option) { throw null; }
        public override Microsoft.CodeAnalysis.Options.OptionSet WithChangedOption(Microsoft.CodeAnalysis.Options.OptionKey optionAndLanguage, object? value) { throw null; }
        public Microsoft.CodeAnalysis.Options.DocumentOptionSet WithChangedOption<T>(Microsoft.CodeAnalysis.Options.PerLanguageOption<T> option, T value) { throw null; }
    }
    public partial interface IOption
    {
        object? DefaultValue { get; }
        string Feature { get; }
        bool IsPerLanguage { get; }
        string Name { get; }
        System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Options.OptionStorageLocation> StorageLocations { get; }
        System.Type Type { get; }
    }
    public readonly partial struct OptionKey : System.IEquatable<Microsoft.CodeAnalysis.Options.OptionKey>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public OptionKey(Microsoft.CodeAnalysis.Options.IOption option, string? language = null) { throw null; }
        public string? Language { get { throw null; } }
        public Microsoft.CodeAnalysis.Options.IOption Option { get { throw null; } }
        public bool Equals(Microsoft.CodeAnalysis.Options.OptionKey other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Microsoft.CodeAnalysis.Options.OptionKey left, Microsoft.CodeAnalysis.Options.OptionKey right) { throw null; }
        public static bool operator !=(Microsoft.CodeAnalysis.Options.OptionKey left, Microsoft.CodeAnalysis.Options.OptionKey right) { throw null; }
        public override string ToString() { throw null; }
    }
    public abstract partial class OptionSet
    {
        protected OptionSet() { }
        internal abstract System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Options.OptionKey> GetChangedOptions(Microsoft.CodeAnalysis.Options.OptionSet optionSet);
        public object? GetOption(Microsoft.CodeAnalysis.Options.OptionKey optionKey) { throw null; }
        private protected abstract object? GetOptionCore(Microsoft.CodeAnalysis.Options.OptionKey optionKey);
        public T GetOption<T>(Microsoft.CodeAnalysis.Options.OptionKey optionKey) { throw null; }
        public T GetOption<T>(Microsoft.CodeAnalysis.Options.Option<T> option) { throw null; }
        public T GetOption<T>(Microsoft.CodeAnalysis.Options.PerLanguageOption<T> option, string? language) { throw null; }
        public abstract Microsoft.CodeAnalysis.Options.OptionSet WithChangedOption(Microsoft.CodeAnalysis.Options.OptionKey optionAndLanguage, object? value);
        public Microsoft.CodeAnalysis.Options.OptionSet WithChangedOption<T>(Microsoft.CodeAnalysis.Options.Option<T> option, T value) { throw null; }
        public Microsoft.CodeAnalysis.Options.OptionSet WithChangedOption<T>(Microsoft.CodeAnalysis.Options.PerLanguageOption<T> option, string? language, T value) { throw null; }
    }
    public abstract partial class OptionStorageLocation
    {
        protected OptionStorageLocation() { }
    }
    public partial class Option<T> : Microsoft.CodeAnalysis.Options.IOption
    {
        [System.ObsoleteAttribute("Use a constructor that specifies an explicit default value.")]
        public Option(string feature, string name) { }
        public Option(string feature, string name, T defaultValue) { }
        public Option(string feature, string name, T defaultValue, params Microsoft.CodeAnalysis.Options.OptionStorageLocation[] storageLocations) { }
        public T DefaultValue { get { throw null; } }
        public string Feature { get { throw null; } }
        object? Microsoft.CodeAnalysis.Options.IOption.DefaultValue { get { throw null; } }
        bool Microsoft.CodeAnalysis.Options.IOption.IsPerLanguage { get { throw null; } }
        public string Name { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Options.OptionStorageLocation> StorageLocations { get { throw null; } }
        public System.Type Type { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static implicit operator Microsoft.CodeAnalysis.Options.OptionKey (Microsoft.CodeAnalysis.Options.Option<T> option) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class PerLanguageOption<T> : Microsoft.CodeAnalysis.Options.IOption
    {
        public PerLanguageOption(string feature, string name, T defaultValue) { }
        public PerLanguageOption(string feature, string name, T defaultValue, params Microsoft.CodeAnalysis.Options.OptionStorageLocation[] storageLocations) { }
        public T DefaultValue { get { throw null; } }
        public string Feature { get { throw null; } }
        object? Microsoft.CodeAnalysis.Options.IOption.DefaultValue { get { throw null; } }
        bool Microsoft.CodeAnalysis.Options.IOption.IsPerLanguage { get { throw null; } }
        public string Name { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Options.OptionStorageLocation> StorageLocations { get { throw null; } }
        public System.Type Type { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public override string ToString() { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.Recommendations
{
    public static partial class RecommendationOptions
    {
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> FilterOutOfScopeLocals { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> HideAdvancedMembers { get { throw null; } }
    }
    public static partial class Recommender
    {
        public static System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol> GetRecommendedSymbolsAtPosition(Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        [System.ObsoleteAttribute("Use GetRecommendedSymbolsAtPosition")]
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.ISymbol>> GetRecommendedSymbolsAtPositionAsync(Microsoft.CodeAnalysis.SemanticModel semanticModel, int position, Microsoft.CodeAnalysis.Workspace workspace, Microsoft.CodeAnalysis.Options.OptionSet? options = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.Rename
{
    public enum RenameEntityKind
    {
        BaseSymbol = 0,
        OverloadedSymbols = 1,
    }
    public static partial class RenameOptions
    {
        public static Microsoft.CodeAnalysis.Options.Option<bool> PreviewChanges { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> RenameInComments { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> RenameInStrings { get { throw null; } }
        public static Microsoft.CodeAnalysis.Options.Option<bool> RenameOverloads { get { throw null; } }
    }
    public static partial class Renamer
    {
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Rename.Renamer.RenameDocumentActionSet> RenameDocumentAsync(Microsoft.CodeAnalysis.Document document, string newDocumentName, System.Collections.Generic.IReadOnlyList<string>? newDocumentFolders = null, Microsoft.CodeAnalysis.Options.OptionSet? optionSet = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Solution> RenameSymbolAsync(Microsoft.CodeAnalysis.Solution solution, Microsoft.CodeAnalysis.ISymbol symbol, string newName, Microsoft.CodeAnalysis.Options.OptionSet optionSet, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public abstract partial class RenameDocumentAction
        {
            internal RenameDocumentAction() { }
            public abstract string GetDescription(System.Globalization.CultureInfo? culture = null);
            public System.Collections.Immutable.ImmutableArray<string> GetErrors(System.Globalization.CultureInfo? culture = null) { throw null; }
        }
        public sealed partial class RenameDocumentActionSet
        {
            internal RenameDocumentActionSet() { }
            public System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Rename.Renamer.RenameDocumentAction> ApplicableActions { get { throw null; } }
            public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Solution> UpdateSolutionAsync(Microsoft.CodeAnalysis.Solution solution, System.Collections.Immutable.ImmutableArray<Microsoft.CodeAnalysis.Rename.Renamer.RenameDocumentAction> actions, System.Threading.CancellationToken cancellationToken) { throw null; }
            public System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Solution> UpdateSolutionAsync(Microsoft.CodeAnalysis.Solution solution, System.Threading.CancellationToken cancellationToken) { throw null; }
        }
    }
}
namespace Microsoft.CodeAnalysis.Simplification
{
    public static partial class SimplificationOptions
    {
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.Option<bool> AllowSimplificationToBaseType { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.Option<bool> AllowSimplificationToGenericType { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.Option<bool> PreferAliasToQualification { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.Option<bool> PreferImplicitTypeInference { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.Option<bool> PreferImplicitTypeInLocalDeclaration { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> PreferIntrinsicPredefinedTypeKeywordInDeclaration { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> PreferIntrinsicPredefinedTypeKeywordInMemberAccess { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.Option<bool> PreferOmittingModuleNamesInQualification { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> QualifyEventAccess { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> QualifyFieldAccess { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> QualifyMemberAccessWithThisOrMe { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> QualifyMethodAccess { get { throw null; } }
        [System.ObsoleteAttribute("This option is no longer used")]
        public static Microsoft.CodeAnalysis.Options.PerLanguageOption<bool> QualifyPropertyAccess { get { throw null; } }
    }
    public static partial class Simplifier
    {
        public static Microsoft.CodeAnalysis.SyntaxAnnotation AddImportsAnnotation { get { throw null; } }
        public static Microsoft.CodeAnalysis.SyntaxAnnotation Annotation { get { throw null; } }
        public static Microsoft.CodeAnalysis.SyntaxAnnotation SpecialTypeAnnotation { get { throw null; } }
        public static Microsoft.CodeAnalysis.SyntaxToken Expand(Microsoft.CodeAnalysis.SyntaxToken token, Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Workspace workspace, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool> expandInsideNode = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.SyntaxToken> ExpandAsync(Microsoft.CodeAnalysis.SyntaxToken token, Microsoft.CodeAnalysis.Document document, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool> expandInsideNode = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<TNode> ExpandAsync<TNode>(TNode node, Microsoft.CodeAnalysis.Document document, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool> expandInsideNode = null, bool expandParameter = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static TNode Expand<TNode>(TNode node, Microsoft.CodeAnalysis.SemanticModel semanticModel, Microsoft.CodeAnalysis.Workspace workspace, System.Func<Microsoft.CodeAnalysis.SyntaxNode, bool> expandInsideNode = null, bool expandParameter = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TNode : Microsoft.CodeAnalysis.SyntaxNode { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> ReduceAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Options.OptionSet optionSet = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> ReduceAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.SyntaxAnnotation annotation, Microsoft.CodeAnalysis.Options.OptionSet optionSet = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> ReduceAsync(Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.Text.TextSpan span, Microsoft.CodeAnalysis.Options.OptionSet optionSet = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> ReduceAsync(Microsoft.CodeAnalysis.Document document, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.Text.TextSpan> spans, Microsoft.CodeAnalysis.Options.OptionSet optionSet = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
}
namespace Microsoft.CodeAnalysis.Tags
{
    public static partial class WellKnownTags
    {
        public const string Assembly = "Assembly";
        public const string Class = "Class";
        public const string Constant = "Constant";
        public const string Delegate = "Delegate";
        public const string Enum = "Enum";
        public const string EnumMember = "EnumMember";
        public const string Error = "Error";
        public const string Event = "Event";
        public const string ExtensionMethod = "ExtensionMethod";
        public const string Field = "Field";
        public const string File = "File";
        public const string Folder = "Folder";
        public const string Interface = "Interface";
        public const string Internal = "Internal";
        public const string Intrinsic = "Intrinsic";
        public const string Keyword = "Keyword";
        public const string Label = "Label";
        public const string Local = "Local";
        public const string Method = "Method";
        public const string Module = "Module";
        public const string Namespace = "Namespace";
        public const string Operator = "Operator";
        public const string Parameter = "Parameter";
        public const string Private = "Private";
        public const string Project = "Project";
        public const string Property = "Property";
        public const string Protected = "Protected";
        public const string Public = "Public";
        public const string RangeVariable = "RangeVariable";
        public const string Reference = "Reference";
        public const string Snippet = "Snippet";
        public const string Structure = "Structure";
        public const string TypeParameter = "TypeParameter";
        public const string Warning = "Warning";
    }
}


// The following code was manually created to workaround api generator limitations.
// --------------------------------------------------------------------------------

namespace Microsoft.CodeAnalysis.Editing
{
    internal class SyntaxGeneratorInternal 
    {
    }
}