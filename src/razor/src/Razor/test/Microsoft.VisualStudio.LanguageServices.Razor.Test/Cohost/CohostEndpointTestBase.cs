// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Text;
using Microsoft.NET.Sdk.Razor.SourceGenerators;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Roslyn.Test.Utilities;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public abstract class CohostEndpointTestBase(ITestOutputHelper testOutputHelper) : ToolingTestBase(testOutputHelper)
{
    private ExportProvider? _exportProvider;
    private TestRemoteServiceInvoker? _remoteServiceInvoker;
    private RemoteClientInitializationOptions _clientInitializationOptions;
    private RemoteClientLSPInitializationOptions _clientLSPInitializationOptions;
    private IFilePathService? _filePathService;

    private protected TestRemoteServiceInvoker RemoteServiceInvoker => _remoteServiceInvoker.AssumeNotNull();
    private protected IFilePathService FilePathService => _filePathService.AssumeNotNull();
    private protected RemoteLanguageServerFeatureOptions FeatureOptions => OOPExportProvider.GetExportedValue<RemoteLanguageServerFeatureOptions>();
    private protected RemoteClientCapabilitiesService ClientCapabilitiesService => OOPExportProvider.GetExportedValue<RemoteClientCapabilitiesService>();
    private protected RemoteSemanticTokensLegendService SemanticTokensLegendService => OOPExportProvider.GetExportedValue<RemoteSemanticTokensLegendService>();

    /// <summary>
    /// The export provider for Razor OOP services (not Roslyn)
    /// </summary>
    private protected ExportProvider OOPExportProvider => _exportProvider.AssumeNotNull();

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        // Create a new isolated MEF composition.
        // Note that this uses a cached catalog and configuration for performance.
        try
        {
            _exportProvider = await RemoteMefComposition.CreateExportProviderAsync(DisposalToken);
        }
        catch (CompositionFailedException ex) when (ex.Errors is not null)
        {
            Assert.Fail($"""
                Errors in the Remote MEF composition:

                {string.Join(Environment.NewLine, ex.Errors.SelectMany(e => e).Select(e => e.Message))}
                """);
        }

        AddDisposable(_exportProvider);

        _remoteServiceInvoker = new TestRemoteServiceInvoker(JoinableTaskContext, _exportProvider, LoggerFactory);
        AddDisposable(_remoteServiceInvoker);

        _clientInitializationOptions = new()
        {
            HtmlVirtualDocumentSuffix = ".g.html",
            UsePreciseSemanticTokenRanges = false,
            UseRazorCohostServer = true,
            ReturnCodeActionAndRenamePathsWithPrefixedSlash = false,
            SupportsFileManipulation = true,
            ShowAllCSharpCodeActions = false,
            SupportsSoftSelectionInCompletion = true,
            UseVsCodeCompletionTriggerCharacters = false,
        };
        UpdateClientInitializationOptions(c => c);

        var completionSetting = new CompletionSetting
        {
            CompletionItem = new CompletionItemSetting(),
            CompletionItemKind = new CompletionItemKindSetting()
            {
                ValueSet = (CompletionItemKind[])Enum.GetValues(typeof(CompletionItemKind)),
            },
            CompletionListSetting = new CompletionListSetting()
            {
                ItemDefaults = ["commitCharacters", "editRange", "insertTextFormat"]
            },
            ContextSupport = false,
            InsertTextMode = InsertTextMode.AsIs,
        };

        _clientLSPInitializationOptions = new()
        {
            ClientCapabilities = new VSInternalClientCapabilities()
            {
                SupportsVisualStudioExtensions = true,
                TextDocument = new TextDocumentClientCapabilities
                {
                    Completion = completionSetting
                }
            },
            TokenTypes = [],
            TokenModifiers = []
        };
        UpdateClientLSPInitializationOptions(c => c);

        _filePathService = new RemoteFilePathService(FeatureOptions);
    }

    private protected void UpdateClientInitializationOptions(Func<RemoteClientInitializationOptions, RemoteClientInitializationOptions> mutation)
    {
        _clientInitializationOptions = mutation(_clientInitializationOptions);
        FeatureOptions.SetOptions(_clientInitializationOptions);
    }

    private protected void UpdateClientLSPInitializationOptions(Func<RemoteClientLSPInitializationOptions, RemoteClientLSPInitializationOptions> mutation)
    {
        _clientLSPInitializationOptions = mutation(_clientLSPInitializationOptions);
        ClientCapabilitiesService.SetCapabilities(_clientLSPInitializationOptions.ClientCapabilities);
        SemanticTokensLegendService.SetLegend(_clientLSPInitializationOptions.TokenTypes, _clientLSPInitializationOptions.TokenModifiers);
    }

    private protected virtual TestComposition ConfigureRoslynDevenvComposition(TestComposition composition)
        => composition;

    protected TextDocument CreateProjectAndRazorDocument(
        string contents,
        string? fileKind = null,
        (string fileName, string contents)[]? additionalFiles = null,
        bool createSeparateRemoteAndLocalWorkspaces = false,
        bool inGlobalNamespace = false)
    {
        // Using IsLegacy means null == component, so easier for test authors
        var isComponent = !FileKinds.IsLegacy(fileKind);

        var documentFilePath = isComponent
            ? TestProjectData.SomeProjectComponentFile1.FilePath
            : TestProjectData.SomeProjectFile1.FilePath;

        var projectFilePath = TestProjectData.SomeProject.FilePath;
        var projectName = Path.GetFileNameWithoutExtension(projectFilePath);
        var projectId = ProjectId.CreateNewId(debugName: projectName);
        var documentId = DocumentId.CreateNewId(projectId, debugName: documentFilePath);

        var remoteWorkspace = RemoteWorkspaceAccessor.GetWorkspace();
        var remoteDocument = CreateProjectAndRazorDocument(remoteWorkspace, projectId, projectName, documentId, documentFilePath, contents, additionalFiles, inGlobalNamespace);

        if (createSeparateRemoteAndLocalWorkspaces)
        {
            // Usually its fine to just use the remote workspace, but sometimes we need to also have things available in the
            // "devenv" side of Roslyn, which is a different workspace with a different set of services. We don't have any
            // actual solution syncing set up for testing, and don't really use a service broker, but since we also would
            // expect to never make changes to a workspace, it should be fine to simply create duplicated solutions as part
            // of test setup.
            return CreateLocalProjectAndRazorDocument(
                remoteDocument.Project.Solution,
                projectId,
                projectName,
                documentId,
                documentFilePath,
                contents,
                additionalFiles,
                inGlobalNamespace);
        }

        // If we're just creating one workspace, then its the remote one and we just return the remote document
        // and assume that the endpoint under test doesn't need to do anything on the devenv side. This makes it
        // easier for tests to mutate solutions
        return remoteDocument;
    }

    private TextDocument CreateLocalProjectAndRazorDocument(
        Solution remoteSolution,
        ProjectId projectId,
        string projectName,
        DocumentId documentId,
        string documentFilePath,
        string contents,
        (string fileName, string contents)[]? additionalFiles,
        bool inGlobalNamespace)
    {
        var exportProvider = ConfigureRoslynDevenvComposition(TestComposition.Roslyn).ExportProviderFactory.CreateExportProvider();
        AddDisposable(exportProvider);
        var workspace = TestWorkspace.CreateWithDiagnosticAnalyzers(exportProvider);
        AddDisposable(workspace);

        var razorDocument = CreateProjectAndRazorDocument(workspace, projectId, projectName, documentId, documentFilePath, contents, additionalFiles, inGlobalNamespace);

        // If we're creating remote and local workspaces, then we'll return the local document, and have to allow
        // the remote service invoker to map from the local solution to the remote one.
        RemoteServiceInvoker.MapSolutionIdToRemote(razorDocument.Project.Solution.Id, remoteSolution);

        return razorDocument;
    }

    private TextDocument CreateProjectAndRazorDocument(CodeAnalysis.Workspace workspace, ProjectId projectId, string projectName, DocumentId documentId, string documentFilePath, string contents, (string fileName, string contents)[]? additionalFiles, bool inGlobalNamespace)
    {
        var sgAssembly = typeof(RazorSourceGenerator).Assembly;

        var projectInfo = ProjectInfo
            .Create(
                projectId,
                VersionStamp.Create(),
                name: projectName,
                assemblyName: projectName,
                LanguageNames.CSharp,
                documentFilePath,
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .WithMetadataReferences(AspNet80.ReferenceInfos.All.Select(r => r.Reference))
            .WithDefaultNamespace(TestProjectData.SomeProject.RootNamespace)
            // TODO: Can we just use an object reference? Trying to do so now results in a serialization error from Roslyn
            .WithAnalyzerReferences([new AnalyzerFileReference(sgAssembly.Location, TestAnalyzerAssemblyLoader.LoadFromFile)]);

        if (!inGlobalNamespace)
        {
            projectInfo = projectInfo.WithDefaultNamespace(TestProjectData.SomeProject.RootNamespace);
        }

        var solution = workspace.CurrentSolution.AddProject(projectInfo);

        solution = solution
            .AddAdditionalDocument(
                documentId,
                documentFilePath,
                SourceText.From(contents),
                filePath: documentFilePath)
            .AddAdditionalDocument(
                DocumentId.CreateNewId(projectId),
                name: TestProjectData.SomeProjectComponentImportFile1.FilePath,
                text: SourceText.From("""
                    @using Microsoft.AspNetCore.Components
                    @using Microsoft.AspNetCore.Components.Authorization
                    @using Microsoft.AspNetCore.Components.Forms
                    @using Microsoft.AspNetCore.Components.Routing
                    @using Microsoft.AspNetCore.Components.Web
                    """),
                filePath: TestProjectData.SomeProjectComponentImportFile1.FilePath)
            .AddAdditionalDocument(
                DocumentId.CreateNewId(projectId),
                name: "_ViewImports.cshtml",
                text: SourceText.From("""
                    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
                    """),
                filePath: TestProjectData.SomeProjectImportFile.FilePath);

        if (additionalFiles is not null)
        {
            foreach (var file in additionalFiles)
            {
                solution = Path.GetExtension(file.fileName) == ".cs"
                    ? solution.AddDocument(DocumentId.CreateNewId(projectId), name: file.fileName, text: SourceText.From(file.contents), filePath: file.fileName)
                    : solution.AddAdditionalDocument(DocumentId.CreateNewId(projectId), name: file.fileName, text: SourceText.From(file.contents), filePath: file.fileName);
            }
        }

        var globalConfigContent = new StringBuilder();
        globalConfigContent.AppendLine($"""
                    is_global = true

                    build_property.RazorLangVersion = {FallbackRazorConfiguration.Latest.LanguageVersion}
                    build_property.RazorConfiguration = {FallbackRazorConfiguration.Latest.ConfigurationName}
                    build_property.RootNamespace = {TestProjectData.SomeProject.RootNamespace}
                    """);

        var projectBasePath = TestProjectData.SomeProjectPath;
        // Normally MS Build targets do this for us, but we're on our own!
        foreach (var razorDocument in solution.Projects.Single().AdditionalDocuments)
        {
            if (razorDocument.FilePath is not null &&
                razorDocument.FilePath.StartsWith(projectBasePath))
            {
                var relativePath = razorDocument.FilePath[(projectBasePath.Length + 1)..];
                globalConfigContent.AppendLine($"""

                [{razorDocument.FilePath.AssumeNotNull().Replace('\\', '/')}]
                build_metadata.AdditionalFiles.TargetPath = {Convert.ToBase64String(Encoding.UTF8.GetBytes(relativePath))}
                """);
            }
        }

        solution = solution.AddAnalyzerConfigDocument(
                DocumentId.CreateNewId(projectId),
                name: ".globalconfig",
                text: SourceText.From(globalConfigContent.ToString()),
                filePath: Path.Combine(TestProjectData.SomeProjectPath, ".globalconfig"));

        return solution.GetAdditionalDocument(documentId).AssumeNotNull();
    }

    protected static Uri FileUri(string projectRelativeFileName)
        => new(FilePath(projectRelativeFileName));

    protected static string FilePath(string projectRelativeFileName)
        => Path.GetFullPath(Path.Combine(TestProjectData.SomeProjectPath, projectRelativeFileName));
}
