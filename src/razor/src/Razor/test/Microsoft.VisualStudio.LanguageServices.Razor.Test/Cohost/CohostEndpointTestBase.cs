// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.Razor.Settings;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public abstract class CohostEndpointTestBase(ITestOutputHelper testOutputHelper) : CohostTestBase(testOutputHelper)
{
    private TestRemoteServiceInvoker? _remoteServiceInvoker;
    private IClientSettingsManager? _clientSettingsManager;
    private IFilePathService? _filePathService;
    private ISemanticTokensLegendService? _semanticTokensLegendService;

    private protected override IRemoteServiceInvoker RemoteServiceInvoker => _remoteServiceInvoker.AssumeNotNull();
    private protected TestRemoteServiceInvoker TestRemoteServiceInvoker => _remoteServiceInvoker.AssumeNotNull();
    private protected override IClientSettingsManager ClientSettingsManager => _clientSettingsManager.AssumeNotNull();
    private protected override IFilePathService FilePathService => _filePathService.AssumeNotNull();
    private protected ISemanticTokensLegendService SemanticTokensLegendService => _semanticTokensLegendService.AssumeNotNull();

    /// <summary>
    /// The export provider for Roslyn "devenv" services, if tests opt-in to using them
    /// </summary>
    private protected ExportProvider? RoslynDevenvExportProvider { get; private set; }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        _remoteServiceInvoker = new TestRemoteServiceInvoker(JoinableTaskContext, OOPExportProvider, LoggerFactory);
        AddDisposable(_remoteServiceInvoker);

        _clientSettingsManager = new ClientSettingsManager([], null, null);

        _filePathService = new VisualStudioFilePathService(FeatureOptions);

        _semanticTokensLegendService = TestRazorSemanticTokensLegendService.GetInstance(supportsVSExtensions: true);
    }

    private protected override RemoteClientLSPInitializationOptions GetRemoteClientLSPInitializationOptions()
    {
        return new()
        {
            ClientCapabilities = new VSInternalClientCapabilities()
            {
                SupportsVisualStudioExtensions = true,
                TextDocument = new TextDocumentClientCapabilities
                {
                    Completion = new VSInternalCompletionSetting
                    {
                        CompletionItem = new CompletionItemSetting(),
                        CompletionItemKind = new CompletionItemKindSetting()
                        {
                            ValueSet = (CompletionItemKind[])Enum.GetValues(typeof(CompletionItemKind)),
                        },
                        CompletionList = new VSInternalCompletionListSetting() { Data = true },
                        CompletionListSetting = new CompletionListSetting()
                        {
                            ItemDefaults = ["commitCharacters", "editRange", "insertTextFormat", "data"]
                        },
                        ContextSupport = false,
                        InsertTextMode = InsertTextMode.AsIs,
                    }
                }
            },
            TokenModifiers = [],
            TokenTypes = []
        };
    }

    private protected virtual TestComposition ConfigureRoslynDevenvComposition(TestComposition composition)
        => composition;

    protected TextDocument CreateProjectAndRazorDocument(
        string contents,
        bool remoteOnly)
    {
        if (remoteOnly)
        {
            var remoteWorkspace = RemoteWorkspaceProvider.Instance.GetWorkspace();
            return base.CreateProjectAndRazorDocument(remoteWorkspace, contents, fileKind: null, documentFilePath: null, additionalFiles: null, inGlobalNamespace: false, miscellaneousFile: false);
        }

        return this.CreateProjectAndRazorDocument(contents);
    }

    protected override TextDocument CreateProjectAndRazorDocument(
        string contents,
        RazorFileKind? fileKind = null,
        string? documentFilePath = null,
        (string fileName, string contents)[]? additionalFiles = null,
        bool inGlobalNamespace = false,
        bool miscellaneousFile = false)
    {
        var remoteWorkspace = RemoteWorkspaceProvider.Instance.GetWorkspace();
        var remoteDocument = base.CreateProjectAndRazorDocument(remoteWorkspace, contents, fileKind, documentFilePath, additionalFiles, inGlobalNamespace, miscellaneousFile);

        // In this project we simulate remote services running OOP by creating a different workspace with a different
        // set of services to represent the devenv Roslyn side of things. We don't have any actual solution syncing set
        // up for testing, and don't really use a service broker, but since we also would expect to never make changes
        // to a workspace, it should be fine to simply create duplicated solutions.
        return CreateLocalProjectAndRazorDocument(
            remoteDocument.Project.Solution,
            remoteDocument.Id.ProjectId,
            miscellaneousFile,
            remoteDocument.Id,
            remoteDocument.FilePath.AssumeNotNull(),
            contents,
            additionalFiles,
            inGlobalNamespace);
    }

    private TextDocument CreateLocalProjectAndRazorDocument(
        Solution remoteSolution,
        ProjectId projectId,
        bool miscellaneousFile,
        DocumentId documentId,
        string documentFilePath,
        string contents,
        (string fileName, string contents)[]? additionalFiles,
        bool inGlobalNamespace)
    {
        var composition = ConfigureRoslynDevenvComposition(TestComposition.Roslyn);

        // We can't enforce that the composition is entirely valid, because we don't have a full MEF catalog, but we
        // can assume there should be no errors related to Razor, and having this array makes debugging failures a lot
        // easier.
        var errors = composition.GetCompositionErrors().ToArray();
        Assert.Empty(errors.Where(e => e.Contains("Razor")));

        RoslynDevenvExportProvider = composition.ExportProviderFactory.CreateExportProvider();
        AddDisposable(RoslynDevenvExportProvider);
        var workspace = TestWorkspace.CreateWithDiagnosticAnalyzers(RoslynDevenvExportProvider);
        AddDisposable(workspace);

        var razorDocument = CreateProjectAndRazorDocument(workspace, projectId, miscellaneousFile, documentId, documentFilePath, contents, additionalFiles, inGlobalNamespace);

        // If we're creating remote and local workspaces, then we'll return the local document, and have to allow
        // the remote service invoker to map from the local solution to the remote one.
        TestRemoteServiceInvoker.MapSolutionIdToRemote(razorDocument.Project.Solution.Id, remoteSolution);

        return razorDocument;
    }
}
