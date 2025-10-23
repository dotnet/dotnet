// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.CodeAnalysis.Remote.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.NET.Sdk.Razor.SourceGenerators;
using Microsoft.VisualStudio.Composition;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public abstract class CohostTestBase(ITestOutputHelper testOutputHelper) : ToolingTestBase(testOutputHelper)
{
    private ExportProvider? _exportProvider;
    private TestIncompatibleProjectService _incompatibleProjectService = null!;
    private RemoteClientInitializationOptions _clientInitializationOptions;
    private RemoteClientLSPInitializationOptions _clientLSPInitializationOptions;

    private protected abstract IRemoteServiceInvoker RemoteServiceInvoker { get; }
    private protected abstract IClientSettingsManager ClientSettingsManager { get; }
    private protected abstract IFilePathService FilePathService { get; }

    private protected TestIncompatibleProjectService IncompatibleProjectService => _incompatibleProjectService.AssumeNotNull();
    private protected RemoteLanguageServerFeatureOptions FeatureOptions => OOPExportProvider.GetExportedValue<RemoteLanguageServerFeatureOptions>();
    private protected RemoteClientCapabilitiesService ClientCapabilitiesService => (RemoteClientCapabilitiesService)OOPExportProvider.GetExportedValue<IClientCapabilitiesService>();

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
            _exportProvider = await RemoteMefComposition.CreateExportProviderAsync(cacheDirectory: null, DisposalToken);
        }
        catch (CompositionFailedException ex) when (ex.Errors is not null)
        {
            Assert.Fail($"""
                Errors in the Remote MEF composition:

                {string.Join(Environment.NewLine, ex.Errors.SelectMany(e => e).Select(e => e.Message))}
                """);
        }

        AddDisposable(_exportProvider);

        _incompatibleProjectService = new TestIncompatibleProjectService();

        var remoteLogger = _exportProvider.GetExportedValue<RemoteLoggerFactory>();
        remoteLogger.SetTargetLoggerFactory(LoggerFactory);

        _clientInitializationOptions = new()
        {
            HtmlVirtualDocumentSuffix = ".g.html",
            UseRazorCohostServer = true,
            ReturnCodeActionAndRenamePathsWithPrefixedSlash = false,
            SupportsFileManipulation = true,
            ShowAllCSharpCodeActions = false,
            SupportsSoftSelectionInCompletion = true,
            UseVsCodeCompletionCommitCharacters = false,
        };
        UpdateClientInitializationOptions(c => c);

        _clientLSPInitializationOptions = GetRemoteClientLSPInitializationOptions();
        UpdateClientLSPInitializationOptions(c => c);

        // Force initialization and creation of the remote workspace. It will be filled in later.
        var traceSource = new TraceSource("Cohost test remote initialization");
        traceSource.Listeners.Add(new XunitTraceListener(TestOutputHelper));
        await RemoteWorkspaceProvider.TestAccessor.InitializeRemoteExportProviderBuilderAsync(Path.GetTempPath(), traceSource, DisposalToken);
        _ = RemoteWorkspaceProvider.Instance.GetWorkspace();
    }

    private protected abstract RemoteClientLSPInitializationOptions GetRemoteClientLSPInitializationOptions();

    private protected void UpdateClientInitializationOptions(Func<RemoteClientInitializationOptions, RemoteClientInitializationOptions> mutation)
    {
        _clientInitializationOptions = mutation(_clientInitializationOptions);
        FeatureOptions.SetOptions(_clientInitializationOptions);
    }

    private protected void UpdateClientLSPInitializationOptions(Func<RemoteClientLSPInitializationOptions, RemoteClientLSPInitializationOptions> mutation)
    {
        _clientLSPInitializationOptions = mutation(_clientLSPInitializationOptions);

        var lifetimeServices = OOPExportProvider.GetExportedValues<ILspLifetimeService>();
        foreach (var service in lifetimeServices)
        {
            service.OnLspInitialized(_clientLSPInitializationOptions);
        }
    }

    protected abstract TextDocument CreateProjectAndRazorDocument(
        string contents,
        RazorFileKind? fileKind = null,
        string? documentFilePath = null,
        (string fileName, string contents)[]? additionalFiles = null,
        bool inGlobalNamespace = false,
        bool miscellaneousFile = false);

    protected TextDocument CreateProjectAndRazorDocument(
        CodeAnalysis.Workspace remoteWorkspace,
        string contents,
        RazorFileKind? fileKind = null,
        string? documentFilePath = null,
        (string fileName, string contents)[]? additionalFiles = null,
        bool inGlobalNamespace = false,
        bool miscellaneousFile = false)
    {
        // Using IsLegacy means null == component, so easier for test authors
        var isComponent = fileKind != RazorFileKind.Legacy;

        documentFilePath ??= isComponent
            ? TestProjectData.SomeProjectComponentFile1.FilePath
            : TestProjectData.SomeProjectFile1.FilePath;

        var projectId = ProjectId.CreateNewId(debugName: TestProjectData.SomeProject.DisplayName);
        var documentId = DocumentId.CreateNewId(projectId, debugName: documentFilePath);

        return CreateProjectAndRazorDocument(remoteWorkspace, projectId, miscellaneousFile, documentId, documentFilePath, contents, additionalFiles, inGlobalNamespace);
    }

    protected static TextDocument CreateProjectAndRazorDocument(CodeAnalysis.Workspace workspace, ProjectId projectId, bool miscellaneousFile, DocumentId documentId, string documentFilePath, string contents, (string fileName, string contents)[]? additionalFiles, bool inGlobalNamespace)
    {
        return AddProjectAndRazorDocument(workspace.CurrentSolution, TestProjectData.SomeProject.FilePath, projectId, miscellaneousFile, documentId, documentFilePath, contents, additionalFiles, inGlobalNamespace);
    }

    protected static TextDocument AddProjectAndRazorDocument(Solution solution, [DisallowNull] string? projectFilePath, ProjectId projectId, bool miscellaneousFile, DocumentId documentId, string documentFilePath, string contents, (string fileName, string contents)[]? additionalFiles, bool inGlobalNamespace)
    {
        // We simulate a miscellaneous file project by not having a project file path.
        projectFilePath = miscellaneousFile ? null : projectFilePath;
        var projectName = miscellaneousFile ? "" : Path.GetFileNameWithoutExtension(projectFilePath).AssumeNotNull();

        var sgAssembly = typeof(RazorSourceGenerator).Assembly;

        var projectInfo = ProjectInfo
            .Create(
                projectId,
                VersionStamp.Create(),
                name: projectName,
                assemblyName: projectName,
                LanguageNames.CSharp,
                projectFilePath,
                compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .WithMetadataReferences(
                miscellaneousFile
                    ? Net461.ReferenceInfos.All.Select(r => r.Reference) // This isn't quite what Roslyn does, but its close enough for our tests
                    : AspNet80.ReferenceInfos.All.Select(r => r.Reference))
            .WithAnalyzerReferences([new AnalyzerFileReference(sgAssembly.Location, TestAnalyzerAssemblyLoader.LoadFromFile)]);

        if (!miscellaneousFile && !inGlobalNamespace)
        {
            projectInfo = projectInfo.WithDefaultNamespace(TestProjectData.SomeProject.RootNamespace);
        }

        solution = solution.AddProject(projectInfo);

        solution = solution
            .AddAdditionalDocument(
                documentId,
                documentFilePath,
                SourceText.From(contents),
                filePath: documentFilePath);

        if (!miscellaneousFile)
        {
            solution = solution.AddAdditionalDocument(
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

                    # This might suprise you, but by suppressing the source generator here, we're mirroring what happens in the Razor SDK
                    build_property.SuppressRazorSourceGenerator = true
                    """);

            var projectBasePath = Path.GetDirectoryName(projectFilePath).AssumeNotNull();
            // Normally MS Build targets do this for us, but we're on our own!
            foreach (var razorDocument in solution.GetRequiredProject(projectId).AdditionalDocuments)
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
        }

        return solution.GetAdditionalDocument(documentId).AssumeNotNull();
    }

    protected static Uri FileUri(string projectRelativeFileName)
        => new(FilePath(projectRelativeFileName));

    protected static string FilePath(string projectRelativeFileName)
        => Path.GetFullPath(Path.Combine(TestProjectData.SomeProjectPath, projectRelativeFileName));
}
