// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Composition;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public abstract class CohostEndpointTestBase(ITestOutputHelper testOutputHelper) : WorkspaceTestBase(testOutputHelper)
{
    private const string CSharpVirtualDocumentSuffix = ".g.cs";
    private ExportProvider? _exportProvider;
    private TestRemoteServiceInvoker? _remoteServiceInvoker;
    private RemoteClientInitializationOptions _clientInitializationOptions;

    private protected TestRemoteServiceInvoker RemoteServiceInvoker => _remoteServiceInvoker.AssumeNotNull();

    /// <summary>
    /// The export provider for Razor OOP services (not Roslyn)
    /// </summary>
    private protected ExportProvider OOPExportProvider => _exportProvider.AssumeNotNull();

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        // Create a new isolated MEF composition.
        // Note that this uses a cached catalog and configuration for performance.
        _exportProvider = await RemoteMefComposition.CreateExportProviderAsync(DisposalToken);
        AddDisposable(_exportProvider);

        _remoteServiceInvoker = new TestRemoteServiceInvoker(JoinableTaskContext, _exportProvider, LoggerFactory);
        AddDisposable(_remoteServiceInvoker);

        _clientInitializationOptions = new()
        {
            CSharpVirtualDocumentSuffix = CSharpVirtualDocumentSuffix,
            HtmlVirtualDocumentSuffix = ".g.html",
            IncludeProjectKeyInGeneratedFilePath = false,
            UsePreciseSemanticTokenRanges = false,
            UseRazorCohostServer = true
        };
        UpdateClientInitializationOptions(c => c);
    }

    private protected void UpdateClientInitializationOptions(Func<RemoteClientInitializationOptions, RemoteClientInitializationOptions> mutation)
    {
        _clientInitializationOptions = mutation(_clientInitializationOptions);
        var featureOptions = OOPExportProvider.GetExportedValue<RemoteLanguageServerFeatureOptions>();
        featureOptions.SetOptions(_clientInitializationOptions);
    }

    protected TextDocument CreateProjectAndRazorDocument(string contents, string? fileKind = null)
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

        var projectInfo = ProjectInfo
            .Create(
                projectId,
                VersionStamp.Create(),
                name: projectName,
                assemblyName: projectName,
                LanguageNames.CSharp,
                documentFilePath)
            .WithMetadataReferences(AspNet80.ReferenceInfos.All.Select(r => r.Reference));

        var solution = Workspace.CurrentSolution.AddProject(projectInfo);

        solution = solution
            .AddAdditionalDocument(
                documentId,
                documentFilePath,
                SourceText.From(contents),
                filePath: documentFilePath)
            .AddDocument(
                DocumentId.CreateNewId(projectId),
                name: documentFilePath + CSharpVirtualDocumentSuffix,
                SourceText.From(""),
                filePath: documentFilePath + CSharpVirtualDocumentSuffix)
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

        return solution.GetAdditionalDocument(documentId).AssumeNotNull();
    }
}
